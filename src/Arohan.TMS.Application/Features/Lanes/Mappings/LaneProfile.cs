using AutoMapper;
using Arohan.TMS.Domain.Entities;
using Arohan.TMS.Application.Features.Lanes.DTOs;

namespace Arohan.TMS.Application.Features.Lanes.Mappings
{
    public class LaneProfile : Profile
    {
        public LaneProfile()
        {
            CreateMap<Lane, LaneDto>()
                .ForMember(d => d.AuthType, opt => opt.MapFrom(s => s.AuthType.ToString()));
        }
    }
}
