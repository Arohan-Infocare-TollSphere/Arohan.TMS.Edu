using MediatR;
using Arohan.TMS.Application.Features.Lanes.DTOs;

namespace Arohan.TMS.Application.Features.Lanes.Commands.CreateLane
{
    /// <summary>
    /// Tenant is resolved on the server via ITenantProvider -- do not pass TenantId from the client.
    /// </summary>
    public record CreateLaneCommand(int PlazaId, string Name, string? DocumentUrl, string? CredentialsRef, string AuthType)
        : IRequest<LaneDto>;
}
