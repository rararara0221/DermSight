using System.Data.SqlClient;
using DermSight.Models;
using DermSight.Service;
using DermSight.ViewModels;
using Dapper;

namespace DermSight.Services
{
    public class RecordService(IConfiguration configuration)
    {
        //資料庫連線字串
        private readonly string? cnstr = configuration.GetConnectionString("ConnectionStrings");

        #region 使用者辨識
        public int SaveIdentificationPhoto(int UserId,string Route){
            // return RecordId;
            return 1;
        }
        public IdentificationViewModel GetRecord(int UserId,int RecordId){
            return new();
        }
        #endregion
        #region 獲取記錄列表
        // 列表
        public List<DiseaseRecord> GetAllRecordList(RecordViewModel RecordViewModel)
        {
            List<DiseaseRecord> Data;
            //判斷是否有增加搜尋值
            if(RecordViewModel.DiseaseId == 0){
                SetMaxPage(RecordViewModel.Forpaging);
                Data = GetDiseaseRecordList(RecordViewModel.Forpaging);
            }
            else{
                SetMaxPage(RecordViewModel.DiseaseId,RecordViewModel.Forpaging);
                Data = GetDiseaseRecordList(RecordViewModel.DiseaseId,RecordViewModel.Forpaging);
            }
            return Data;
        }
        
        private List<DiseaseRecord> GetDiseaseRecordList(Forpaging forpaging)
        {
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.DiseaseRecordId DESC) r_num,* FROM [DiseaseRecord] n
                                WHERE isDelete = 0
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item }";
            using var conn = new SqlConnection(cnstr);
            List<DiseaseRecord> data = new(conn.Query<DiseaseRecord>(sql));
            return data;
        }

        private void SetMaxPage(Forpaging Forpaging)
        {
            string sql = $@"
                            SELECT COUNT(*) FROM [DiseaseRecord]
                            WHERE isDelete = 0
                        ";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql);
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.Item));
            Forpaging.SetRightPage();
        }
        
        private List<DiseaseRecord> GetDiseaseRecordList(int DiseaseId,Forpaging forpaging)
        {
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.DiseaseRecordId DESC) r_num,* FROM [DiseaseRecord] n
                                WHERE diseaseId = @DiseaseId AND isDelete = 0
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item }";
            using var conn = new SqlConnection(cnstr);
            List<DiseaseRecord> data = new(conn.Query<DiseaseRecord>(sql, new{ DiseaseId }));
            return data;
        }

        private void SetMaxPage(int DiseaseId, Forpaging Forpaging)
        {
            string sql = $@"SELECT COUNT(*) FROM [DiseaseRecord] WHERE diseaseId = @DiseaseId AND isDelete = 0";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql, new{ DiseaseId });
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.Item));
            Forpaging.SetRightPage();
        }


        // 查詢資料
        public DiseaseRecord Get(int id)
        {
            var sql = $@"SELECT * FROM [DiseaseRecord] WHERE DiseaseRecordId = {id} AND isDelete = 0";
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirstOrDefault<DiseaseRecord>(sql);
        }
        #endregion
    }
}