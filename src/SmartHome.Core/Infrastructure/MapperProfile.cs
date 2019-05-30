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
            CreateCommandMapping();
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
                .ForAllOtherMembers(x => x.Ignore());
        }

        private void CreateCommandMapping()
        {
            CreateMap<CommandEntityDto, Command>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Alias, opt => opt.MapFrom(x => x.Alias))
                .ForMember(x => x.ExecutorClassName, opt => opt.MapFrom(x => x.ExecutorClassName))
                .ForMember(x => x.Nodes, opt => opt.Ignore())
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
        }

        private void CreateControlStrategyMapping()
        {
            CreateMap<ControlStrategyDto, ControlStrategy>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.ControlContext, opt => opt.MapFrom(x => x.ControlContext))
                .ForMember(x => x.ReceiveContext, opt => opt.MapFrom(x => x.ReceiveContext))
                .ForMember(x => x.ControlProviderName, opt => opt.MapFrom(x => x.ControlProvider))
                .ForMember(x => x.ReceiveProviderName, opt => opt.MapFrom(x => x.ReceiveProvider))
                .ForMember(x => x.IsActive, opt => opt.MapFrom(x => x.IsActive))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.Created, opt => opt.MapFrom(x => x.Created))
                .ForMember(x => x.CreatedById, opt => opt.MapFrom(x => x.CreatedById))
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.RegisteredSensors, opt => opt.Ignore())
                .ForMember(x => x.Nodes, opt => opt.Ignore())
                .ForMember(x => x.AllowedCommands, opt => opt.Ignore())
                .ReverseMap()
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
