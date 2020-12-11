using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.DataAccess;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Class;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.SchoolLife;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Class;
using Microsoft.EntityFrameworkCore;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Class
{
    [Route("api/v1/Class/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class ClassController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        //private IWebHostEnvironment _hostingEnvironment;

        public ClassController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            //_hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 班级列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(ClassRequestpayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.Class.Include(x => x.SchoolUu).Where(x => x.IsDelete == 0).Select(x => new
                {
                    x.ClassName,
                    x.ClassUuid,
                    x.Id,
                    x.IsDelete,
                    x.SchoolUuid,
                    x.SchoolUu.SchoolName,
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (payload.Sc!=null)
                {
                    query = query.Where(x => x.SchoolUuid
                      == payload.Sc);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.ClassName.Contains(payload.Kw));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);

                return Ok(response);
            }
        }

        /// <summary>
        /// 添加学校
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(ClassViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.Class.Any(x => x.IsDelete == 0 && x.ClassName == model.ClassName&&x.SchoolUuid== AuthContextService.CurrentUser.SchoolGuid))
                {
                    response.SetFailed("已存在该班级名称");
                    return Ok(response);
                }
                var entity = new Entities.Class();
                entity.ClassUuid = Guid.NewGuid();
                //entity.SchoolUuid = model.SchoolUuid;
                entity.IsDelete = 0;
                entity.ClassName = model.ClassName;
                entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                _dbContext.Class.Add(entity);
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
        public IActionResult Edit(ClassViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Class.FirstOrDefault(x => x.ClassUuid == model.ClassUuid&&x.IsDelete==0);
                if (entity == null)
                {
                    response.SetFailed("该班级不存在");
                    return Ok(response);
                }
                if (entity.ClassName != model.ClassName && _dbContext.Class.Any(x => x.IsDelete == 0 && x.ClassName == model.ClassName && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid))
                {
                    
                        response.SetFailed("已存在该班级名称");
                        return Ok(response);
                    
                }
                entity.ClassName = model.ClassName;
                var num = _dbContext.SaveChanges();
                if (num > 0)
                {
                    response.SetSuccess("修改成功");
                }
                else
                {
                    response.SetFailed("未修改");
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询单条班级信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Load(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Class.Include(x => x.SchoolUu).Select(x => new
                {
                    x.Id,
                    x.ClassName,
                    x.ClassUuid,
                    x.SchoolUuid,
                    x.IsDelete,
                    x.SchoolUu.SchoolName,
                }).FirstOrDefault(x => x.ClassUuid == guid);
                
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }


        /// <summary>
        /// 删除班级
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }

        /// <summary>
        /// 批量
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids"></param>
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
                //case "forbidden":
                //    if (ConfigurationManager.AppSettings.IsTrialVersion)
                //    {
                //        response.SetIsTrial();
                //        return Ok(response);
                //    }
                //    response = UpdateStatus(UserStatus.Forbidden, ids);
                //    break;
                //case "normal":
                //    response = UpdateStatus(UserStatus.Normal, ids);
                //    break;
                default:
                    break;
            }
            return Ok(response);
        }



        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="isDelete"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDelete, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE Class SET IsDelete=@IsDelete WHERE ClassUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDelete));
                var num = _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
    }
}
