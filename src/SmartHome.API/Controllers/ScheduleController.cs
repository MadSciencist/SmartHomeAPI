﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartHome.API.Utils;
using SmartHome.Core.Dto;
using SmartHome.Core.Infrastructure;
using SmartHome.Core.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ScheduleController : ControllerBase
    {
        private readonly ISchedulingService _schedulingService;

        public ScheduleController(IHttpContextAccessor contextAccessor, ISchedulingService schedulingService)
        {
            _schedulingService = schedulingService;
            _schedulingService.Principal = contextAccessor.HttpContext.User;
        }

        /// <summary>
        /// Gets all scheduled jobs
        /// </summary>
        /// <returns>Date of next execution</returns>
        [HttpGet("jobs")]
        [ProducesResponseType(typeof(ServiceResult<List<string>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetScheduledJobs()
        {
            var serviceResult = await _schedulingService.GetJobs();
            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }

        /// <summary>
        /// Creates new schedule
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Date of next execution</returns>
        [HttpPost("jobs/node-command")]
        [ProducesResponseType(typeof(ServiceResult<DateTimeOffset>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ScheduleNodeCommandJob(ScheduleNodeCommandJobDto dto)
        {
            var serviceResult = await _schedulingService.AddNodeCommandJobAsync(dto);
            return ControllerResponseHelper.GetDefaultResponse(serviceResult);
        }
    }
}