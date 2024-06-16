using DermSight.Models;
using DermSight.Service;

namespace DermSight.ViewModels
{
    public class ClinicViewModel
    {
        public string Search { get; set; }
        public Forpaging Forpaging{ get; set; }
        public int CityId { get; set; }
        public List<Clinic> ClinicList { get; set; }
        
    }
}