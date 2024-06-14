
namespace DermSight.Models
{
    public class RecordPhoto
    {
        public int RecordPhotoId { get; set; }
        public int RecordId { get; set; }
        public required string Route { get; set; } // nvarchar(MAX)
        
    }
}