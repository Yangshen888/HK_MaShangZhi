using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.MessageFeedBack;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.MessageFeedBack
{
    [Route("api/v1/messagefeedback/[controller]/[action]")]
    [ApiController]
    public class WxMessageBoardController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public WxMessageBoardController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        #region 创建
        /// <summary>
        /// 留言板创建
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(WxMessageBoardViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _mapper.Map<WxMessageBoardViewModel, MessageBoard>(model);
                entity.MessageUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                entity.State = 0;
                entity.MessageDate = DateTime.Now;
                _dbContext.MessageBoard.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 举报信箱创建
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult CreateReport(WxReportViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _mapper.Map<WxReportViewModel, Report>(model);
                entity.ReportUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                entity.State = 0;
                _dbContext.Report.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }
        #endregion
        #region 留言反馈列表
        /// <summary>
        /// 显示所有可以申请的岗位
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(WxMessageBackRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from p in _dbContext.MessageBoard
                            where p.IsDelete == 0 &&p.SchoolUuid==payload.schoolUuid && p.AddPeople==payload.people
                            orderby p.MessageDate descending
                            select new
                            {
                                p.Content,
                                p.Type,
                                p.MessageDate,
                                p.Response,
                                p.ResponseDate,
                            };
                
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        #endregion
    }
}
