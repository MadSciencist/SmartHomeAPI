using System.Linq;
using AutoMapper;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;

namespace SmartHome.Core.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateNodeMapping();
        }
        private void CreateNodeMapping()
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
                .ForMember(x => x.ControlStrategyId, opt => opt.Ignore())
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<Node, NodeDto>()
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
                .ForMember(x => x.ControlStrategyName, opt => opt.MapFrom(x => x.ControlStrategy.AssemblyProduct))
                .ForMember(x => x.Magnitudes, opt => opt.MapFrom(x => x.ControlStrategy.RegisteredMagnitudes.Select(m => m.Magnitude)))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
