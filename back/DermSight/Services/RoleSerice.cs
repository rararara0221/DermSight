using DermSight.ViewModels;
using DermSight.Models;
using System.Data.SqlClient;
using Dapper;

namespace DermSight.Service
{
    public class RoleService
    {
        #region 宣告連線字串
        private readonly string? cnstr;
        public RoleService(IConfiguration configuration){
            cnstr = configuration.GetConnectionString("ConnectionStrings");
        }
        #endregion

        //修改使用者權限(帳號)
        public void UpdateMemberRole(int userId,int role){
            string sql = $@"UPDATE [User] SET role = {role} WHERE userId = {userId}";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql);
        }
        public void SetMemberRole_ForgetPassword(int userId){
            string sql = $@"UPDATE [User] SET role = 2 WHERE userId = {userId}";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql);
        }
    }
}