using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.MYEntities;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.General;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Process;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Acticle
{
    [Route("api/v1/Acticle/[controller]/[action]")]
    [ApiController]
    public class ActicleController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;


        public ActicleController(haikanSDMSContext dbContext, IMapper mapper, haikanITMContext dbITMContext)
        {
            _dbContext = dbContext;
            _dbITMContext = dbITMContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public IActionResult GetList(GeneralRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.ArticleManage.Where(x => x.IsDelete == 0).Select(x=>new
                    {
                        x.SchoolUuid,
                        x.AddTime,
                        x.Title,
                        x.Image,
                        x.ArticleUuid
                    });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.AddTime);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);
                return Ok(response);
            }
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
            if (model.schoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = new ArticleManage();
                entity.ArticleUuid = Guid.NewGuid();
                entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid.Value;
                entity.IsDelete = 0;
                entity.Title = model.title;
                entity.Content = model.content;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dbContext.ArticleManage.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
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
            Guid guid = model.articleUuid;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.ArticleManage.FirstOrDefault(x => x.ArticleUuid == guid);
                entity.Title = model.title;
                entity.Content = model.content;
                _dbContext.SaveChanges();
                response.SetSuccess("修改成功");
                return Ok(response);
            }
        }

        //获取信息
        [HttpGet]
        public IActionResult Show(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.ArticleManage.Where(x => x.ArticleUuid == guid && x.IsDelete != 1).ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除单个
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
                var entity = _dbContext.ArticleManage.FirstOrDefault(x => x.ArticleUuid.ToString() == ids);
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
                    //    var query1 = _dbContext.SchoolLife.FirstOrDefault(x => x.SchoollifeUuid == Guid.Parse(item.Value.ToString()));
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

                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE ArticleManage SET IsDelete=@IsDelete WHERE ArticleUuid IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        /// <summary>
        /// 获取app列表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAppList(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                var list = _dbContext.ArticleManage.Where(x => x.IsDelete == 0 && x.SchoolUuid== Guid.Parse(guid)).OrderByDescending(s=>s.AddTime).Select(x=>new { 
                    x.Title,
                    x.ArticleUuid,
                    AddTime=x.AddTime.Substring(0,10)
                }).ToList();
                response.SetData(list);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取app列表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAppShow(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                var list = _dbContext.ArticleManage.FirstOrDefault(x => x.ArticleUuid == Guid.Parse(guid));
                response.SetData(list);
                return Ok(response);
            }
        }
    }
}
