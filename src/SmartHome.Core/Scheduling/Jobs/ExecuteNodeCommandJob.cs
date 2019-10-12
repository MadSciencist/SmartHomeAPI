using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Quartz;
using SmartHome.Core.Security;
using SmartHome.Core.Services.Abstractions;
using System.Threading.Tasks;

namespace SmartHome.Core.Scheduling.Jobs
{
    [DisallowConcurrentExecution]
    public class ExecuteNodeCommandJob : IJob
    {
        private readonly ILogger<ExecuteNodeCommandJob> _logger;
        private readonly INodeService _nodeService;

        public ExecuteNodeCommandJob(ILogger<ExecuteNodeCommandJob> logger, INodeService nodeService)
        {
            _logger = logger;
            _nodeService = nodeService;
            _nodeService.Principal = SystemClaimPrincipalProvider.GetSystemClaimPrincipal();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogDebug($"NODE EXECUTION NODE EXECUTION NODE EXECUTION NODE EXECUTION");

            var nodeId = context.JobDetail.JobDataMap.GetInt(nameof(NodeJobSchedule.NodeId));
            var command = context.JobDetail.JobDataMap.GetString(nameof(NodeJobSchedule.Command));

            // Parameters might be null, as not all commands require them
            var parameters = new JObject();
            var commandParams = context.JobDetail.JobDataMap.Get(nameof(NodeJobSchedule.CommandParams));
            if (commandParams != null) parameters = new JObject(commandParams);
            
            await _nodeService.ExecuteCommand(nodeId, command, parameters);
        }
    }
}
