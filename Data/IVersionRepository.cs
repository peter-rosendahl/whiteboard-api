using System.Collections.Generic;
using System.Threading.Tasks;
using Whiteboard.API.Models;

namespace Whiteboard.API.Data
{
    public interface IVersionRepository
    {
        Task<DbVersion> GetVersion();
    }
}