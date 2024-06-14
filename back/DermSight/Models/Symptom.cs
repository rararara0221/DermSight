
namespace DermSight.Models
{
    public class Symptom
    {
        public int SymptomId { get; set; }
        public int DiseaseId { get; set; }
        public required string Content { get; set; } // nvarchar(MAX)
        
    }
}