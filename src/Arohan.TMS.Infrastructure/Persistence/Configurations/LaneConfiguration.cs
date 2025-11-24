using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Arohan.TMS.Domain.Entities;

namespace Arohan.TMS.Infrastructure.Persistence.Configurations
{
    public class LaneConfiguration : IEntityTypeConfiguration<Lane>
    {
        public void Configure(EntityTypeBuilder<Lane> builder)
        {
            builder.ToTable("Lanes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.DocumentUrl).HasMaxLength(1000);
            builder.Property(x => x.CredentialsRef).HasMaxLength(500);

            // store enum as string
            builder.Property(x => x.AuthType)
                   .HasConversion<string>()
                   .HasMaxLength(50);

            // example index for tenant + plaza + name uniqueness
            builder.HasIndex(x => new { x.TenantId, x.PlazaId, x.Name }).HasDatabaseName("IX_Lanes_Tenant_Plaza_Name");
        }
    }
}
