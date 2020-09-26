using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whiteboard.API.Models;

namespace Whiteboard.API.Data
{
    public class WhiteboardRepository : IWhiteboardRepository
    {
        private readonly DataContext _context;

        public WhiteboardRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<WhiteboardItem> CreateWhiteboard(int userId, string title)
        {
            User user = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) {
                return null;
            }
            
            List<WhiteboardItem> boards = await _context.Whiteboard.ToListAsync();

            WhiteboardItem board = new WhiteboardItem();
            board.UserId = userId;
            board.User = user;
            board.TabOrderIndex = (boards.Count() + 1);
            board.Title = title;
            var dbBoard = await _context.Whiteboard.AddAsync(board);
            await _context.SaveChangesAsync();

            return board;
        }

        public async Task<List<WhiteboardItem>> GetWhiteboards(int userId)
        {
            return await _context.Whiteboard.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<WhiteboardItem> UpdateWhiteboard(WhiteboardItem whiteboard)
        {
            WhiteboardItem dbBoard = await _context.Whiteboard.FirstOrDefaultAsync(x => x.Id == whiteboard.Id);
            if (dbBoard == null)
            {
                return null;
            }

            dbBoard.Title = whiteboard.Title;
            dbBoard.TabOrderIndex = whiteboard.TabOrderIndex;

            await _context.SaveChangesAsync();

            return dbBoard;
        }

        public async Task<WhiteboardItem> DeleteWhiteboard(int userId, int whiteboardId)
        {
            WhiteboardItem dbBoard = await _context.Whiteboard.FirstOrDefaultAsync(x => x.Id == whiteboardId);

            if (dbBoard == null)
            {
                return null;
            }
            else {
                var totalNotes = await _context.Postit.ToListAsync();
                var notesInBoard = totalNotes.Where(x => x.Id == dbBoard.Id);
                if (notesInBoard.Count() >= 1) 
                {
                    _context.Postit.RemoveRange(notesInBoard);
                }
                _context.Whiteboard.Remove(dbBoard);
                await _context.SaveChangesAsync();
                return null;
            }
        }
    }
}