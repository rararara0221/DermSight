using DermSight.Models;
using Dapper;
using System.Data.SqlClient;
using DermSight.Service;
using DermSight.ViewModels;
using DermSight.Parameter;

namespace DermSight.Services
{
    public class DiseaseService(IConfiguration configuration)
    {
        //資料庫連線字串
        private readonly string? cnstr = configuration.GetConnectionString("ConnectionStrings");

        #region 取得資料
        // 列表
        public List<Disease> GetAllDiseaseList(DiseaseViewModel DiseaseViewModel)
        {
            List<Disease> Data;
            //判斷是否有增加搜尋值
            if(string.IsNullOrEmpty(DiseaseViewModel.Search)){
                SetMaxPage(DiseaseViewModel.Forpaging);
                Data = GetDiseaseList(DiseaseViewModel.Forpaging);
            }
            else{
                SetMaxPage(DiseaseViewModel.Search,DiseaseViewModel.Forpaging);
                Data = GetDiseaseList(DiseaseViewModel.Search,DiseaseViewModel.Forpaging);
            }
            return Data;
        }

        private List<Disease> GetDiseaseList(Forpaging forpaging)
        {
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.diseaseId DESC) r_num,* FROM [Disease] n
                                WHERE isDelete = 0
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item }";
            using var conn = new SqlConnection(cnstr);
            List<Disease> data = new(conn.Query<Disease>(sql));
            return (List<Disease>)conn.Query<Disease>(sql);
        }

        private void SetMaxPage(Forpaging Forpaging)
        {
            string sql = $@"
                            SELECT COUNT(*) FROM [Disease]
                            WHERE isDelete = 0
                        ";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql);
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.Item));
            Forpaging.SetRightPage();
        }
        
        private List<Disease> GetDiseaseList(string Search,Forpaging forpaging)
        {
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.diseaseId DESC) r_num,* FROM [Disease] n
                                WHERE name LIKE '%{Search}%' OR description LIKE '%{Search}%' AND isDelete = 0
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.Item + 1} AND {forpaging.NowPage * forpaging.Item }";
            using var conn = new SqlConnection(cnstr);
            List<Disease> data = new(conn.Query<Disease>(sql));
            return (List<Disease>)conn.Query<Disease>(sql);
        }

        private void SetMaxPage(string Search, Forpaging Forpaging)
        {
            string sql = $@"SELECT COUNT(*) FROM [Disease] WHERE name LIKE '%{Search}%' OR description LIKE '%{Search}%' AND isDelete = 0";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql);
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.Item));
            Forpaging.SetRightPage();
        }


        // 查詢資料
        public Disease Get(int id)
        {
            var sql = $@"SELECT * FROM [Disease] WHERE diseaseId = {id} AND isDelete = 0";
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirstOrDefault<Disease>(sql);
        }

        public DiseaseSymptom GetDiseaseSymptom(int DiseaseId)
        {
            var Dsql = $@"
                            SELECT
                                d.*,
                                s.content
                            FROM Disease d
                            JOIN Symptom s ON s.diseaseId = d.diseaseId
                            WHERE d.diseaseId = @DiseaseId AND d.isDelete = 0
                        ";
            var Ssql = $@"
                            SELECT
                                s.*
                            FROM Disease d
                            JOIN Symptom s ON s.diseaseId = d.diseaseId
                            WHERE d.diseaseId = @DiseaseId AND d.isDelete = 0
                        ";
            var Psql = $@"
                            SELECT
                                route
                            FROM [Photo] p
                            JOIN [Disease] d ON d.diseaseId = p.diseaseId
                            WHERE p.diseaseId = @DiseaseId AND d.isDelete = 0
                        ";
            using var conn = new SqlConnection(cnstr);
            DiseaseSymptom data = new(){
                Disease = conn.QueryFirstOrDefault<Disease>(Dsql, new{ DiseaseId }),
                Symptoms = conn.Query<Symptom>(Ssql, new{ DiseaseId }).ToList(),
                Route = conn.QueryFirstOrDefault<string>(Psql, new{ DiseaseId })
            };
            return data;
        }
        #endregion
        #region 新增資料
        // 新增資料
        public int Create(Disease Data,List<string> Symptoms)
        {
            var sql = $@" 
                            INSERT INTO Disease(name,description)
                            VALUES(@Name,@Description)
                            DECLARE @DiseaseId INT = SCOPE_IDENTITY() /*自動擷取剛剛新增資料的id*/
                            SELECT @DiseaseId
                        ";
            foreach(var symptom in Symptoms){
                sql += $@"
                            INSERT INTO Symptom(diseaseId,content)
                            VALUES(@DiseaseId,'{symptom}')
                        ";
            }
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirst<int>(sql, new{Data.Name,Data.Description});
        }
        public void CreatePhoto(int DiseaseId,string Route){
            var sql = $@"
                            INSERT INTO [Photo](diseaseId,route)
                            VALUES(@DiseaseId,@Route)
                    ";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql,new{DiseaseId,Route});
        }
        #endregion
        #region 修改資料
        // 修改資料
        public void Update(Disease Disease,List<string> newSymptoms)
        {
            var sql = $@"UPDATE Disease SET name = @Name ,description = @Description WHERE diseaseId = @Diseaseid";
            using var conn = new SqlConnection(cnstr);
            List<Symptom> data = GetDiseaseSymptom(Disease.DiseaseId).Symptoms;
            List<string> oldSymptoms = data.Select( s => s.Content).ToList();
            var sToAdd = newSymptoms.Except(oldSymptoms).ToList();
            var sToDelete = oldSymptoms.Except(newSymptoms).ToList();
            // 刪除題目
            if(sToDelete.Count > 0){
                foreach (var content in sToDelete){
                    conn.Execute(
                        $@" DELETE FROM Symptom
                            WHERE diseaseId = @DiseaseId AND content = @content
                        ",
                        new { Disease.DiseaseId, content }
                    );
                }
            }
            // 新增題目
            if(sToAdd.Count > 0){
                foreach (var content in sToAdd){
                    conn.Execute(
                        $@" INSERT INTO Symptom(diseaseId,content)
                            VALUES(@DiseaseId,@content)
                        ",
                        new { Disease.DiseaseId, content }
                    );
                }
            }
            conn.Execute(sql, Disease); // new { Data.disease, Data.name, Data.Content ,Data.Pin}
        }
        public void UpdatePhoto(int DiseaseId,string Route){
            var sql = $@"
                            UPDATE [Photo] SET route = @Route WHERE diseaseId = @DiseaseId
                    ";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql,new{DiseaseId,Route});
        }
        #endregion
        #region 刪除資料
        // 刪除資料
        public void Delete(int Diseaseid)
        {
            var sql = $@"UPDATE Disease SET isDelete = 1 WHERE diseaseId = @Diseaseid ;";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql, new{Diseaseid});
        }
        #endregion
        
    }
}
