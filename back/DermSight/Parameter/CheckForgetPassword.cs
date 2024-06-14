using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DermSight.Parameter
{
    public class CheckForgetPassword{
        // 新密碼
        [DisplayName("新密碼")]
        [Required(ErrorMessage = "請輸入新密碼")]
        public required string NewPassword { get; set; }
        // 確認新密碼
        [DisplayName("確認新密碼")]
        [Required(ErrorMessage = "請輸入確認新密碼")]
        [Compare("NewPassword", ErrorMessage = "兩個密碼不一致")]
        public required string CheckNewPassword { get; set; }
        // Email
        public required string Mail { get; set; }
    }
}