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
    public class SynthesizeController : ControllerBase
    {
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;

        public SynthesizeController(haikanITMContext dbITMContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
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
            var query = from p in _dbITMContext.Synthesizes
                        join o in _dbITMContext.Orgs
                        on p.OrganizationId equals o.OrganizationId
                        select new
                        {
                            p.Id,
                            p.SynthesizeId,
                            p.SubjectName,
                            CreatedAt = Convert.ToDateTime(p.CreatedAt).ToString("yyyy-MM-dd HH:mm:ss"),
                            p.DepartmentName,
                            p.Introduce,
                            p.ContinueTime,
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

        /// <summary>
        /// 主题列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TypesList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbITMContext.Types.Where(x => x.TypeName == "综合台帐主题").Select(x => new { TypeId = x.TypeId.ToString(), x.Description });
            var query = entity.ToList().Distinct();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 部门列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DepartList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = from i in _dbITMContext.Departments
                         where i.Status == 1
                         select new
                         {
                             i.OrganizationId,
                             i.Id,
                             i.Name
                         };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (ene!=null)
                {
                    entity = entity.Where(x => x.OrganizationId == ene.OrganizationId || x.OrganizationId == -2);
                }
            }
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 人员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UsersList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = from i in _dbITMContext.Users
                         where i.SysUserId != null
                         select new
                         {
                             i.OrganizationId,
                             i.SysUserId,
                             UserId=i.UserId.ToString(),
                             i.Name
                         };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (ene != null)
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
        public IActionResult Create(SynthesizesViewModel model)
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
                var entity = new MYEntities.Synthesizes();
                entity.OrganizationId = ene.OrganizationId;
                entity.Status = 0;
                entity.Imgs = model.Imgs;
                entity.SubjectId = model.SubjectId;
                entity.SubjectName = model.SubjectName;
                entity.ContinueTime = model.ContinueTime;
                entity.Department = model.Department;
                entity.DepartmentName = model.DepartmentName;
                var RepersonName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.Reperson && x.OrganizationId == ene.OrganizationId);
                if (RepersonName != null)
                {
                    entity.Reperson = model.Reperson;
                    entity.RepersonId = RepersonName.UserId.ToString();
                }
                else
                {
                    response.SetFailed("暂无负责人信息");
                    return Ok(response);
                }
                entity.Introduce = model.Introduce;
                entity.Result = model.Result;
                entity.Number = model.Number;
                entity.Sync = 0;
                entity.CreateUserId = model.CreateUserId;
                entity.CreateDate = model.CreatedAt;
                entity.CreatedAt = model.CreatedAt;
                _dbITMContext.Synthesizes.Add(entity);
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
                var query = _dbITMContext.Synthesizes.FirstOrDefault(x => x.Id.ToString() == Id);
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
        public IActionResult Edit(SynthesizesViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                var entity = _dbITMContext.Synthesizes.FirstOrDefault(x => x.Id == model.Id);
                entity.Imgs = model.Imgs;
                entity.SubjectId = model.SubjectId;
                entity.SubjectName = model.SubjectName;
                entity.ContinueTime = model.ContinueTime;
                entity.Department = model.Department;
                entity.DepartmentName = model.DepartmentName;
                var RepersonName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.Reperson && x.OrganizationId == ene.OrganizationId);
                if (RepersonName != null)
                {
                    entity.Reperson = model.Reperson;
                    entity.RepersonId = RepersonName.UserId.ToString();
                }
                else
                {
                    response.SetFailed("暂无负责人信息");
                    return Ok(response);
                }
                entity.Introduce = model.Introduce;
                entity.Result = model.Result;
                entity.Number = model.Number;
                entity.UpdateUserId = Convert.ToInt32(model.CreateUserId);
                entity.UpdatedAt = model.CreatedAt;
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
                var sql = string.Format("DELETE FROM Synthesizes WHERE id IN (" + ids + ")");
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
