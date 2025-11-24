using MediatR;
using Arohan.TMS.Application.Features.Lanes.DTOs;

namespace Arohan.TMS.Application.Features.Lanes.Queries.GetLanes
{
    /// <summary>
    /// No TenantId in request: server will resolve tenant via ITenantProvider.
    /// </summary>
    public record GetLanesQuery() : IRequest<List<LaneDto>>;
}
