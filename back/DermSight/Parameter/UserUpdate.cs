using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DermSight.Parameter
{
    public class UserUpdate
    {
        public required string Name { get; set; }
        public IFormFile? file{ get; set; }
        public string? Photo ;
        public int userId ;
    }
}