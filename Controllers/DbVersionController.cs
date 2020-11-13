using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Whiteboard.API.Data;
using Whiteboard.API.Models;

namespace Whiteboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DbVersionController : ControllerBase
    {
        
        private readonly IVersionRepository _repo;

        public DbVersionController(IVersionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("Get")]
        public async Task<DbVersion> GetVersion()
        {
            return await _repo.GetVersion();
        }
    }
}