using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DermSight.Parameter
{
    public class ClinicInsert
    {
        [DisplayName("名稱")]
        [Required(ErrorMessage = "請輸入名稱")]
        public required string Name { get; set; }
        [DisplayName("城市")]
        [Required(ErrorMessage = "請選擇城市")]
        public int CityId { get; set; }
        [DisplayName("地址")]
        [Required(ErrorMessage = "請輸入地址")]
        public required string Address { get; set; }
    }
}