using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using HaiKan3.Utils;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Extensions.DataAccess;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.DiningRoom;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;
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
    public class LiveShotController : ControllerBase
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
        public LiveShotController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnv = env;
        }

        /// <summary>
        /// 每餐实拍列表
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
                var query = _dbContext.LiveShot.Where(x => x.IsDelete == 0).Include(x => x.CuisineUu).ThenInclude(x=>x.SchoolUu).Select(x => new
                {
                    x.Id,
                    x.IsDelete,
                    x.CuisineUu.CuisineName,
                    x.SchoolUu.SchoolName,
                    Ingredient = FoodName(x.CuisineUu.Ingredient, _dbContext),
                    Burdening = FoodName(x.CuisineUu.Burdening, _dbContext),
                    x.CuisineUu.Abstract,
                    x.LiveShotUuid,
                    x.CuisineUuid,
                    x.AddTime,
                    x.Accessory,
                    x.SchoolUuid,
                    x.AddPeople,
                    x.Datetime,
                    Datetype = ShowType(x.Datetype),
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.CuisineName.Contains(payload.Kw));
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

        private static string ShowType(string type)
        {
            if (type == "morn")
            {
                return "早餐";
            }
            else if (type == "night")
            {
                return "晚餐";
            }
            else if (type == "noon")
            {
                return "中餐";
            }
            else
            {
                return "其他";
            }
        }
  
        /// <summary>
        /// 获取食材名称
        /// </summary>
        /// <returns></returns>
        private static string FoodName(string uuids, haikanSDMSContext dbContext)
        {
            if (string.IsNullOrEmpty(uuids))
            {
                return "";
            }
            var idls = uuids.ToUpper().Split(',').ToList();
            var list = dbContext.Ingredient.Where(x => idls.Contains(x.IngredientUuid.ToString())).Select(x => x.FoodName).ToList();
            var names = string.Join(',', list);
            return names;
        }

        /// <summary>
        /// 添加每餐实拍
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(LiveShotViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                if(_dbContext.LiveShot.Any(x=>x.CuisineUuid==model.CuisineUuid&&x.Datetime== Convert.ToDateTime(model.Datetime).ToString("yyyy-MM-dd") && x.Datetype == model.Datetype&&x.IsDelete==0))
                {
                    response.SetFailed("该日期用餐类型已存在相同的菜品");
                    return Ok(response);
                }
                var liveShot = new Entities.LiveShot();
                liveShot.LiveShotUuid = Guid.NewGuid();
                liveShot.CuisineUuid = model.CuisineUuid;
                if (!string.IsNullOrEmpty(model.Accessory))
                {
                    liveShot.Accessory = model.Accessory;
                }
                else
                {
                    var query = _dbContext.Cuisine.FirstOrDefault(x => x.CuisineUuid == model.CuisineUuid);

                    DateTime start = Convert.ToDateTime(Convert.ToDateTime(query.UpdateTime).ToShortDateString());
                    DateTime currentDateTime = DateTime.Now;
                    DateTime currentStartYear = new DateTime(currentDateTime.Year, Convert.ToInt32(query.UpdateTime.Split("-")[1]), Convert.ToInt32(query.UpdateTime.Split("-")[2]));
                    DateTime end = Convert.ToDateTime(currentStartYear.AddYears(1).ToShortDateString());
                    TimeSpan sp = end.Subtract(start);
                    int days = sp.Days;
                    if (days>=0)
                    {
                        liveShot.Accessory = query.Accessory;
                        System.IO.File.Copy(Path.Combine(@"C:\inetpub\wwwroot\wwwroot\UploadFiles\Picture", query.Accessory), Path.Combine(@"C:\inetpub\wwwroot\wwwroot\UploadFiles\LiveShotPicture", query.Accessory), true);
                        //System.IO.File.Copy(Path.Combine(@"D:\我的工作目录\work_萧山码上知\201024_14.17\萧山区教育局码上知学生用餐安全管理平台\HaiKanStudentDiningManagementSystem.Api\wwwroot\UploadFiles\Picture", query.Accessory), Path.Combine(@"D:\我的工作目录\work_萧山码上知\201024_14.17\萧山区教育局码上知学生用餐安全管理平台\HaiKanStudentDiningManagementSystem.Api\wwwroot\UploadFiles\LiveShotPicture", query.Accessory), true);
                    }
                    else
                    {
                        response.SetFailed("请更新照片");
                        return Ok(response);
                    }
                }
                
                liveShot.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                liveShot.AddPeople = model.AddPeople;
                liveShot.Datetime = Convert.ToDateTime(model.Datetime).ToString("yyyy-MM-dd");
                liveShot.Datetype = model.Datetype;
                liveShot.IsDelete = 0;
                liveShot.SchoolUuid = model.SchoolUuid != null ? model.SchoolUuid : AuthContextService.CurrentUser.SchoolGuid;
                _dbContext.LiveShot.Add(liveShot);
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
            };

        }

        [HttpGet]
        public IActionResult Todatetime()
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateResultInstance;
                var query = _dbContext.Cuisine.FirstOrDefault(x => x.CuisineUuid == Guid.Parse("BAF8B5D5-6813-4B8C-AF77-FA0CAF479261"));
                DateTime start = Convert.ToDateTime(Convert.ToDateTime(query.UpdateTime).ToShortDateString());
                DateTime currentDateTime = DateTime.Now;
                DateTime currentStartYear = new DateTime(currentDateTime.Year, Convert.ToInt32(query.UpdateTime.Split("-")[1]), Convert.ToInt32(query.UpdateTime.Split("-")[2]));
                DateTime end = Convert.ToDateTime(currentStartYear.ToShortDateString());
                TimeSpan sp = end.Subtract(start);
                response.SetData(query);
                return Ok(response);

            }
        }

        /// <summary>
        /// 编辑每餐实拍
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.LiveShot.Include(x => x.CuisineUu).ThenInclude(x=>x.SchoolUu).Select(x => new
                {
                    x.LiveShotUuid,
                    x.CuisineUuid,
                    x.CuisineUu.CuisineName,
                    Ingredient = FoodName(x.CuisineUu.Ingredient, _dbContext),
                    Burdening = FoodName(x.CuisineUu.Burdening, _dbContext),
                    x.CuisineUu.Abstract,
                    x.SchoolUuid,
                    Accessory=x.Accessory==null?"":x.Accessory,
                    x.Datetype,
                    x.Datetime,
                    x.SchoolUu.SchoolName,
                }).FirstOrDefault(x => x.LiveShotUuid == guid);
                //var path = Path.Combine(_hostEnv.WebRootPath, "/UploadFiles/LiveShotPicture/" + entity.Accessory);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(new { entity });
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的每餐实拍
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize]
        public IActionResult Edit(LiveShotViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.LiveShot.FirstOrDefault(x => x.LiveShotUuid == model.LiveShotUuid);
                if (entity == null)
                {
                    response.SetFailed("该实拍不存在");
                    return Ok(response);
                }
                if (entity.CuisineUuid!=model.CuisineUuid&&_dbContext.LiveShot.Any(x => x.CuisineUuid == model.CuisineUuid && x.Datetime == Convert.ToDateTime(model.Datetime).ToString("yyyy-MM-dd") && x.Datetype == model.Datetype&&x.IsDelete==0))
                {
                    response.SetFailed("该日期用餐类型已存在相同的菜品");
                    return Ok(response);
                }
                entity.CuisineUuid = model.CuisineUuid;
                entity.Datetime = Convert.ToDateTime(model.Datetime).ToString("yyyy-MM-dd");
                entity.Datetype = model.Datetype;
                entity.Accessory = model.Accessory;
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
        /// 删除每餐实拍
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
            var allowType = new string[] { "image/jpeg", "image/png","image/jpg" };
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
                    string strpath = Path.Combine("UploadFiles/LiveShotPicture", fname);
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
                        strpath2 = Path.Combine("UploadFiles/LiveShotPicture", fname2);
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
                        strpath2 = Path.Combine("UploadFiles/LiveShotPicture", fname2);
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
        /// 视频上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> VideoUpLoadAsync([FromForm] string filename)
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
        /// 删除图片
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteFile(LSPicture lsp)
        {
            var response = ResponseModelFactory.CreateInstance;

            var path = Path.Combine(_hostEnv.WebRootPath + "/UploadFiles/LiveShotPicture", lsp.Path);
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
        /// 删除文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult VideoDeleteFile(VideoFile vf)
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
        /// 菜品下拉选择列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        
        public IActionResult CuisineList(string datetime, string type)
        {
            var response = ResponseModelFactory.CreateInstance;
            DateTime time = DateTime.Now;
            if (string.IsNullOrEmpty(datetime)||!(DateTime.TryParse(datetime,out time)))
            {
                //time = Convert.ToDateTime(datetime);
                return Ok(response);
            }
            using (_dbContext)
            {
                var nweekmenus = _dbContext.NweekMenu.AsEnumerable()
                    .Where(x => Convert.ToDateTime(x.Datetimes.Split('-')[0]) <= time && Convert.ToDateTime(x.Datetimes.Split('-')[1]) >= time).AsQueryable();
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    nweekmenus = nweekmenus.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                var weekmenu = nweekmenus.FirstOrDefault();
                var uuids = GetUUIDs(weekmenu, time, type);
                var query = _dbContext.Cuisine.Where(x => x.IsDelete == 0 && uuids.Contains(x.CuisineUuid.ToString())).Select(x => new
                {
                    x.CuisineUuid,
                    x.SchoolUuid,
                    x.CuisineName,
                    Ingredient = GetIngredient(x.Ingredient, _dbContext),
                    Burdening = GetIngredient(x.Burdening, _dbContext),
                    x.Abstract,
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                response.SetData(query.ToList());
                return Ok(response);
            }

        }

        private static List<string> GetUUIDs(NweekMenu weekmenu, DateTime time, string type)
        {

            var week = time.DayOfWeek;
            List<string> uuids = new List<string>();
            if (weekmenu == null)
            {
                return uuids;
            }
            string sids = "";
            switch (week)
            {
                case DayOfWeek.Monday:
                    if (type == "morn")
                    {
                        sids = weekmenu.MonMon;
                    }
                    else if (type == "noon")
                    {
                        sids = weekmenu.MonNoon;
                    }
                    else if (type == "night")
                    {
                        sids = weekmenu.MonNight;
                    }
                    break;
                case DayOfWeek.Tuesday:
                    if (type == "morn")
                    {
                        sids = weekmenu.TuesMon;
                    }
                    else if (type == "noon")
                    {
                        sids = weekmenu.TuesNoon;
                    }
                    else if (type == "night")
                    {
                        sids = weekmenu.TuesNight;
                    }
                    break;
                case DayOfWeek.Wednesday:
                    if (type == "morn")
                    {
                        sids = weekmenu.WedMon;
                    }
                    else if (type == "noon")
                    {
                        sids = weekmenu.WedNoon;
                    }
                    else if (type == "night")
                    {
                        sids = weekmenu.WedNight;
                    }
                    break;
                case DayOfWeek.Thursday:
                    if (type == "morn")
                    {
                        sids = weekmenu.ThursMon;
                    }
                    else if (type == "noon")
                    {
                        sids = weekmenu.ThursNoon;
                    }
                    else if (type == "night")
                    {
                        sids = weekmenu.ThursNight;
                    }
                    break;
                case DayOfWeek.Friday:
                    if (type == "morn")
                    {
                        sids = weekmenu.FriMon;
                    }
                    else if (type == "noon")
                    {
                        sids = weekmenu.FriNoon;
                    }
                    else if (type == "night")
                    {
                        sids = weekmenu.FriNight;
                    }
                    break;
                case DayOfWeek.Saturday:
                    if (type == "morn")
                    {
                        sids = weekmenu.SatMon;
                    }
                    else if (type == "noon")
                    {
                        sids = weekmenu.SatNoon;
                    }
                    else if (type == "night")
                    {
                        sids = weekmenu.SatNight;
                    }
                    break;
                case DayOfWeek.Sunday:
                    if (type == "morn")
                    {
                        sids = weekmenu.SunMon;
                    }
                    else if (type == "noon")
                    {
                        sids = weekmenu.SunNoon;
                    }
                    else if (type == "night")
                    {
                        sids = weekmenu.SunNight;
                    }
                    break;
                default:
                    break;
            }
            uuids = sids.ToUpper().Split(',').ToList();
            return uuids;
        }

        private static string GetIngredient(string Ingredient, haikanSDMSContext _dbContext)
        {
            if (string.IsNullOrEmpty(Ingredient))
            {
                return "";
            }
            var uuids = Ingredient.ToUpper().Split(',').ToList();
            var list = _dbContext.Ingredient.Where(x => uuids.Contains(x.IngredientUuid.ToString())).Select(x => x.FoodName).ToList();
            var names = string.Join(',', list);
            return names;
        }

        /// <summary>
        /// wxapp显示实拍图片列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult GetPictureList(string date, string uuid, string type,int change)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                Guid guid = default;
                DateTime time = default;
                var check = Guid.TryParse(uuid, out guid);
                if (string.IsNullOrEmpty(uuid) || (!Guid.TryParse(uuid, out guid)))
                {
                    response.SetFailed("学校guid参数有误");
                    return Ok(response);
                }
                if (string.IsNullOrEmpty(date) || (!DateTime.TryParse(date, out time)))
                {
                    response.SetFailed("时间有误");
                    return Ok(response);
                }
                string st = time.ToString("yyyy-MM-dd");
                if (_dbContext.LiveShot.Any(x => x.SchoolUuid == guid) && change==1)
                {
                    var query = _dbContext.LiveShot.Where(x => x.SchoolUuid == guid && x.Datetime == st);
                    if (!string.IsNullOrEmpty(type))
                    {
                        query = query.Where(x => x.Datetype == type.Trim());
                    }
                    var list = query.Include(x => x.CuisineUu).Select(x => new { x.LiveShotUuid, x.CuisineUu.CuisineName, x.CuisineUuid, x.CuisineUu.LikeNum, Prctlist = LoadPaths(x.Accessory) }).ToList();
                    response.SetData(new { list, date, type });
                }
                else
                {
                    var query = _dbContext.LiveShot.Where(x => x.SchoolUuid == guid).OrderByDescending(x=>x.Datetime);
                    var entity = query.FirstOrDefault();
                    if (entity != null)
                    {
                        date = entity.Datetime;
                        if (entity != null)
                        {
                            query = query.Where(x => x.Datetime == entity.Datetime).OrderBy(x => x.Datetype);
                        }
                        if (query.Any(x => x.Datetype == "morn"))
                        {
                            type = "morn";
                            var query2 = query.Where(x => x.Datetype == "morn");
                            var list = query2.Include(x => x.CuisineUu).Select(x => new { x.LiveShotUuid, x.CuisineUu.CuisineName, x.CuisineUuid, x.CuisineUu.LikeNum, Prctlist = LoadPaths(x.Accessory) }).ToList();
                            response.SetData(new { list, date, type });
                        }
                        if (query.Any(x => x.Datetype == "noon"))
                        {
                            type = "noon";
                            var query2 = query.Where(x => x.Datetype == "noon");
                            var list = query2.Include(x => x.CuisineUu).Select(x => new { x.LiveShotUuid, x.CuisineUu.CuisineName, x.CuisineUuid, x.CuisineUu.LikeNum, Prctlist = LoadPaths(x.Accessory) }).ToList();
                            response.SetData(new { list, date, type });
                        }
                        if (query.Any(x => x.Datetype == "night"))
                        {
                            type = "night";
                            var query2 = query.Where(x => x.Datetype == "night");
                            var list = query2.Include(x => x.CuisineUu).Select(x => new { x.LiveShotUuid, x.CuisineUu.CuisineName, x.CuisineUuid, x.CuisineUu.LikeNum, Prctlist = LoadPaths(x.Accessory) }).ToList();
                            response.SetData(new { list, date, type });
                        }
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
                return accessory.Split('|');
            }
        }

        /// <summary>
        /// 每餐实拍的时间选择列表(上周一到下周日)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult GeTimeHorizon()
        {
            DateTime date = DateTime.Now;  //当前时间  
            DateTime lastMonday
                = date.DayOfWeek != DayOfWeek.Sunday ? date.AddDays(-(int)date.DayOfWeek - 6) : date.AddDays(-13);
            DateTime nextSunday
                = date.DayOfWeek != DayOfWeek.Sunday ? date.AddDays(14 - (int)date.DayOfWeek) : date.AddDays(7);
            var idate = lastMonday;
            List<string> timelist = new List<string>();
            int index = 0;
            int num = 0;
            while (idate <= nextSunday)
            {
                if (date.ToString("yyyy-MM-dd") == idate.ToString("yyyy-MM-dd"))
                {
                    index = num;
                }
                timelist.Add(idate.ToString("yyyy-MM-dd"));
                idate = idate.AddDays(1);
                num++;
            }
            return Ok(new { timelist, index });

        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult GeLiveShotInfo(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (string.IsNullOrEmpty(guid))
            {
                response.SetFailed("guid有误");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.LiveShot.Include(x => x.CuisineUu).Select(x => new
                {
                    x.LiveShotUuid,
                    x.CuisineUuid,
                    x.CuisineUu.CuisineName,
                    Prctlist = LoadPaths(x.Accessory),
                    x.Datetime,
                    x.Datetype,
                }).FirstOrDefault(x => x.LiveShotUuid == Guid.Parse(guid));
                var mealFlow = _dbContext.MealFlow.FirstOrDefault(x=>x.CuisineUuid==entity.CuisineUuid&&x.MealType==entity.Datetype&&x.AddTime==entity.Datetime);
                if (mealFlow == null)
                {
                    response.SetData(new { entity, mealFlow });
                    return Ok(response);
                }
                    var arrb = mealFlow.Buying.ToUpper().Split(",").ToList();
                    var arrd = mealFlow.Detection.ToUpper().Split(",").ToList();
                    var arrw = mealFlow.WashVege.ToUpper().Split(",").ToList();
                    var arrcho = mealFlow.WashVege.ToUpper().Split(",").ToList();
                    var arrcook = mealFlow.Cook.ToUpper().Split(",").ToList();
                    var buys = _dbContext.SystemUser.Where(x => arrb.Contains(x.SystemUserUuid.ToString())).Select(x => new { x.RealName, x.HealthCertificate }).ToList();
                    var dets = _dbContext.SystemUser.Where(x => arrd.Contains(x.SystemUserUuid.ToString())).Select(x => new { x.RealName, x.HealthCertificate }).ToList();
                var wass = _dbContext.SystemUser.Where(x => arrw.Contains(x.SystemUserUuid.ToString())).Select(x => new { x.RealName, x.HealthCertificate }).ToList();
                var chops = _dbContext.SystemUser.Where(x => arrcho.Contains(x.SystemUserUuid.ToString())).Select(x => new { x.RealName, x.HealthCertificate }).ToList();
                var coos = _dbContext.SystemUser.Where(x => arrcook.Contains(x.SystemUserUuid.ToString())).Select(x => new { x.RealName, x.HealthCertificate }).ToList();


                response.SetData(new { entity, mealFlow ,buys,dets,wass,chops,coos});
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
                var sql = string.Format("UPDATE LiveShot SET IsDelete=@IsDelete WHERE LiveShotUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDelete));
                var num = _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
    }

}
