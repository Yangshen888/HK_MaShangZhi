using System;
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
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Ingredient;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Ingredient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Ingredient
{
    [Route("api/v1/Ingredient/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class IngredientController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostEnv;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public IngredientController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnv = env;
        }

        /// <summary>
        /// 食材库列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(IngredientRequestpayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {


                var query = _dbContext.Ingredient.Include(x=>x.SchoolUu).Select(x=>new 
                {
                    x.Id,
                    x.IngredientUuid,
                    x.FoodName,
                    x.TypeUuid,
                    x.AddTime,
                    x.AddPeople,
                    x.IsDelete,
                    x.SchoolUuid,
                    x.SchoolUu.SchoolName,
                    x.HeatEnergy,
                    x.Protein,
                    x.Fat,
                    x.Va,
                    x.Saccharides,
                    x.Accessory,
                    type=x.TypeUu.Name
                }).Where(x => x.IsDelete == 0);
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.FoodName.Contains(payload.Kw));
                }
                if (!string.IsNullOrEmpty(payload.kw2))
                {
                    query = query.Where(x => x.TypeUuid ==Guid.Parse( payload.kw2));
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
        /// 类型列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult FoodTypeList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.FoodType.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 创建食材信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(IngredientViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (model.SchoolUuid!=null)
                {
                    var query = _dbContext.Ingredient.FirstOrDefault(x => x.IsDelete != 1 && x.SchoolUuid == model.SchoolUuid && x.FoodName == model.FoodName);
                    if (query!=null)
                    {
                        response.SetFailed("该食材已存在");
                        return Ok(response);
                    }
                }
                else
                {
                    var query = _dbContext.Ingredient.FirstOrDefault(x => x.IsDelete != 1 && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid && x.FoodName == model.FoodName);
                    if (query != null)
                    {
                        response.SetFailed("该食材已存在");
                        return Ok(response);
                    }
                }
                var Ingredient = new Entities.Ingredient()
                {
                    IngredientUuid = Guid.NewGuid(),
                    FoodName = model.FoodName,
                    TypeUuid = model.TypeUuid,
                    HeatEnergy=model.HeatEnergy,
                    Protein=model.Protein,
                    Fat=model.Fat,
                    Saccharides=model.Saccharides,
                    Va=model.Va,
                    AddTime = model.AddTime,
                    AddPeople = model.AddPeople,
                    IsDelete = 0,
                    Accessory=model.Accessory,
                    SchoolUuid = model.SchoolUuid != null ? model.SchoolUuid : AuthContextService.CurrentUser.SchoolGuid,
                };
                _dbContext.Ingredient.Add(Ingredient);
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
        /// 编辑食材信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Ingredient.Include(x=>x.SchoolUu).Select(x=>new {x.IngredientUuid,x.FoodName,x.TypeUuid,x.HeatEnergy,x.Protein,x.Fat,x.Saccharides,x.Va,x.AddPeople,x.AddTime,x.IsDelete,x.SchoolUuid,x.SchoolUu.SchoolName,x.Accessory }).FirstOrDefault(x => x.IngredientUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑的食材信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(IngredientViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Ingredient.FirstOrDefault(x => x.IngredientUuid == model.IngredientUuid);
                if (entity == null)
                {
                    response.SetFailed("该流程不存在");
                    return Ok(response);
                }
                if (entity.FoodName != model.FoodName)
                {
                    if (model.SchoolUuid != null)
                    {
                        var query = _dbContext.Ingredient.FirstOrDefault(x => x.IsDelete != 1 && x.SchoolUuid == model.SchoolUuid && x.FoodName == model.FoodName);
                        if (query != null)
                        {
                            response.SetFailed("该食材已存在");
                            return Ok(response);
                        }
                    }
                    else
                    {
                        var query = _dbContext.Ingredient.FirstOrDefault(x => x.IsDelete != 1 && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid && x.FoodName == model.FoodName);
                        if (query != null)
                        {
                            response.SetFailed("该食材已存在");
                            return Ok(response);
                        }
                    }
                }
                entity.FoodName = model.FoodName;
                entity.TypeUuid = model.TypeUuid;
                entity.HeatEnergy = model.HeatEnergy;
                entity.Protein = model.Protein;
                entity.Fat = model.Fat;
                entity.Saccharides = model.Saccharides;
                entity.Va = model.Va;
                entity.AddTime = model.AddTime;
                entity.AddPeople = model.AddPeople;
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
        /// 删除食材信息
        /// </summary>
        /// <param name="ids"></param>
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
        /// 批量删除
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
        /// 根据名字获取食材
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(200)]
        public IActionResult Get(string name)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (name.Trim() == "null")
            {
                response.SetData(null);
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Ingredient.FirstOrDefault(x => x.FoodName == name.Trim()&&x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                if (entity!=null&&entity.Accessory == null)
                {
                    entity.Accessory = "";
                }
                
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 根据名字获取食材(添加/二次编辑)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(200)]
        public IActionResult Get2(string name)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Ingredient.FirstOrDefault(x => x.FoodName == name.Trim() && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid&&x.IsDelete==0);
                if (entity != null && entity.Accessory == null)
                {
                    entity.Accessory = "";
                }
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
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
                var sql = string.Format("UPDATE Ingredient SET IsDelete=@IsDelete WHERE IngredientUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDelete));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
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
    }
}
