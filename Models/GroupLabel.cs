namespace Whiteboard.API.Models
{
    public class GroupLabel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }
        public int WhiteboardId { get; set; }
        public WhiteboardItem Whiteboard { get; set; }
    }
}