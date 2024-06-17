using Microsoft.AspNetCore.Mvc;
using DermSight.Models;
using DermSight.Services;
using DermSight.Parameter;
using DermSight.Service;
using DermSight.ViewModels;

namespace DermSight.Controller
{
    [Route("DermSight/[controller]")]
    public class UserController(IWebHostEnvironment _evn,UserService _userService,MailService _mailservice, JwtHelpers _jwtHelpers,Forpaging _forpaging,RoleService _roleservice) : ControllerBase
    {
        #region 呼叫函式
        readonly UserService UserService = _userService;
        readonly MailService MailService = _mailservice;
        readonly JwtHelpers JwtHelpers = _jwtHelpers;
        readonly Forpaging Forpaging = _forpaging;
        readonly RoleService RoleService = _roleservice;
        readonly IWebHostEnvironment evn = _evn;

        #endregion

        #region 註冊
        // 註冊 
        [HttpPost("[Action]")]
        public IActionResult Register([FromForm]UserRegister RegisterData)
        {
            if (ModelState.IsValid)
            {
                var validatestr = UserService.RegisterCheck(RegisterData.Account,RegisterData.Mail);
                if (!string.IsNullOrEmpty(validatestr)){
                    return Ok(new Response(){
                        status_code = 400,
                        message = validatestr
                    });
                }
                var wwwroot = @"..\..\back\DermSight\wwwroot\images\User\";
                User User = new()
                {
                    Name = RegisterData.Name,
                    Account = RegisterData.Account,
                    Mail = RegisterData.Mail,
                    Password = UserService.HashPassword(RegisterData.Password),
                    AuthCode = MailService.GenerateAuthCode()
                };
                if(RegisterData.Photo != null ){
                    var imgname = RegisterData.Account + ".jpg";
                    var img_path = wwwroot + imgname;
                    using var stream = System.IO.File.Create(img_path);
                    RegisterData.Photo.CopyTo(stream);
                    User.Photo = img_path;
                }
                else{
                    User.Photo = wwwroot + "default.jpg";
                }

                var path = Directory.GetCurrentDirectory() + "/Verificationletter/RegisterTempMail.html";
                string TempMail = System.IO.File.ReadAllText(path);

                var querystr =  $@"Account={User.Account}&AuthCode={User.AuthCode}";

                var request = HttpContext.Request;
                UriBuilder ValidateUrl = new()
                {
                    Scheme = request.Scheme, // 使用請求的協議 (http/https)
                    Host = request.Host.Host, // 使用請求的主機名
                    Port = request.Host.Port ?? 80, // 使用請求的端口，如果未指定則默認使用80
                    Path = "/DermSight/User/MailValidate?" + querystr
                };
                string finalUrl = ValidateUrl.ToString().Replace("%3F","?");

                string MailBody = MailService.GetMailBody(TempMail, User.Name, finalUrl);
                MailService.SendMail(MailBody, User.Mail);
                string str = "寄信成功，請收信。";
                
                UserService.Register(User);
                return Ok(new Response(){
                                    status_code = 200,
                                    message = str
                                });
            }
            else{
                var errors = ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();
                return BadRequest(new {
                                        status_code = 400,
                                        message = errors
                                    });
            }
        }

        // 郵件驗證
        [HttpGet("[Action]")]
        public IActionResult MailValidate(string Account,string AuthCode)
        {
            // if(User.Identity == null || User.Identity.Name == null) return BadRequest(new Response{status_code = 400,message = "請先登入"});
            if (UserService.MailValidate(Account, AuthCode))
                return Ok(new Response(){
                    message = "已驗證成功",
                    status_code = 200,
                });
            else
                return BadRequest(new Response(){
                    message = "請重新確認或重新註冊",
                    status_code = 400,
                });
        }
        #endregion
        #region 登入
        // 登入
        [HttpPost("[Action]")]
        public IActionResult Login([FromForm]UserLogin User)
        {
            string ValidateStr = UserService.LoginCheck(User.Account, User.Password);
            if (!string.IsNullOrWhiteSpace(ValidateStr) || UserService.GetDataByAccount(User.Account) == null){
                Response result = new(){
                    message = ValidateStr,
                    status_code = 400
                };
                return BadRequest(result);
            }
            else
            {
                int Role = UserService.GetRole(User.Account);
                var jwt = JwtHelpers.GenerateToken(User.Account,Role);
                Response result = new(){ 
                    message = "登入成功",
                    status_code = 200,
                    data = jwt 
                };
                return Ok(result);
            }
        }
        #endregion
        #region 修改個人資料
        //修改個人資料
        [HttpPut]
        [Route("")]
        public IActionResult UpdateUserData(UserUpdate Data){
            try{
                if(User.Identity == null || User.Identity.Name == null){
                    return BadRequest(new Response(){
                        status_code = 400,
                        message = "請先登入"
                    });
                }
                UserUpdate user = new(){
                    userId = UserService.GetDataByAccount(User.Identity.Name).userId,
                    Name = Data.Name
                };
                //處理圖片
                var wwwroot = @"..\..\back\DermSight\wwwroot\images\User\";
                if(Data.file != null){
                    var imgname = User.Identity.Name + ".jpg";
                    var img_path = wwwroot + imgname;
                    using var stream = System.IO.File.Create(img_path);
                    Data.file.CopyTo(stream);
                    user.Photo = img_path;
                }
                else{
                    user.Photo = wwwroot + "default.jpg";
                }
                UserService.UpdateUserData(user);
                var userData = UserService.GetDataByAccount(User.Identity.Name);
                userData.Password = string.Empty;
                Response result = new(){
                    status_code = 200,
                    message = "修改成功",
                    data = userData
                };
                return Ok(result);
            }
            catch(Exception ex){
                Response result = new(){
                    status_code = 400,
                    message = ex.Message
                };
                return Ok(result);
            }
        }
        #endregion
        #region 後台管理者
        //取得目前所有使用者
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("AllUser")]
        public IActionResult AllUser([FromQuery]string? Search,[FromQuery]int page = 1){
            if(User.Identity == null || User.Identity.Name == null) return BadRequest(new Response{status_code = 400,message = "請先登入"});
            UserViewModel data = new(){
                Forpaging = new Forpaging(page)
            };
            data.User = UserService.GetAllMemberList(Search,data.Forpaging);
            data.Search = Search;
            return Ok(new Response(){
                status_code = 200,
                message = "讀取成功",
                data = data
            });
        }
        [HttpGet]
        [Route("MySelf")]
        public IActionResult MySelf(){
            try{
                if(User.Identity == null || User.Identity.Name == null) return BadRequest(new Response{status_code = 400,message = "請先登入"});
                User data = UserService.GetDataByAccount(User.Identity.Name);
                data.Password = string.Empty;
                return Ok(new Response(){
                    status_code = 200,
                    message = "讀取成功",
                    data = data
                });
            }
            catch (Exception e){
                return BadRequest(new Response(){
                    status_code = 400,
                    message = e.Message
                });
            }
        }
        // 取得單一使用者(帳號)
        // 未來可加任課科目&上課科目
        // [HttpGet]
        // public IActionResult UserByAcc([FromQuery]string account){
        //     try{
        //         User data = UserService.GetDataByAccount(account);
        //         data.Password = string.Empty;
        //         return Ok(new Response(){
        //             status_code = 200,
        //             data = data
        //         });
        //     }
        //     catch (Exception e){
        //         return BadRequest(new Response(){
        //             status_code = 400,
        //             message = e.Message
        //         });
        //     }
        // }
        #endregion

        #region 忘記密碼
        // 輸入Email後寄驗證信
        [HttpPost]
        [Route("[Action]")]
        public IActionResult ForgetPassword([FromForm]ForgetPassword Email)
        {
            // 看有沒有Email的資料
            User Data = UserService.GetDataByMail(Email.Mail);
            // 有就寄驗證信，沒有則回傳「查無用戶」
            if (Data != null)
            {
                Data.AuthCode = MailService.GenerateAuthCode();
                // 製作AuthCode
                UserService.ChangeAuthCode(Data.AuthCode, Email.Mail);
                // 寄驗證信
                var path = Directory.GetCurrentDirectory() + "/Verificationletter/ForgetPasswordTempMail.html";
                string TempMail = System.IO.File.ReadAllText(path);
                string MailBody = MailService.GetMailBody(TempMail, Data.Name, Data.AuthCode);
                MailService.SendForgetMail(MailBody, Email.Mail);
                string str = "寄信成功，請收信。";
                return Ok(new Response(){
                    status_code = 200,
                    message = str
                });
            }
            else
                return BadRequest(new Response(){
                    status_code = 400,
                    message = "查無此戶"
                });
        }

        // 檢查驗證碼
        [HttpPost]
        [Route("[Action]")]
        public IActionResult CheckForgetPasswordCode([FromForm]CheckForgetPasswordAuthCode Data)
        {
            // 取得此Email的會員資訊
            User user = UserService.GetDataByMail(Data.Mail);
            // 判斷驗證碼是否正確
            if (user.AuthCode == Data.AuthCode)
            {
                RoleService.SetMemberRole_ForgetPassword(user.userId);
                int Role = UserService.GetRole(user.Account);
                var jwt = JwtHelpers.GenerateToken(user.Account, Role);
                
                // 回傳成功
                return Ok(new Response()
                {
                    message = "驗證成功",
                    status_code = 200,
                    data = jwt
                });
            }
            else
            {
                // 回傳失敗
                return BadRequest(new Response()
                {
                    message = "驗證碼錯誤",
                    status_code = 400
                });
            }
        }

        // 修改密碼
        [HttpPost]
        [Route("ChangePasswordByForget")]
        // [Authorize(Roles = "ForgetPassword")]
        public IActionResult ChangePassword([FromForm]CheckForgetPassword Data)
        {
            // 取得此Email的會員資訊
            if (User.IsInRole("ForgetPassword"))
            {
                User user = UserService.GetDataByMail(Data.Mail);
                if(User.Identity == null || User.Identity.Name != user.Account || user == null)
                    return BadRequest(new Response(){
                        status_code = 400,
                        message = "電子郵件不符，請重新輸入"
                    });

                UserService.ClearAuthCode(Data.Mail);
                UserService.ChangePasswordByForget(Data);
                return Ok(new Response(){
                    status_code = 200,  
                    message = "修改密碼成功！請再次登入！"
                });
            }
            else
            {
                // 用戶未獲得足夠的權限
                return BadRequest(new Response(){
                    status_code = 400,
                    message = "您無權執行此操作。"
                });
            }

        }
        #endregion

    }
}