using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DermSight.Models;
using NPOI.HSSF.Record;

namespace DermSight.ViewModels
{
    public class IdentificationViewModel
    {
        public DiseaseRecord Record { get; set; }
        public RecordPhoto Photo { get; set; }
    }
}