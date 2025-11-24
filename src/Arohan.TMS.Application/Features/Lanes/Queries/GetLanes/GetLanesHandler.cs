using MediatR;
using AutoMapper;
using Arohan.TMS.Application.Interfaces;
using Arohan.TMS.Domain.Entities;

namespace Arohan.TMS.Application.Features.Lanes.Queries.GetLanes
{
    public class GetLanesHandler : IRequestHandler<GetLanesQuery, List<DTOs.LaneDto>>
    {
        private readonly IRepository<Lane> _repo;
        private readonly ITenantProvider _tenant;
        private readonly IMapper _mapper;

        public GetLanesHandler(IRepository<Lane> repo, ITenantProvider tenant, IMapper mapper)
        {
            _repo = repo;
            _tenant = tenant;
            _mapper = mapper;
        }

        public async Task<List<DTOs.LaneDto>> Handle(GetLanesQuery request, CancellationToken ct)
        {
            var tenantId = _tenant.TenantId ?? throw new UnauthorizedAccessException("Tenant not resolved.");

            // If you rely on the EF query filter (TenantId) this explicit predicate may be redundant,
            // but it's OK and safe to include.
            var lanes = await _repo.ListAsync(x => x.TenantId == tenantId, ct);
            return lanes.Select(l => _mapper.Map<DTOs.LaneDto>(l)).ToList();
        }
    }
}
