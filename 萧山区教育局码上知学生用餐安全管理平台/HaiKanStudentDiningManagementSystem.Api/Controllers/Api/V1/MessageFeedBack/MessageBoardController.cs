using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.MessageFeedBack;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.MessageFeedBack
{
    [Route("api/v1/messagefeedback/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class MessageBoardController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public MessageBoardController(haikanSDMSContext dbContext, IMapper mapper)
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
        public IActionResult List(MessageBoardRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from p in _dbContext.MessageBoard
                            select new
                            {
                                p.MessageUuid,
                                p.Content,
                                p.Type,
                                p.MessageDate,
                                p.Response,
                                p.ResponseType,
                                p.ResponseDate,
                                p.State,
                                p.SchoolUuid,
                                p.SchoolUu.SchoolName
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Type.Contains(payload.Kw.Trim()) || x.Content.Contains(payload.Kw.Trim()));
                }
               
                if (payload.State > -1)
                {
                    query = query.Where(x => x.State == payload.State);
                }
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                query = query.OrderByDescending(x => x.MessageDate);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();

                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        #endregion
        #region 回复
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
                var entity = _dbContext.MessageBoard.FirstOrDefault(x => x.MessageUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<MessageBoard, MessageBoardViewModel>(entity));
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
        public IActionResult Edit(MessageBoardViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _dbContext.MessageBoard.FirstOrDefault(x => x.MessageUuid == model.MessageUuid);
                if (entity == null)
                {
                    response.SetFailed("不存在");
                    return Ok(response);
                }

                entity.Response = model.Response;
                entity.ResponseDate = DateTime.Now;
                entity.State = 1;

                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }
        #endregion
    }
}
