using SmartHome.Core.DataAccess.Repository;
using SmartHome.Core.Domain.Entity;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using System;
using System.Threading.Tasks;

namespace SmartHome.Core.Services
{
    public class CommandsService : ServiceBase<object, Command>, ICommandService
    {
        public CommandsService(IGenericRepository<Command> commandRepository) : base(commandRepository)
        {
        }

        public async Task<ServiceResult<CommandEntityDto>> CreateCommand(string alias, string executorClass)
        {
            var response = new ServiceResult<CommandEntityDto>(Principal);

            var command = new Command
            {
                Alias = alias,
                ExecutorClassName = executorClass
            };

            //Todo command validator here

            try
            {
                var created = await GenericRepository.CreateAsync(command);
                response.Data = response.Data = Mapper.Map<CommandEntityDto>(created);
                response.Alerts.Add(new Alert("Successfully created", MessageType.Success));

                return response;
            }
            catch (Exception ex)
            {
                response.Alerts.Add(new Alert(ex.Message, MessageType.Exception));
                throw;
            }
        }
    }
}
