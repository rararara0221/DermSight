using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DermSight.Models;
using DermSight.Service;
using DermSight.ViewModels;

namespace DermSight.Services
{
    public class ClinicService(IConfiguration configuration)
    {
        //資料庫連線字串
        private readonly string? cnstr = configuration.GetConnectionString("ConnectionStrings");
        public List<Clinic> GetAllClinicList(ClinicViewModel ClinicViewModel)
        {
            List<Clinic> Data;
            //判斷是否有增加搜尋值
            if (string.IsNullOrEmpty(ClinicViewModel.Search))
            {
                SetMaxPage(ClinicViewModel.Forpaging, ClinicViewModel.CityId);
                Data = GetClinicList(ClinicViewModel.Forpaging, ClinicViewModel.CityId);
            }
            else
            {
                SetMaxPage(ClinicViewModel.Search, ClinicViewModel.Forpaging, ClinicViewModel.CityId);
                Data = GetClinicList(ClinicViewModel.Search, ClinicViewModel.Forpaging, ClinicViewModel.CityId);
            }
            return Data;
        }

        private List<Clinic> GetClinicList(Forpaging forpaging, int CityId)
        {
            string sql;
            if (CityId != 0)
            {
                sql = $@"   
                            SELECT a.clinicId,a.name,a.phone,a.cityId,c.name+a.address address FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.cityId,address,name) r_num,* FROM [Clinic] n
                                WHERE isDelete = 0 AND cityId = @CityId
                            )a
                            JOIN [City] c ON c.cityId = a.cityId
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item}
                        ";
            }
            else
            {
                sql = $@"   
                            SELECT a.clinicId,a.name,a.phone,a.cityId,c.name+a.address address FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.cityId,address,name) r_num,* FROM [Clinic] n
                                WHERE isDelete = 0
                            )a
                            LEFT JOIN [City] c ON c.cityId = a.cityId
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item}
                        ";
            }
            using var conn = new SqlConnection(cnstr);
            List<Clinic> data = conn.Query<Clinic>(sql, new { CityId }).ToList();
            return data;
        }

        private void SetMaxPage(Forpaging Forpaging, int CityId)
        {
            string sql;
            if (CityId != 0)
            {
                sql = $@"
                            SELECT COUNT(*) FROM [Clinic]
                            WHERE isDelete = 0 AND cityId = @CityId
                        ";
            }
            else
            {
                sql = $@"
                            SELECT COUNT(*) FROM [Clinic]
                            WHERE isDelete = 0
                        ";

            }
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql, new { CityId });
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.Item));
            Forpaging.SetRightPage();
        }

        private List<Clinic> GetClinicList(string Search, Forpaging forpaging, int CityId)
        {
            string sql;
            if (CityId != 0)
            {
                sql = $@"   
                            SELECT a.clinicId,a.name,a.phone,a.cityId,c.name+a.address address FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.cityId,address,name) r_num,* FROM [Clinic] n
                                WHERE n.cityId = @CityId AND name LIKE '%{Search}%' OR address LIKE '%{Search}%' AND isDelete = 0
                            )a
                            LEFT JOIN [City] c ON c.cityId = a.cityId
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item}
                        ";
            }
            else
            {
                sql = $@"   
                            SELECT a.clinicId,a.name,a.phone,a.cityId,c.name+a.address address FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.cityId,address,name) r_num,* FROM [Clinic] n
                                WHERE name LIKE '%{Search}%' OR address LIKE '%{Search}%' AND isDelete = 0
                            )a
                            LEFT JOIN [City] c ON c.cityId = a.cityId
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item}
                        ";
            }
            using var conn = new SqlConnection(cnstr);
            List<Clinic> data = conn.Query<Clinic>(sql, new { CityId }).ToList();
            return data;
        }

        private void SetMaxPage(string Search, Forpaging Forpaging, int CityId)
        {
            string sql = $@"SELECT COUNT(*) FROM [Clinic] WHERE name LIKE '%{Search}%' OR address LIKE '%{Search}%' AND isDelete = 0";
            if(CityId != 0){
                sql = $@"SELECT COUNT(*) FROM [Clinic] WHERE cityId = @CityId AND name LIKE '%{Search}%' OR address LIKE '%{Search}%' AND isDelete = 0";
            }
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql, new { CityId });
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.Item));
            Forpaging.SetRightPage();
        }

        // 查詢資料
        public Clinic Get(int ClinicId)
        {
            var sql = $@"
                            SELECT 
                                cl.clinicId,
                                cl.name,
                                cl.cityId,
                                cl.phone,
                                ci.name+cl.address address
                            FROM [Clinic] cl 
                            LEFT JOIN [City] ci ON ci.cityId = cl.cityId
                            WHERE clinicId = @ClinicId AND isDelete = 0
                        ";
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirstOrDefault<Clinic>(sql, new { ClinicId });
        }

        // 新增資料
        public int Create(Clinic Data)
        {
            var sql = $@" 
                        INSERT INTO [Clinic](name, cityId, address)
                        VALUES(@Name, @CityId, @Address);
                        DECLARE @clinicId INT = SCOPE_IDENTITY() /*自動擷取剛剛新增資料的id*/
                        SELECT @clinicId
                        ";
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirst<int>(sql, new { Data.Name, Data.CityId, Data.Address });
        }
        // 修改資料
        public void Update(Clinic Data)
        {
            var sql = $@"UPDATE [Clinic] SET name = @Name ,cityId = @CityId ,address = @Address,phone = @Phone WHERE clinicId = @Clinicid";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql, Data); // new { Data.ClinicId, Data.Title, Data.Content ,Data.Pin}
        }
        // 刪除資料
        public void Delete(int Clinicid)
        {
            var sql = $@"UPDATE [Clinic] SET isDelete = 1 WHERE clinicId = @Clinicid ;";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql, new { Clinicid });    
        }

        public List<City> GetCityList(){
            var sql = $@"
                            SELECT 
                                *
                            FROM [City]
                        ";
            using var conn = new SqlConnection(cnstr);
            List<City> data = conn.Query<City>(sql).ToList();
            return data;
        }
    }
}