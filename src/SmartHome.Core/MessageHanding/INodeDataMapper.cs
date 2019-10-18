using SmartHome.Core.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Core.MessageHanding
{
    public interface INodeDataMapper
    {
        /// <summary>
        /// Check if ValidProperties collection contains property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool IsPropertyValid(string property);

        /// <summary>
        /// Get system value of vendor magnitude
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        string GetMapping(string magnitude);

        /// <summary>
        /// Gets registered type of converter by vendor magnitude
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        Type GetConverter(string magnitude);

        /// <summary>
        /// Returns system physical property by using magnitude in contract specific contract
        /// </summary>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        Task<PhysicalProperty> GetPhysicalPropertyByContractMagnitudeAsync(string magnitude);

        /// <summary>
        /// Returns all system physical properties in specific contract
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PhysicalProperty>> GetAllContractPhysicalProperties();
    }
}
