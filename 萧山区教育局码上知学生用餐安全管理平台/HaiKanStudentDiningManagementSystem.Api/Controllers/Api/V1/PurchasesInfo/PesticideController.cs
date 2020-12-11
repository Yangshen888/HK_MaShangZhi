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
    public class PesticideController : ControllerBase
    {
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;

        public PesticideController(haikanITMContext dbITMContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
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
            var query = from p in _dbITMContext.Pesticides
                        join o in _dbITMContext.Orgs
                        on p.OrganizationId equals o.OrganizationId
                        select new
                        {
                            p.Id,
                            p.UserName, //登记人员
                            CreatedAt=Convert.ToDateTime(p.CreatedAt).ToString("yyyy-MM-dd HH:mm:ss"), //登记时间
                            p.Inspector, //检测人员
                            CheckedAt= Convert.ToDateTime(p.CheckedAt).ToString("yyyy-MM-dd HH:mm:ss"), //检测时间
                            p.VegetablesName, //果蔬品名
                            InspectionOrder=p.InspectionOrder == 0 ? "清洗前" : "清洗后", //检测次序
                            InspectionResult=p.InspectionResult == 0 ? "阴性" : "阳性", //检测结果
                            HandleMeasure=p.HandleMeasure==0?"正常使用":"暂停使用", //处理结果
                            p.Note, //备注
                            p.TestPaper, //检测结果图片
                            o.SchoolName
                        };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                query = query.Where(x=>x.SchoolName == AuthContextService.CurrentUser.SchoolName);
            }
            query = query.OrderByDescending(x => x.Id);
            var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
            var totalCount = query.Count();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list, totalCount);
            return Ok(response);
        }

        /// <summary>
        /// 果蔬品名
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult VList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbITMContext.Vegetables.Where(x => x.Del==true);
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
        public IActionResult Create(PesticideViewModel model)
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
                var entity = new MYEntities.Pesticides();
                entity.Status = 1;
                entity.PesticideId = 0;
                entity.OrganizationId = ene.OrganizationId;
                entity.CreateUserId = 308343;
                var eneUserName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.UserName && x.OrganizationId==ene.OrganizationId);
                if (eneUserName != null)
                {
                    entity.UserName = model.UserName;
                    entity.UserOrganizationId = eneUserName.OrganizationId;
                }
                else
                {
                    response.SetFailed("暂无该人员信息");
                    return Ok(response);
                }
                var eneInspector = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.Inspector && x.OrganizationId == ene.OrganizationId);
                if (eneInspector != null)
                {
                    entity.Inspector = model.Inspector;
                }
                else
                {
                    response.SetFailed("暂无该人员信息");
                    return Ok(response);
                }
                entity.TestPaper = model.TestPaper;
                entity.Vegetables = model.Vegetables;
                entity.VegetablesName = model.VegetablesName;
                entity.InspectionOrder = model.InspectionOrder;
                entity.InspectionOrders = model.InspectionOrders;
                entity.InspectionResult = model.InspectionResult;
                entity.InspectionResults = model.InspectionResults;
                entity.HandleMeasure = model.HandleMeasure;
                entity.HandleMeasures = model.HandleMeasures;
                entity.CheckedAt = model.CheckedAt;
                entity.CreatedAt = DateTime.Now;
                entity.Note = model.Note;
                entity.Sync = 0;
                _dbITMContext.Pesticides.Add(entity);
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
                var query = _dbITMContext.Pesticides.FirstOrDefault(x=>x.Id.ToString()==Id);
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
        public IActionResult Edit(PesticideViewModel model)
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
                var entity = _dbITMContext.Pesticides.FirstOrDefault(x => x.Id == model.Id);
                var eneUserName = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.UserName && x.OrganizationId == ene.OrganizationId);
                if (eneUserName != null)
                {
                    entity.UserName = model.UserName;
                    entity.UserOrganizationId = eneUserName.OrganizationId;
                }
                else
                {
                    response.SetFailed("暂无该人员信息");
                    return Ok(response);
                }
                var eneInspector = _dbITMContext.Users.FirstOrDefault(x => x.Name == model.Inspector && x.OrganizationId == ene.OrganizationId);
                if (eneInspector != null)
                {
                    entity.Inspector = model.Inspector;
                }
                else
                {
                    response.SetFailed("暂无该人员信息");
                    return Ok(response);
                }
                entity.TestPaper = model.TestPaper;
                entity.Vegetables = model.Vegetables;
                entity.VegetablesName = model.VegetablesName;
                entity.InspectionOrder = model.InspectionOrder;
                entity.InspectionOrders = model.InspectionOrders;
                entity.InspectionResult = model.InspectionResult;
                entity.InspectionResults = model.InspectionResults;
                entity.HandleMeasure = model.HandleMeasure;
                entity.HandleMeasures = model.HandleMeasures;
                entity.CheckedAt =model.CheckedAt;
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
                var sql = string.Format("DELETE FROM pesticides WHERE id IN (" + ids + ")");
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
