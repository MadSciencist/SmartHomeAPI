using AutoMapper;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<NodeDto, Node>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.IpAddress, opt => opt.MapFrom(x => x.IpAddress))
                .ForMember(x => x.Port, opt => opt.MapFrom(x => x.Port))
                .ForMember(x => x.GatewayIpAddress, opt => opt.MapFrom(x => x.GatewayIpAddress))
                .ForMember(x => x.Login, opt => opt.MapFrom(x => x.Login))
                .ForMember(x => x.Password, opt => opt.MapFrom(x => x.Password))
                .ForMember(x => x.ApiKey, opt => opt.MapFrom(x => x.ApiKey))
                .ForMember(x => x.BaseTopic, opt => opt.MapFrom(x => x.BaseTopic))
                .ForMember(x => x.ClientId, opt => opt.MapFrom(x => x.ClientId))
                .ForMember(x => x.ConfigMetadata, opt => opt.MapFrom(x => x.ConfigMetadata))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<ControlStrategyDto, ControlStrategy>()
                .ForMember(x => x.ControlContext, opt => opt.MapFrom(x => x.ControlContext))
                .ForMember(x => x.ReceiveContext, opt => opt.MapFrom(x => x.ReceiveContext))
                .ForMember(x => x.ControlProviderName, opt => opt.MapFrom(x => x.ControlProvider))
                .ForMember(x => x.ReceiveProviderName, opt => opt.MapFrom(x => x.ReceiveProvider))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(x => x.IsActive))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
