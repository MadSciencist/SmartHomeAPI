using System;
using SmartHome.Core.Entities.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Core.Services.Abstractions;

namespace SmartHome.Core.MessageHanding
{
    public interface INodeDataMapper
    {
        //IPhysicalPropertyService PhysicalPropertyService { get; set; }
        /// <summary>
        /// Check if ValidProperties collection contains property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        bool IsPropertyValid(string property);

        string GetMapping(string property);
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
        ICollection<PhysicalProperty> GetAllContractPhysicalProperties();
    }
}
