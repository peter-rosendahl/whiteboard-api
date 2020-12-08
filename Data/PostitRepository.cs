using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whiteboard.API.Models;

namespace Whiteboard.API.Data
{
    public class PostitRepository : IPostitRepository
    {
        private readonly DataContext _context;

        public PostitRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Postit> CopyPostit(int postitId, int boardId)
        {
            WhiteboardItem board = await _context.Whiteboard.FirstOrDefaultAsync(x => x.Id == boardId);
            Postit postit = await _context.Postit.FirstOrDefaultAsync(y => y.Id == postitId);

            if (board == null || postit == null) {
                return null;
            }

            Postit newNote = new Postit();
            newNote.WhiteboardId = board.Id;
            newNote.Content = postit.Content;
            newNote.Width = postit.Width;
            newNote.Height = postit.Height;
            newNote.PosX = postit.PosX;
            newNote.PosY = postit.PosY;
            newNote.IsCollapsed = postit.IsCollapsed;
            newNote.ColorCode = postit.ColorCode;

            await _context.Postit.AddAsync(newNote);
            await _context.SaveChangesAsync();

            return newNote;
        }

        public async Task<Postit> CreatePostit(int boardId)
        {
            WhiteboardItem board = await _context.Whiteboard.FirstOrDefaultAsync(x => x.Id == boardId);

            if (board == null) {
                return null;
            }

            Postit note = new Postit();
            
            var random = new Random();
            note.WhiteboardId = boardId;
            note.Width = 200;
            note.Height = 166;
            note.PosX = random.Next(0, 100);
            note.PosY = random.Next(0, 100);
            note.IsCollapsed = false;

            int r = random.Next(150,255);
            int g = random.Next(150,255);
            int b = random.Next(150,255);
            note.ColorCode = "rgba(" + r + "," + g + "," + b + ", 1)";

            await _context.Postit.AddAsync(note);
            await _context.SaveChangesAsync();

            return note;
        }

        public async Task<Postit> DeletePostit(int boardId, int postitId)
        {
            Postit note = await _context.Postit.FirstOrDefaultAsync(x => x.Id == postitId);
            if (note == null) {
                return null;
            }

            _context.Postit.Remove(note);

            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<List<Postit>> GetPostits(int boardId)
        {
            List<Postit> noteList = await _context.Postit.ToListAsync();

            if (noteList == null) {
                return null;
            }

            return noteList.Where(x => x.WhiteboardId == boardId).ToList();
        }

        public async Task<Postit> UpdatePostit(Postit postit)
        {
            // List<Postit> notes = await _context.Postit.ToListAsync();
            // if (notes == null) {
            //     return null;
            // }

            Postit note = await _context.Postit.FirstOrDefaultAsync(x => x.Id == postit.Id);
            note.Content = postit.Content;
            note.PosX = postit.PosX;
            note.PosY = postit.PosY;
            note.Width = postit.Width;
            note.Height = postit.Height;
            note.IsCollapsed = postit.IsCollapsed;
            note.WhiteboardId = postit.WhiteboardId;

            await _context.SaveChangesAsync();

            return note;
        }
    }
}