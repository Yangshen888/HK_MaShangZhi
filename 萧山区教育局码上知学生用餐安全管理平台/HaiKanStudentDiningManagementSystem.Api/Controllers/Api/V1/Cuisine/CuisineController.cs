using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Cuisine;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Formula.Functions;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Cuisine;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Cuisine
{
    [Route("api/v1/Cuisine/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class CuisineController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public CuisineController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
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
        [HttpPost]
        public IActionResult CuisineList(CuisineRequestpayload payload)
        {
            var query = from c in _dbContext.Cuisine
                        where c.IsDelete != 1
                        select new
                        {
                            c.CuisineUuid,
                            c.SchoolUuid,
                            c.CuisineName,
                            Ingredient = GetIngredient(c.Ingredient, _dbContext),
                            Burdening = GetIngredient(c.Burdening, _dbContext),
                            c.Price,
                            c.Id,
                            c.Abstract
                        };

            if (!string.IsNullOrEmpty(payload.kw))
            {
                query = query.Where(x => x.SchoolUuid == Guid.Parse(payload.kw));
            }
            if (!string.IsNullOrEmpty(payload.kw1))
            {
                query = query.Where(x => x.CuisineName.Contains(payload.kw1));
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

        [HttpGet]
        public IActionResult iscuisine(string name)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.Cuisine.FirstOrDefault(x=>x.CuisineName==name && x.IsDelete==0&&x.SchoolUuid==AuthContextService.CurrentUser.SchoolGuid);
                if (query!=null)
                {
                    response.SetData(true);
                }
                else
                {
                    response.SetData(false);
                }
                return Ok(response);
            }
        }

        private static string GetIngredient(string Ingredient, haikanSDMSContext _dbContext)
        {
            bool isChinese = false;
            if (string.IsNullOrEmpty(Ingredient))
            {
                return "";
            }
            foreach (char ch in Ingredient)
            {
                //判断字符ch是否为汉字字符
                if (ch >= 0x4e00 && ch <= 0x9fbb)
                {
                    isChinese = true;
                    break;
                }
            }
            if (isChinese)
            {
                return Ingredient;
            }
            else
            {
                var uuids = Ingredient.ToUpper().Split(',').ToList();
                var list = _dbContext.Ingredient.Where(x => uuids.Contains(x.IngredientUuid.ToString())).Select(x => x.FoodName).ToList();
                var names = string.Join(',', list);
                return names;
            }

        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CuisineCreate(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new HaiKanStudentDiningManagementSystem.Api.Entities.Cuisine();
                string schoolUuid = model.schoolUuid;
                if (schoolUuid == null)
                {
                    response.SetFailed("请登录学校账号");
                    return Ok(response);
                }
                string name = model.cuisineName;
                var cuiname = _dbContext.Cuisine.Where(x => x.CuisineName == name && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid && x.IsDelete==0).ToList();
                if (cuiname.Count() > 0)
                {
                    response.SetFailed("该菜品已存在");
                    return Ok(response);
                }
                entity.CuisineUuid = Guid.NewGuid();
                entity.CuisineName = model.cuisineName;
                entity.Price = model.price;
                entity.Burdening = model.burdening;
                entity.Ingredient = model.ingredient;
                entity.Abstract = model.abstracts;
                entity.CuisineType = model.cuisineType;
                entity.Accessory = model.accessory;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.AddPeople = model.addPeople;
                entity.IsDelete = 0;
                entity.SchoolUuid = Guid.Parse(schoolUuid);
                entity.LikeNum = 0;
                _dbContext.Cuisine.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取学校信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SchoolList()
        {
            using (_dbContext)
            {
                var entity = _dbContext.School.Where(x => x.IsDelete != 1);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取食材信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IngredientGet(string guid)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                if (guid == null)
                {
                    //var entity = _dbContext.Ingredient.Where(x => x.IsDelete != 1).ToList();
                    var entity = _dbContext.Ingredient.ToList();
                    response.SetData(entity);
                }
                else
                {
                    var entity = _dbContext.Ingredient.Where(x => x.SchoolUuid == Guid.Parse(guid)).ToList();
                    response.SetData(entity);
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取食材信息(添加/二次编辑)
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IngredientGet2(string guid)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateInstance;
                if (guid == null)
                {
                    var entity = _dbContext.Ingredient.Where(x => x.IsDelete != 1).ToList();
                    
                    response.SetData(entity);
                }
                else
                {
                    var entity = _dbContext.Ingredient.Where(x => x.SchoolUuid == Guid.Parse(guid) && x.IsDelete != 1).ToList();
                    response.SetData(entity);
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取菜品信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CuisineGet(Guid guid)
        {
            using (_dbContext)
            {
                //var entity = _dbContext.Cuisine.FirstOrDefault(x => x.CuisineUuid == guid);
                var entity = from c in _dbContext.Cuisine
                             where c.CuisineUuid == guid
                             select new
                             {
                                 c.CuisineName,
                                 c.Price,
                                 //c.Ingredient,
                                 //c.Burdening,
                                 Ingredient=GetIngredient(c.Ingredient,_dbContext),
                                 Burdening = GetIngredient(c.Burdening, _dbContext),
                                 c.CuisineType,
                                 c.Abstract,
                                 c.Accessory,
                                 c.CuisineUuid,
                                 c.SchoolUu.SchoolName
                             };
                var query = entity.ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CuisineEdit(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            string guid = model.cuisineUuid;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            string schoolUuid = model.schoolUuid;
            if (schoolUuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Cuisine.FirstOrDefault(x => x.CuisineUuid == Guid.Parse(guid));
                string name = model.cuisineName;
                if (name!= entity.CuisineName)
                { 
                    var cuiname = _dbContext.Cuisine.Where(x => x.CuisineName == name && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid && x.IsDelete == 0).ToList();
                    if (cuiname.Count() > 0)
                    {
                        response.SetFailed("该菜品已存在");
                        return Ok(response);
                    }
                }
                entity.CuisineName = model.cuisineName;
                entity.Price = model.price;
                entity.Burdening = model.burdening;
                entity.Ingredient = model.ingredient;
                entity.Abstract = model.abstracts;
                entity.CuisineType = model.cuisineType;
                entity.Accessory = model.accessory;
                _dbContext.SaveChanges();
                response.SetSuccess("修改成功");
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
                    string fname = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + Guid.NewGuid().ToString() + files[0].FileName.Substring(index);
                    string strpath = Path.Combine("UploadFiles/RegistPicture", fname);
                    string path = Path.Combine(_hostingEnvironment.WebRootPath, strpath);
                    //System.IO.File.Create(path);
                    var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    await files[0].CopyToAsync(stream);

                    stream.Close();
                    response.SetData(new { strpath, fname });
                    //    paths.Add(strpath);
                    //dataPaths.Add(fname);
                    //}

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

            var path = Path.Combine(_hostingEnvironment.WebRootPath + "/UploadFiles/RegistPicture", lsp.Path);
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
                var sql = string.Format("UPDATE Cuisine SET IsDelete=@IsDelete WHERE CuisineUUID IN ({0})", parameterNames);
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
        /// 导入菜品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CuisineImport(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportExcel";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "菜品信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{uploadtitle}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                //string conStr = ConnectionStrings.DefaultConnection;
                string responsemsgsuccess = "";
                string responsemsgrepeat = "";
                string responsemsgdefault = "";
                int successcount = 0;
                int repeatcount = 0;
                int defaultcount = 0;
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                try
                {
                    //把excelfile中的数据复制到file中
                    using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
                    {
                        excelfile.CopyTo(fs);
                        fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
                    }
                    DataTable dt = Haikan3.Utils.ExcelTools.ExcelToDataTable(file.ToString(), "Sheet1", true);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        response.SetFailed("表格无数据");
                        return Ok(response);
                    }
                    else
                    {
                        if (!dt.Columns.Contains("菜品名称"))
                        {
                            response.SetFailed("无‘菜品名称’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("价格"))
                        {
                            response.SetFailed("无‘价格’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("主料"))
                        {
                            response.SetFailed("无‘主料’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("配料"))
                        {
                            response.SetFailed("无‘配料’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("类别"))
                        {
                            response.SetFailed("无‘类别’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("简介"))
                        {
                            response.SetFailed("无‘简介’列");
                            return Ok(response);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new Entities.Cuisine();
                            entity.CuisineUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["菜品名称"].ToString()))
                            {
                                var a = dt.Rows[i]["菜品名称"].ToString();
                                var user = _dbContext.Cuisine.Where(x => x.IsDelete == 0 && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid).FirstOrDefault(x => x.CuisineName == dt.Rows[i]["菜品名称"].ToString() );

                                if (user == null)
                                {
                                    entity.CuisineName = dt.Rows[i]["菜品名称"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行菜品名称已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行菜品名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["价格"].ToString()))
                            {
                                entity.Price = dt.Rows[i]["价格"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行价格为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["主料"].ToString()))
                            {
                                //var uuname = dt.Rows[i]["主料"].ToString().ToUpper().Split('，').ToList();
                                //var lists = _dbContext.Ingredient.Where(x => uuname.Contains(x.FoodName) && x.IsDelete != 1).ToList();
                                //if (lists.Count == 0)
                                //{
                                    //entity.Ingredient = "";
                                    entity.Ingredient = dt.Rows[i]["主料"].ToString();
                                    //responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行主料不存在" + "</p></br>";
                                    //repeatcount++;
                                    //continue;
                                //}
                                //else
                                //{
                                //    var list = _dbContext.Ingredient.Where(x => uuname.Contains(x.FoodName) && x.IsDelete != 1 && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid).Select(x => x.IngredientUuid).ToList();
                                //    var uuid = string.Join(',', list);
                                //    entity.Ingredient = uuid;
                                //}
                            }
                            //else
                            //{
                            //    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行主料为空" + "</p></br>";
                            //    defaultcount++;
                            //    continue;
                            //}
                            if (!string.IsNullOrEmpty(dt.Rows[i]["配料"].ToString()))
                            {
                                //var uuname = dt.Rows[i]["配料"].ToString().ToUpper().Split(',').ToList();
                                //var lists = _dbContext.Ingredient.Where(x => uuname.Contains(x.FoodName) && x.IsDelete != 1).ToList();
                                //if (lists.Count == 0)
                                //{
                                    //entity.Burdening = "";
                                    entity.Burdening = dt.Rows[i]["配料"].ToString();
                                    //responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行配料不存在" + "</p></br>";
                                    //repeatcount++;
                                    //continue;
                                //}
                                //else
                                //{
                                //    var list = _dbContext.Ingredient.Where(x => uuname.Contains(x.FoodName) && x.IsDelete != 1 && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid).Select(x => x.IngredientUuid).ToList();
                                //    var uuid = string.Join(',', list);
                                //    entity.Burdening = uuid;
                                //}
                            }
                            //else
                            //{
                            //    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行配料为空" + "</p></br>";
                            //    defaultcount++;
                            //    continue;
                            //}
                            if (!string.IsNullOrEmpty(dt.Rows[i]["类别"].ToString()))
                            {
                                string type = dt.Rows[i]["类别"].ToString();
                                if (type == "荤菜" || type == "半荤菜" || type == "素菜" || type == "其它" || type == "甜品")
                                {
                                    entity.CuisineType = dt.Rows[i]["类别"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行类别错误" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行类别为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["简介"].ToString()))
                            {
                                entity.Abstract = dt.Rows[i]["简介"].ToString();
                            }
                            entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                            entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                            entity.LikeNum = 0;
                            entity.IsDelete = 0;
                            _dbContext.Cuisine.Add(entity);
                            _dbContext.SaveChanges();
                            successcount++;
                        }
                    }
                    responsemsgsuccess = "<p style='color:green'>导入成功:" + successcount + "条</p></br>" + responsemsgsuccess;
                    responsemsgrepeat = "<p style='color:orange'>重复需手动修改数据:" + repeatcount + "条</p></br>" + responsemsgrepeat;
                    responsemsgdefault = "<p style='color:red'>导入失败:" + defaultcount + "条</p></br>" + responsemsgdefault;


                    DateTime endTime = DateTime.Now;
                    TimeSpan useTime = endTime - beginTime;
                    string taketime = "导入时间" + useTime.TotalSeconds.ToString() + "秒  ";
                    response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
                    {
                        time = taketime,
                        successmsg = responsemsgsuccess
                        ,
                        repeatmsg = responsemsgrepeat,
                        defaultmsg = responsemsgdefault
                    })));
                    return Ok(response);
                }
                catch (Exception ex)
                {

                    response.SetFailed(ex.Message);
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 导出菜品信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExportPass(string ids)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var parameters = ids.Split(",");
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = parameters[i].ToUpper();
                }
                var query = _dbContext.Cuisine.Where(x => x.IsDelete != 1 && parameters.Contains(x.CuisineUuid.ToString())).Select(x => new Entities.Cuisine
                {
                    CuisineName = x.CuisineName,
                    Price = x.Price,
                    CuisineType=x.CuisineType,
                    Abstract = x.Abstract,
                    Ingredient = GetIngredient(x.Ingredient, _dbContext),
                    Burdening = GetIngredient(x.Burdening, _dbContext),
                }).ToList();
                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportExcel\\";
                string uploadtitle = "菜品信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{sWebRootFolder + uploadtitle}.xlsx";
                //CuisineViewModel model = new CuisineViewModel();
                //model.Demos = query;
                TablesToExcel(query, sFileName);
                response.SetData("\\UploadFiles\\ImportExcel\\" + uploadtitle + ".xlsx");
                return Ok(response);
            }

        }

        /// <summary>
        /// 菜品下拉选择列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult List()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Cuisine.Where(x => x.IsDelete == 0).Select(x => new
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



        public static bool TablesToExcel(List<Entities.Cuisine> query, string filename)
        {
            MemoryStream ms = new MemoryStream();

            IWorkbook workBook;
            //IWorkbook workBook=WorkbookFactory.Create(filename);
            string suffix = filename.Substring(filename.LastIndexOf(".") + 1, filename.Length - filename.LastIndexOf(".") - 1);
            if (suffix == "xls")
            {
                workBook = new HSSFWorkbook();
            }
            else
                workBook = new XSSFWorkbook();

            ISheet sheet = workBook.CreateSheet("菜品表");

            CreatSheet(sheet, query);


            workBook.Write(ms);
            try
            {
                SaveToFile(ms, filename);
                ms.Flush();
                return true;
            }
            catch
            {
                ms.Flush();
                throw;
            }

        }

        private static void CreatSheet(ISheet sheet, List<Entities.Cuisine> query)
        {
            IRow headerRow = sheet.CreateRow(0);
            List<string> list = new List<string>() {
                "菜品名称","价格","配料","主料","简介","类型"
            };

            //表头
            for (int i = 0; i < list.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(list[i]);
            }
            //foreach (var column in list)
            //    headerRow.CreateCell(column.Ordinal).SetCellValue(column);//If Caption not set, returns the ColumnName value
            int rowIndex = 1;
            foreach (var row in query)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                dataRow.CreateCell(0).SetCellValue(row.CuisineName);
                dataRow.CreateCell(1).SetCellValue(row.Price);
                dataRow.CreateCell(2).SetCellValue(row.Burdening);
                dataRow.CreateCell(3).SetCellValue(row.Ingredient);
                dataRow.CreateCell(4).SetCellValue(row.Abstract);
                dataRow.CreateCell(5).SetCellValue(row.CuisineType);
                //foreach (DataColumn column in table.Columns)
                //{
                //    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                //}
                rowIndex++;
            }
        }
        private static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();         //转为字节数组 
                fs.Write(data, 0, data.Length);     //保存为Excel文件
                fs.Flush();
                data = null;
            }
        }

    }
}
