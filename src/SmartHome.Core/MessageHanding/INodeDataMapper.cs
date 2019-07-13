using System.Collections.Generic;
using SmartHome.Core.Domain.Models;

namespace SmartHome.Core.MessageHanding
{
    public interface INodeDataMapper
    {
        /// <summary>
        /// Dictionary of mappings between contract and SmartHome.Core.Domain.SystemMagnitudes properties
        /// </summary>
        IDictionary<string, string> Mapping { get; }

        /// <summary>
        /// Check if ValidProperties collection contains property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool IsPropertyValid(string property);

        /// <summary>
        /// Returns system physical property by using magnitude in contract specific contract
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        PhysicalProperty GetPhysicalPropertyByContractMagnitude(string magnitude);

        /// <summary>
        /// Returns all system physical properties in specific contract
        /// </summary>
        /// <returns></returns>
        ICollection<PhysicalProperty> GetAllContractPhysicalProperties();
    }
}
