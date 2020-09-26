using System.Collections.Generic;
using System.Threading.Tasks;
using Whiteboard.API.Models;

namespace Whiteboard.API.Data
{
    public interface IWhiteboardRepository
    {
         Task<WhiteboardItem> CreateWhiteboard(int userId, string title);
         Task<List<WhiteboardItem>> GetWhiteboards(int userId); 
         Task<WhiteboardItem> UpdateWhiteboard (WhiteboardItem whiteboard);
         Task<WhiteboardItem> DeleteWhiteboard(int userId, int whiteboardId);
    }
}