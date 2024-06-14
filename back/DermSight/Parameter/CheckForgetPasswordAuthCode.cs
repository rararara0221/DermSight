using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DermSight.Parameter
{
    public class CheckForgetPasswordAuthCode
    {
        // 信箱
        public required string Mail{get;set;}
        // 驗證碼
        public required string AuthCode{get;set;}
    }
}