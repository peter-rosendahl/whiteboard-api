using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Whiteboard.API.Data;
using Whiteboard.API.Models;

namespace Whiteboard.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostitController : ControllerBase
    {
        private readonly IPostitRepository _repo;

        public PostitController(IPostitRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("Get")]
        public async Task<List<Postit>> GetPostits(int boardId)
        {
            return await _repo.GetPostits(boardId);
        }

        [HttpPost("Insert")]
        public async Task<Postit> CreatePostit(int boardId)
        {
            return await _repo.CreatePostit(boardId);
        }

        [HttpPut("Update")]
        public async Task<Postit> UpdatePostit(Postit postit)
        {
            return await _repo.UpdatePostit(postit);
        }

        [HttpDelete("Delete")]
        public async Task<Postit> DeletePostit(int userId, int postitId)
        {
            return await _repo.DeletePostit(userId, postitId);
        }
    }
}