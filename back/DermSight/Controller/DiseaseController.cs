using Microsoft.AspNetCore.Mvc;
using DermSight.Models;
using DermSight.Services;
using DermSight.Parameter;
using DermSight.Service;
using DermSight.ViewModels;

namespace DermSight.Controller
{
    [Route("DermSight/[controller]")]
    public class DiseaseController(DiseaseService _DiseaseService,UserService _UserService) : ControllerBase
    {

        public DiseaseService DiseaseService = _DiseaseService;
        public UserService UserService = _UserService;

        #region 取得疾病列表
        [HttpGet]
        [Route("AllDisease")]
        public IActionResult GetAllDiseaseList([FromQuery]string? Search,[FromQuery]int page = 1){
            try
            {
                // if(User.Identity == null || User.Identity.Name == null){
                //     return BadRequest(new Response(){
                //         status_code = 400,
                //         message = "請先登入"
                //     });
                // }
                DiseaseViewModel data = new()
                {
                    Forpaging = new Forpaging(page),
                    Search = string.IsNullOrEmpty(Search) ? "" : Search
                };
                data.DiseaseList = DiseaseService.GetAllDiseaseList(data);
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

        #region 取得疾病(單一)
        [HttpGet]
        [Route("")]
        public IActionResult GetDisease([FromQuery]int DiseaseId){
            try
            {
                // if(User.Identity == null || User.Identity.Name == null){
                //     return BadRequest(new Response(){
                //         status_code = 400,
                //         message = "請先登入"
                //     });
                // }
                DiseaseSymptom Data = DiseaseService.GetDiseaseSymptom(DiseaseId);
                if(Data.Disease == null){
                    return Ok(new Response(){
                        status_code = 204,
                        message = "無該疾病或以刪除"
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

        #region 新增疾病
        [HttpPost]
        [Route("")]
        public IActionResult InsertDisease([FromBody]DiseaseInsert Data){
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
                    Disease Disease = new(){
                        DiseaseId = Data.DiseaseId,
                        Name = Data.Name,
                        Description = Data.Description
                    };
                    List<string> Symptom = Data.Symptom;
                    Disease.DiseaseId = DiseaseService.Create(Disease, Symptom);
                    return Ok(new Response{
                        status_code = 200,
                        message = "新增成功",
                        data = DiseaseService.GetDiseaseSymptom(Disease.DiseaseId)
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

        #region 修改疾病
        [HttpPut]
        [Route("")]
        public IActionResult UpdateDisease([FromBody]DiseaseUpdate Data){
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
                    if(DiseaseService.Get(Data.DiseaseId)==null){
                        return BadRequest(new Response(){
                            status_code = 400,
                            message = "無該疾病或以刪除"
                        });
                    }
                    int userId = UserService.GetDataByAccount(User.Identity.Name).userId;
                    Disease Disease = new(){
                        DiseaseId = Data.DiseaseId,
                        Name = Data.Name,
                        Description = Data.Description
                    };
                    DiseaseService.Update(Disease,Data.Symptoms);
                    return Ok(new Response{
                        status_code = 200,
                        message = "修改成功",
                        data = DiseaseService.GetDiseaseSymptom(Disease.DiseaseId)
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

        #region 刪除疾病
        [HttpDelete]
        [Route("")]
        public IActionResult DeleteDisease([FromQuery]int DiseaseId){
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
                if(DiseaseService.Get(DiseaseId)==null){
                    return BadRequest(new Response(){
                        status_code = 400,
                        message = "無該疾病或以刪除"
                    });
                }
                DiseaseService.Delete(DiseaseId);
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