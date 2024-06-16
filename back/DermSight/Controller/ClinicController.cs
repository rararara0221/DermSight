using DermSight.Models;
using DermSight.Parameter;
using DermSight.Service;
using DermSight.Services;
using DermSight.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DermSight.Controller
{
    [Route("DermSight/[controller]")]
    public class ClinicController(ClinicService _ClinicService,UserService _UserService) : ControllerBase
    {
        
        readonly ClinicService ClinicService = _ClinicService;
        public UserService UserService = _UserService;

        #region 取得疾病列表
        [HttpGet]
        [Route("AllClinic")]
        public IActionResult GetAllClinicList([FromQuery]string? Search,[FromQuery]int CityId = 0,[FromQuery]int page = 1){
            try
            {
                ClinicViewModel data = new()
                {
                    Forpaging = new Forpaging(page),
                    Search = string.IsNullOrEmpty(Search) ? "" : Search,
                    CityId = CityId,
                };
                data.ClinicList = ClinicService.GetAllClinicList(data);
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

        #region 新增疾病
        [HttpPost]
        [Route("")]
        public IActionResult InsertClinic([FromBody]ClinicInsert Data){
            try{
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
                    int userId = UserService.GetDataByAccount(User.Identity.Name).userId;
                    Clinic Clinic = new(){
                        CityId = Data.CityId,
                        Name = Data.Name,
                        Address = Data.Address
                    };
                    Clinic.ClinicId = ClinicService.Create(Clinic);
                    return Ok(new Response{
                        status_code = 200,
                        message = "新增成功",
                        data = ClinicService.Get(Clinic.ClinicId)
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
        public IActionResult UpdateClinic([FromBody]ClinicUpdate Data){
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
                    if(ClinicService.Get(Data.ClinicId) == null){
                        return BadRequest(new Response(){
                            status_code = 400,
                            message = "無該最新消息或以刪除"
                        });
                    }
                    int userId = UserService.GetDataByAccount(User.Identity.Name).userId;
                    Clinic Clinic = new(){
                        ClinicId = Data.ClinicId,
                        CityId = Data.CityId,
                        Name = Data.Name,
                        Address = Data.Address
                    };
                    ClinicService.Update(Clinic);
                    Clinic = ClinicService.Get(Clinic.ClinicId);
                    return Ok(new Response{
                        status_code = 200,
                        message = "修改成功",
                        data = Clinic
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
        public IActionResult DeleteClinic([FromQuery]int ClinicId){
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
                if(ClinicService.Get(ClinicId) == null){
                    return BadRequest(new Response(){
                        status_code = 400,
                        message = "無該最新消息或以刪除"
                    });
                }
                ClinicService.Delete(ClinicId);
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