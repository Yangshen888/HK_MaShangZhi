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
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.PurchasesInfo
{
    [Route("api/v1/PurchasesInfo/[controller]/[action]")]
    [ApiController]
    public class MorningCheckController : ControllerBase
    {
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;

        public MorningCheckController(haikanITMContext dbITMContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
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
        public IActionResult List(MorningCheckRequestPayload payload)
        {
            var query = from i in _dbITMContext.Inspections
                        join o in _dbITMContext.Orgs
                        on i.OrganizationId equals o.OrganizationId
                        orderby i.CreatedAt descending
                        select new
                        {
                            o.SchoolName,
                            o.Name,
                            i.Id,
                            i.OrganizationId,
                            i.ShouldCount,
                            i.ActualCount,
                            i.UnqualifiedCount,
                            i.CreatedUser,
                            i.Inspector,
                            i.CreatedDate,
                            i.IsSupply,
                            i.Type,
                            i.IsDifferent,
                            i.InspectionId,
                            CreatedAt = i.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            //i.UpdatedAt,
                        };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                query = query.Where(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
            }
            //query = query.OrderByDescending(x =>);
            var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
            var totalCount = query.Count();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list, totalCount);
            return Ok(response);
        }

        /// <summary>
        /// 获取当前行信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Load(ulong Id)
        {
            using (_dbITMContext)
            {
                var query = from i in _dbITMContext.Inspections
                            join o in _dbITMContext.Orgs
                            on i.OrganizationId equals o.OrganizationId
                            select new
                            {
                                o.SchoolName,
                                o.Name,
                                i.Id,
                                i.OrganizationId,
                                i.OrganizationName,
                                i.ShouldCount,
                                i.ActualCount,
                                i.UnqualifiedCount,
                                i.CreatedUser,
                                i.Inspector,
                                i.CreatedDate,
                                i.IsSupply,
                                i.Type,
                                i.IsDifferent,
                                i.InspectionId,
                                i.DepartmentId,
                                CreatedAt = i.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                //i.UpdatedAt,
                            };
                var entity = query.Select(x => new
                {
                    x.Id,
                    x.OrganizationId,
                    x.Name,
                    x.Inspector,
                    ShouldCount = int.Parse(x.ShouldCount),
                    ActualCount = int.Parse(x.ActualCount),
                    x.IsSupply,
                    x.Type,
                    x.IsDifferent,
                    x.CreatedUser,
                    UnqualifiedCount = int.Parse(x.UnqualifiedCount),
                    x.CreatedAt,
                    x.DepartmentId
                }).FirstOrDefault(x => x.Id == Id);


                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(MorningCheckViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var org = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (org != null)
                {
                    var entity = new Inspections();
                    entity.InspectionId = 0;
                    entity.OrganizationId = org.OrganizationId;
                    entity.OrganizationName = org.Name;
                    entity.ShouldCount = model.ShouldCount.ToString();
                    entity.ActualCount = "0";
                    entity.UnqualifiedCount = "0";
                    entity.CreatedUser = 0;
                    entity.Inspector = model.Inspector;
                    entity.CreatedDate = DateTime.Now;
                    entity.CreatedAt = DateTime.Now;
                    entity.IsSupply = true;
                    entity.Type = 2;
                    entity.Sync = 0;
                    entity.IsDifferent = true;
                    entity.DepartmentId = model.DepartmentId;
                    //暂无班组选择
                    entity.TeamGroupId = 0;
                    _dbITMContext.Inspections.Add(entity);
                    _dbITMContext.SaveChanges();
                    response.SetSuccess("添加成功");
                }
                else
                {
                    response.SetFailed("不存在该组织");

                }


                return Ok(response);
            }
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(MorningCheckViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var entity = _dbITMContext.Inspections.FirstOrDefault(x => x.Id == model.Id);
                entity.ShouldCount = model.ShouldCount.ToString();
                //entity.ActualCount = model.ActualCount;
                //entity.UnqualifiedCount = (model.ShouldCount- model.ActualCount).ToString();
                entity.Inspector = model.Inspector;
                entity.DepartmentId = model.DepartmentId;
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
        /// 删除
        /// </summary>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(string ids)
        {

            using (_dbITMContext)
            {


                var sql = string.Format("DELETE FROM inspections WHERE id IN (" + ids + ")");
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




        /// <summary>
        /// 查询某个晨检的详细信息
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult InfoList(MorningCheckInfoRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (payload.Inspectionid != 0)
            {
                var query = from i in _dbITMContext.InspectionInformation
                            where i.InspectionId == payload.Inspectionid
                            select new
                            {
                                i.Id,
                                i.OrganizationId,
                                i.InspectionId,
                                i.UserId,
                                i.UserName,
                                i.CardNumber,
                                Duty = i.Duty ?? "",
                                i.CheckStatus,
                                i.Attendance,
                                i.AttendanceTime,
                                i.ProsessInfo,
                                //i.CreatedAt,
                                CreatedAt = i.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                //i.UpdatedAt,
                            };
                query = query.OrderByDescending(x => x.Id);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();

                response.SetData(list, totalCount);
            }
            else
            {
                var query = from i in _dbITMContext.InspectionInformation
                            where i.OwnerInspectionId == payload.Id
                            select new
                            {
                                i.Id,
                                i.OrganizationId,
                                i.InspectionId,
                                i.UserId,
                                i.UserName,
                                i.CardNumber,
                                Duty = i.Duty ?? "",
                                i.CheckStatus,
                                i.Attendance,
                                i.AttendanceTime,
                                i.ProsessInfo,
                                //i.CreatedAt,
                                CreatedAt = i.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
                                //i.UpdatedAt,
                            };
                query = query.OrderByDescending(x => x.Id);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();

                response.SetData(list, totalCount);

            }

            return Ok(response);
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateInfo(InspectionInformationVModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var org = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (_dbITMContext.InspectionInformation.Any(x => x.OwnerInspectionId == model.OwnerInspectionId && x.UserId == model.UserId))
                {
                    response.SetFailed("该人员已存在");
                    return Ok(response);
                }
                if (org != null)
                {
                    var entity = new InspectionInformation();
                    entity.InspectionId = 0;
                    entity.OrganizationId = org.OrganizationId;
                    entity.OwnerInspectionId = model.OwnerInspectionId;
                    entity.InspectionId = 0;
                    entity.InspectionInformationId = 0;

                    var user = _dbITMContext.Users.FirstOrDefault(X => X.SysUserId == model.UserId);
                    if (user != null)
                    {
                        entity.UserName = user.Name;
                        entity.UserId = (uint?)user.SysUserId;
                    }
                    

                    entity.CardNumber = model.CardNumber;
                    entity.Duty = model.Duty;
                    //entity.CheckStatus = model.CheckStatus;
                    entity.Attendance = model.Attendance;
                    entity.AttendanceTime = model.AttendanceTime;
                    entity.ProsessInfo = model.ProsessInfo;
                    entity.ImgUrl = model.ImgUrl;
                    entity.Note = model.Note;
                    entity.Temperature = model.Temperature;
                    entity.CreatedAt = DateTime.Now;
                    entity.PeopleStatus = model.PeopleStatus;
                    entity.PeopleStatusName = model.PeopleStatusName;
                    if (string.IsNullOrEmpty(entity.PeopleStatus))
                    {
                        entity.CheckStatus = "正常";
                    }
                    else
                    {
                        entity.CheckStatus = "异常";
                    }

                    _dbITMContext.InspectionInformation.Add(entity);
                    var num= _dbITMContext.SaveChanges();
                    if (num > 0)
                    {
                        var inspection = _dbITMContext.Inspections.FirstOrDefault(x => x.Id == model.OwnerInspectionId);
                        var sun1 = _dbITMContext.InspectionInformation.Count(x => (x.OwnerInspectionId == inspection.Id || x.InspectionId == inspection.InspectionId)&&x.CheckStatus=="正常");
                        var sun2 = _dbITMContext.InspectionInformation.Count(x => (x.OwnerInspectionId == inspection.Id || x.InspectionId == inspection.InspectionId) && x.CheckStatus == "异常");
                        inspection.ActualCount = (sun1 + sun2).ToString();
                        inspection.UnqualifiedCount = sun2.ToString();
                        _dbITMContext.SaveChanges();
                    }
                    response.SetSuccess("添加成功");
                }
                else
                {
                    response.SetFailed("不存在该组织");

                }


                return Ok(response);
            }
        }


        /// <summary>
        /// 获取当前行信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LoadInfo(ulong Id)
        {
            using (_dbITMContext)
            {

                var entity = _dbITMContext.InspectionInformation.Select(x => new
                {
                    x.Id,
                    x.OrganizationId,
                    x.OwnerInspectionId,
                    x.InspectionId,
                    x.InspectionInformationId,
                    x.UserId,
                    x.UserName,
                    x.CardNumber,
                    x.Duty,
                    x.CheckStatus,
                    x.Attendance,
                    x.AttendanceTime,
                    x.ProsessInfo,
                    x.ImgUrl,
                    x.Note,
                    x.Temperature,
                    //UpdatedAt = x.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreatedAt = x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    x.PeopleStatus,
                    x.PeopleStatusName,
                }).FirstOrDefault(x => x.Id == Id);


                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存标记
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditInfo(InspectionInformationVModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {

                var entity = _dbITMContext.InspectionInformation.FirstOrDefault(x => x.Id == model.Id);
                if (entity!=null&&entity.UserId!=model.UserId&& _dbITMContext.InspectionInformation.Any(x => x.OwnerInspectionId == model.OwnerInspectionId && x.UserId == model.UserId))
                {
                    response.SetFailed("该人员已存在");
                    return Ok(response);
                }
                var user = _dbITMContext.Users.FirstOrDefault(X => X.SysUserId == model.UserId);
                if (user != null)
                {
                    entity.UserName = user.Name;
                    entity.UserId = (uint?)user.SysUserId;
                }
                

                entity.CardNumber = model.CardNumber;
                entity.Duty = model.Duty;
                //entity.CheckStatus = model.CheckStatus;
                entity.Attendance = model.Attendance;
                entity.AttendanceTime = model.AttendanceTime;
                entity.ProsessInfo = model.ProsessInfo;
                entity.ImgUrl = model.ImgUrl;
                entity.Note = model.Note;
                entity.Temperature = model.Temperature;
                //entity.CreatedAt = DateTime.Now;
                entity.PeopleStatus = model.PeopleStatus;
                entity.PeopleStatusName = model.PeopleStatusName;
                if (string.IsNullOrEmpty(entity.PeopleStatus) )
                {
                    entity.CheckStatus = "正常";
                }
                else
                {
                    entity.CheckStatus = "异常";
                }
                var num = _dbITMContext.SaveChanges();
                var inspection = new Inspections();
                if (num > 0)
                {
                    if (model.OwnerInspectionId != 0)
                    {
                        inspection = _dbITMContext.Inspections.FirstOrDefault(x => x.Id == model.OwnerInspectionId);
                    }
                    else
                    {
                        inspection = _dbITMContext.Inspections.FirstOrDefault(x => x.InspectionId == entity.InspectionId);
                    }
                    
                    var sun1 = _dbITMContext.InspectionInformation.Count(x => (x.OwnerInspectionId == inspection.Id || x.InspectionId == inspection.InspectionId) && x.CheckStatus == "正常");
                    var sun2 = _dbITMContext.InspectionInformation.Count(x => (x.OwnerInspectionId == inspection.Id || x.InspectionId == inspection.InspectionId) && x.CheckStatus == "异常");
                    inspection.ActualCount = (sun1 + sun2).ToString();
                    inspection.UnqualifiedCount = sun2.ToString();
                    _dbITMContext.SaveChanges();
                }
                response.SetSuccess("修改成功");
                return Ok(response);
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
        public IActionResult Batch2(string command, string ids,uint oid,uint iid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbITMContext)
            {
                switch (command)
                {
                    case "delete":
                        if (ConfigurationManager.AppSettings.IsTrialVersion)
                        {
                            response.SetIsTrial();
                            return Ok(response);
                        }
                        response = UpdateIsDelete2(ids);
                        break;
                }
                Inspections inspection = new Inspections();
                if (iid == 0)
                {
                    inspection = _dbITMContext.Inspections.FirstOrDefault(x => x.Id == oid);
                }
                else
                {
                    inspection = _dbITMContext.Inspections.FirstOrDefault(x => x.InspectionId == iid);
                }

                var sun1 = _dbITMContext.InspectionInformation.Count(x => (x.OwnerInspectionId == inspection.Id || x.InspectionId == inspection.InspectionId) && x.CheckStatus == "正常");
                var sun2 = _dbITMContext.InspectionInformation.Count(x => (x.OwnerInspectionId == inspection.Id || x.InspectionId == inspection.InspectionId) && x.CheckStatus == "异常");
                inspection.ActualCount = (sun1 + sun2).ToString();
                inspection.UnqualifiedCount = sun2.ToString();
                _dbITMContext.SaveChanges();
            }


            return Ok(response);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete2(string ids)
        {

            
                var sql = string.Format("DELETE FROM inspection_information WHERE id IN (" + ids + ")");
                _dbITMContext.Database.ExecuteSqlRaw(sql);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            
        }


        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DepList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var org = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
            if (org != null)
            {
                var query = _dbITMContext.Departments.Where(x => (x.OrganizationId == -2 || x.OrganizationId == org.OrganizationId) && x.Status == 1);
                response.SetData(query.Select(x => new { x.Id, x.Name }).ToList());
            }
            else
            {
                var query = _dbITMContext.Departments.Where(x => x.OrganizationId == -2 && x.Status == 1);
                response.SetData(query.Select(x => new { x.Id, x.Name }).ToList());
            }
            return Ok(response);
        }

        /// <summary>
        /// 获取晨检类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Typelist()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var query = _dbITMContext.Types.Where(x => x.TypeName == "晨检状态");
            response.SetData(query.ToList());
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
                             UserId = i.UserId.ToString(),
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

    }
}
