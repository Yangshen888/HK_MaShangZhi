using System;
using System.Collections.Generic;
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
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Rbac.School;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.School;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Dml;
using NPOI.SS.Formula.Functions;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Rbac
{
    [Route("api/v1/rbac/[controller]/[action]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostEnv;

        public SchoolController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnv = env;
        }

        /// <summary>
        /// 学校列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        public IActionResult List(SchoolRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.School.Select(x => new
                {
                    SchoolUuid = x.SchoolUuid,
                    SchoolName = x.SchoolName,
                    AddPeople = x.AddPeople,
                    AddTime = x.AddTime,
                    Id = x.Id,
                    IsDelete = x.IsDelete,
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }

                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.SchoolName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => (CommonEnum.IsDeleted)x.IsDelete == payload.IsDeleted);
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
        /// 可用学校列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult List()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.School.Where(x => x.IsDelete == 0).Select(x => new
                {
                    x.SchoolUuid,
                    x.SchoolName,
                    x.AddPeople,
                    x.AddTime,
                    x.Id,
                    x.IsDelete,
                    Disabled = false,
                });

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 可用学校列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult List1()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var yeyquery = _dbContext.School.Where(x => x.IsDelete == 0 && x.SchoolType == "幼儿园").Select(x => new
                {
                    x.SchoolUuid,
                    x.SchoolName,
                    x.AddPeople,
                    x.AddTime,
                    x.Id,
                    x.IsDelete,
                    x.SchoolType,
                    Disabled = false,
                }).ToList();
                var xxquery = _dbContext.School.Where(x => x.IsDelete == 0 && x.SchoolType == "小学").Select(x => new
                {
                    x.SchoolUuid,
                    x.SchoolName,
                    x.AddPeople,
                    x.AddTime,
                    x.Id,
                    x.IsDelete,
                    x.SchoolType,
                    Disabled = false,
                }).ToList();
                var czquery = _dbContext.School.Where(x => x.IsDelete == 0 && x.SchoolType == "初中").Select(x => new
                {
                    x.SchoolUuid,
                    x.SchoolName,
                    x.AddPeople,
                    x.AddTime,
                    x.Id,
                    x.IsDelete,
                    x.SchoolType,
                    Disabled = false,
                }).ToList();
                var gzquery = _dbContext.School.Where(x => x.IsDelete == 0 && x.SchoolType == "高中").Select(x => new
                {
                    x.SchoolUuid,
                    x.SchoolName,
                    x.AddPeople,
                    x.AddTime,
                    x.Id,
                    x.IsDelete,
                    x.SchoolType,
                    Disabled = false,
                }).ToList();
                response.SetData(new { yeyquery, xxquery, czquery, gzquery });
                return Ok(response);
            }
        }

        /// <summary>
        /// 后台可用学校列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult List2()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.School.Where(x => x.IsDelete == 0).Select(x => new
                {
                    x.SchoolUuid,
                    x.SchoolName,
                    x.AddPeople,
                    x.AddTime,
                    x.Id,
                    x.IsDelete,
                    Disabled = false,
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取学校link
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult GetLink(string uuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(uuid))
            {
                response.SetFailed("uuid无效");
                return Ok(response);
            }
            using (_dbContext)
            {
                var link = _dbContext.School.FirstOrDefault(x => x.SchoolUuid == Guid.Parse(uuid)).Link;
                response.SetData(link);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加学校
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize]
        [ProducesResponseType(200)]
        public IActionResult Create(SchoolViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(model.SchoolName.Trim()))
            {
                response.SetFailed("请输入学校名称");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (_dbContext.School.Any(x => x.SchoolName == model.SchoolName))
                {
                    response.SetFailed("学校已存在");
                    return Ok(response);
                }
                var school = new School()
                {
                    SchoolName = model.SchoolName,
                    SchoolType = model.SchoolType,
                    SchoolUuid = Guid.NewGuid(),
                    AddPeople = model.AddPeople,
                    AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                    IsDelete = 0,
                    PurchasingStandard = model.PurchasingStandard,
                    Link = model.Link,
                    SchoolImg = model.SchoolImg,
                    Isptjob = model.Isptjob,
                    Isrecharge = model.Isrecharge,
                    Isreservation = model.Isreservation,
                    IsCuiPrices = model.IsCuiPrices,
                    Yard = model.Yard,
                    IsshowReport=model.IsshowReport,
                    Secretkey = model.Secretkey == "" ? "" : HaiKan3.Utils.AES.AesEncrypt(model.Secretkey, HaiKan3.Utils.AES.Key),
                    IsYard = model.IsYard,
                    Qrcode = model.QRcode,
                    Accessory = model.Accessory,
                    Menus=model.Menus,
                };
                _dbContext.School.Add(school);
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
        /// 编辑学校信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        [CustomAuthorize]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.School.FirstOrDefault(x => x.SchoolUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }


        /// <summary>
        /// 保存编辑后的学校信息
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [CustomAuthorize]
        public IActionResult Edit(SchoolViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.School.FirstOrDefault(x => x.SchoolUuid == model.SchoolUuid);
                if (entity == null)
                {
                    response.SetFailed("学校不存在");
                    return Ok(response);
                }
                if (entity.SchoolName != model.SchoolName && _dbContext.School.Any(x => x.SchoolName == model.SchoolName))
                {
                    response.SetFailed("学校已存在");
                    return Ok(response);
                }
                entity.SchoolName = model.SchoolName;
                entity.SchoolType = model.SchoolType;
                entity.PurchasingStandard = model.PurchasingStandard;
                entity.Link = model.Link;
                entity.SchoolImg = model.SchoolImg;
                entity.Isptjob = model.Isptjob;
                entity.Isrecharge = model.Isrecharge;
                entity.Isreservation = model.Isreservation;
                entity.IsCuiPrices = model.IsCuiPrices;
                entity.Yard = model.Yard;
                entity.IsshowReport = model.IsshowReport;
                entity.IsYard = model.IsYard;
                entity.Qrcode = model.QRcode;
                entity.Accessory = model.Accessory;
                entity.Menus = model.Menus;
                if (entity.Secretkey != model.Secretkey)
                {
                    entity.Secretkey = model.Secretkey == "" ? "" : HaiKan3.Utils.AES.AesEncrypt(model.Secretkey, HaiKan3.Utils.AES.Key);
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
        /// 删除学校
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
        /// 恢复学校
        /// </summary>
        /// <param name="ids">用户GUID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        [CustomAuthorize]
        public IActionResult Recover(string ids)
        {
            var response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
            return Ok(response);
        }


        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">用户ID,多个以逗号分隔</param>
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
        /// 根据guid获取学校名称
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSchoolName(string guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(guid))
            {
                response.SetData("");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.School.FirstOrDefault(x => x.SchoolUuid == Guid.Parse(guid));
                if (entity == null || entity.SchoolName == null)
                {
                    response.SetData("");
                }
                else
                {
                    response.SetData(entity.SchoolName);
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 根据学校名称获取学校信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult GetSchoolInfo(string name)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(name))
            {
                response.SetData("");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.School.FirstOrDefault(x => x.SchoolName == name);
                if (entity == null || entity.SchoolName == null)
                {
                    //response.SetData(null);
                    response.SetFailed("无学校信息");
                }
                else
                {
                    response.SetData(entity);
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 根据学校id获取学校信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult GetSchoolInfo2(string guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(guid))
            {
                response.SetData("");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.School.FirstOrDefault(x => x.SchoolUuid == Guid.Parse(guid));
                if (entity == null || entity.SchoolName == null)
                {
                    //response.SetData(null);
                    response.SetFailed("无学校信息");
                }
                else
                {
                    response.SetData(entity);
                }
                return Ok(response);
            }
        }




        /// <summary>
        /// 批量删除/恢复
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
                var sql = string.Format("UPDATE School SET IsDelete=@IsDelete WHERE SchoolUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDelete));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }


        /// <summary>
        /// 获取微信菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetWXMenuList()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Wxmenu.Where(x=>x.IsDelete==0).OrderBy(x=>x.Id).Select(x=>new {x.WxmenuUuid,x.MenuName,x.Id });
                response.SetData(query.ToList());
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
                    string fname = prefix + files[0].FileName.Substring(index);
                    string strpath = Path.Combine("UploadFiles/SchoolImg", fname);
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
                        strpath2 = Path.Combine("UploadFiles/SchoolImg", fname2);
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
                        strpath2 = Path.Combine("UploadFiles/SchoolImg", fname2);
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
                else
                {
                    message += "文件类型有误,请上传jpg/png";
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
        /// <param name="lsp"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteFile(LSPicture lsp)
        {
            var response = ResponseModelFactory.CreateInstance;

            var path = Path.Combine(_hostEnv.WebRootPath, lsp.Path);
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
        /// 同步食材....
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ToSync()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var schoollist = _dbContext.School.Where(x => x.IsDelete == 0).ToList();
                var schoolEnt = schoollist.Find(x => x.SchoolName == "萧山三中");
                //var schoolEnt = schoollist.Find(x => x.SchoolName == "xxxx1");
                schoollist.Remove(schoolEnt);
                var ingredients = _dbContext.Ingredient.Where(x => x.SchoolUuid == schoolEnt.SchoolUuid && x.IsDelete == 0).ToList();
                for (int n = 0; n < schoollist.Count; n++)
                {
                    for (int i = 0; i < ingredients.Count; i++)
                    {
                        if (!_dbContext.Ingredient.Any(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && x.FoodName == ingredients[i].FoodName))
                        {
                            var ing = new Entities.Ingredient()
                            {
                                IngredientUuid = Guid.NewGuid(),
                                SchoolUuid = schoollist[n].SchoolUuid,
                                AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                AddPeople = "",
                                FoodName = ingredients[i].FoodName,
                                TypeUuid = ingredients[i].TypeUuid,
                                IsDelete = 0,
                                HeatEnergy = ingredients[i].HeatEnergy,
                                Protein = ingredients[i].Protein,
                                Fat = ingredients[i].Fat,
                                Va = ingredients[i].Va,
                                Saccharides = ingredients[i].Saccharides,
                                Accessory = ingredients[i].Accessory,
                            };


                            _dbContext.Ingredient.Add(ing);
                        }

                    }
                    //_dbContext.Ingredient.AddRange(ingredients);
                    _dbContext.SaveChanges();
                }
                var cuisines = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoolEnt.SchoolUuid && x.IsDelete == 0).ToList();
                for (int n = 0; n < schoollist.Count; n++)
                {
                    for (int i = 0; i < cuisines.Count; i++)
                    {
                        if (!_dbContext.Cuisine.Any(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && x.CuisineName == cuisines[i].CuisineName))
                        {
                            var cui = new Entities.Cuisine()
                            {
                                CuisineUuid = Guid.NewGuid(),
                                SchoolUuid = schoollist[n].SchoolUuid,
                                AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                AddPeople = "",
                                LikeNum = 0,
                                IsDelete = 0,
                                CuisineName = cuisines[i].CuisineName,
                                Price = cuisines[i].Price,
                                Burdening = cuisines[i].Burdening,
                                Ingredient = cuisines[i].Ingredient,
                                Abstract = cuisines[i].Abstract,
                                CuisineType = cuisines[i].CuisineType,
                                Accessory = cuisines[i].Accessory,
                            };



                            _dbContext.Cuisine.Add(cui);
                        }
                    }
                    //_dbContext.Cuisine.AddRange(cuisines);
                    _dbContext.SaveChanges();
                }
                var nweekmenus = _dbContext.NweekMenu.Where(x => x.SchoolUuid == schoolEnt.SchoolUuid && x.IsDelete == 0).OrderByDescending(x => x.Datetimes).Take(3).ToList();
                for (int n = 0; n < schoollist.Count; n++)
                {
                    for (int i = 0; i < nweekmenus.Count; i++)
                    {
                        if (!_dbContext.NweekMenu.Any(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.Datetimes == nweekmenus[0].Datetimes && x.IsDelete == 0))
                        {
                            var ids1 = nweekmenus[i].MonMon.ToLower().Split(',');
                            var names1 = _dbContext.Cuisine.Where(x => ids1.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var MonMonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names1
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var MonMon = string.Join(",", MonMonLs);

                            var ids2 = nweekmenus[i].MonNoon.ToLower().Split(',');
                            var names2 = _dbContext.Cuisine.Where(x => ids2.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var MonNoonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names2
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var MonNoon = string.Join(",", MonNoonLs);

                            var ids3 = nweekmenus[i].MonNight.ToLower().Split(',');
                            var names3 = _dbContext.Cuisine.Where(x => ids3.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var MonNightLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names3
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var MonNight = string.Join(",", MonNightLs);

                            var ids4 = nweekmenus[i].TuesMon.ToLower().Split(',');
                            var names4 = _dbContext.Cuisine.Where(x => ids4.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var TuesMonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names4
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var TuesMon = string.Join(",", TuesMonLs);

                            var ids5 = nweekmenus[i].TuesNoon.ToLower().Split(',');
                            var names5 = _dbContext.Cuisine.Where(x => ids5.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var TuesNoonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names5
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var TuesNoon = string.Join(",", TuesNoonLs);

                            var ids6 = nweekmenus[i].TuesNight.ToLower().Split(',');
                            var names6 = _dbContext.Cuisine.Where(x => ids6.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var TuesNightLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names6
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var TuesNight = string.Join(",", TuesNightLs);

                            var ids7 = nweekmenus[i].WedMon.ToLower().Split(',');
                            var names7 = _dbContext.Cuisine.Where(x => ids7.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var WedMonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names7
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var WedMon = string.Join(",", WedMonLs);

                            var ids8 = nweekmenus[i].WedNoon.ToLower().Split(',');
                            var names8 = _dbContext.Cuisine.Where(x => ids8.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var WedNoonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names8
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var WedNoon = string.Join(",", WedNoonLs);

                            var ids9 = nweekmenus[i].WedNight.ToLower().Split(',');
                            var names9 = _dbContext.Cuisine.Where(x => ids9.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var WedNightLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names9
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var WedNight = string.Join(",", WedNightLs);

                            var ids10 = nweekmenus[i].ThursMon.ToLower().Split(',');
                            var names10 = _dbContext.Cuisine.Where(x => ids10.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var ThursMonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names10
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var ThursMon = string.Join(",", ThursMonLs);

                            var ids11 = nweekmenus[i].ThursNoon.ToLower().Split(',');
                            var names11 = _dbContext.Cuisine.Where(x => ids11.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var ThursNoonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names11
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var ThursNoon = string.Join(",", ThursNoonLs);

                            var ids12 = nweekmenus[i].ThursNight.ToLower().Split(',');
                            var names12 = _dbContext.Cuisine.Where(x => ids12.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var ThursNightLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names12
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var ThursNight = string.Join(",", ThursNightLs);

                            var ids13 = nweekmenus[i].FriMon.ToLower().Split(',');
                            var names13 = _dbContext.Cuisine.Where(x => ids13.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var FriMonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names13
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var FriMon = string.Join(",", FriMonLs);

                            var ids14 = nweekmenus[i].FriNoon.ToLower().Split(',');
                            var names14 = _dbContext.Cuisine.Where(x => ids14.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var FriNoonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names14
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var FriNoon = string.Join(",", FriNoonLs);

                            var ids15 = nweekmenus[i].FriNight.ToLower().Split(',');
                            var names15 = _dbContext.Cuisine.Where(x => ids15.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var FriNightLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names15
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var FriNight = string.Join(",", FriNightLs);

                            var ids16 = nweekmenus[i].SatMon.ToLower().Split(',');
                            var names16 = _dbContext.Cuisine.Where(x => ids16.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var SatMonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names16
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var SatMon = string.Join(",", SatMonLs);

                            var ids17 = nweekmenus[i].SatNoon.ToLower().Split(',');
                            var names17 = _dbContext.Cuisine.Where(x => ids17.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var SatNoonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names17
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var SatNoon = string.Join(",", SatNoonLs);

                            var ids18 = nweekmenus[i].SatNight.ToLower().Split(',');
                            var names18 = _dbContext.Cuisine.Where(x => ids18.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var SatNightLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names18
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var SatNight = string.Join(",", SatNightLs);

                            var ids19 = nweekmenus[i].SunMon.ToLower().Split(',');
                            var names19 = _dbContext.Cuisine.Where(x => ids19.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var SunMonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names19
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var SunMon = string.Join(",", SunMonLs);

                            var ids20 = nweekmenus[i].SunNoon.ToLower().Split(',');
                            var names20 = _dbContext.Cuisine.Where(x => ids20.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var SunNoonLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names20
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var SunNoon = string.Join(",", SunNoonLs);

                            var ids21 = nweekmenus[i].SunNight.ToLower().Split(',');
                            var names21 = _dbContext.Cuisine.Where(x => ids21.Contains(x.CuisineUuid.ToString().ToLower())).Select(x => x.CuisineName).ToList();
                            var SunNightLs = _dbContext.Cuisine.Where(x => x.SchoolUuid == schoollist[n].SchoolUuid && x.IsDelete == 0 && names21
                              .Contains(x.CuisineName)).Select(x => x.CuisineUuid).ToArray();
                            var SunNight = string.Join(",", SunNightLs);

                            var menu = new NweekMenu()
                            {
                                NweekMenuUuid = Guid.NewGuid(),
                                SchoolUuid = schoollist[n].SchoolUuid,
                                MonMon = MonMon,
                                MonNight = MonNight,
                                MonNoon = MonNoon,
                                TuesMon = TuesMon,
                                TuesNight = TuesNight,
                                TuesNoon = TuesNoon,
                                WedMon = WedMon,
                                WedNight = WedNight,
                                WedNoon = WedNoon,
                                ThursMon = ThursMon,
                                ThursNight = ThursNight,
                                ThursNoon = ThursNoon,
                                FriMon = FriMon,
                                FriNight = FriNight,
                                FriNoon = FriNoon,
                                SatMon = SatMon,
                                SatNight = SatNight,
                                SatNoon = SatNoon,
                                SunMon = SunMon,
                                SunNight = SunNight,
                                SunNoon = SunNoon,
                                AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                AddPeople = "",
                                IsDelete = 0,
                                Datetimes = nweekmenus[0].Datetimes,

                            };

                            _dbContext.NweekMenu.Add(menu);
                        }



                    }
                    _dbContext.SaveChanges();


                }
            }
            return Ok(response);
        }
    }
}
