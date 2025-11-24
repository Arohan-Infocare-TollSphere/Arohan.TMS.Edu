using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Arohan.TMS.Application.Interfaces;
using Arohan.TMS.Domain.Interfaces;
using Arohan.TMS.Domain.Entities;
using Arohan.TMS.Infrastructure.Persistence.Configurations;



namespace Arohan.TMS.Infrastructure.Persistence
{
    /// <summary>
    /// DbContext with an instance-level CurrentTenantId property.
    /// Set CurrentTenantId (e.g. in middleware) for each request/scope before executing queries.
    /// HasQueryFilter expressions reference this property so they are evaluated at runtime.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // Instance property used by EF Core query filters. Set this for each scope/request.
        public Guid? CurrentTenantId { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Lane> Lanes => Set<Lane>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // apply explicit configurations
            modelBuilder.ApplyConfiguration(new LaneConfiguration());

            // Apply generic tenant filter (references this.CurrentTenantId so it's evaluated per DbContext instance)
            ApplyTenantQueryFilter(modelBuilder);
        }

        /// <summary>
        /// Applies HasQueryFilter for each entity type that implements ITenantEntity.
        /// The filter references this.CurrentTenantId so it will use the value set on the DbContext instance at runtime.
        /// </summary>
        private void ApplyTenantQueryFilter(ModelBuilder modelBuilder)
        {
            var tenantEntityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(ITenantEntity).IsAssignableFrom(t.ClrType))
                .ToList();

            foreach (var et in tenantEntityTypes)
            {
                var clrType = et.ClrType;

                // Build expression: (T e) => EF.Property<Guid>(e, "TenantId") == this.CurrentTenantId
                var parameter = Expression.Parameter(clrType, "e");

                // EF.Property<Guid>(e, "TenantId")
                var efPropertyMethod = typeof(EF).GetMethod(nameof(EF.Property), BindingFlags.Static | BindingFlags.Public)!
                    .MakeGenericMethod(typeof(Guid));
                var tenantPropertyAccess = Expression.Call(efPropertyMethod, parameter, Expression.Constant(nameof(ITenantEntity.TenantId)));

                // this.CurrentTenantId (member access against the current DbContext instance)
                var dbContextConstant = Expression.Constant(this);
                var currentTenantProperty = Expression.Property(dbContextConstant, nameof(CurrentTenantId));

                // Because CurrentTenantId is Guid? and EF.Property returns Guid, compare equality appropriately:
                // EF.Property<Guid>(e, "TenantId").Equals(CurrentTenantId.Value)
                // But easier: create equality expression after converting nullable to non-nullable:
                // Use Expression.Equal(tenantPropertyAccess, Expression.Convert(currentTenantProperty, typeof(Guid)))

                // Convert currentTenantProperty (Guid?) to Guid (this will throw in expression if null, so we guard using a null-check)
                var hasTenant = Expression.NotEqual(currentTenantProperty, Expression.Constant(null, typeof(Guid?)));

                var currentTenantValue = Expression.Convert(currentTenantProperty, typeof(Guid));
                var equality = Expression.Equal(tenantPropertyAccess, currentTenantValue);

                // Final predicate: (this.CurrentTenantId != null) && (EF.Property<Guid>(e, "TenantId") == this.CurrentTenantId.Value)
                var predicateBody = Expression.AndAlso(hasTenant, equality);

                var lambda = Expression.Lambda(predicateBody, parameter);

                // Apply filter to the entity
                modelBuilder.Entity(clrType).HasQueryFilter(lambda);
            }
        }

        // Optionally override SaveChanges to fill audit fields etc.
        public override int SaveChanges()
        {
            // handle auditing, etc.
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // handle auditing, etc.
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
