using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Autofac;
using Microsoft.EntityFrameworkCore;
using SmartHome.API.Persistence.App;
using SmartHome.API.Services.Crud;
using SmartHome.DeviceController;
using System.Reflection;

namespace SmartHome.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NodeController : ControllerBase
    {
        private readonly ICrudNodeService _crudNodeService;
        private readonly AppDbContext _context;
        private readonly ILifetimeScope _container;

        public NodeController(ICrudNodeService crudNodeService, AppDbContext context, ILifetimeScope container)
        {
            _crudNodeService = crudNodeService;
            _context = context;
            _container = container;
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await _crudNodeService.CreateNode(this.User, "TestNodeName", "TestIdentifier", "aaaa", "sensor");
            
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{test}")]
        public async Task<IActionResult> A(string test)
        {
            var strategy = _container.ResolveNamed(test, typeof(IControlStrategy)) as IControlStrategy;
            
            strategy.Execute();
            return Ok();
        }



        //[AllowAnonymous]
        //[HttpGet("test")]
        //public async Task<IActionResult> A()
        //{
        //    var nodesWithIncludedUsers = _context.Nodes.Include(x => x.AllowedUsers).ThenInclude(x => x.User);
        //    return Ok(usnodesWithIncludedUsersers);
        //}
    }
}
