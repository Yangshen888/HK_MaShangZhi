using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKan3.Utils;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Extensions.DataAccess;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Ingredient;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Rbac.School;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Ingredient;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Process;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.School;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Ingredient
{
    [Route("api/v1/Ingredient/[controller]/[action]")]
    [ApiController]
    public class PurchaseRecordController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostEnv;

        public PurchaseRecordController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnv = env;
        }


        /// <summary>
        /// 采购记录列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public IActionResult List(PurchaseRecordRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {


                var query = _dbContext.PurchaseRecord.Where(x => x.IsDelete == 0).Include(x=>x.IngredientUu).ThenInclude(x=>x.SchoolUu).Select(x=>new
                {
                    x.Id,
                    x.PurchaseUuid,
                    x.PurchaseNum,
                    x.PurchaseDate,
                    x.IsDelete,
                    x.IngredientUu.FoodName,
                    type=x.IngredientUu.TypeUu.Name,
                    x.Supplier,
                    x.State,
                    x.SchoolUuid,
                    x.SystemUserUuid,
                    x.SchoolUu.SchoolName,
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.FoodName.Contains(payload.Kw));
                }
                if (!string.IsNullOrEmpty(payload.Kw3))
                {
                    query = query.Where(x => x.SystemUserUuid.Contains(payload.Kw3));
                }
                if (payload.Kw2!=null&&!string.IsNullOrEmpty(payload.Kw2[0]) && !string.IsNullOrEmpty(payload.Kw2[1]))
                {
                    var start = Convert.ToDateTime(payload.Kw2[0]);
                    var end= Convert.ToDateTime(payload.Kw2[1]);
                    query = query.AsEnumerable().Where(x => Convert.ToDateTime( x.PurchaseDate) >=start&& Convert.ToDateTime(x.PurchaseDate)<end).AsQueryable();
                }
                query = query.OrderBy(x => x.State).ThenBy(x=>x.Id);
                //if (payload.FirstSort != null)
                //{
                //    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                //}

                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);


                return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult GetCanteen()
        {
            var response = ResponseModelFactory.CreateInstance;
            var query = _dbContext.PurchaseRecord.Where(x=>x.IsDelete==0 && x.SystemUserUuid!="");
            if (AuthContextService.CurrentUser.SchoolGuid != null)
            {
                query = query.Where(x=>x.SchoolUuid== AuthContextService.CurrentUser.SchoolGuid);
            }
            var list = query.GroupBy(s => s.SystemUserUuid).Select(q => new
            {
                Value = q.Key,
                label = q.Key
            });
            response.SetData(list);
            return Ok(response);
        }

        private static string GetPeople(string systemUserUuid, haikanSDMSContext _dbContext)
        {
            if (string.IsNullOrEmpty(systemUserUuid))
            {
                return "";
            }
            var uuids = systemUserUuid.ToUpper().Split(',').ToList();

            var list = _dbContext.SystemUser.Where(x => uuids.Contains(x.SystemUserUuid.ToString())).Select(x => x.RealName).ToList();
            var names = string.Join(',', list);
            return names;
        }


        /// <summary>
        /// 添加采购记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(PurchaseRecordViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Ingredient.FirstOrDefault(x => x.FoodName == model.FoodName.Trim() && x.IsDelete == 0 && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                if (entity == null)
                {
                    response.SetFailed("该食材不存在");
                    return Ok(response);
                }
                var purchaseRecord = new Entities.PurchaseRecord()
                {
                    PurchaseUuid = Guid.NewGuid(),
                    IngredientUuid = model.IngredientUuid,
                    Supplier = model.Supplier,
                    //PurchaseDate = Convert.ToDateTime(model.PurchaseDate).AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss"),
                    PurchaseDate = Convert.ToDateTime(model.PurchaseDate).ToString("yyyy-MM-dd HH:mm:ss"),
                    PurchaseNum = model.PurchaseNum,
                    HeatEnergy = model.HeatEnergy,
                    Protein = model.Protein,
                    Fat = model.Fat,
                    Saccharides = model.Saccharides,
                    Va = model.Va,
                    State = model.State,
                    Accessory = model.Accessory,
                    SystemUserUuid = model.SystemUserUuid,
                    AddTime=DateTime.Now.ToString("yyyy-MM-dd"),
                    AddPeople = model.AddPeople,
                    IsDelete = 0,
                    Price=model.Price,
                    Unit=model.Unit,
                    SchoolUuid = model.SchoolUuid != null ? model.SchoolUuid : AuthContextService.CurrentUser.SchoolGuid,
                };
                _dbContext.PurchaseRecord.Add(purchaseRecord);
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
        /// 编辑采购记录
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.PurchaseRecord.Include(x=>x.SchoolUu).Select(x=>new
                {
                    x.PurchaseDate,
                    //PurchaseNum=Convert.ToDouble(x.PurchaseNum),
                    x.PurchaseNum,
                    x.PurchaseUuid,
                    x.Supplier,
                    x.HeatEnergy,
                    x.Protein,
                    x.Fat,
                    x.Saccharides,
                    x.Va,
                    x.AddPeople,
                    x.AddTime,
                    x.IsDelete,
                    x.SchoolUuid,
                    x.SchoolUu.SchoolName,
                    x.State,
                    x.SystemUserUuid,
                    x.IngredientUu.FoodName,
                    x.IngredientUu.TypeUuid,
                    Accessory=(x.Accessory ?? ""),
                    x.Price,
                    x.Unit
                }).FirstOrDefault(x => x.PurchaseUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 通过名字加载食材
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Load(string name)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Ingredient.FirstOrDefault(x => x.FoodName == name);
                response.SetData( entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的采购记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(PurchaseRecordViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.PurchaseRecord.FirstOrDefault(x => x.PurchaseUuid == model.PurchaseUuid);
                if (entity == null)
                {
                    response.SetFailed("该流程不存在");
                    return Ok(response);
                }
                var ingredient = _dbContext.Ingredient.FirstOrDefault(x => x.FoodName == model.FoodName.Trim() && x.IsDelete == 0 && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                if (ingredient==null)
                {
                    response.SetFailed("该食材不存在");
                    return Ok(response);
                }
                entity.Supplier = model.Supplier;
                //entity.PurchaseDate = Convert.ToDateTime(model.PurchaseDate).AddHours(-8).ToString("yyyy-MM-dd HH:mm:ss");
                entity.PurchaseDate = Convert.ToDateTime(model.PurchaseDate).ToString("yyyy-MM-dd HH:mm:ss");
                entity.PurchaseNum = model.PurchaseNum;
                entity.HeatEnergy = model.HeatEnergy;
                entity.Protein = model.Protein;
                entity.Fat = model.Fat;
                entity.Saccharides = model.Saccharides;
                entity.Va = model.Va;
                entity.State = model.State;
                entity.Accessory = model.Accessory;
                entity.SystemUserUuid = model.SystemUserUuid;
                entity.Price = model.Price;
                entity.Unit = model.Unit;
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
        /// 删除采购记录
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
        /// 批量删除
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
        /// 采购价格公示
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult PurchaseRecordList(string date,string uuid,int change)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd");
            if (!string.IsNullOrEmpty(date))
            {
                time = date.Replace("/","-");
            }
            var response = ResponseModelFactory.CreateInstance;
            Guid suid=default;
            if((!Guid.TryParse(uuid, out suid)) && string.IsNullOrEmpty(uuid))
            {
                response.SetFailed("学校id格式有误");
            }
            using (_dbContext)
            {
                //suid = Guid.Parse(uuid);
                if (_dbContext.PurchaseRecord.Any(x => x.IsDelete == 0  && x.SchoolUuid == suid) && change==1)
                {
                    var query = from pr in _dbContext.PurchaseRecord
                                join i in _dbContext.Ingredient
                                on pr.IngredientUuid equals i.IngredientUuid
                                where pr.IsDelete == 0 && i.IsDelete == 0 && pr.PurchaseDate.Substring(0, 10) == time && pr.SchoolUuid==suid
                                select new
                                {
                                    pr.PurchaseUuid,
                                    pr.IngredientUuid,
                                    pr.Price,
                                    i.FoodName,
                                    i.Accessory,
                                    pr.PurchaseDate,
                                    pr.PurchaseNum,
                                    pr.Unit,
                                    pr.Supplier
                                };

                    var list = query.ToList();
                    //var query2 = _dbContext.PurchaseRecord.Where(x => x.PurchaseDate.Substring(0, 10) == DateTime.Now.ToString("yyyy-MM-dd"));
                    var query2 = from pr in _dbContext.PurchaseRecord.AsEnumerable().Where(x => x.IsDelete == 0 && x.PurchaseDate.Substring(0, 10) == time && x.SchoolUuid == suid)
                                 group pr by pr.IngredientUuid into g
                                 select new
                                 {
                                     g.Key,
                                     AvgPirce = g.Average(x => x.Price),
                                 };
                    var list2 = query2.ToList();
                    var list3 = list.Select(x => new
                    {
                        x.PurchaseUuid,
                        x.IngredientUuid,
                        x.Price,
                        UnitPrice = x.Unit == null || x.Unit == "" ? x.Price + "元/斤" : x.Price + "元/"+ x.Unit,
                        x.FoodName,
                        x.Accessory,
                        x.PurchaseNum,
                        x.Unit,
                        x.Supplier,
                        AvgPrice = list2.Find(y => y.Key == x.IngredientUuid).AvgPirce,
                        UnitAvgPrice = x.Unit == null || x.Unit == "" ? list2.Find(y => y.Key == x.IngredientUuid).AvgPirce + "元/斤" : list2.Find(y => y.Key == x.IngredientUuid).AvgPirce + "元/" + x.Unit,
                    }).ToList();
                    var pricesum = Math.Round(Convert.ToDecimal(list3.Sum(x => x.Price* (x.PurchaseNum == "" ? 0 : Convert.ToDouble(x.PurchaseNum)))), 2, MidpointRounding.AwayFromZero);
                    response.SetData(new { list3, time, pricesum });
                }
                else
                {
                    var query = from pr in _dbContext.PurchaseRecord
                                join i in _dbContext.Ingredient
                                on pr.IngredientUuid equals i.IngredientUuid
                                where pr.IsDelete == 0 && i.IsDelete == 0 && pr.SchoolUuid == suid
                                select new
                                {
                                    pr.PurchaseUuid,
                                    pr.IngredientUuid,
                                    pr.Price,
                                    i.FoodName,
                                    i.Accessory,
                                    pr.PurchaseDate,
                                    pr.PurchaseNum,
                                    pr.Unit,
                                    pr.Supplier
                                };
                    query = query.OrderByDescending(x => x.PurchaseDate);
                    var entity = query.FirstOrDefault();
                    if (entity != null)
                    {
                        time = entity.PurchaseDate.Substring(0, 10);
                        var list = query.Where(x => x.PurchaseDate.Substring(0, 10) == entity.PurchaseDate.Substring(0, 10));
                        var query2 = from pr in _dbContext.PurchaseRecord.AsEnumerable().Where(x => x.IsDelete == 0 && x.PurchaseDate.Substring(0, 10) == entity.PurchaseDate.Substring(0, 10) && x.SchoolUuid == suid)
                                     group pr by pr.IngredientUuid into g
                                     select new Gb
                                     {
                                         Key = g.Key,
                                         AvgPirce = g.Average(x => x.Price),
                                     };
                        var list2 = query2.ToList();
                        var list3 = list.Select(x => new
                        {
                            x.PurchaseUuid,
                            x.IngredientUuid,
                            x.Price,
                            UnitPrice = x.Unit == null || x.Unit == "" ? x.Price + "元/斤" : x.Price + "元/" + x.Unit,
                            x.FoodName,
                            x.Accessory,
                            x.PurchaseNum,
                            x.Unit,
                            x.Supplier,
                            //AvgPrice = list2.Find(y => y.Key == x.IngredientUuid).AvgPirce,
                            AvgPrice = SetAvg(list2, x.IngredientUuid),
                            UnitAvgPrice = x.Unit == null || x.Unit == "" ? SetAvg(list2, x.IngredientUuid) + "元/斤" : SetAvg(list2, x.IngredientUuid) + "元/" + x.Unit,
                        }).ToList();
                         var pricesum = Math.Round(Convert.ToDecimal(list3.Sum(x => x.Price * (x.PurchaseNum==""?0:Convert.ToDouble(x.PurchaseNum)))), 2, MidpointRounding.AwayFromZero);
                        response.SetData(new { list3, time, pricesum });
                    }
                    
                    
                    
                }
                
                return Ok(response);
            }

            
        }

        private static double SetAvg(List<Gb> list2, Guid? ingredientUuid)
        {
            var x = list2.Find(x => x.Key == ingredientUuid);
            if (x == null)
            {
                return 0;
            }
            else
            {
                return x.AvgPirce.Value;
            }
            
        }

        

        /// <summary>
        /// 采购信息详情
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult PurchaseInfo(string uuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(uuid))
            {
                response.SetFailed("uuid有误");
                return Ok(response);
            }

            using (_dbContext)
            {
                var entity = _dbContext.PurchaseRecord.FirstOrDefault(x => x.PurchaseUuid == Guid.Parse(uuid));
                var arr = entity.SystemUserUuid;
                var schoolPS = _dbContext.School.FirstOrDefault(x => x.SchoolUuid == entity.SchoolUuid).PurchasingStandard;
                response.SetData(new { arr, schoolPS });
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
                var sql = string.Format("UPDATE PurchaseRecord SET IsDelete=@IsDelete WHERE PurchaseUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDelete));
                var num=_dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        /// <summary>
        /// 导入信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Import(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostEnv.WebRootPath + "\\UploadFiles\\ImportExcel";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "食材采购记录导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                    DataTable dt = Haikan3.Utils.ExcelTools.ExcelToDataTable(file.ToString(), "食材采购记录", true);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        response.SetFailed("表格无数据");
                        return Ok(response);
                    }
                    else
                    {
                        if (!dt.Columns.Contains("食材名称"))
                        {
                            response.SetFailed("无‘食材名称’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("单价(元/斤)"))
                        {
                            response.SetFailed("无‘单价(元/斤)’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("供应商"))
                        {
                            response.SetFailed("无‘供应商’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("采购时间"))
                        {
                            response.SetFailed("无‘采购时间’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("采购数量"))
                        {
                            response.SetFailed("无‘采购数量’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("采购人"))
                        {
                            response.SetFailed("无‘采购人’列");
                            return Ok(response);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(dt.Rows[i]["采购时间"].ToString()) && !string.IsNullOrEmpty(dt.Rows[i]["采购人"].ToString()))
                            {
                                var entity = new Entities.PurchaseRecord();
                                entity.PurchaseUuid = Guid.NewGuid();
                                if (!string.IsNullOrEmpty(dt.Rows[i]["食材名称"].ToString()))
                                {
                                    var food = _dbContext.Ingredient.Where(x => x.IsDelete == 0).FirstOrDefault(x => x.FoodName == dt.Rows[i]["食材名称"].ToString());
                                    if (food != null)
                                    {
                                        entity.IngredientUuid = food.IngredientUuid;
                                    }
                                    else
                                    {
                                        responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行食材名称不存在" + "</p></br>";
                                        repeatcount++;
                                        continue;
                                    }
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行食材名称为空" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(dt.Rows[i]["单价(元/斤)"].ToString()))
                                {
                                    entity.Price = Convert.ToDouble( dt.Rows[i]["单价(元/斤)"].ToString());
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行单价为空" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(dt.Rows[i]["供应商"].ToString()))
                                {
                                    entity.Supplier = dt.Rows[i]["供应商"].ToString();
                                }
                                if (!string.IsNullOrEmpty(dt.Rows[i]["采购时间"].ToString()))
                                {
                                    DateTime time;
                                    if(DateTime.TryParse(dt.Rows[i]["采购时间"].ToString(),out time))
                                    {
                                        entity.PurchaseDate = Convert.ToDateTime(dt.Rows[i]["采购时间"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    else
                                    {
                                        responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行采购时间格式有错：举例（2020-07-01）" + "</p></br>";
                                        defaultcount++;
                                        continue;
                                    }
                                    
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行采购时间为空" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(dt.Rows[i]["采购数量"].ToString()))
                                {
                                    entity.PurchaseNum = dt.Rows[i]["采购数量"].ToString();
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行采购数量为空" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(dt.Rows[i]["采购人"].ToString()))
                                {
                                    entity.SystemUserUuid = dt.Rows[i]["采购人"].ToString();
                                }
                                else
                                {
                                    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行采购人为空" + "</p></br>";
                                    defaultcount++;
                                    continue;
                                }
                                entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                                entity.IsDelete = 0;
                                entity.State = "0";
                                _dbContext.PurchaseRecord.Add(entity);
                                _dbContext.SaveChanges();
                                successcount++;
                            }
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
        /// 导出信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExportPass()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var parameters = AuthContextService.CurrentUser.SchoolGuid;
                var query = _dbContext.Ingredient.Where(x => x.IsDelete != 1 && x.SchoolUuid==parameters).Select(x => new Entities.Ingredient
                {
                    FoodName = x.FoodName,
                }).ToList();
                string sWebRootFolder = _hostEnv.WebRootPath + "\\UploadFiles\\ImportExcel\\";
                string uploadtitle = "食材采购记录" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{sWebRootFolder + uploadtitle}.xlsx";
                //CuisineViewModel model = new CuisineViewModel();
                //model.Demos = query;
                TablesToExcel(query, sFileName);
                response.SetData("\\UploadFiles\\ImportExcel\\" + uploadtitle + ".xlsx");
                return Ok(response);
            }
        }

        public static bool TablesToExcel(List<Entities.Ingredient> query, string filename)
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


            //var contextstyle = workBook.CreateCellStyle();
            var format = workBook.CreateDataFormat();
            //contextstyle.DataFormat = format.GetFormat("@");

            ISheet sheet = workBook.CreateSheet("食材采购记录");
            
            CreatSheet(sheet, query,format);


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

        

        private static void CreatSheet(ISheet sheet, List<Entities.Ingredient> query, IDataFormat format)
        {
            IRow headerRow = sheet.CreateRow(0);
            List<string> list = new List<string>() {
                "食材名称","单价(元/斤)","供应商","采购时间","采购数量","采购人"
            };

            //表头
            for (int i = 0; i < list.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(list[i]);
            }

            int rowIndex = 1;
            foreach (var row in query)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                var irow = dataRow.CreateCell(0);
                irow.CellStyle.DataFormat = format.GetFormat("@");
                irow.SetCellValue(row.FoodName);
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

    public class Gb
    {
        public Guid? Key { get; set; }
        public double? AvgPirce { get; set; }
    }
}
