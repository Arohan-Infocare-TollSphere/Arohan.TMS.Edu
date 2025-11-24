using MediatR;
using Arohan.TMS.Application.Features.Lanes.DTOs;

namespace Arohan.TMS.Application.Features.Lanes.Queries.GetLane
{
    /// <summary>Get a single lane by id. Tenant is resolved on the server.</summary>
    public record GetLaneQuery(Guid LaneId) : IRequest<LaneDto>;
}
