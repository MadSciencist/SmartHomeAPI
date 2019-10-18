using AutoMapper;
using SmartHome.Core.Dto;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.Entities.SchedulingEntity;
using System.Linq;

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
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
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
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
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

            CreateMap<UiConfiguration, UiConfigurationDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Type, opt => opt.MapFrom(x => x.Type))
                .ForMember(x => x.Data, opt => opt.MapFrom(x => x.Data))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<UiConfigurationDto, UiConfiguration>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Type, opt => opt.MapFrom(x => x.Type))
                .ForMember(x => x.Data, opt => opt.MapFrom(x => x.Data))
                .ForAllOtherMembers(x => x.Ignore());

            CreateMap<ScheduleEntity, JobScheduleDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Name))
                .ForMember(x => x.CronExpression, opt => opt.MapFrom(x => x.CronExpression))
                .ForMember(x => x.JobStatusId, opt => opt.MapFrom(x => x.JobStatusEntityId))
                .ForMember(x => x.JobTypeId, opt => opt.MapFrom(x => x.JobTypeId))
                .ForMember(x => x.CreatedById, opt => opt.MapFrom(x => x.CreatedById))
                .ForMember(x => x.CreatedBy, opt => opt.MapFrom(x => x.CreatedBy.UserName))
                .ForMember(x => x.Created, opt => opt.MapFrom(x => x.Created))
                .ForMember(x => x.LastUpdatedById, opt => opt.MapFrom(x => x.UpdatedById))
                .ForMember(x => x.LastUpdatedBy, opt => opt.MapFrom(x => x.UpdatedBy.UserName))
                .ForMember(x => x.LastUpdated, opt => opt.MapFrom(x => x.Updated))
                .ForAllOtherMembers(x => x.Ignore());
        }
    }
}
