using Microsoft.AspNetCore.Mvc;
using DermSight.Models;
using DermSight.Services;
using DermSight.Parameter;
using DermSight.Service;
using DermSight.ViewModels;

namespace DermSight.Controller
{
    [Route("DermSight/[controller]")]
    public class NewsController(NewsService _NewsService,UserService _UserService) : ControllerBase
    {

        public NewsService NewsService = _NewsService;
        public UserService UserService = _UserService;

        #region 取得最新消息列表
        [HttpGet]
        [Route("AllNews")]
        public IActionResult GetAllNewsList([FromQuery]string? Search,[FromQuery]int page = 1){
            try
            {
                NewsViewModel data = new()
                {
                    Forpaging = new Forpaging(page),
                    Search = string.IsNullOrEmpty(Search) ? "" : Search
                };
                data.NewsList = NewsService.GetAllNewsList(data);
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

        #region 取得最新消息(單一)
        [HttpGet]
        [Route("")]
        public IActionResult GetNews([FromQuery]int NewsId){
            try
            {
                // if(User.Identity == null || User.Identity.Name == null){
                //     return BadRequest(new Response(){
                //         status_code = 400,
                //         message = "請先登入"
                //     });
                // }
                News Data = NewsService.Get(NewsId);
                if(Data == null){
                    return Ok(new Response(){
                        status_code = 204,
                        message = "無該最新消息或以刪除"
                    });
                }
                return Ok(new Response(){
                    status_code = 200,
                    message = "讀取成功",
                    data = Data
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

        #region 新增最新消息
        [HttpPost]
        [Route("")]
        public IActionResult InsertNews([FromForm]NewsInsert Data){
            try{
                if(ModelState.IsValid){
                    // if(User.Identity == null || User.Identity.Name == null){
                    //     return BadRequest(new Response(){
                    //         status_code = 400,
                    //         message = "請先登入"
                    //     });
                    // }
                    // else if(!User.IsInRole("Admin")){
                    //     return BadRequest(new Response{
                    //         status_code = 400,
                    //         message = "權限不足"
                    //     });
                    // }
                    int userId = UserService.GetDataByAccount(User.Identity.Name).userId;
                    News news = new(){
                        UserId = userId,
                        Title = Data.Title,
                        Type = Data.Type,
                        Content = Data.Content,
                        isPin = Data.isPin
                    };
                    news.NewsId = NewsService.Create(news);
                    news = NewsService.Get(news.NewsId);
                    return Ok(new Response{
                        status_code = 200,
                        message = "新增成功",
                        data = news
                    });
                }
                else{
                    var errors = ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .ToList();
                    return BadRequest(new {
                                            status_code = 400,
                                            message = errors,
                                            data = Data
                                        });
                }
            }
            catch (Exception e){
                return BadRequest(new Response{
                    status_code = 400,
                    message = e.Message,
                    data = Data
                });
            }
        }
        #endregion

        #region 修改最新消息
        [HttpPut]
        [Route("")]
        public IActionResult UpdateNews([FromBody]NewsUpdate Data){
            try
            {
                if(ModelState.IsValid){
                    if(User.Identity == null || User.Identity.Name == null){
                        return BadRequest(new Response(){
                            status_code = 400,
                            message = "請先登入"
                        });
                    }
                    else if(!User.IsInRole("Admin")){
                        return BadRequest(new Response{
                            status_code = 400,
                            message = "權限不足"
                        });
                    }
                    if(NewsService.Get(Data.NewsId) == null){
                        return BadRequest(new Response(){
                            status_code = 400,
                            message = "無該最新消息或以刪除"
                        });
                    }
                    int userId = UserService.GetDataByAccount(User.Identity.Name).userId;
                    News news = new(){
                        NewsId = Data.NewsId,
                        Type = Data.Type,
                        Title = Data.Title,
                        Content = Data.Content,
                        isPin = Data.isPin
                    };
                    NewsService.Update(news);
                    news = NewsService.Get(news.NewsId);
                    return Ok(new Response{
                        status_code = 200,
                        message = "修改成功",
                        data = news
                    });
                }
                else{
                    var errors = ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage)
                                    .ToList();
                    return BadRequest(new {
                                            status_code = 400,
                                            message = errors,
                                            data = Data
                                        });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new Response{
                    status_code = 400,
                    message = e.Message,
                    data = Data
                });
            }
        }
        #endregion

        #region 刪除最新消息
        [HttpDelete]
        [Route("")]
        public IActionResult DeleteNews([FromQuery]int NewsId){
            try
            {
                if(User.Identity == null || User.Identity.Name == null){
                    return BadRequest(new Response{
                        status_code = 400,
                        message = "請先登入"
                    });
                }
                else if(!User.IsInRole("Admin")){
                    return BadRequest(new Response{
                        status_code = 400,
                        message = "權限不足"
                    });
                }
                if(NewsService.Get(NewsId) == null){
                    return BadRequest(new Response(){
                        status_code = 400,
                        message = "無該最新消息或以刪除"
                    });
                }
                NewsService.Delete(NewsId);
                return Ok(new Response{
                    status_code = 200,
                    message = "成功刪除"
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
    }
}