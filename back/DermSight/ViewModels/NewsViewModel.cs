using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DermSight.Models;
using DermSight.Service;

namespace DermSight.ViewModels
{
    public class NewsViewModel
    {
        public string Search { get; set; }
        public Forpaging Forpaging{ get; set; }
        public List<News> NewsList { get; set; }
    }
}