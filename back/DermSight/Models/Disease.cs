namespace DermSight.Models
{
    public class Disease
    {
        public int DiseaseId { get; set; }
        public required string Name { get; set; } // nvarchar(20)
        public required string Description { get; set; } // nvarchar(MAX)
        
    }
}