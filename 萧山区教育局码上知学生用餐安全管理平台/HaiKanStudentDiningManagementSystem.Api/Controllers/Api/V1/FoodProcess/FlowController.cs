using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using HaiKan3.Utils;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.MYEntities;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Ledger;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Process;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.FoodProcess
{
    [Route("api/v1/FoodProcess/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class FlowController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostEnv;

        public FlowController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment env,haikanITMContext dbITMContext)
        {
            _dbContext = dbContext;
            _dbITMContext = dbITMContext;
            _mapper = mapper;
            _hostEnv = env;
        }

        /// <summary>
        /// 流程列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(FlowRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.Flow.Where(x => x.IsDelete == 0);
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.FlowName.Contains(payload.Kw));
                }
                if (!string.IsNullOrEmpty(payload.kw1))
                {
                    query = query.Where(x => x.FlowTime.Contains(Convert.ToDateTime(payload.kw1).ToString("yyyy-MM-dd")));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.FlowTime);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        [HttpPost]
        public IActionResult Getscreen(FlowRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                var school = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == payload.Kw);
                if (school != null)
                {
                    //var query = from s in _dbITMContext.Screenshots
                    //             join m in _dbITMContext.Monitors
                    //             on s.MonitorId equals m.Id
                    //             where m.OrganizationId == school.OrganizationId
                    //             && s.CreatedAt.ToString().Contains(payload.kw1)
                    //             select new
                    //             {
                    //                 s.Id,
                    //                 Path = "https://img.jiulong.yoruan.com/" + s.Path,
                    //                 CreatedAt = s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                    //                 ispath = false,
                    //                 m.Procedure
                    //             };
                    var query = from s in _dbITMContext.Screenshots
                                 join m in _dbITMContext.Monitors
                                 on s.MonitorId equals m.Id
                                 where m.OrganizationId == school.OrganizationId
                                 && s.CreatedAt.ToString().Contains(payload.kw1)
                                 select new
                                 {
                                    s,
                                    m
                                 };
                    if (!string.IsNullOrEmpty(payload.kw2))
                    {
                        query = query.Where(x=>x.m.Procedure == payload.kw2);
                    }
                    //var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                    var list = query.ToList();
                    var menu = new List<dynamic>();
                    var list02 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 00:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 02:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time= "00:00-02:00"
                    });
                    var list24 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 02:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 04:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "02:00-04:00"
                    });
                    var list46 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 04:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 06:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "04:00-06:00"
                    });
                    var list68 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 06:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 08:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "06:00-8:00"
                    });
                    var list810 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 08:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 10:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "08:00-10:00"
                    });
                    var list1012 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 10:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 12:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "10:00-12:00"
                    });
                    var list1214 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 12:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 14:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "12:00-14:00"
                    });
                    var list1416 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 14:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 16:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "14:00-16:00"
                    });
                    var list1618 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 16:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 18:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "16:00-18:00"
                    });
                    var list1820 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 18:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 20:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "18:00-20:00"
                    });
                    var list2022 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 20:00") && x.s.CreatedAt <= Convert.ToDateTime(payload.kw1 + " 22:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "20:00-22:00"
                    });
                    var list2224 = list.Where(x => x.s.CreatedAt >= Convert.ToDateTime(payload.kw1 + " 22:00")).Select(x => new
                    {
                        x.s.Id,
                        Path = "https://img.jiulong.yoruan.com/" + x.s.Path,
                        CreatedAt = x.s.CreatedAt.Value.ToString("yyyy-MM-dd"),
                        ispath = false,
                        x.m.Procedure,
                        Time = "22:00-24:00"
                    });
                    menu.Add(list02);
                    menu.Add(list24);
                    menu.Add(list46);
                    menu.Add(list68);
                    menu.Add(list810);
                    menu.Add(list1012);
                    menu.Add(list1214);
                    menu.Add(list1416);
                    menu.Add(list1618);
                    menu.Add(list1820);
                    menu.Add(list2022);
                    menu.Add(list2224);
                    response.SetData(menu);
                    return Ok(response);
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 网络上传
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult wlcreate(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                string newtime = model.datetime;
                string name = model.flowName;
                Guid schooluuid = model.schoolUuid;
                var query = _dbContext.Flow.FirstOrDefault(x => x.FlowTime == newtime && x.FlowName == name && x.SchoolUuid == schooluuid);
                if (query == null)
                {
                    var entity = new Flow();
                    entity.FlowUuid = Guid.NewGuid();
                    entity.FlowName = model.flowName;
                    entity.FlowTime = model.datetime;
                    entity.IsDelete = 0;
                    entity.SchoolUuid = model.schoolUuid;
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.AddPeople = model.addPeople;
                    string accessory = model.accessory;
                    for (int i = 0; i < accessory.Split(',').Length; i++)
                    {
                        if (!string.IsNullOrEmpty(accessory.Split(',')[i]))
                        {
                            int index = accessory.Split(',')[i].LastIndexOf('.');
                            string suffix = accessory.Split(',')[i].Substring(index);
                            string infix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + Guid.NewGuid().ToString();
                            string filename = @"C:\inetpub\wwwroot\wwwroot\UploadFiles\RegistPicture\" + infix + suffix;
                            //string filename = @"D:\我的工作目录\work_萧山码上知\201013_14.40\萧山区教育局码上知学生用餐安全管理平台\HaiKanStudentDiningManagementSystem.Api\wwwroot\UploadFiles\RegistPicture\" + infix + suffix;
                            var check = WriteBytesToFile(filename, GetBytesFromUrl(accessory.Split(',')[i])) ;
                            if (check)
                            {
                                if (!string.IsNullOrEmpty(entity.Accessory))
                                {
                                    entity.Accessory +=","+ infix + suffix;
                                }
                                else
                                {
                                    entity.Accessory = infix + suffix;
                                }
                            }
                        }
                    }
                    _dbContext.Flow.Add(entity);
                    _dbContext.SaveChanges();
                }
                else
                {
                    string accessory = model.accessory;
                    for (int i = 0; i < accessory.Split(',').Length; i++)
                    {
                        if (!string.IsNullOrEmpty(accessory.Split(',')[i]))
                        {
                            int index = accessory.Split(',')[i].LastIndexOf('.');
                            string suffix = accessory.Split(',')[i].Substring(index);
                            string infix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + Guid.NewGuid().ToString();
                            string filename = @"C:\inetpub\wwwroot\wwwroot\UploadFiles\RegistPicture\" + infix + suffix;
                            //string filename = @"D:\我的工作目录\work_萧山码上知\201013_14.40\萧山区教育局码上知学生用餐安全管理平台\HaiKanStudentDiningManagementSystem.Api\wwwroot\UploadFiles\RegistPicture\" + infix + suffix;
                            var check = WriteBytesToFile(filename, GetBytesFromUrl(accessory.Split(',')[i]));
                            if (check)
                            {
                                if (!string.IsNullOrEmpty(query.Accessory))
                                {
                                    query.Accessory += "," + infix + suffix;
                                }
                                else
                                {
                                    query.Accessory = infix + suffix;
                                }
                            }
                        }
                    }
                    _dbContext.SaveChanges();
                }
                return Ok(response);
            }
        }

        public static byte[] GetBytesFromUrl(string url)
        {
            byte[] b = { };
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse myResp = myReq.GetResponse();
                Stream stream = myResp.GetResponseStream();
                //int i;
                using (BinaryReader br = new BinaryReader(stream))
                {
                    //i = (int)(stream.Length);
                    b = br.ReadBytes(500000);
                    br.Close();
                }
                myResp.Close();
                return b;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return b;
            }
        }

        /// <summary>
        /// 将图片字节写入文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        static public bool WriteBytesToFile(string fileName, byte[] content)
        {
            if (content.Length <= 0)
            {
                return false;
            }
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                fs.Close();
                w.Close();
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                string name = model.flowName;
                string time = model.flowTime;
                var query = _dbContext.Flow.FirstOrDefault(x=>x.FlowName== name && x.FlowTime==time && x.SchoolUuid== AuthContextService.CurrentUser.SchoolGuid&& x.IsDelete==0);
                if (query!=null)
                {
                    if (!string.IsNullOrEmpty(query.Accessory))
                    {
                        query.Accessory += "," + model.accessory;
                    }


                    
                }
                else
                {
                    var entity = new Flow();
                    entity.FlowUuid = Guid.NewGuid();
                    entity.FlowName = model.flowName;
                    entity.FlowTime = model.flowTime;
                    entity.Accessory = model.accessory;
                    entity.IsDelete = 0;
                    entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                    _dbContext.Flow.Add(entity);
                }
               
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult show(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Flow.FirstOrDefault(x => x.FlowUuid == guid);
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                Guid guid = model.flowUuid;
                var entity = _dbContext.Flow.FirstOrDefault(x => x.FlowUuid == guid && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                entity.FlowName = model.flowName;
                entity.FlowTime = model.flowTime.ToString("yyyy-MM-dd");
                entity.Accessory = model.accessory;
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
        /// 删除单个角色
        /// </summary>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
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
        /// 删除角色
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE Flow SET IsDelete=@IsDelete WHERE FlowUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
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
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
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
            var allowType = new string[] { "image/jpeg", "image/png", "image/jpg" };
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
                    string fname = prefix + files[0].FileName.Substring(index);
                    string strpath = Path.Combine("UploadFiles/RegistPicture", fname);
                    string path = Path.Combine(_hostEnv.WebRootPath, strpath);
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
                        strpath2 = Path.Combine("UploadFiles/RegistPicture", fname2);
                        var dFile = Path.Combine(_hostEnv.WebRootPath, strpath2);
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
                        strpath2 = Path.Combine("UploadFiles/RegistPicture", fname2);
                        var dFile = Path.Combine(_hostEnv.WebRootPath, strpath2);

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
        public IActionResult DeleteFile(LSPicture lsp)
        {
            var response = ResponseModelFactory.CreateInstance;

            var path = Path.Combine(_hostEnv.WebRootPath + "/UploadFiles/RegistPicture", lsp.Path);
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

        /// <summary>
        /// 小程序成菜流程添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppCreate(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                string newtime = model.datetime;
                string name = model.flowName;
                Guid schooluuid = model.schoolUuid;
                var query = _dbContext.Flow.FirstOrDefault(x => x.FlowTime == newtime && x.FlowName == name && x.SchoolUuid== schooluuid && x.IsDelete==0);
                if (query == null)
                {
                    var entity = new Flow();
                    entity.FlowUuid = Guid.NewGuid();
                    entity.FlowName = model.flowName;
                    entity.FlowTime = model.datetime;
                    entity.Accessory = model.accessory;
                    entity.IsDelete = 0;
                    entity.SchoolUuid = model.schoolUuid;
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.AddPeople = model.addPeople;
                    _dbContext.Flow.Add(entity);
                    _dbContext.SaveChanges();
                }
                else
                {
                    if (query.Accessory == "" || query.Accessory == null)
                    {
                        query.Accessory = model.accessory;
                    }
                    else
                    {
                        query.Accessory = query.Accessory + "," + model.accessory;
                    }
                    _dbContext.SaveChanges();
                }

                return Ok(response);
            }
        }

        /// <summary>
        /// 小程序成菜流程列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppList(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                Guid guid = model.uuid;
                string time = model.date;
                if (guid == null)
                {
                    response.SetFailed("学校guid参数有误");
                    return Ok(response);
                }
                if (string.IsNullOrEmpty(time))
                {
                    response.SetFailed("时间有误");
                    return Ok(response);
                }
                if (_dbContext.Flow.Any(x => x.SchoolUuid == guid && x.FlowTime == time && x.IsDelete == 0))
                {
                    var query = _dbContext.Flow.Where(x => x.SchoolUuid == guid && x.FlowTime == time && x.IsDelete == 0);
                    var ys = query.Where(x => x.FlowName == "验收").Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                    var qx = query.Where(x => x.FlowName == "清洗").Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                    var qp = query.Where(x => x.FlowName == "切配").Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                    var jg = query.Where(x => x.FlowName == "加工").Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                    var cc = query.Where(x => x.FlowName == "成菜").Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                    response.SetData(new {  ys, qx, qp, jg, cc });
                }
                else
                {
                    var query1 = _dbContext.Flow.Where(x => x.SchoolUuid == guid && x.IsDelete == 0);
                    if (query1.ToList().Count>0)
                    {
                        var entity = query1.OrderByDescending(x => x.FlowTime).First();
                        if (entity != null)
                        {
                            var times = entity.FlowTime;
                            var ys = query1.Where(x => x.FlowName == "验收" && x.FlowTime == entity.FlowTime).Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                            var qx = query1.Where(x => x.FlowName == "清洗" && x.FlowTime == entity.FlowTime).Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                            var qp = query1.Where(x => x.FlowName == "切配" && x.FlowTime == entity.FlowTime).Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                            var jg = query1.Where(x => x.FlowName == "加工" && x.FlowTime == entity.FlowTime).Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                            var cc = query1.Where(x => x.FlowName == "成菜" && x.FlowTime == entity.FlowTime).Select(x => new { x.FlowUuid, x.FlowName, Prctlist = LoadPaths(x.Accessory) }).ToList();
                            response.SetData(new { times, ys, qx, qp, jg, cc });
                        }
                        else
                        {
                            response.SetFailed("entity==null");
                        }
                    }
                    else
                    {
                        response.SetFailed("entity==null");
                    }
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 路径切割
        /// </summary>
        /// <param name="accessory"></param>
        /// <returns></returns>
        private static string[] LoadPaths(string accessory)
        {
            if (string.IsNullOrEmpty(accessory))
            {
                return new string[] { "" };
            }
            else
            {
                return accessory.Split(',');
            }
        }
    }
}
