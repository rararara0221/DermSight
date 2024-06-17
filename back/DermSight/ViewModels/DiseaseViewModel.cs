using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DermSight.Models;
using DermSight.Service;

namespace DermSight.ViewModels
{
    public class DiseaseViewModel
    {
        public string Search { get; set; }
        public Forpaging Forpaging{ get; set; }
        public List<Disease> DiseaseList { get; set; }
    }
    public class DiseaseSymptom{
        public Disease Disease{ get; set; }
        public List<Symptom> Symptoms { get; set; }
        public required string Route { get; set; }
    }
}