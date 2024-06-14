
namespace DermSight.Models
{
    public class DiseaseRecord
    {
        public int RecordId { get; set; }
        public int UserId { get; set; }
        public bool IsCorrect { get; set; }
        public int DiseaseId { get; set; }
        public DateTime Time { get; set; }
        
    }
}