
namespace DermSight.Models
{
    public class News
    {
        public int NewsId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; } // nvarchar(10)
        public string Title { get; set; } // nvarchar(30)
        public string Content { get; set; } // nvarchar(500)
        public DateTime Time { get; set; }
        public bool Pin { get; set; }
    }
}