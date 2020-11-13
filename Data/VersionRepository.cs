using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Whiteboard.API.Models;

namespace Whiteboard.API.Data
{
    public class VersionRepository : IVersionRepository
    {
        private readonly DataContext _context;

        public VersionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<DbVersion> GetVersion()
        {
            List<DbVersion> versions = await _context.DbVersion.ToListAsync();
            
            if (versions.Count == 0)
            {
                return null;
            }

            return versions.Last();
        }
    }
}