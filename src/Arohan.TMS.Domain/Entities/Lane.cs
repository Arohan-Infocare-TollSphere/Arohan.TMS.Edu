using System.ComponentModel.DataAnnotations;
using Arohan.TMS.Domain.Interfaces;

namespace Arohan.TMS.Domain.Entities
{
    public enum AuthType
    {
        None,
        Basic,
        ApiKey,
        OAuth2
    }

    public class Lane : ITenantEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }           // tenant-scoped
        public int PlazaId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; } = default!;

        [MaxLength(1000)]
        public string? DocumentUrl { get; set; }     // reference to docs (could be storage URL)

        public AuthType AuthType { get; set; } = AuthType.None;

        public string? CredentialsRef { get; set; }  // vault reference only, not secret

        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
