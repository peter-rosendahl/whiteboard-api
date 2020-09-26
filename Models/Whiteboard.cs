using System.Collections.Generic;
using Whiteboard.API.Models;

namespace Whiteboard.API.Models
{
    public class WhiteboardItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int TabOrderIndex { get; set; }
        public ICollection<Postit> Postits { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}