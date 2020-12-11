using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.MYEntities;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.General;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.UserInfo
{
    [Route("api/v1/UserInfo/[controller]/[action]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;


        public UserInfoController(haikanSDMSContext dbContext, IMapper mapper, haikanITMContext dbITMContext)
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
        public IActionResult GetUserList(GeneralRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                var school = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == payload.Kw);
                var query = _dbITMContext.Users.Where(x=>x.OrganizationId==school.OrganizationId && x.DutyId!=0 && x.DutyId!=null);
                var usernum = query.Count();
                var user = query.Where(x => x.DutyId != 118);
                var gly = query.Where(x => x.DutyId == 118);
                var noglynum = usernum - gly.Count();
                var reality = user.Where(x=>x.HealthStatus==1).Count();
                if (!string.IsNullOrEmpty(payload.kw1))
                {
                    user = user.Where(x => x.Name.Contains(payload.kw1));
                    gly = gly.Where(x => x.Name.Contains(payload.kw1));
                }
                user = user.OrderBy(x => x.Name);
                gly = gly.OrderBy(x => x.Name);
                var userlist = user.ToList();
                var glylist = gly.ToList();
                response.SetData(new { userlist, glylist , usernum, noglynum, reality });
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserInfo(int id)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                //var query = _dbITMContext.Users.FirstOrDefault(x => x.Id.ToString()==id);
                var query = from u in _dbITMContext.Users
                            join d in _dbITMContext.Departments
                            on u.DepartmentId.ToString() equals d.Id.ToString()
                            where u.Id.ToString() == id.ToString()
                            select new{
                                u.Name,
                                Sex=u.Sex==1?"女":"男",
                                u.HealthTypeId,
                                u.EducationId,
                                u.Phone,
                                u.Idcard,
                                u.DutyName,
                                DName=d.Name,
                                u.TeamGroupName,
                                Ishealth=u.DutyName=="管理员"?"否":"是",
                                u.HealthNum,
                                //jkzKeyValue = t2.KeyValue,
                                Healthdate= u.HealthBeginDate!=null && u.HealthEndDate != null ? Convert.ToDateTime(u.HealthBeginDate).ToString("yyyy-MM-dd")+"至"+ Convert.ToDateTime(u.HealthEndDate).ToString("yyyy-MM-dd"):"",
                                u.HealthFrom,
                                u.HealthImg
                            };
                var entity = query.FirstOrDefault();
                var whcdKeyValue= _dbITMContext.Types.FirstOrDefault(x => x.TypeId == entity.EducationId).KeyValue;
                var jkzKeyValue = _dbITMContext.Types.FirstOrDefault(x => x.TypeId == entity.HealthTypeId).KeyValue;
                response.SetData(new { entity,whcdKeyValue,jkzKeyValue});
                return Ok(response);
            }
        }

        private List<Types> Gettype(string types)
        {
            var query = _dbITMContext.Types.Where(x=>x.Type==types).ToList();
            return query;
        }
    }
}
