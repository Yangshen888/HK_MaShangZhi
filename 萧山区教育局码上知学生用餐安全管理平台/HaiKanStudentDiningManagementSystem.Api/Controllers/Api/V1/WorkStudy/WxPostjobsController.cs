using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.WorkStudy;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.WorkStudy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.WorkStudy
{
    [Route("api/v1/workstudy/[controller]/[action]")]
    [ApiController]
    public class WxPostjobsController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public WxPostjobsController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        #region 岗位列表
        /// <summary>
        /// 显示所有可以申请的岗位
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(WxPostJobsRequestPayload payload)
        {
            using (_dbContext)
            {
                var appeal = _dbContext.PostJobsAppeal.Where(x => x.AppealPeople == payload.appealPeople).Select(x => x.PostUuid).ToList();

                var query = from p in _dbContext.Postjobs
                            where p.IsDelete==0 && p.ReleaseState==1 && p.FullState==0&&p.SchoolUuid == Guid.Parse(payload.SchoolUuid)
                            && !appeal.Contains(p.PostUuid)
                            select new WxPostjobsViewModel
                            {
                                PostUuid=p.PostUuid,
                                Unit=p.Unit,
                                UnitName = p.UnitName,
                                Site = p.Site,
                                Require = p.Require,
                                AddTime = p.AddTime,
                                Num = p.Num,
                            };
                
                //if (!string.IsNullOrEmpty(payload.SchoolUuid))
                //{
                //    query = query.Where(x => x.SchoolUuid == Guid.Parse(payload.SchoolUuid));
                //}
                query = query.OrderByDescending(x => x.AddTime);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();

                foreach(var item in list)
                {
                    var count = _dbContext.PostJobsAppeal.Count(x=>x.PostUuid==item.PostUuid&&x.State==1);
                    item.PassNum = count;
                }
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 显示所有申请记录
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppealList(WxPostJobsAppealRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from p in _dbContext.PostJobsAppeal
                            where p.AppealPeople==payload.AppealPeople
                            select new
                            {
                                p.StuName,
                                p.PostUu.Unit,
                                p.PostUu.UnitName,
                                p.PostUu.SchoolUuid,
                                p.State,
                                p.AddTime
                            };
                
                if (payload.SchoolUuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == payload.SchoolUuid);
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
        #region 岗位详情
        /// <summary>
        /// 岗位详情
        /// </summary>
        /// <param name="guid">惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        public IActionResult PostjobsDetial(Guid guid)
        {
            using (_dbContext)
            {
                var entity = (from p in _dbContext.Postjobs
                                         where p.PostUuid==guid 
                                         select new
                                         {
                                             PostUuid = p.PostUuid,
                                             Unit = p.Unit,
                                             UnitName = p.UnitName,
                                             Site = p.Site,
                                             Require = p.Require,
                                             AddTime = p.AddTime,
                                             Num = p.Num,
                                         }).FirstOrDefault(); 
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
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
        public IActionResult Create(WxPostjobsAppealViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
           
            using (_dbContext)
            {
                if (_dbContext.PostJobsAppeal.Count(x => x.PostUuid == model.PostUuid && x.AppealPeople == model.AppealPeople) > 0)
                {
                    response.SetFailed("已申请该岗位，请勿重复申请");
                    return Ok(response);
                }

                var entity = _mapper.Map<WxPostjobsAppealViewModel, PostJobsAppeal>(model);
                entity.PostJobsAppealUuid = Guid.NewGuid();
                entity.State = 0;
                entity.AddTime = DateTime.Now;
                _dbContext.PostJobsAppeal.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }
        #endregion
    }
}
