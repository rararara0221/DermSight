using DermSight.Models;
using DermSight.Service;

namespace DermSight.ViewModels
{
    public class RecordViewModel
    {
        public int UserId { get; set; }
        public int DiseaseId { get; set; }
        public bool isCorrect { get; set; }
        public Forpaging Forpaging{ get; set; }
        public List<RecordData> RecordList { get; set; }
    }
    public class RecordData{
        public DiseaseRecord Record { get; set; }
        public RecordPhoto RecordPhoto { get; set; }
    }
}