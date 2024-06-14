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

        [HttpGet]
        [Route("AllNews")]
        public IActionResult GetAllNewsList([FromQuery]string? Search,[FromQuery]int page = 1){
            try
            {
                if(User.Identity == null || User.Identity.Name == null){
                    return BadRequest(new Response(){
                        status_code = 400,
                        message = "請先登入"
                    });
                }
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

        [HttpPost]
        [Route("")]
        public IActionResult InsertNews([FromBody]NewsInsert Data){
            try{
                if(ModelState.IsValid){
                    if(User.Identity == null || User.Identity.Name == null){
                        return BadRequest(new Response(){
                            status_code = 400,
                            message = "請先登入"
                        });
                    }
                    int userId = UserService.GetDataByAccount(User.Identity.Name).userId;
                    News news = new(){
                        UserId = userId,
                        Title = Data.Title,
                        Type = Data.Type,
                        Content = Data.Content,
                        Pin = Data.Pin
                    };
                    news.NewsId = NewsService.Create(news);
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
    }
}