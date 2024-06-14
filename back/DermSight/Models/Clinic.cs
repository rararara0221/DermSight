
namespace DermSight.Models
{
    public class Clinic
    {
        public int ClinicId { get; set; }
        public required string Name { get; set; } // nvarchar(20)
        public required string Photo { get; set; } // nvarchar(20)
        
    }
}