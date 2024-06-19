using DermSight.Models;
using Dapper;
using System.Data.SqlClient;
using DermSight.Service;
using DermSight.ViewModels;
using DermSight.Parameter;

namespace DermSight.Services
{
    public class NewsService(IConfiguration configuration)
    {
        //資料庫連線字串
        private readonly string? cnstr = configuration.GetConnectionString("ConnectionStrings");

        // 列表
        public List<News> GetAllNewsList(NewsViewModel newsViewModel)
        {
            List<News> Data;
            //判斷是否有增加搜尋值
            if(string.IsNullOrEmpty(newsViewModel.Search)){
                SetMaxPage(newsViewModel.Forpaging);
                Data = GetNewsList(newsViewModel.Forpaging);
            }
            else{
                SetMaxPage(newsViewModel.Search,newsViewModel.Forpaging);
                Data = GetNewsList(newsViewModel.Search,newsViewModel.Forpaging);
            }
            return Data;
        }

        private List<News> GetNewsList(Forpaging forpaging)
        {
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.isPin DESC, n.newsId DESC) r_num,* FROM [News] n
                                WHERE isDelete = 0
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.NewsItem + 1} AND {forpaging.NowPage * forpaging.NewsItem }";
            using var conn = new SqlConnection(cnstr);
            List<News> data = new(conn.Query<News>(sql));
            return (List<News>)conn.Query<News>(sql);
        }

        private void SetMaxPage(Forpaging Forpaging)
        {
            string sql = $@"
                            SELECT COUNT(*) FROM [News]
                            WHERE isDelete = 0
                        ";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql);
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.NewsItem));
            Forpaging.SetRightPage();
        }
        
        private List<News> GetNewsList(string Search,Forpaging forpaging)
        {
            string sql = $@"SELECT * FROM (
                                SELECT ROW_NUMBER() OVER(ORDER BY n.isPin, n.newsId DESC) r_num,* FROM [News] n
                                WHERE title LIKE '%{Search}%' OR content LIKE '%{Search}%' AND isDelete = 0
                            )a
                            WHERE a.r_num BETWEEN {(forpaging.NowPage - 1) * forpaging.NewsItem + 1} AND {forpaging.NowPage * forpaging.NewsItem }";
            using var conn = new SqlConnection(cnstr);
            List<News> data = new(conn.Query<News>(sql));
            return (List<News>)conn.Query<News>(sql);
        }

        private void SetMaxPage(string Search, Forpaging Forpaging)
        {
            string sql = $@"SELECT COUNT(*) FROM [News] WHERE title LIKE '%{Search}%' OR content LIKE '%{Search}%' AND isDelete = 0";
            using var conn = new SqlConnection(cnstr);
            int row = conn.QueryFirst<int>(sql);
            Forpaging.MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(row) / Forpaging.NewsItem));
            Forpaging.SetRightPage();
        }


        // 查詢資料
        public News Get(int id)
        {
            var sql = $@"SELECT * FROM [News] WHERE newsId = {id} AND isDelete = 0";
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirstOrDefault<News>(sql);
        }
        // 新增資料
        public int Create(News Data)
        {
            var sql = $@" 
                        INSERT INTO News(userId,title,type,content,isPin)
                        VALUES(@UserId,@Title,@Type,@Content,@isPin)
                        DECLARE @newsId INT = SCOPE_IDENTITY() /*自動擷取剛剛新增資料的id*/
                        SELECT @newsId
                        "; 
            using var conn = new SqlConnection(cnstr);
            return conn.QueryFirst<int>(sql, new{Data.UserId, Data.Title, Data.Type, Data.Content, Data.isPin});
        }
        // 修改資料
        public void Update(News Data)
        {
            var sql = $@"UPDATE News SET title = @Title ,content = @Content ,isPin = @isPin WHERE newsId = @Newsid";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql, Data); // new { Data.NewsId, Data.Title, Data.Content ,Data.Pin}
        }
        // 刪除資料
        public void Delete(int Newsid)
        {
            var sql = $@"UPDATE News SET isDelete = 1 WHERE newsId = @Newsid ;";
            using var conn = new SqlConnection(cnstr);
            conn.Execute(sql, new{Newsid});
        }
    }
}
