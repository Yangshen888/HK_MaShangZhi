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
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.WorkStudy;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.WorkStudy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.WorkStudy
{
    [Route("api/v1/workstudy/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class PostJobsController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public PostJobsController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        #region 后台管理列表
        /// <summary>
        /// 显示所有信息
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(PostJobsRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from p in _dbContext.Postjobs
                            select new
                            {
                                p.PostUuid,
                                p.Unit,
                                p.UnitName,
                                p.Site,
                                p.Require,
                                p.IsDelete,
                                p.SchoolUuid,
                                p.SchoolUu.SchoolName,
                                p.AddTime,
                                p.Num,
                                p.ReleaseState,
                                p.FullState
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Unit.Contains(payload.Kw.Trim())|| x.UnitName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDelete == Convert.ToInt32(((CommonEnum.IsDeleted)payload.IsDeleted)));
                }
                if (payload.ReleaseState > -1)
                {
                    query = query.Where(x => x.ReleaseState == payload.ReleaseState);
                }
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                query = query.OrderByDescending(x => x.AddTime);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();

                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        #endregion
        #region 创建
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(PostjobsViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            //if (string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolUuid))
            //{
            //    response.SetFailed("");
            //    return Ok(response);
            //}
            using (_dbContext)
            {
                if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
                {
                    response.SetFailed("请登录学校账号");
                    return Ok(response);
                }
                //if (_dbContext.Postjobs.Count(x => x.SchoolUuid.ToString() == AuthContextService.CurrentUser.SchoolUuid) > 0)
                //{
                //    response.SetFailed("已存在");
                //    return Ok(response);
                //}

                var entity = _mapper.Map<PostjobsViewModel, Postjobs>(model);
                entity.PostUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                entity.FullState = 0;
                entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                entity.AddTime = DateTime.Now;
                _dbContext.Postjobs.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }
        #endregion
        #region 编辑
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="guid">惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Postjobs.FirstOrDefault(x => x.PostUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<Postjobs, PostjobsViewModel>(entity));
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的信息
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(PostjobsViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _dbContext.Postjobs.FirstOrDefault(x => x.PostUuid == model.PostUuid);
                if (entity == null)
                {
                    response.SetFailed("不存在");
                    return Ok(response);
                }

                entity.Unit = model.Unit;
                entity.UnitName = model.UnitName;
                entity.Require = model.Require;
                entity.Site = model.Site;               
                if (entity.Num != model.Num)
                {
                    var passcount = _dbContext.PostJobsAppeal.Count(x => x.PostUuid == entity.PostUuid && x.State == 1);
                    if(passcount< model.Num)
                    {
                        entity.FullState = 0;
                    }
                    else
                    {
                        entity.FullState = 1;
                    }
                }
                entity.Num = model.Num;
                entity.ReleaseState = model.ReleaseState;

                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }
        #endregion
        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }
        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">ID,多个以逗号分隔</param>
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

                default:
                    break;
            }
            return Ok(response);
        }
        #region 私有方法

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE Postjobs SET IsDelete=@IsDeleted WHERE PostUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        #endregion
        #endregion
    }
}
