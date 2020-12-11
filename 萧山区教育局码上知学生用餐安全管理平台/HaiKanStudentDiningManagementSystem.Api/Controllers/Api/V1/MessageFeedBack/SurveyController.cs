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
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.MessageFeedBack;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.MessageFeedBack
{
    [Route("api/v1/messagefeedback/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class SurveyController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public SurveyController(haikanSDMSContext dbContext, IMapper mapper)
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
        public IActionResult List(SurveyRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from s in _dbContext.Survey
                            select new
                            {
                                s.SurveyUuid,
                                s.SchoolUuid,
                                s.SchoolUu.SchoolName,
                                s.Headline,
                                s.Type,
                                s.BeginTime,
                                s.EndTime,
                                s.ProceedState,
                                s.ProductState,
                                s.Number,
                                s.IsDelete
                            };
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Headline.Contains(payload.Kw.Trim()) || x.Type.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => x.IsDelete == Convert.ToInt32(((CommonEnum.IsDeleted)payload.IsDeleted)));
                }
                if (payload.ProceedState > -1)
                {
                    query = query.Where(x => x.ProceedState == payload.ProceedState);
                }
                if (payload.ProductState > -1)
                {
                    query = query.Where(x => x.ProductState == payload.ProductState);
                }
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();

                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 显示所有信息
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        public IActionResult ListQuestion(Guid guid)
        {
            using (_dbContext)
            {
                var query = from s in _dbContext.SurveyQuestions
                            where s.SurveyUuid == guid && s.IsDelete == 0
                            orderby s.Id
                            select new
                            {
                                s.SurveyQuestionsUuid,
                                s.SurveyUuid,
                                s.QuestionTitle,
                                s.IsMuti,
                                s.QuestionType
                            };

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query.ToList());
                return Ok(response);
            }
        }
        /// <summary>
        /// 显示所有信息
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        public IActionResult ListQuestionItem(Guid guid)
        {
            using (_dbContext)
            {
                var query = from s in _dbContext.SurveyQuestionsItems
                            where s.SurveyQuestionsUuid == guid && s.IsDelete == 0
                            orderby s.Id
                            select new
                            {
                                s.SurveyQuestionsItemsUuid,
                                s.Optionts,
                                s.QuestionStr
                            };
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query.ToList());
                return Ok(response);
            }
        }
        #endregion
        #region 创建
        /// <summary>
        /// 问卷创建
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(SurveyViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
                {
                    response.SetFailed("请登录学校账号");
                    return Ok(response);
                }
                var entity = _mapper.Map<SurveyViewModel, Survey>(model);
                entity.SurveyUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                entity.Number = 0;
                entity.ProceedState = 0;
                entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                _dbContext.Survey.Add(entity);
                _dbContext.SaveChanges();

                response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
                {
                    SurveyUuid = entity.SurveyUuid
                })));
                return Ok(response);
            }
        }
        /// <summary>
        /// 问卷题目创建
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult CreateQuestion(SurveyQuestionsViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _mapper.Map<SurveyQuestionsViewModel, SurveyQuestions>(model);
                entity.SurveyQuestionsUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                //entity.AddPeople = "";
                //entity.SchoolUuid = "";
                _dbContext.SurveyQuestions.Add(entity);
                _dbContext.SaveChanges();

                response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
                {
                    SurveyQuestionsUuid = entity.SurveyQuestionsUuid
                })));
                return Ok(response);
            }
        }
        /// <summary>
        /// 问卷题目选项创建
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult CreateQuestionItem(SurveyQuestionsItemsViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _mapper.Map<SurveyQuestionsItemsViewModel, SurveyQuestionsItems>(model);
                entity.SurveyQuestionsItemsUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                //entity.AddPeople = "";
                //entity.SchoolUuid = "";
                _dbContext.SurveyQuestionsItems.Add(entity);
                _dbContext.SaveChanges();

                response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
                {
                    SurveyQuestionsUuid = entity.SurveyQuestionsUuid
                })));
                return Ok(response);
            }
        }
        #endregion
        #region 编辑
        /// <summary>
        /// 开始/结束问卷
        /// </summary>
        /// <param name="guid">惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult OpenOrClose(Guid guid)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                var entity = _dbContext.Survey.FirstOrDefault(x => x.SurveyUuid == guid);
                if (entity == null)
                {
                    response.SetFailed();
                    return Ok(response);
                }
                if (entity.ProceedState == 0 || entity.ProceedState == 2)
                {
                    entity.ProceedState = 1;
                }
                else if (entity.ProceedState == 1)
                {
                    entity.ProceedState = 2;
                }
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
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
                var entity = _dbContext.Survey.FirstOrDefault(x => x.SurveyUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<Survey, SurveyViewModel>(entity));
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
        public IActionResult Edit(SurveyViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _dbContext.Survey.FirstOrDefault(x => x.SurveyUuid == model.SurveyUuid);
                if (entity == null)
                {
                    response.SetFailed("不存在");
                    return Ok(response);
                }
                entity.Headline = model.Headline;
                entity.Type = model.Type;
                entity.BeginTime = model.BeginTime;
                entity.EndTime = model.EndTime;
                entity.ProductState = model.ProductState;

                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑题目
        /// </summary>
        /// <param name="guid">惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult EditQuestion(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SurveyQuestions.FirstOrDefault(x => x.SurveyQuestionsUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<SurveyQuestions, SurveyQuestionsViewModel>(entity));
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的题目信息
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EditQuestion(SurveyQuestionsViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _dbContext.SurveyQuestions.FirstOrDefault(x => x.SurveyQuestionsUuid == model.SurveyQuestionsUuid);
                if (entity == null)
                {
                    response.SetFailed("不存在");
                    return Ok(response);
                }
                entity.QuestionTitle = model.QuestionTitle;
                entity.IsMuti = model.IsMuti;
                entity.QuestionType = model.QuestionType;
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑题目
        /// </summary>
        /// <param name="guid">惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult EditQuestionItem(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SurveyQuestionsItems.FirstOrDefault(x => x.SurveyQuestionsItemsUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<SurveyQuestionsItems, SurveyQuestionsItemsViewModel>(entity));
                return Ok(response);
            }
        }
        /// <summary>
        /// 保存编辑后的选项信息
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EditQuestionItem(SurveyQuestionsItemsViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _dbContext.SurveyQuestionsItems.FirstOrDefault(x => x.SurveyQuestionsItemsUuid == model.SurveyQuestionsItemsUuid);
                if (entity == null)
                {
                    response.SetFailed("不存在");
                    return Ok(response);
                }
                entity.QuestionStr = model.QuestionStr;
                entity.Optionts = model.Optionts;
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
        /// 删除
        /// </summary>
        /// <param name="ids">ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteQuestion(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            response = UpdateIsDeleteQuestion(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult DeleteQuestionItem(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            response = UpdateIsDeleteQuestionItem(CommonEnum.IsDeleted.Yes, ids);
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
                var sql = string.Format("UPDATE Survey SET IsDelete=@IsDeleted WHERE SurveyUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDeleteQuestion(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE SurveyQuestions SET IsDelete=@IsDeleted WHERE SurveyQuestionsUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDeleteQuestionItem(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE SurveyQuestionsItems SET IsDelete=@IsDeleted WHERE SurveyQuestionsItemsUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        #endregion
        #endregion
        #region 获取问卷统计详情
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
                if (query == null)
                {
                    response.SetFailed("获取问卷失败");
                }
                SurveyDetail entity = new SurveyDetail();
                entity.SurveyUuid = query.SurveyUuid;
                entity.Headline = query.Headline;
                entity.Type = query.Type;
                entity.SurveyQuestionDetail = new List<SurveyQuestionDetail>();

                var answer = _dbContext.SurveyAnswer.Where(x => x.SurveyUuid == query.SurveyUuid).Select(x => x.AnswerStr).ToList();

                List<AnswerStatisticModel> AnswerStatistic = new List<AnswerStatisticModel>();

                foreach (var item in answer)
                {
                    AnswerStatisticModel Statistic = new AnswerStatisticModel();
                    string[] itemstr = item.Split("||");
                    List<AnswerListViewModel> answerlist = new List<AnswerListViewModel>();
                    for (int i = 0; i < itemstr.Length; i++)
                    {
                        AnswerListViewModel answercontent = new AnswerListViewModel();
                        answercontent.Content = itemstr[i];
                        answercontent.Multiple = new List<string>(itemstr[i].Split(','));
                        answerlist.Add(answercontent);
                    }
                    Statistic.answerlist = answerlist;
                    AnswerStatistic.Add(Statistic);
                }


                var question = _dbContext.SurveyQuestions.Where(x => x.SurveyUuid == query.SurveyUuid && x.IsDelete == 0).OrderBy(x => x.Id);
                int index = 0;
                foreach (var item in question)
                {
                    
                    SurveyQuestionDetail questiondetail = new SurveyQuestionDetail();
                    questiondetail.QuestionTitle = item.QuestionTitle;
                    questiondetail.IsMuti = item.IsMuti;
                    questiondetail.QuestionType = item.QuestionType;
                    questiondetail.SurveyQuestionItemDetail = new List<SurveyQuestionItemDetail>();
                    //选择题
                    if (item.QuestionType == 0)
                    {
                        var questionitem = _dbContext.SurveyQuestionsItems.Where(x => x.SurveyQuestionsUuid == item.SurveyQuestionsUuid && x.IsDelete == 0).OrderBy(x => x.Id);

                        foreach (var item1 in questionitem)
                        {
                            SurveyQuestionItemDetail questionitemdetail = new SurveyQuestionItemDetail();
                            questionitemdetail.Optionts = item1.Optionts;
                            questionitemdetail.QuestionStr = item1.QuestionStr;
                            questionitemdetail.checkbox = false;
                            questionitemdetail.disabled = false;

                            int chosenum = 0;
                            //单选
                            if(item.IsMuti==0)
                            {
                                for(int j=0;j< AnswerStatistic.Count;j++)
                                {
                                    if(AnswerStatistic[j].answerlist[index].Content== item1.Optionts)
                                    {
                                        chosenum++;
                                    }
                                }
                            }
                            //多选
                            else if (item.IsMuti == 1)
                            {
                                for (int j = 0; j < AnswerStatistic.Count; j++)
                                {
                                    if (AnswerStatistic[j].answerlist[index].Multiple.Contains(item1.Optionts))
                                    {
                                        chosenum++;
                                    }
                                }
                            }
                            questionitemdetail.num = chosenum;

                            questiondetail.SurveyQuestionItemDetail.Add(questionitemdetail);
                        }
                    }
                    //主观题
                    else if(item.QuestionType ==1)
                    {
                        string str = "";
                        for (int j = 0; j < AnswerStatistic.Count; j++)
                        {
                            if (AnswerStatistic[j].answerlist[index].Content!="")
                            {
                                str+= AnswerStatistic[j].answerlist[index].Content+" ";
                            }
                        }
                        questiondetail.SubText = str;
                    }
                    entity.SurveyQuestionDetail.Add(questiondetail);
                    index++;
                }
                response.SetData(entity);
                return Ok(response);
            }
        }
        #endregion
    }
}
