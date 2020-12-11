using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.MYEntities;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.PurchasesInfo;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.PurchasesInfo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.PurchasesInfo
{
    [Route("api/v1/PurchasesInfo/[controller]/[action]")]
    [ApiController]
    public class SamplesController : ControllerBase
    {
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;

        public SamplesController(haikanITMContext dbITMContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _dbITMContext = dbITMContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        ///  列表显示
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(PesticideRequestPayload payload)
        {
            var query = from p in _dbITMContext.Samples
                        join o in _dbITMContext.Orgs
                        on p.OrganizationId equals o.OrganizationId
                        select new
                        {
                            p.Id,
                            p.SampleName, 
                            p.MealTimeName,
                            SampledAt = Convert.ToDateTime(p.SampledAt).ToString("yyyy-MM-dd HH:mm:ss"), 
                            p.Weight,
                            p.Hours,
                            p.FoodName,
                            p.AuditorName,
                            p.Note, //备注
                            p.Img, //图片
                            o.SchoolName
                        };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                query = query.Where(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
            }
            query = query.OrderByDescending(x => x.Id);
            var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
            var totalCount = query.Count();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list, totalCount);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult TypeList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbITMContext.Types.Where(x => x.TypeName == "餐次");
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 菜品
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult foodList(string type)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbITMContext.Goods.Where(x => x.MealTimeName ==type);
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (ene!=null)
                {
                    entity = entity.Where(x => x.OrganizationId == ene.OrganizationId);
                }
            }
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(SamplesViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                var entity = new MYEntities.Samples();
                entity.SampleId = 0;
                entity.OrganizationId = ene.OrganizationId;
                entity.FoodIds = model.FoodIds;
                entity.FoodName = model.FoodName;
                entity.Note = model.Note;
                entity.Img = model.Img;
                entity.Weight = model.Weight;
                entity.Hours = model.Hours;
                entity.Del = 1;
                entity.CreateUserId = 308343;
                entity.MealTime = model.MealTime;
                entity.MealTimeName = model.MealTimeName;
                entity.EliminateId = model.EliminateId;
                entity.EliminateName = model.EliminateName;
                var eneAuditorName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.AuditorName && x.OrganizationId == ene.OrganizationId);
                if (eneAuditorName != null)
                {
                    entity.AuditorName = model.AuditorName;
                    entity.AuditorId = Convert.ToInt32(eneAuditorName.UserId);
                }
                else
                {
                    response.SetFailed("暂无该人员信息");
                    return Ok(response);
                }
                var eneSampleName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.SampleName && x.OrganizationId == ene.OrganizationId);
                if (eneSampleName != null)
                {
                    entity.SampleName = model.SampleName;
                    entity.SampleUserId = Convert.ToInt32(eneSampleName.UserId);
                }
                else
                {
                    response.SetFailed("暂无该人员信息");
                    return Ok(response);
                }
                entity.SampledAt = model.SampledAt;
                entity.CreatedAt = DateTime.Now;
                entity.MaturedAt = model.MaturedAt;
                entity.EliminatedAt = model.EliminatedAt;
                entity.Note = model.Note;
                entity.Sync = 0;
                _dbITMContext.Samples.Add(entity);
                _dbITMContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取当前行信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult show(string Id)
        {
            using (_dbITMContext)
            {
                var query = _dbITMContext.Samples.FirstOrDefault(x => x.Id.ToString() == Id);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(SamplesViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                var entity = _dbITMContext.Samples.FirstOrDefault(x => x.Id == model.Id);
                entity.FoodIds = model.FoodIds;
                entity.FoodName = model.FoodName;
                entity.Note = model.Note;
                entity.Img = model.Img;
                entity.Weight = model.Weight;
                entity.Hours = model.Hours;
                entity.MealTime = model.MealTime;
                entity.MealTimeName = model.MealTimeName;
                var eneAuditorName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.AuditorName && x.OrganizationId == ene.OrganizationId);
                if (eneAuditorName != null)
                {
                    entity.AuditorName = model.AuditorName;
                    entity.AuditorId = Convert.ToInt32(eneAuditorName.UserId);
                }
                else
                {
                    response.SetFailed("暂无该人员信息");
                    return Ok(response);
                }
                var eneSampleName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.SampleName && x.OrganizationId == ene.OrganizationId);
                if (eneSampleName != null)
                {
                    entity.SampleName = model.SampleName;
                    entity.SampleUserId = Convert.ToInt32(eneSampleName.UserId);
                }
                else
                {
                    response.SetFailed("暂无该人员信息");
                    return Ok(response);
                }
                entity.SampledAt = model.SampledAt;
                entity.MaturedAt = model.MaturedAt;
                entity.EliminatedAt = model.EliminatedAt;
                entity.UpdatedAt = DateTime.Now;
                entity.Note = model.Note;
                _dbITMContext.SaveChanges();
                response.SetSuccess("修改成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除单个角色
        /// </summary>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
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
            response = UpdateIsDelete(ids);
            return Ok(response);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(string ids)
        {

            using (_dbITMContext)
            {
                var sql = string.Format("DELETE FROM samples WHERE id IN (" + ids + ")");
                _dbITMContext.Database.ExecuteSqlRaw(sql);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        /// <summary>
        /// 删除多条批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = UpdateIsDelete(ids);
                    break;
            }
            return Ok(response);
        }
    }
}
