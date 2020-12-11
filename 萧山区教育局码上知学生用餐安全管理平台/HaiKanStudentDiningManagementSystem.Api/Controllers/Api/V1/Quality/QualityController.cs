using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Quality;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.DiningRoom;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Quality;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Quality
{
    [Route("api/v1/Quality/[controller]/[action]")]
    [ApiController]

    public class QualityController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;
        public QualityController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 质量文件列表展示
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public IActionResult QualityList(QualityRequestpayload payload)
        {
            using (_dbContext)
            {
                var query = from p in _dbContext.Quality
                            where p.IsDelete == 0
                            select new
                            {
                                p.QualityUuid,
                                p.SchoolUuid,
                                p.FlieName,
                                p.JianJie,
                                p.IsDelete,
                                p.EffectiveTime
                                //p.Accessory,
                            };
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.FlieName.Contains(payload.kw));
                }
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.EffectiveTime);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
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
        public IActionResult Create(QualityViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            //if (string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolUuid))
            //{
            //    response.SetFailed("");
            //    return Ok(response);
            //}
            using (_dbContext)
            {

                var entity = new HaiKanStudentDiningManagementSystem.Api.Entities.Quality();
                entity.QualityUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.FlieName = model.FlieName;
                entity.JianJie = model.JianJie;
                entity.SchoolUuid = model.SchoolUuid;
                entity.Accessory = model.Accessory;
                entity.EffectiveTime = Convert.ToDateTime( model.EffectiveTime).ToString("yyyy-MM-dd");
                //entity.SchoolUuid = Guid.NewGuid();
                _dbContext.Quality.Add(entity);
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
        public IActionResult QualityEdit(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;

            string guid = model.qualityUuid;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Quality.FirstOrDefault(x => x.QualityUuid == Guid.Parse(guid));
                entity.FlieName = model.flieName;
                entity.JianJie = model.jianJie;
                entity.Accessory = model.accessory;
                entity.EffectiveTime = Convert.ToDateTime(model.effectiveTime).ToString("yyyy-MM-dd");
                string[] list=entity.EffectiveTime.Split("-");
                entity.EffectiveTime = list[0] + "-" + list[1] + "-" + (int.Parse(list[2])+1);
                _dbContext.SaveChanges();
                response.SetSuccess("修改成功");
                return Ok(response);
            }
        }


        //获取信息
        [HttpGet]
        public IActionResult QualityGet(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Quality.Where(x => x.QualityUuid == guid && x.IsDelete != 1).ToList();
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
                var entity = _dbContext.Quality.FirstOrDefault(x => x.QualityUuid.ToString() == ids);
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
                    response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);

                    //var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    //foreach (var item in parameters)
                    //{
                    //    var query1 = _dbContext.Quality.FirstOrDefault(x => x.QualityUuid == Guid.Parse(item.Value.ToString()));
                    //    query1.IsDelete = 1;
                    //    _dbContext.SaveChanges();
                    //}
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {

                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE Quality SET IsDelete=@IsDelete WHERE QualityUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        /// <summary>
        /// 文件上传
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
                response.SetFailed("请上传文件");
                return Ok(response);
            }
            try
            {
                //foreach (var file in files)
                //{
                int index = files[0].FileName.LastIndexOf('.');
                string fname = Guid.NewGuid().ToString() + files[0].FileName.Substring(index);
                string allfname = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + fname;
                string strpath = Path.Combine("UploadFiles/File", allfname);
                string path = Path.Combine(_hostingEnvironment.WebRootPath, strpath);
                //System.IO.File.Create(path);
                var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //var stream2 = new FileStream(Environment.GetEnvironmentVariable("TEMP") + "/"+ allfname, FileMode.OpenOrCreate, FileAccess.ReadWrite);


                //await files[0].CopyToAsync(stream2);
                //if (files[0].FileName.Substring(index) == ".txt")
                //{
                //    StreamWriter writer = new StreamWriter(stream, Encoding.Unicode);
                //    writer.Flush();
                //    writer.Close();
                //}
                //else
                //{
                //    StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                //    writer.Flush();
                //    writer.Close();
                //}
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
        public IActionResult DeletetoFile(LSPicture lsp)
        {
            var response = ResponseModelFactory.CreateInstance;

            var path = Path.Combine(_hostingEnvironment.WebRootPath + "/UploadFiles/File", lsp.Path);
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
        /// 取得一个文本文件流的编码方式。   
        /// </summary>   
        /// <param name="stream">文本文件流。</param>   
        /// <param name="defaultEncoding">默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。</param>   
        /// <returns></returns>   
        private static Encoding GetEncoding(FileStream stream, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            if (stream != null && stream.Length >= 2)
            {
                //保存文件流的前4个字节   
                byte byte1 = 0;
                byte byte2 = 0;
                byte byte3 = 0;
                byte byte4 = 0;
                //保存当前Seek位置   
                long origPos = stream.Seek(0, SeekOrigin.Begin);
                stream.Seek(0, SeekOrigin.Begin);

                int nByte = stream.ReadByte();
                byte1 = Convert.ToByte(nByte);
                byte2 = Convert.ToByte(stream.ReadByte());
                if (stream.Length >= 3)
                {
                    byte3 = Convert.ToByte(stream.ReadByte());
                }
                if (stream.Length >= 4)
                {
                    byte4 = Convert.ToByte(stream.ReadByte());
                }
                //根据文件流的前4个字节判断Encoding   
                //Unicode {0xFF, 0xFE};   
                //BE-Unicode {0xFE, 0xFF};   
                //UTF8 = {0xEF, 0xBB, 0xBF};   
                if (byte1 == 0xFE && byte2 == 0xFF)//UnicodeBe   
                {
                    targetEncoding = Encoding.BigEndianUnicode;
                }
                if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF)//Unicode   
                {
                    targetEncoding = Encoding.Unicode;
                }
                if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF)//UTF8   
                {
                    targetEncoding = Encoding.UTF8;
                }
                //恢复Seek位置         
                stream.Seek(origPos, SeekOrigin.Begin);
            }
            return targetEncoding;
        }
    }
}
