using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.WorkStudy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.WorkStudy
{
    [Route("api/v1/workstudy/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class PostjobsAppealController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public PostjobsAppealController(haikanSDMSContext dbContext, IMapper mapper)
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
        public IActionResult List(PostJobsAppealRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from p in _dbContext.PostJobsAppeal
                            select new
                            {
                                p.PostJobsAppealUuid,
                                p.PostUu.Unit,
                                p.PostUu.UnitName,
                                p.PostUu.SchoolUuid,
                                p.StuName,
                                p.Sex,
                                p.Grade,
                                p.Class,
                                p.PoorState,
                                p.GuardianName,
                                p.GuardianPhone,
                                p.AddTime,
                                p.State
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.StuName.Contains(payload.Kw.Trim())|| x.Unit.Contains(payload.Kw.Trim())|| x.UnitName.Contains(payload.Kw.Trim()));
                }
                if(payload.State>-1)
                {
                    query = query.Where(x => x.State == payload.State);
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
        #region 审核
        /// <summary>
        /// 审核_通过
        /// </summary>
        /// <param name="guid">惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult AuditPass(Guid guid)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                var entity = _dbContext.PostJobsAppeal.FirstOrDefault(x => x.PostJobsAppealUuid == guid);

                if(entity==null)
                {
                    response.SetFailed("未查找到该数据");
                    return Ok(response);
                }
                if(entity.State!=0)
                {
                    response.SetFailed("不处于待审核状态");
                    return Ok(response);
                }
                var query = _dbContext.Postjobs.FirstOrDefault(x=>x.PostUuid==entity.PostUuid);
                if(query==null)
                {
                    response.SetFailed("未查找对应岗位信息");
                    return Ok(response);
                }
                var passcount = _dbContext.PostJobsAppeal.Count(x=>x.PostUuid==query.PostUuid&& x.State==1); 
                if(passcount>=query.Num)
                {
                    response.SetFailed("招收人数已满");
                    return Ok(response);
                }
                if (passcount == query.Num-1)
                {
                    query.FullState = 1;
                }

                entity.State = 1;
                _dbContext.SaveChanges();


                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 审核_不通过
        /// </summary>
        /// <param name="guid">惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult AuditNoPass(Guid guid)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                var entity = _dbContext.PostJobsAppeal.FirstOrDefault(x => x.PostJobsAppealUuid == guid);

                if (entity == null)
                {
                    response.SetFailed("未查找到该数据");
                    return Ok(response);
                }
                if (entity.State != 0)
                {
                    response.SetFailed("不处于待审核状态");
                    return Ok(response);
                }
                entity.State = 2;
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }
        #endregion
    }
}
