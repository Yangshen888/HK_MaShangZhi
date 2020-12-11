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
    public class DisinfectionController : ControllerBase
    {
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;

        public DisinfectionController(haikanITMContext dbITMContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
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
        public IActionResult GetList(MorningCheckRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var query = from i in _dbITMContext.Disinfections
                        join s in _dbITMContext.Orgs
                        on i.OrganizationId equals s.OrganizationId
                        select new
                        {
                            i.Id,
                            s.Name,
                            CreatedDate = i.CreatedDate.Value.ToString("yyyy-MM-dd"),
                            i.TypeName,
                            i.DisinfectionUserName,
                            i.Method,
                            i.Disinfection,
                            i.OrganizationId,
                            XDTypeName = i.TypeName
                        };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var org = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                query = query.Where(x => x.OrganizationId == org.OrganizationId);
            }
            query = query.OrderByDescending(x => x.Id);
            var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
            var totalCount = query.Count();
            response.SetData(list, totalCount);
            return Ok(response);
        }

        /// <summary>
        /// 获取当前行信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetfoGet(string Id)
        {
            using (_dbITMContext)
            {
                var query = _dbITMContext.Disinfections.FirstOrDefault(x => x.Id.ToString() == Id);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCreate(DisinfectionViewModel model)
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
                var entity = new Disinfections();
                entity.OrganizationId = ene.OrganizationId;
                entity.DisinfectionId = 0;
                entity.Sync = 0;
                //entity.Method = model.Method;
                //entity.Disinfection = model.DxTypeName;         //消毒对象是单个的
                entity.Type = model.XdccType;
                entity.TypeName = model.XdccTypeName;
                entity.CreateUserId = model.CreateUserId;
                entity.DisinfectionUserId = model.DisinfectionUserId;
                entity.DisinfectionUserName = model.DisinfectionUserName;
                entity.CreatedDate = DateTime.Now;
                entity.ImgUrls = model.ImgUrls;
                entity.Img = model.Img;
                entity.CreatedAt = DateTime.Now;
                _dbITMContext.Disinfections.Add(entity);
                _dbITMContext.SaveChanges();
                response.SetSuccess("添加成功");

                return Ok(response);
            }
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEdit(DisinfectionViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var entity3 = _dbITMContext.Disinfections.FirstOrDefault(x=>x.Id==model.Id);
                entity3.Type = model.XdccType;
                entity3.TypeName = model.XdccTypeName;
                entity3.DisinfectionUserId = model.DisinfectionUserId;
                entity3.DisinfectionUserName = model.DisinfectionUserName;
                entity3.ImgUrls = model.ImgUrls;
                entity3.Img = model.Img;
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


                var sql = string.Format("DELETE FROM disinfections WHERE id IN (" + ids + ")");
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
        /// 消毒方法
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult methodList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbITMContext.Types.Where(x => x.Type == "xdff");
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 消毒对象
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult disinfectionList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbITMContext.Types.Where(x => x.Type == "xddx");
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 消毒餐次
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ccList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbITMContext.Types.Where(x => x.Type == "cc");
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 消毒间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult roomList()
        {

            var entity = _dbITMContext.Lights.Where(x => 1 == 1);
            var response = ResponseModelFactory.CreateResultInstance;
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var org = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (org != null)
                {
                    entity = entity.Where(x => (x.OrganizationId == org.OrganizationId || x.OrganizationId == -1) && x.Type == 1);
                }
                //var query = entity.ToList();
                //response.SetData(query);
            }
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 消毒工具
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult toolList()
        {
            var entity = _dbITMContext.Lights.Where(x => 1 == 1);
            var response = ResponseModelFactory.CreateResultInstance;
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var org = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (org != null)
                {
                    entity = entity.Where(x => (x.OrganizationId == org.OrganizationId || x.OrganizationId == -1) && x.Type == 0);
                }
                //var query = entity.ToList();
                //response.SetData(query);
            }
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 该组织下人员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult userList()
        {
            var entity = _dbITMContext.Users.Where(x => 1 == 1);
            var response = ResponseModelFactory.CreateResultInstance;
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var org = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (org != null)
                {
                    entity = entity.Where(x => x.OrganizationId == org.OrganizationId);
                }
                //var query = entity.ToList();
                //response.SetData(query);
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

        [HttpPost]
        public IActionResult aList(PesticideRequestPayload payload)
        {
            var entity = _dbITMContext.Disinfections.FirstOrDefault(x=>x.Id.ToString()== payload.Kw);
            var query = from i in _dbITMContext.Disinfections
                        join o in _dbITMContext.DisinfectionRecords
                        on i.DisinfectionId equals o.DisinfectionId
                        select new
                        {
                            o.Id,
                            o.TypeName,
                            o.Method,
                            o.Number,
                            o.RoomName,
                            o.ToolName,
                            o.DisinfectionId,
                            o.OwnerDisinfectionId
                        };
            if (entity.DisinfectionId != 0)
            {
                query = query.Where(x => x.DisinfectionId == entity.DisinfectionId);
            }
            else
            {
                query = query.Where(x => x.OwnerDisinfectionId.ToString() == payload.Kw);
            }
            query = query.OrderByDescending(x => x.Id);
            var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
            var totalCount = query.Count();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list, totalCount);
            return Ok(response);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult aCreate(DisinfectionViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var entity = _dbITMContext.Disinfections.FirstOrDefault(x => x.Id == model.dId);
                var entity2 = new MYEntities.DisinfectionRecords();
                entity2.OwnerDisinfectionId = model.Id;
                entity2.DisinfectionRecordId = 0;
                entity2.DisinfectionId = entity.DisinfectionId;
                entity2.CreateUserId = (int?)model.CreateUserId;
                entity2.Type = (int)model.DxType;
                entity2.TypeName = model.DxTypeName;
                entity2.Method = model.Method;
                entity2.MethodId = model.MethodId;
                entity2.Number = model.Number;
                entity2.RoomId = model.RoomId;
                entity2.RoomName = model.RoomName;
                entity2.ToolId = model.ToolId;
                entity2.ToolName = model.ToolName;
                entity2.CreatedAt = DateTime.Now;
                _dbITMContext.DisinfectionRecords.Add(entity2);
                _dbITMContext.SaveChanges();
                if (model.Method!="")
                {
                    if (entity.Method!="")
                    {
                        entity.Method +="," + model.Method;
                    }
                    else
                    {
                        entity.Method = model.Method;
                    }
                }
                if (model.Disinfection != "")
                {
                    if (entity.Disinfection != "")
                    {
                        entity.Disinfection += "," + model.Disinfection;
                    }
                    else
                    {
                        entity.Disinfection = model.Disinfection;
                    }
                }
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
        public IActionResult ashow(string Id)
        {
            using (_dbITMContext)
            {
                var query = _dbITMContext.DisinfectionRecords.FirstOrDefault(x => x.Id.ToString() == Id);
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
        public IActionResult aEdit(DisinfectionViewModel model)
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
                var entity = _dbITMContext.Disinfections.FirstOrDefault(x => x.Id == model.dId);
                var entity2 = _dbITMContext.DisinfectionRecords.FirstOrDefault(x => x.Id == model.Id);
                entity2.CreateUserId = (int?)model.CreateUserId;
                entity2.Type = (int)model.DxType;
                entity2.TypeName = model.DxTypeName;
                entity2.Method = model.Method;
                entity2.MethodId = model.MethodId;
                entity2.Number = model.Number;
                entity2.RoomId = model.RoomId;
                entity2.RoomName = model.RoomName;
                entity2.ToolId = model.ToolId;
                entity2.ToolName = model.ToolName;
                _dbITMContext.SaveChanges();
                if (model.Method != "")
                {
                    if (entity.Method != "")
                    {
                        entity.Method += "," + model.Method;
                    }
                    else
                    {
                        entity.Method = model.Method;
                    }
                }
                if (model.Disinfection != "")
                {
                    if (entity.Disinfection != "")
                    {
                        entity.Disinfection += "," + model.Disinfection;
                    }
                    else
                    {
                        entity.Disinfection = model.Disinfection;
                    }
                }
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
        public IActionResult aDelete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = aUpdateIsDelete(ids);
            return Ok(response);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel aUpdateIsDelete(string ids)
        {

            using (_dbITMContext)
            {
                var sql = string.Format("DELETE FROM DisinfectionRecords WHERE id IN (" + ids + ")");
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
        public IActionResult aBatch(string command, string ids)
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
                    response = aUpdateIsDelete(ids);
                    break;
            }
            return Ok(response);
        }
    }
}
