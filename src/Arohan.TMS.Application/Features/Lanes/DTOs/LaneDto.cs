using System;
using System.Collections.Generic;
using System.Text;

namespace Arohan.TMS.Application.Features.Lanes.DTOs
{
    public class LaneDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public int PlazaId { get; set; }
        public string Name { get; set; } = default!;
        public string? DocumentUrl { get; set; }
        public string AuthType { get; set; } = default!;
        public string? CredentialsRef { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
