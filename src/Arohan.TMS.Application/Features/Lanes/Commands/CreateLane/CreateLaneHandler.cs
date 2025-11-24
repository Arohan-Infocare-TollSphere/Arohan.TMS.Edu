using MediatR;
using AutoMapper;
using Arohan.TMS.Application.Interfaces;
using Arohan.TMS.Domain.Entities;

namespace Arohan.TMS.Application.Features.Lanes.Commands.CreateLane
{
    public class CreateLaneHandler : IRequestHandler<CreateLaneCommand, DTOs.LaneDto>
    {
        private readonly IRepository<Lane> _repo;
        private readonly ApplicationDbContext _context;
        private readonly ITenantProvider _tenant;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateLaneHandler(IRepository<Lane> repo, ApplicationDbContext context, ITenantProvider tenant, IMapper mapper, ICurrentUserService currentUser)
        {
            _repo = repo;
            _context = context;
            _tenant = tenant;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<DTOs.LaneDto> Handle(CreateLaneCommand request, CancellationToken ct)
        {
            var tenantId = _tenant.TenantId ?? throw new UnauthorizedAccessException("Tenant not resolved.");

            var lane = new Lane
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                PlazaId = request.PlazaId,
                Name = request.Name,
                DocumentUrl = request.DocumentUrl,
                CredentialsRef = request.CredentialsRef,
                AuthType = Enum.TryParse<AuthType>(request.AuthType, true, out var t) ? t : AuthType.None,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUser.UserName ?? _currentUser.UserId ?? "system"
            };

            await _repo.AddAsync(lane, ct);
            await _context.SaveChangesAsync(ct);

            return _mapper.Map<DTOs.LaneDto>(lane);
        }
    }
}
