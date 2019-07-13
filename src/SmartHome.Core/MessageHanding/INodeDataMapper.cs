using SmartHome.Core.Domain.Models;
using System.Collections.Generic;

namespace SmartHome.Core.MessageHanding
{
    public interface INodeDataMapper
    {
        /// <summary>
        /// Dictionary of mappings between contract and SmartHome.Core.Domain.SystemMagnitudes properties
        /// </summary>
        IDictionary<string, string> Mapping { get; }

        /// <summary>
        /// Contains all the valid properties for specific message contract
        /// </summary>
        ICollection<PhysicalProperty> ValidProperties { get; }

        /// <summary>
        /// Check if ValidProperties collection contains property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool IsPropertyValid(string property);
    }
}
