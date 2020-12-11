using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Extensions.DataAccess;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Process;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.FoodProcess
{
    [Route("api/v1/FoodProcess/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class ProcessController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;

        public ProcessController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 成菜流程列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(ProcessRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query2 = _dbContext.SystemUser.Where(x => x.IsDeleted == 0);
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query2 = query2.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                var list2 = query2.ToList();
                var query3 = _dbContext.Ingredient.Where(x => x.IsDelete == 0);
                //if (AuthContextService.CurrentUser.SchoolGuid != null)
                //{
                //    query3 = query3.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                //}
                var list3 = query3.ToList();
                var query = _dbContext.MealFlow.Where(x => x.IsDelete == 0).Include(x => x.CuisineUu).ThenInclude(x => x.SchoolUu).Select(x => new
                {
                    x.MealFlowUuid,
                    x.IsDelete,
                    x.AddPeople,
                    x.AddTime,
                    x.Buying,
                    x.Chopper,
                    x.Cook,
                    x.CuisineUu.CuisineName,
                    Ingredient = FoodName(x.CuisineUu.Ingredient, list3),
                    Burdening = FoodName(x.CuisineUu.Burdening, list3),
                    x.CuisineUu.Abstract,
                    x.CuisineUuid,
                    x.Detection,
                    x.Id,
                    MealType=x.MealType=="morn"?"早餐": x.MealType == "noon" ?"午餐":"晚餐",
                    x.SchoolUu.SchoolName,
                    x.SchoolUuid,
                    x.WashVege,
                    State=State(x,list2),
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.CuisineName.Contains(payload.Kw));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }

                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);


                return Ok(response);
            }
        }


        /// <summary>
        /// 已完成状态
        /// </summary>
        /// <param name="x"></param>
        /// <param name="list2"></param>
        /// <returns></returns>
        private static string State(MealFlow x, System.Collections.Generic.List<SystemUser> list2)
        {
            var s = "";
            var u = "";
            if (!string.IsNullOrEmpty(x.BuyingTime))
            {
                s = "买入:";
                u = x.Buying;
            }
            if (!string.IsNullOrEmpty(x.DetectionTime))
            {
                s = "检测:";
                u = x.Detection;
            }
            if (!string.IsNullOrEmpty(x.WashVegeTime))
            {
                s = "洗菜:";
                u = x.WashVege;
            }
            if (!string.IsNullOrEmpty(x.ChopperTime))
            {
                s = "切配:";
                u = x.Chopper;
            }
            if (!string.IsNullOrEmpty(x.CookTime))
            {
                s = "成菜:";
                u = x.Cook;
            }
            if (!string.IsNullOrEmpty(u))
            {
                var arr = u.Split(',');
                var list = list2.Where(x => arr.Contains(x.SystemUserUuid.ToString())).Select(x=>x.RealName).ToList();
                u = string.Join(',', list);
            }
            s += u;
            return s;
        }


        /// <summary>
        /// 添加成菜流程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(ProcessViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                //if (_dbContext.MealFlow.Any(x => x.CuisineUuid == model.CuisineUuid && x.MealType == model.MealType && x.AddTime == Convert.ToDateTime(model.AddTime).ToString("yyyy-MM-dd")))
                //{
                //    response.SetFailed("当天该用餐类型的菜品已存在");
                //    return Ok(response);
                //}
                var mealFlow = new MealFlow()
                {
                    MealFlowUuid = Guid.NewGuid(),
                    CuisineUuid = model.CuisineUuid,
                    MealType = model.MealType,
                    Buying = model.Buying,
                    Detection = model.Detection,
                    WashVege = model.WashVege,
                    Chopper = model.Chopper,
                    Cook = model.Cook,
                    BuyingTime = model.BuyingTime,
                    DetectionTime = model.DetectionTime,
                    WashVegeTime = model.WashVegeTime,
                    ChopperTime = model.ChopperTime,
                    CookTime = model.CookTime,
                    //BuyingTime = Convert.ToDateTime(model.BuyingTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    //DetectionTime = Convert.ToDateTime(model.DetectionTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    //WashVegeTime = Convert.ToDateTime(model.WashVegeTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    //ChopperTime = Convert.ToDateTime(model.ChopperTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    //CookTime = Convert.ToDateTime(model.CookTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    //AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    AddTime = Convert.ToDateTime(model.AddTime).ToString("yyyy-MM-dd"),
                    IsDelete = 0,
                    SchoolUuid = model.SchoolUuid != null ? model.SchoolUuid : AuthContextService.CurrentUser.SchoolGuid,
                };
                _dbContext.MealFlow.Add(mealFlow);
                var num = _dbContext.SaveChanges();
                if (num > 0)
                {
                    response.SetSuccess("添加成功");
                }
                else
                {
                    response.SetFailed("添加失败");
                }

                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑成菜流程信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.MealFlow.Include(x=>x.SchoolUu).Select(x=>new 
                {
                    x.SchoolUu.SchoolName,
                    x.SchoolUuid,
                    x.CuisineUuid,
                    x.Buying,
                    x.BuyingTime,
                    x.Detection,
                    x.WashVege,
                    x.DetectionTime,
                    x.WashVegeTime,
                    x.Chopper,
                    x.ChopperTime,
                    x.Cook,
                    x.CookTime,
                    x.AddTime,
                    x.AddPeople,
                    x.IsDelete,
                    x.Id,
                    x.MealFlowUuid,
                    x.MealType,
                }).FirstOrDefault(x => x.MealFlowUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑的成菜流程信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(ProcessViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                
                var entity = _dbContext.MealFlow.FirstOrDefault(x => x.MealFlowUuid == model.MealFlowUuid);

                if (entity == null)
                {
                    response.SetFailed("该流程不存在");
                    return Ok(response);
                }
                //if (entity.CuisineUuid!=model.CuisineUuid&& _dbContext.MealFlow.Any(x => x.CuisineUuid == model.CuisineUuid && x.MealType == model.MealType && x.AddTime == Convert.ToDateTime(model.AddTime).ToString("yyyy-MM-dd")))
                //{
                //    response.SetFailed("当天该用餐类型的菜品已存在");
                //    return Ok(response);
                //}
                entity.CuisineUuid = model.CuisineUuid;
                entity.MealType = model.MealType;
                entity.Buying = model.Buying;
                entity.Detection = model.Detection;
                entity.WashVege = model.WashVege;
                entity.Chopper = model.Chopper;
                entity.Cook = model.Cook;
                entity.BuyingTime = model.BuyingTime;
                entity.DetectionTime = model.DetectionTime;
                entity.WashVegeTime = model.WashVegeTime;
                entity.ChopperTime = model.ChopperTime;
                entity.CookTime = model.CookTime;
                //entity.BuyingTime = Convert.ToDateTime(model.BuyingTime).ToString("yyyy-MM-dd HH:mm:ss");
                //entity.DetectionTime = Convert.ToDateTime(model.DetectionTime).ToString("yyyy-MM-dd HH:mm:ss");
                //entity.WashVegeTime = Convert.ToDateTime(model.WashVegeTime).ToString("yyyy-MM-dd HH:mm:ss");
                //entity.ChopperTime = Convert.ToDateTime(model.ChopperTime).ToString("yyyy-MM-dd HH:mm:ss");
                //entity.CookTime = Convert.ToDateTime(model.CookTime).ToString("yyyy-MM-dd HH:mm:ss");
                entity.AddTime = Convert.ToDateTime(model.AddTime).ToString("yyyy-MM-dd");
                var num = _dbContext.SaveChanges();
                if (num > 0)
                {
                    response.SetSuccess("修改成功");
                }
                else
                {
                    response.SetFailed("未修改");
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除流程
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
                    break;
                case "recover":
                    response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
                    break;
                //case "forbidden":
                //    if (ConfigurationManager.AppSettings.IsTrialVersion)
                //    {
                //        response.SetIsTrial();
                //        return Ok(response);
                //    }
                //    response = UpdateStatus(UserStatus.Forbidden, ids);
                //    break;
                //case "normal":
                //    response = UpdateStatus(UserStatus.Normal, ids);
                //    break;
                default:
                    break;
            }
            return Ok(response);
        }



        /// <summary>
        /// 获取食材名称
        /// </summary>
        /// <returns></returns>
        private static string FoodName(string uuids, List<Entities.Ingredient> list3)
        {
            if (string.IsNullOrEmpty(uuids))
            {
                return "";
            }
            var idls = uuids.Split(',').ToList();
            var list = list3.Where(x => idls.Contains(x.IngredientUuid.ToString())).Select(x => x.FoodName).ToList();
            var names = string.Join(',', list);
            return names;
        }





        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="isDelete"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDelete, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE MealFlow SET IsDelete=@IsDelete WHERE MealFlowUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDelete));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
    }
}
