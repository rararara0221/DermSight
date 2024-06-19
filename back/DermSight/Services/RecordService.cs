using System.Data.SqlClient;
using DermSight.Models;
using DermSight.Service;
using DermSight.ViewModels;
using Dapper;
using static DermSight.Controller.IdentificationController;

namespace DermSight.Services
{
    public class RecordService(IConfiguration configuration)
    {
        //資料庫連線字串
        private readonly string? cnstr = configuration.GetConnectionString("ConnectionStrings");

        #region 使用者辨識
        public int Insert(ModelResponse Data){
            string sql = $@"
                            INSERT INTO [DiseaseRecord](userId,isCorrect,diseaseId)
                            VALUES(@UserId,@isCorrect,@DiseaseId)

                            DECLARE @RecordId INT = SCOPE_IDENTITY() /*自動擷取剛剛新增資料的id*/
                            INSERT INTO [RecordPhoto](recordId,route)
                            VALUES(@RecordId,@PhotoRoute)

                            SELECT @RecordId
                        ";
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirstOrDefault<int>(sql, Data);
        }

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
        public List<RecordData> GetAllRecordList(RecordViewModel RecordViewModel)
        {
            List<RecordData> Data = [];
            //判斷是否有增加搜尋值
            if(RecordViewModel.DiseaseId == 0){
                SetMaxPage(RecordViewModel.Forpaging,RecordViewModel.UserId);
                Data = GetDiseaseRecordList(RecordViewModel.Forpaging,RecordViewModel.UserId);
            }
            else{
                SetMaxPage(RecordViewModel.Forpaging,RecordViewModel.UserId,RecordViewModel.DiseaseId);
                Data = GetDiseaseRecordList(RecordViewModel.Forpaging,RecordViewModel.UserId,RecordViewModel.DiseaseId);
            }
            return Data;
        }
        
        private List<RecordData> GetDiseaseRecordList(Forpaging forpaging,int UserId)
        {
            using var conn = new SqlConnection(cnstr);
            List<RecordData> data = [];
            // data = new(conn.Query<DiseaseRecord>(sql));
            // SQL 查詢
            var rpsql = $@"
                            SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY b.recordId DESC) r_num,* FROM 
                                (
                                    SELECT
                                        r.recordId,
                                        r.diseaseId,
                                        r.userId,
                                        r.isCorrect,
                                        r.time,
                                        rp.RecordPhotoId,
                                        rp.route
                                    FROM 
                                        [DiseaseRecord] r
                                    INNER JOIN 
                                        [RecordPhoto] rp ON r.recordId = rp.recordId
                                    WHERE r.userId = @UserId AND isDelete = 0 /* AND r.DiseaseId = @DiseaseId */
                                )b
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item }
                        ";
            // 使用 Dapper 查詢資料
            var result = conn.Query<RecordData, DiseaseRecord, RecordPhoto, RecordData>(
                rpsql,
                (recordData, record, photo) =>
                {
                    recordData.Record = record;
                    recordData.RecordPhoto = photo;
                    return recordData;
                },
                new{ UserId },
                splitOn: "recordId,RecordPhotoId"
            ).ToList();
            data = result;
            return data;
        }

        private void SetMaxPage(Forpaging Forpaging,int UserId)
        {
            string sql = $@"
                            SELECT COUNT(*) FROM [DiseaseRecord]
                            WHERE isDelete = 0 AND userId = @UserId 
                        ";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql,new{ UserId  });
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.Item));
            Forpaging.SetRightPage();
        }
        
        private List<RecordData> GetDiseaseRecordList(Forpaging forpaging,int UserId,int DiseaseId)
        {
            List<RecordData> data = [];
            string sql = $@"
                            SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY b.recordId DESC) r_num,* FROM 
                                (
                                    SELECT
                                        r.recordId,
                                        r.diseaseId,
                                        r.userId,
                                        r.isCorrect,
                                        r.time,
                                        rp.RecordPhotoId,
                                        rp.route
                                    FROM 
                                        [DiseaseRecord] r
                                    INNER JOIN 
                                        [RecordPhoto] rp ON r.recordId = rp.recordId
                                    WHERE r.userId = @UserId AND r.DiseaseId = @DiseaseId AND isDelete = 0
                                )b
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item }
                        ";
            using var conn = new SqlConnection(cnstr);
            // 使用 Dapper 查詢資料
            var result = conn.Query<RecordData, DiseaseRecord, RecordPhoto, RecordData>(
                sql,
                (recordData, record, photo) =>
                {
                    recordData.Record = record;
                    recordData.RecordPhoto = photo;
                    return recordData;
                },
                new{ UserId , DiseaseId },
                splitOn: "RecordId,RecordPhotoId"
            ).ToList();
            data = result;
            return data;
        }

        private void SetMaxPage(Forpaging Forpaging,int UserId,int DiseaseId)
        {
            string sql = $@"SELECT COUNT(*) FROM [DiseaseRecord] WHERE userId = @UserId AND diseaseId = @DiseaseId AND isDelete = 0";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql, new{ UserId, DiseaseId });
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.Item));
            Forpaging.SetRightPage();
        }


        // 查詢資料
        public DiseaseRecord Get(int id)
        {
            var sql = $@"SELECT * FROM [DiseaseRecord] WHERE recordId = {id} AND isDelete = 0";
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirstOrDefault<DiseaseRecord>(sql);
        }
        #endregion

        public void DeleteRecord(int UserId , int RecordId){
            var sql = $@"UPDATE [DiseaseRecord] SET isDelete = 1 WHERE recordId = @RecordId";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql,new{ UserId, RecordId });
        }
    }
}