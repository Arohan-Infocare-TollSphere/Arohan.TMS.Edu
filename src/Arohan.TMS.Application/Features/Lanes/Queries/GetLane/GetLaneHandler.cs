using MediatR;
using AutoMapper;
using Arohan.TMS.Application.Interfaces;
using Arohan.TMS.Domain.Entities;

namespace Arohan.TMS.Application.Features.Lanes.Queries.GetLane
{
    public class GetLaneHandler : IRequestHandler<GetLaneQuery, DTOs.LaneDto>
    {
        private readonly IRepository<Lane> _repo;
        private readonly ITenantProvider _tenant;
        private readonly IMapper _mapper;

        public GetLaneHandler(IRepository<Lane> repo, ITenantProvider tenant, IMapper mapper)
        {
            _repo = repo;
            _tenant = tenant;
            _mapper = mapper;
        }

        public async Task<DTOs.LaneDto> Handle(GetLaneQuery request, CancellationToken ct)
        {
            var tenantId = _tenant.TenantId ?? throw new UnauthorizedAccessException("Tenant not resolved.");

            var lane = await _repo.SingleOrDefaultAsync(x => x.Id == request.LaneId && x.TenantId == tenantId, ct);
            if (lane == null) throw new KeyNotFoundException($"Lane {request.LaneId} not found for tenant {tenantId}.");

            return _mapper.Map<DTOs.LaneDto>(lane);
        }
    }
}
