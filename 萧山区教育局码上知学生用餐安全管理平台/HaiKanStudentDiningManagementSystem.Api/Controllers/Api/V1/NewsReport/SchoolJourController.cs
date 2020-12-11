using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.SchoolJour;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.SchoolJour;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaiKan3.Utils;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.NewsReport
{
    /// <summary>
    /// 权限控制器
    /// </summary>
    [Route("api/v1/NewsReport/[controller]/[action]")]
    [ApiController]
    public class SchoolJourController : Controller
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public SchoolJourController(haikanSDMSContext dbContext, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;

        }

        /// <summary>
        /// 菜品库列表显示
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public IActionResult SchoolJourList(SchoolJourRequestpayload payload)
        {
            using (_dbContext)
            {
                var query = from p in _dbContext.SchoolJour
                            where p.IsDelete == 0
                            select new
                            {
                                SchoolJourUuid= p.SchoolJourUuid,
                                SchoolUuid=p.SchoolUuid,
                                Headline=p.Headline,
                                //Content=p.Content,
                                AddPeople=p.AddPeople,
                                Addtime=p.Addtime,
                                p.Digest,
                                //Start=p.Start.ToString()=="0"?"保存中":"已发布",
                                IsDelete=p.IsDelete,
                                //Accessory=p.Accessory,
                                p.Id,
                                p.Tag
                            };
                if (!string.IsNullOrEmpty(payload.kw.Trim()))
                {
                    query = query.Where(x => x.Headline.Contains(payload.kw));
                }
                if (!string.IsNullOrEmpty(payload.kw1.Trim()))
                {
                    query = query.Where(x => x.Tag.Contains(payload.kw1));
                }
                //判断444
                if (!string.IsNullOrEmpty(payload.schoolguid))
                {
                    query = query.Where(x => x.SchoolUuid == Guid.Parse(payload.schoolguid));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.Id);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取学校信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SchoolList(string guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.School.Where(x => x.IsDelete != 1);
                if (guid != "null")
                {
                    entity = entity.Where(x => x.SchoolUuid == Guid.Parse(guid));
                }
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity.ToList());
                return Ok(response);
            }
        }

        //通过标题查询新闻
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetSchoolJour(string Headline)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SchoolJour.FirstOrDefault(x => x.Headline == Headline);
                var response = ResponseModelFactory.CreateInstance;
                response.SetSuccess();
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(SchoolJourViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                

                var entity = new SchoolJour();
                entity.SchoolJourUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                entity.Headline = model.Headline;
                entity.SchoolUuid = model.SchoolUuid;
                entity.Content  = model.Content;
                entity.Accessory = model.Accessory;
                entity.Digest = model.Digest;
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.Addtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.Start = "0";
                entity.Tag = model.Tag;
                //entity.SchoolUuid = Guid.NewGuid();
                _dbContext.SchoolJour.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SchoolJourEdit(SchoolJourViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.SchoolJour.FirstOrDefault(x => x.SchoolJourUuid == model.SchoolJourUuid);
                entity.Headline = model.Headline;
                entity.Content = model.Content;
                entity.Tag = model.Tag;
                entity.Digest = model.Digest;
                //entity.Addtime = model.Addtime.ToString();
                entity.Accessory = model.Accessory;
                _dbContext.SaveChanges();
                response.SetSuccess("修改成功");
                return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult SchoolJourGet(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SchoolJour.Where(x => x.SchoolJourUuid == guid && x.IsDelete != 1).FirstOrDefault();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除单个角色
        /// </summary>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.SchoolJour.FirstOrDefault(x => x.SchoolJourUuid.ToString() == ids);
                entity.IsDelete = 1;//删除状态
                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除多条批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (command == "delete")
                {

                    var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    //foreach (var item in parameters)
                    //{
                    //    var query1 = _dbContext.SchoolJour.FirstOrDefault(x => x.SchoolJourUuid == Guid.Parse(item.Value.ToString()));
                    //    query1.IsDelete = 1;
                    //    _dbContext.SaveChanges();
                    //}
                    var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                    var sql = string.Format("UPDATE SchoolJour SET IsDelete=@IsDelete WHERE SchoolJourUuid IN ({0})", parameterNames);
                    parameters.Add(new SqlParameter("@IsDelete", 1));
                    _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                }
                return Ok(response);
            }
        }
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> UpLoadAsync([FromForm] Guid uuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            //var check = System.IO.File.Exists(filename);

            var files = Request.Form.Files;
            if (files == null || files.Count() <= 0)
            {
                response.SetFailed("请上图片文件");
                return Ok(response);
            }
            //var paths = new List<string>();
            //var dataPaths = new List<string>();
            var allowType = new string[] { "image/jpeg", "image/png" };
            var message = "";
            try
            {
                if (files.Any(c => allowType.Contains(c.ContentType)))
                {

                    //foreach (var file in files)
                    //{
                    if (files[0].Length > 1024 * 1024 * 5)
                    {
                        message += files[0].FileName + "大小超过5M";
                        response.SetFailed(message);
                        return Ok(response);
                    }
                    int index = files[0].FileName.LastIndexOf('.');
                    string prefix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + Guid.NewGuid().ToString();
                    string fname = prefix+ files[0].FileName.Substring(index);
                    string strpath = Path.Combine("UploadFiles/Picture", fname);
                    string path = Path.Combine(_hostingEnvironment.WebRootPath, strpath);
                    //System.IO.File.Create(path);
                    var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    await files[0].CopyToAsync(stream);

                    stream.Close();

                    bool toCompression = false;
                    string fname2 = "";
                    string strpath2 = "";

                    if (files[0].Length > (1024 * 1024 * 1))
                    {
                        fname2 = prefix + "_cp" + files[0].FileName.Substring(index);
                        strpath2 = Path.Combine("UploadFiles/Picture", fname2);
                        var dFile = Path.Combine(_hostingEnvironment.WebRootPath, strpath2);
                        var num = 1024 * 1024 * 1.0 / files[0].Length;
                        int flag = (int)(num * 100);
                        if (flag < 50)
                        {
                            flag = (int)(flag * 1.7);
                        }
                        toCompression = PhotoCompression.CompressImage(path, dFile, flag, 1024);
                        if (toCompression)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                FileInfo info = new FileInfo(path);
                                if (info.Attributes != FileAttributes.ReadOnly)
                                {
                                    System.IO.File.Delete(path);

                                }

                            }
                        }
                    }
                    else if (files[0].Length > (1024 * 1024 * 0.5) && files[0].Length <= (1024 * 1024 * 1))
                    {
                        fname2 = prefix + "_cp" + files[0].FileName.Substring(index);
                        strpath2 = Path.Combine("UploadFiles/Picture", fname2);
                        var dFile = Path.Combine(_hostingEnvironment.WebRootPath, strpath2);

                        int flag = 90;

                        toCompression = PhotoCompression.CompressImage(path, dFile, flag, 500);
                        if (toCompression)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                FileInfo info = new FileInfo(path);
                                if (info.Attributes != FileAttributes.ReadOnly)
                                {
                                    System.IO.File.Delete(path);

                                }

                            }
                        }
                    }
                    if (toCompression)
                    {
                        response.SetData(new { strpath = strpath2, fname = fname2 });

                    }
                    else
                    {
                        response.SetData(new { strpath, fname });
                    }

                }
                if (message.Length > 0)
                {
                    response.SetFailed(message);
                }
                //response.SetData(new { paths, dataPaths });
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetFailed(ex.Message);
                return Ok(response);
            }


        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeletetoFile(LSPicture lsp)
        {
            var response = ResponseModelFactory.CreateInstance;

            var path = Path.Combine(_hostingEnvironment.WebRootPath + "/UploadFiles/Picture", lsp.Path);
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
                        response.SetFailed("只读文件");
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
    }
}