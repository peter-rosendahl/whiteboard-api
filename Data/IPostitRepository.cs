using System.Collections.Generic;
using System.Threading.Tasks;
using Whiteboard.API.Models;

namespace Whiteboard.API.Data
{
    public interface IPostitRepository
    {
         Task<Postit> CreatePostit(int boardId);
         Task<List<Postit>> GetPostits(int boardId); 
         Task<Postit> UpdatePostit (Postit postit);
         Task<Postit> CopyPostit (int postitId, int boardId);
         Task<Postit> DeletePostit(int boardId, int postitId);
    }
}