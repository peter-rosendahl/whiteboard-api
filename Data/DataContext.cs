using Whiteboard.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Whiteboard.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<User> User { get; set; }
        public DbSet<WhiteboardItem> Whiteboard { get; set; }
        public DbSet<Postit> Postit { get; set; }
    }
}