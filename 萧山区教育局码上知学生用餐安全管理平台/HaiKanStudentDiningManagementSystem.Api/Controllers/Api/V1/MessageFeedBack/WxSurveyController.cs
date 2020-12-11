using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.MessageFeedBack;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.MessageFeedBack
{
    [Route("api/v1/messagefeedback/[controller]/[action]")]
    [ApiController]
    public class WxSurveyController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public WxSurveyController(haikanSDMSContext dbContext, IMapper mapper)
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
        public IActionResult List(WxSurveyRequestPayload payload)
        {
            using (_dbContext)
            {
                var answer = _dbContext.SurveyAnswer.Where(x => x.AddPeople == payload.People&&x.IsDelete==0).Select(x=>x.SurveyUuid).ToList();

                var query = from s in _dbContext.Survey
                            where s.ProceedState==1&&s.ProductState==1&&s.SchoolUuid==payload.SchoolUuid&&s.IsDelete==0
                            && !answer.Contains(s.SurveyUuid) 
                            select new
                            {
                                s.SurveyUuid,
                                s.Headline,
                                s.Type,
                            };
             
              
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();

                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        #endregion
        #region 获取问卷详情
        /// <summary>
        /// 显示所有信息
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        public IActionResult SurveyDetail(Guid guid)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateResultInstance;

                var query = _dbContext.Survey.FirstOrDefault(x => x.SurveyUuid == guid);
                if(query==null)
                {
                    response.SetFailed("获取问卷失败");
                }
                SurveyDetail entity = new SurveyDetail();
                entity.SurveyUuid = query.SurveyUuid;
                entity.Headline = query.Headline;
                entity.Type = query.Type;
                entity.SurveyQuestionDetail = new List<SurveyQuestionDetail>();

                var question = _dbContext.SurveyQuestions.Where(x => x.SurveyUuid == query.SurveyUuid&&x.IsDelete==0).OrderBy(x=>x.Id);
                foreach(var item in question)
                {
                    SurveyQuestionDetail questiondetail = new SurveyQuestionDetail();
                    questiondetail.QuestionTitle = item.QuestionTitle;
                    questiondetail.IsMuti = item.IsMuti;
                    questiondetail.QuestionType = item.QuestionType;
                    questiondetail.SurveyQuestionItemDetail = new List<SurveyQuestionItemDetail>();
                    if (item.QuestionType==0)
                    {
                        var questionitem = _dbContext.SurveyQuestionsItems.Where(x => x.SurveyQuestionsUuid == item.SurveyQuestionsUuid && x.IsDelete == 0).OrderBy(x => x.Id);

                        foreach(var item1 in questionitem)
                        {
                            SurveyQuestionItemDetail questionitemdetail = new SurveyQuestionItemDetail();
                            questionitemdetail.Optionts = item1.Optionts;
                            questionitemdetail.QuestionStr = item1.QuestionStr;
                            questionitemdetail.checkbox = false;
                            questionitemdetail.disabled = false;
                            questiondetail.SurveyQuestionItemDetail.Add(questionitemdetail);
                        }
                    }
                    entity.SurveyQuestionDetail.Add(questiondetail);
                }
                response.SetData(entity);
                return Ok(response);
            }
        }
        #endregion
        #region 提交问卷答案
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(WxSurveyAnswerViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (_dbContext.SurveyAnswer.Count(x => x.SurveyUuid == model.SurveyUuid && x.AddPeople == model.AddPeople) > 0)
                {
                    response.SetFailed("请勿重复填写问卷");
                    return Ok(response);
                }
                var survey = _dbContext.Survey.FirstOrDefault(x=>x.SurveyUuid==model.SurveyUuid);
                if(survey==null)
                {
                    response.SetFailed("获取问卷信息失败");
                    return Ok(response);
                }
                survey.Number = survey.Number+1;

                var entity = _mapper.Map<WxSurveyAnswerViewModel, SurveyAnswer>(model);
                entity.SurveyAnswerUuid = Guid.NewGuid();
                entity.IsDelete = 0;

                _dbContext.SurveyAnswer.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }
        #endregion
    }
}
