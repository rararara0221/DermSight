using Microsoft.AspNetCore.Mvc;
using DermSight.Models;
using DermSight.Services;
using DermSight.Parameter;
using DermSight.Service;
using DermSight.ViewModels;

namespace DermSight.Controller
{
    [Route("DermSight/[controller]")]
    public class IdentificationController(IConfiguration _configuration,IWebHostEnvironment _evn,RecordService _RecordService,DiseaseService _DiseaseService,UserService _UserService) : ControllerBase
    {
        public readonly string wwwrootRoute = _configuration.GetValue<string>("FrontRoute:Route");
        public RecordService RecordService = _RecordService;
        public DiseaseService DiseaseService = _DiseaseService;
        public UserService UserService = _UserService;
        readonly IWebHostEnvironment evn = _evn;
        public class ModelResponse{
            public int UserId { get; set; }
            public bool isCorrect { get; set; }
            public int DiseaseId { get; set; }
            public required string PhotoRoute { get; set; }
        };
        #region 使用者辨識
        [HttpPost]
        [Route("")]
        public IActionResult Identification([FromBody]int DiseaseId){
            try
            {
                if(User.Identity == null || User.Identity.Name == null){
                    return BadRequest(new Response{
                        status_code = 400,
                        message = "請先登入"
                    });
                }
                User user = UserService.GetDataByAccount(User.Identity.Name);

                // (模擬)模型回傳結果
                ModelResponse modelResponse = new(){
                    UserId = user.userId,
                    DiseaseId = DiseaseId,
                    isCorrect = DiseaseId > 0,
                    PhotoRoute = "Record/default.jpg" // user.userId + ".jpg";
                };
                int recordId = RecordService.Insert(modelResponse);

                // 日後呼叫辨識模型處理後
                // 儲存結果並回傳結果
                // var response = IdentificationModel( user.userId, Photo );
                // string Route;
                // int RecordId;
                // if(response){
                //     RecordId = DiseaseService.SaveIdentificationPhoto(response.Photo); // return RecordId
                //     var wwwroot = evn.ContentRootPath + @"\wwwroot\images\Record\" + user.userId;
                //     var imgname = RecordId + ".jpg";
                //     var img_path = wwwroot + imgname;
                //     using var stream = System.IO.File.Create(img_path);
                //     response.Photo.CopyTo(stream);
                //     Route = img_path;
                // }
                // else{
                //         return BadRequest(new Response{
                //             status_code = 400,
                //             message = "錯誤訊息"
                //         });
                // }
                // return Ok(new Response{
                //     status_code = 200,
                //     message = "辨識完成",
                //     data = DiseaseService.GetRecord(user.userId,RecordId)
                // });
                return Ok(new Response{
                    status_code = 200,
                    message = "辨識完成",
                    data = recordId
                });
            }
            catch (Exception e){
                return BadRequest(new Response{
                    status_code = 400,
                    message = e.Message
                });
            }
        }
        #endregion
        
        #region 獲取紀錄列表
        [HttpGet]
        [Route("AllRecord")]
        public IActionResult GetAllRecordList([FromQuery]bool isCorrect,[FromQuery]int DiseaseId = 0,[FromQuery]int page = 1){
            try
            {
                if(User.Identity == null || User.Identity.Name == null){
                    return BadRequest(new Response(){
                        status_code = 400,
                        message = "請先登入"
                    });
                }
                RecordViewModel data = new()
                {
                    UserId = UserService.GetDataByAccount(User.Identity.Name).userId,
                    Forpaging = new Forpaging(page),
                    DiseaseId = DiseaseId,
                    isCorrect = isCorrect
                };
                data.RecordList = RecordService.GetAllRecordList(data);
                return Ok(new Response(){
                    status_code = 200,
                    message = "讀取成功",
                    data = data
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response{
                    status_code = 400,
                    message = e.Message
                });
            }
        }
        #endregion

        #region 刪除紀錄
        // 刪除紀錄
        [HttpDelete]
        [Route("")]
        public IActionResult DeleteRecord([FromQuery]int RecordId){
            try
            {
                // 刪除
                if(User.Identity == null || User.Identity.Name == null){
                    return BadRequest(new Response{
                        status_code = 400,
                        message = "請先登入"
                    });
                }
                User user = UserService.GetDataByAccount(User.Identity.Name);
                RecordService.DeleteRecord(user.userId,RecordId);
                return Ok(new Response{
                    status_code = 200,
                    message = "刪除成功"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response{
                    status_code = 400,
                    message = e.Message,
                });
            }
        }
        #endregion
    }
}