using DermSight.Models;
using DermSight.Service;

namespace DermSight.ViewModels
{
    public class RecordViewModel
    {
        public int DiseaseId { get; set; }
        public int isCorrect { get; set; }
        public Forpaging Forpaging{ get; set; }
        public List<DiseaseRecord> RecordList { get; set; }
    }
}