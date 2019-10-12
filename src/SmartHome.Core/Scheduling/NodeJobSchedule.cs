using System;
using Quartz;

namespace SmartHome.Core.Scheduling
{
    public class NodeJobSchedule : JobSchedule
    {
        /// <summary>
        /// Node that will be target od the job.
        /// </summary>
        public int NodeId{ get; }

        /// <summary>
        /// Command to execute on the node on job trigger
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Additional parameters that are passed to command handler - JSON
        /// </summary>
        public object CommandParams { get; }

        public NodeJobSchedule(Type type, int nodeId, string command, object commandParams, string cronExpression) : base(type, cronExpression)
        {
            NodeId = nodeId;
            Command = command;
            CommandParams = commandParams;
        }

        /// <inheritdoc/>
        public override JobDataMap GetJobData()
        {
            return new JobDataMap
            {
                { nameof(NodeId), NodeId },
                { nameof(Command), Command },
                { nameof(CommandParams), CommandParams }
            };
        }

        /// <inheritdoc/>
        public override string GetIdentity()
        {
            var hash = CommandParams == null ? "NO_PARAMS" : $"PARAMS_HASH: {CommandParams.GetHashCode()}";
            return $"{base.GetIdentity()}_NODE_ID_{NodeId}_CMD_{Command}_{hash}";
        }
    }
}