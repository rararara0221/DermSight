using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using DermSight.Models;
using DermSight.Parameter;
using Dapper;

namespace DermSight.Service
{
    public class UserService(IConfiguration configuration)
    {
        #region 宣告連線字串
        private readonly string? cnstr = configuration.GetConnectionString("ConnectionStrings");
        #endregion

        #region 登入
        // 獲得權限
        public int GetRole(string Account)
        {
            string sql = $@"SELECT role FROM [User] WHERE account = @Account";
            using var conn = new SqlConnection(cnstr);
            int role_id = conn.QueryFirstOrDefault<int>(sql,new{Account});
            return role_id;
        }

        // 登入確認
        public string LoginCheck(string Account, string password)
        {
            User Data = GetDataByAccount(Account);
            if (Data != null)
            {
                if (String.IsNullOrWhiteSpace(Data.AuthCode))
                {
                    if (PasswordCheck(Data.Password, password))
                        return "";
                    else
                        return "密碼錯誤";
                }
                else
                    return "此帳號尚未經過Mail驗證";
            }
            else
                return "無此會員資料";
        }
        #endregion
        
        // 註冊
        // 密碼確認
        public bool PasswordCheck(string Data, string Password)
        {
            return Data.Equals(HashPassword(Password));
        }

        // 確認註冊
        public string RegisterCheck(string Account,string Mail){
            if(GetDataByAccount(Account)!=null)
                return "帳號已被註冊";
            else if(GetDataByMail(Mail)!=null)
                return "電子郵件已被註冊";
            return "";
        }

        // 雜湊密碼
        public string HashPassword(string Password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                string salt = "foiw03pltmvle6";
                string saltandpas = string.Concat(salt, Password);
                byte[] data = Encoding.UTF8.GetBytes(saltandpas);
                byte[] hash = sha512.ComputeHash(data);
                string result = Convert.ToBase64String(hash);
                return result;
            }
        }

        public void Register(User user)
        {
            string sql = @$"INSERT INTO [User](name,photo,account,password,mail,authcode)
                                          VALUES(@name,@photo,@account,@password,@mail,@authcode)";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql,new{ user.Name,user.Photo,user.Account,user.Password,user.Mail,user.AuthCode });
        }

        public bool MailValidate(string Account, string AuthCode)
        {
            User Data = GetDataByAccount(Account);
            //判斷有無會員資料並比對驗證碼是否正確
            if (Data != null && Data.AuthCode == AuthCode)
            {
                string sql = $@"UPDATE [User] SET authcode = '{string.Empty}' WHERE account = '{Account}'";
                using var conn = new SqlConnection(cnstr);
                conn.Execute(sql);
                return Data.AuthCode == AuthCode;
            }
            else return false;
        }

        //(後台管理者)
        //取得所有使用者
        public List<User> GetAllMemberList(string Search,Forpaging forpaging){
            List<User> Data;
            //判斷是否有增加搜尋值
            if(string.IsNullOrEmpty(Search)){
                SetMaxPage(forpaging);
                Data = GetUserList(forpaging);
            }
            else{
                SetMaxPage(Search,forpaging);
                Data = GetUserList(Search,forpaging);
            }
            return Data;
        }
        //無搜尋值查詢的使用者列表
        public List<User> GetUserList(Forpaging forpaging){
            List<User> data = [];
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY u.userId DESC) r_num,u.userId,u.account,u.name,u.mail,u.role FROM [User] u 
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item }";
            using var conn = new SqlConnection(cnstr);
            data = new List<User>(conn.Query<User>(sql));
            return data;
        }
        //有搜尋值查詢的使用者列表
        public List<User> GetUserList(string Search,Forpaging forpaging){
            List<User> data = new();
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY u.userId DESC) r_num,u.userId,u.account,u.name,u.mail,u.role FROM [User] u 
                                WHERE u.account LIKE '%{Search}%' OR u.name LIKE '%{Search}%'
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item }";
            using var conn = new SqlConnection(cnstr);
            data = new List<User>(conn.Query<User>(sql));
            return data;
        }
        //無搜尋值計算所有使用者並設定頁數
        public void SetMaxPage(Forpaging forpaging){
            string sql = $@"SELECT COUNT(*) FROM [User]";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql);
            forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / forpaging.Item));
            forpaging.SetRightPage();
        }
        //有搜尋值計算所有使用者並設定頁數
        public void SetMaxPage(string Search,Forpaging forpaging){
            string sql = $@"SELECT COUNT(*) FROM [User] WHERE account LIKE '%{Search}%' OR name LIKE '%{Search}%'";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql);
            forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / forpaging.Item));
            forpaging.SetRightPage();
        }

        #region 忘記密碼
        // 更新驗證碼
        public void ChangeAuthCode(string NewAuthCode,string Mail){
            string sql = $@" UPDATE [User] SET authcode = '{NewAuthCode}' WHERE mail = '{Mail}';";
            using (var conn = new SqlConnection(cnstr))
            conn.Execute(sql);
        }
        
        // 清除驗證碼
        public void ClearAuthCode(string Mail){
            string sql = $@" UPDATE [User] SET authcode = '{String.Empty}' WHERE mail = '{Mail}';";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql);
        }

        // 更改密碼ByForget
        public void ChangePasswordByForget(CheckForgetPassword Data){
            User user = GetDataByMail(Data.Mail);
            user.Password = HashPassword(Data.NewPassword);
            string sql = $@"UPDATE [User] SET password = @password , role = 1 WHERE mail = @mail;";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql,user);
        }
        
        // 用mail獲得資料
        public User GetDataByMail(string mail){
            string sql = $@"SELECT * FROM [User] WHERE mail = '{mail}' ";
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirstOrDefault<User>(sql);
        }
        // 用account獲得資料
        public User GetDataByAccount(string account){
            string sql = $@"SELECT 
                                u.*
                            FROM [User] u
                            WHERE u.account = @account";
            using (var conn = new SqlConnection(cnstr))
            return conn.QueryFirstOrDefault<User>(sql, new{ account });
        }
        #endregion

        #region 修改密碼
        public void ChangePassword(int member_id, string pwd){
            string password = HashPassword(pwd);
            string sql = $@" UPDATE User SET password = '{password}' WHERE userId = {member_id} ";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql);
        }
        #endregion

        #region 修改個人資訊
        //修改個人資料
        public void UpdateUserData(UserUpdate user){
            string sql = $@"UPDATE [User] SET 
                                name = @name, 
                                photo = @photo
                            WHERE userId = @userId";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql,new{user.Name,user.Photo,user.userId});
        }
        #endregion
    }
}