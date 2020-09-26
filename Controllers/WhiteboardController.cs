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
    public class WhiteboardController : ControllerBase
    {
        private readonly IWhiteboardRepository _repo;

        public WhiteboardController(IWhiteboardRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("Get")]
        public async Task<List<WhiteboardItem>> GetWhiteboards(int userId)
        {
            return await _repo.GetWhiteboards(userId);
        }

        [HttpPost("Insert")]
        public async Task<WhiteboardItem> CreateWhiteboard(int userId, string title)
        {
            return await _repo.CreateWhiteboard(userId, title);
        }

        [HttpPut("Update")]
        public async Task<WhiteboardItem> UpdateWhiteboard(WhiteboardItem board)
        {
            return await _repo.UpdateWhiteboard(board);
        }

        [HttpDelete("Delete")]
        public async Task<WhiteboardItem> DeleteWhiteboard(int userId, int boardId)
        {
            return await _repo.DeleteWhiteboard(userId, boardId);
        }
    }
}