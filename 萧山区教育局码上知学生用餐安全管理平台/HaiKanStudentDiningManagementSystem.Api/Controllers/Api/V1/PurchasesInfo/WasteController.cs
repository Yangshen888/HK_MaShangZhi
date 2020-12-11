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
    public class WasteController : ControllerBase
    {
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;

        public WasteController(haikanITMContext dbITMContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
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
            var query = from p in _dbITMContext.Wastes
                        join o in _dbITMContext.Orgs
                        on p.OrganizationId equals o.OrganizationId
                        select new
                        {
                            p.Id,
                            HandleDate = Convert.ToDateTime(p.HandleDate).ToString("yyyy-MM-dd HH:mm:ss"),
                            p.NewHandlerName,
                            p.FullName,
                            p.Receiver,
                            p.ReceiverIdentityCard,
                            p.SwillNumber,
                            p.WasteoilNumber,
                            p.OtherWasteNumber,
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
        /// 单位列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PartList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = from i in _dbITMContext.Partners
                         where i.TypeId == 36
                         select new
                         {
                             i.OrganizationId,
                             i.PartnerId,
                             i.ProvideId,
                             i.FullName
                         };
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
        public IActionResult Create(WasteViewModel model)
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
                var entity = new MYEntities.Wastes();
                entity.OrganizationId = ene.OrganizationId;
                if (model.Receiver!="")
                {
                    entity.Status = 1;
                }
                entity.Imgs = model.Imgs;
                entity.FullName = model.FullName;
                entity.HandleDate = model.HandleDate;
                entity.SwillNumber = model.SwillNumber;
                entity.WasteoilNumber = model.WasteoilNumber;
                entity.OtherWasteNumber = model.OtherWasteNumber;
                var eneNewHandlerName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.NewHandlerName && x.OrganizationId == ene.OrganizationId);
                if (eneNewHandlerName != null)
                {
                    entity.NewHandlerName = model.NewHandlerName;
                    entity.HandlerId = eneNewHandlerName.UserId;
                }
                else
                {
                    response.SetFailed("暂无交接负责人信息");
                    return Ok(response);
                }
                entity.ReceiverCompanyId = model.ReceiverCompanyId;
                entity.Receiver = model.Receiver;
                entity.ReceiverCompanyName = model.ReceiverCompanyName;
                entity.ReceiverIdentityCard = model.ReceiverIdentityCard;
                entity.Note = model.Note;
                entity.CreateUserId = 308343;
                entity.CreateTime = DateTime.Now;
                _dbITMContext.Wastes.Add(entity);
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
                var query = _dbITMContext.Wastes.FirstOrDefault(x => x.Id.ToString() == Id);
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
        public IActionResult Edit(WasteViewModel model)
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
                var entity = _dbITMContext.Wastes.FirstOrDefault(x => x.Id == model.Id);
                if (model.Receiver != "")
                {
                    entity.Status = 1;
                }
                entity.Imgs = model.Imgs;
                entity.FullName = model.FullName;
                entity.HandleDate = model.HandleDate;
                entity.SwillNumber = model.SwillNumber;
                entity.WasteoilNumber = model.WasteoilNumber;
                entity.OtherWasteNumber = model.OtherWasteNumber;
                var eneNewHandlerName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.NewHandlerName && x.OrganizationId == ene.OrganizationId);
                if (eneNewHandlerName != null)
                {
                    entity.NewHandlerName = model.NewHandlerName;
                    entity.HandlerId = eneNewHandlerName.UserId;
                }
                else
                {
                    response.SetFailed("暂无交接负责人信息");
                    return Ok(response);
                }
                entity.ReceiverCompanyId = model.ReceiverCompanyId;
                entity.Receiver = model.Receiver;
                entity.ReceiverCompanyName = model.ReceiverCompanyName;
                entity.ReceiverIdentityCard = model.ReceiverIdentityCard;
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
                var sql = string.Format("DELETE FROM wastes WHERE id IN (" + ids + ")");
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
