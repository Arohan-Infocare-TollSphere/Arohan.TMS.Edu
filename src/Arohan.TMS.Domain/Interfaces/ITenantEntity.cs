namespace Arohan.TMS.Domain.Interfaces
{
    /// <summary>
    /// Marker interface for tenant-scoped entities that contain a TenantId property (Guid).
    /// Entities implementing this interface will get an automatic EF Core HasQueryFilter applied.
    /// </summary>
    public interface ITenantEntity
    {
        Guid TenantId { get; set; }
    }
}
