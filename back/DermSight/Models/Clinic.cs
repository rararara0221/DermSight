
namespace DermSight.Models
{
    public class Clinic
    {
        public int ClinicId { get; set; }
        public int CityId { get; set; }
        public required string Name { get; set; } // nvarchar(20)
        public string? Phone { get; set; } // nvarchar(12)
        public required string Address { get; set; } // nvarchar(20)
        
    }
}