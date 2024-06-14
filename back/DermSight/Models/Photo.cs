
namespace DermSight.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public int DiseaseId { get; set; }
        public required string Route { get; set; } // nvarchar(MAX)
        
    }
}