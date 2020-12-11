using System;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Extensions.DataAccess;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.DiningRoom;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Process;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Process;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.DiningRoom
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/v1/DiningRoom/[controller]/[action]")]
    [ApiController]
    public class KitchenVideoController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostEnv;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        /// <param name="env"></param>
        public KitchenVideoController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnv = env;
        }

        /// <summary>
        /// 厨房食品列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public IActionResult List(DiningRoomRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.KitchenVideo.Where(x => x.IsDelete == 0).Include(x => x.SchoolUu).Select(x => new
                {
                    x.Id,
                    x.VideoUuid,
                    x.Name,
                    x.Type,
                    x.AddTime,
                    x.AddPeople,
                    x.Accessory,
                    x.IsDelete,
                    x.SchoolUuid,
                    x.SchoolUu.SchoolName,
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Name.Contains(payload.Kw));
                }
                if (payload.Kw2 != null && !string.IsNullOrEmpty(payload.Kw2[0]) && !string.IsNullOrEmpty(payload.Kw2[1]))
                {
                    var start = Convert.ToDateTime(payload.Kw2[0]);
                    var end = Convert.ToDateTime(payload.Kw2[1]);
                    query = query.AsEnumerable().Where(x => Convert.ToDateTime(x.AddTime) >= start && Convert.ToDateTime(x.AddTime) < end).AsQueryable();
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
        /// 添加视频信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(DiningRoomViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var kitchenVideo = new Entities.KitchenVideo()
                {
                    VideoUuid = Guid.NewGuid(),
                    Name = model.Name,
                    AddPeople = model.AddPeople,
                    //AddTime = DateTime.Parse(model.AddTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Type = model.Type,
                    IsDelete = 0,
                    Accessory = model.Accessory,
                    SchoolUuid = model.SchoolUuid != null ? model.SchoolUuid : AuthContextService.CurrentUser.SchoolGuid,
                };
                _dbContext.KitchenVideo.Add(kitchenVideo);
                var num = _dbContext.SaveChanges();
                if (num > 0)
                {
                    response.SetSuccess("添加成功");
                }
                else
                {
                    response.SetFailed("添加失败");
                }

                return Ok(response);
            }

        }

        /// <summary>
        /// 编辑视频信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.KitchenVideo.Include(x=>x.SchoolUu).Select(x=>new
                {
                    x.Id,
                    x.VideoUuid,
                    x.Name,
                    x.Accessory,
                    x.AddTime,
                    x.AddPeople,
                    x.IsDelete,
                    x.SchoolUu.SchoolName,
                    x.SchoolUuid,
                    x.Type,
                }).FirstOrDefault(x => x.VideoUuid == guid);
                var path = Path.Combine(_hostEnv.WebRootPath, entity.Accessory);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(new { entity, path });
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的视频信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize]
        public IActionResult Edit(DiningRoomViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.KitchenVideo.FirstOrDefault(x => x.VideoUuid == model.VideoUuid);
                if (entity == null)
                {
                    response.SetFailed("该视频不存在");
                    return Ok(response);
                }
                entity.Name = model.Name;
                entity.Type = model.Type;
                entity.Accessory = model.Accessory;
                if (string.IsNullOrEmpty(model.AddTime.Trim()))
                {
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    entity.AddTime = DateTime.Parse(model.AddTime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                
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
        /// 删除视频
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        [CustomAuthorize]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
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
        [CustomAuthorize]
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
        /// 视频上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> UpLoadAsync([FromForm] string filename)
        {
            var response = ResponseModelFactory.CreateInstance;
            var check = System.IO.File.Exists(filename);
            if ((!string.IsNullOrEmpty(filename)) && System.IO.File.Exists(filename))
            {
                FileInfo info = new FileInfo(filename);
                if (info.Attributes != FileAttributes.ReadOnly)
                {
                    System.IO.File.Delete(filename);
                }
            }
            var files = Request.Form.Files;
            if (files == null || files.Count() <= 0)
            {
                response.SetFailed("请上传视频文件");
                return Ok(response);
            }
            try
            {
                //foreach (var file in files)
                //{
                int index = files[0].FileName.LastIndexOf('.');
                string fname = Guid.NewGuid().ToString() + files[0].FileName.Substring(index);
                string strpath = Path.Combine("UploadFiles/Video", DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + fname);
                string path = Path.Combine(_hostEnv.WebRootPath, strpath);
                //System.IO.File.Create(path);
                var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                await files[0].CopyToAsync(stream);

                stream.Close();

                //}
                response.SetData(new { path, DataPath = strpath });
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetFailed(ex.Message);
                return Ok(response);
            }


        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteFile(VideoFile vf)
        {
            var response = ResponseModelFactory.CreateInstance;
            var path = Path.Combine(_hostEnv.WebRootPath, vf.Filename);
            if (string.IsNullOrEmpty(path))
            {
                response.SetFailed("无路径");
                return Ok(response);
            }
            try
            {
                if (System.IO.File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);
                    if (info.Attributes != FileAttributes.ReadOnly)
                    {
                        System.IO.File.Delete(path);
                        response.SetSuccess("删除成功");


                    }
                    else
                    {
                        response.SetFailed();
                    }

                }
                else
                {
                    response.SetFailed("文件不存在");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetFailed(ex.Message);
                return Ok(response);
            }




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
                var sql = string.Format("UPDATE KitchenVideo SET IsDelete=@IsDelete WHERE VideoUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDelete));
                var num = _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        /// <summary>
        /// 小程序视频列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult showVideo(string type,string uuid,string time)
        {

            var response = ResponseModelFactory.CreateResultInstance;
            if (string.IsNullOrEmpty(type))
            {
                response.SetFailed("类型为空");
                return Ok(response);
            }
            Guid suid = default;
            if(!Guid.TryParse(uuid,out suid))
            {
                response.SetFailed("uuid转换失败");
                return Ok(response);
            }
            using (_dbContext)
            {
                //var date = DateTime.Now.ToString("yyyy-MM-dd");
                //if (_dbContext.KitchenVideo.Any(x => x.AddTime.Substring(0, 10) == date && x.IsDelete == 0 && x.SchoolUuid == suid))
                //{
                //    var list = _dbContext.KitchenVideo.Where(x => x.IsDelete != 1 && x.Type == type && x.AddTime.Substring
                //(0, 10) == date && x.SchoolUuid == suid).OrderByDescending(x => x.AddTime).ToList();
                //    response.SetData(list);
                //}
                //else
                //{
                //    var query = _dbContext.KitchenVideo.Where(x => x.IsDelete != 1 && x.Type == type && x.SchoolUuid == suid).OrderByDescending(x => x.AddTime);
                //    var entity = query.First();
                //    if (entity != null)
                //    {
                //        var query2 = query.Where(x => x.AddTime.Substring
                //  (0, 10) == entity.AddTime.Substring
                //  (0, 10));
                //        var list = query2.ToList();
                //        response.SetData(list);
                //    }
                //    else
                //    {
                //        response.SetFailed("entity==null");
                //    }
                //}
                var list = _dbContext.KitchenVideo.Where(x => x.IsDelete != 1 && x.Type == type && x.SchoolUuid == suid && x.AddTime.Substring(0, 10) == time).OrderByDescending(x => x.AddTime).ToList();
                response.SetData(list);
                return Ok(response);
            }
        }


        /// <summary>
        /// 小程序视频列表
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[Microsoft.AspNetCore.Authorization.AllowAnonymous]
        //public IActionResult showVideo2(string type, string uuid)
        //{

        //    var response = ResponseModelFactory.CreateResultInstance;
        //    if (string.IsNullOrEmpty(type))
        //    {
        //        response.SetFailed("类型为空");
        //        return Ok(response);
        //    }
        //    Guid suid = default;
        //    if (!Guid.TryParse(uuid, out suid))
        //    {
        //        response.SetFailed("uuid转换失败");
        //        return Ok(response);
        //    }
        //    using (_dbContext)
        //    {
        //        var date = DateTime.Now.ToString("yyyy-MM-dd");
        //        if (_dbContext.KitchenVideo.Any(x => x.AddTime.Substring(0, 10) == date&&x.IsDelete==0 && x.SchoolUuid == suid))
        //        {
        //            var list = _dbContext.KitchenVideo.Where(x => x.IsDelete != 1 && x.Type == type && x.AddTime.Substring
        //        (0, 10) == date && x.SchoolUuid == suid).OrderByDescending(x => x.AddTime).ToList();
        //            response.SetData(new { list,date });
        //        }
        //        else
        //        {
        //            var query = _dbContext.KitchenVideo.Where(x => x.IsDelete != 1 && x.Type == type && x.SchoolUuid == suid).OrderByDescending(x => x.AddTime);
        //            var entity = query.First();
        //            if (entity != null)
        //            {
        //                var query2 = query.Where(x => x.AddTime.Substring
        //          (0, 10) == entity.AddTime.Substring
        //          (0, 10));
        //                var list = query2.ToList();
        //                response.SetData(new { list,entity,query= query.ToList() });
        //            }
        //            else
        //            {
        //                response.SetFailed("entity==null");
        //            }
        //        }

        //        return Ok(response);
        //    }
        //}
    }
}
