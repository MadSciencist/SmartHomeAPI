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
            CreateControlStrategyMapping();
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
                .ForMember(x => x.ControlStrategyId, opt => opt.MapFrom(x => x.ControlStrategyId))
                .ForAllOtherMembers(x => x.Ignore());
        }
        
        private void CreateControlStrategyMapping()
        {
            CreateMap<ControlStrategyDto, ControlStrategy>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.AssemblyProduct, opt => opt.MapFrom(x => x.ControlStrategyName))
                .ForMember(x => x.ContractAssembly, opt => opt.Ignore())
                .ForMember(x => x.IsActive, opt => opt.MapFrom(x => x.IsActive))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.Created, opt => opt.MapFrom(x => x.Created))
                .ForMember(x => x.CreatedById, opt => opt.MapFrom(x => x.CreatedById))
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.Nodes, opt => opt.Ignore())
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
