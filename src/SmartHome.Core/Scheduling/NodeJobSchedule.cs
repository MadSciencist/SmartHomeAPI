using System;
using SmartHome.Core.Entities.Entity;

namespace SmartHome.Core.Scheduling
{
    public class NodeJobSchedule : JobSchedule
    {
        public Node Node { get; }

        public NodeJobSchedule(Type type, Node node, string cronExpression) : base(type, cronExpression)
        {
            Node = node;
        }

        /// <summary>
        /// Gets the identity which identifies job. Guid is added to make possible of adding multiple jobs on same node.
        /// </summary>
        /// <returns></returns>
        public override string GetIdentity()
        {
            return base.GetIdentity() + "_NODE_ID_" + Node.Id + Guid.NewGuid();
        }
    }
}