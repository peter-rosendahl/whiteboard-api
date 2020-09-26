namespace Whiteboard.API.Models
{
    public class Postit
    {
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string Content { get; set; }
        public string ColorCode { get; set; }
        public int WhiteboardId { get; set; }
        public WhiteboardItem Whiteboard { get; set; }
    }
}