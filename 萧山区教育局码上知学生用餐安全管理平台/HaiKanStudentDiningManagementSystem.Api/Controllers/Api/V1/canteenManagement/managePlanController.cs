using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.canteenManagement;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.canteenManageent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.新文件夹
{
    [Route("api/v1/canteenManagement/[controller]/[action]")]
    [ApiController]
    public class managePlanController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;

        public managePlanController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 菜品库列表显示
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ManagePlanList(ManagePlanRequestpayload payload)

        {
            var query = from c in _dbContext.ManagePlan
                        where c.IsDelete == 0
                        select new
                        {
                            c.ManageUuid,
                            c.Headline,
                            //c.SchoolUuid,
                            //c.Content,
                            c.IsDelete,
                            c.SchoolUuid,
                            c.AddTime,
                            c.AddPeople,
                            c.Id
                        };

            //if (!string.IsNullOrEmpty(payload.kw))
            //{
            //    query = query.Where(x => x.SchoolUuid == Guid.Parse(payload.kw));
            //}
            if (!string.IsNullOrEmpty(payload.kw1))
            {
                query = query.Where(x => x.Headline.Contains(payload.kw1));
            }
            //判断444
            if (!string.IsNullOrEmpty(payload.schoolguid))
            {
                query = query.Where(x => x.SchoolUuid == Guid.Parse(payload.schoolguid));
            }
            if (payload.FirstSort != null)
            {
                query = query.OrderByDescending(x => x.Id);
            }
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
        public IActionResult Create(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new HaiKanStudentDiningManagementSystem.Api.Entities.ManagePlan();
                entity.ManageUuid = Guid.NewGuid();
                entity.Headline = model.headline;
                entity.IsDelete = 0;
                entity.Content = model.content;
                entity.SchoolUuid = model.schoolUuid;
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dbContext.ManagePlan.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
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
            using (_dbContext)
            {
                var entity = _dbContext.ManagePlan.FirstOrDefault(x => x.ManageUuid.ToString() == ids);
                entity.IsDelete = 1;//删除状态
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
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
        public IActionResult Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (command == "delete")
                {
                    response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
                    //var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    //foreach (var item in parameters)
                    //{
                    //    var query1 = _dbContext.ManagePlan.FirstOrDefault(x => x.ManageUuid == Guid.Parse(item.Value.ToString()));
                    //    query1.IsDelete = 1;
                    //    _dbContext.SaveChanges();
                    //}
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                //var idList = ids.Split(",").ToList();
                ////idList.ForEach(x => {
                ////  _dbContext.Database.ExecuteSqlCommand($"UPDATE DncUser SET IsDeleted=1 WHERE Id = {x}");
                ////});
                //_dbContext.Database.ExecuteSqlCommand($"UPDATE DncUser SET IsDeleted={(int)isDeleted} WHERE Id IN ({ids})");
                //var id = ids.Split(",");
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE ManagePlan SET IsDelete=@IsDelete WHERE ManageUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        /// <summary>
        /// 获取菜品信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PlanGet(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.ManagePlan.FirstOrDefault(x => x.ManageUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑（保存）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;

            string guid = model.manageUuid;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.ManagePlan.FirstOrDefault(x => x.ManageUuid == Guid.Parse(guid));
                entity.Headline = model.headline;
                entity.Content = model.content;
                _dbContext.SaveChanges();
                response.SetSuccess("修改成功");
                return Ok(response);
            }
        }
    }
}
