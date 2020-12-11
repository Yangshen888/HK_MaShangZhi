using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Picture
{
    [Route("api/v1/Picture/[controller]/[action]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;

        private IHostingEnvironment hostingEnv;
        string[] pictureFormatArray = { "png", "jpg", "jpeg", "bmp", "gif", "ico", "PNG", "JPG", "JPEG", "BMP", "GIF", "ICO" };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public PictureController(haikanSDMSContext dbContext, IMapper mapper, IHostingEnvironment env)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            hostingEnv = env;
        }
        /// <summary>
        /// 返回图片结果
        /// </summary>
        public class PicResultData
        {
            public string Msg { get; set; }
            public string Path { get; set; }
            public string Code { get; set; }
        }

        [HttpPost]
        public async Task<PicResultData> RegistPicture([FromServices] IHostingEnvironment environment)
        {
            var data = new PicResultData();
            string path = string.Empty;
            var files = Request.Form.Files;
            string strpath = "";
            string strimage = "";
            if (files == null || files.Count() <= 0)
            {
                data.Msg = "请选择上传的文件。";
                data.Code = "404";
                return data;
            }

            //格式限制
            var allowType = new string[] { "image/jpg", "image/png", "image/jpeg", "image/gif", "image/bmp" };
            if (files.Any(c => allowType.Contains(c.ContentType)))
            {
                if (files.Sum(c => c.Length) <= 1024 * 1024 * 2)
                {
                    foreach (var file in files)
                    {
                        strpath = Path.Combine("UploadFiles/RegistPicture", DateTime.Now.ToString("MMddHHmmss") + Guid.NewGuid().ToString().Replace("-", "") +
                            file.FileName.Substring(file.FileName.LastIndexOf("."), (file.FileName.Length - file.FileName.LastIndexOf("."))));
                        path = Path.Combine(environment.WebRootPath, strpath);
                        if (strimage == "")
                        {
                            strimage = strpath;
                        }
                        else
                        {
                            strimage +="|"+ strpath;
                        }
                        using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    var response = ResponseModelFactory.CreateInstance;
                    //using (_dbContext)
                    //{
                    //    ProcessPictureViewModel picture = new ProcessPictureViewModel();
                    //    picture.PicturePath = strpath;
                    //    picture.IsDelete = "0";
                    //    picture.AddTime = DateTime.Now.ToString("MMddHHmmss");
                    //    if (_dbContext.SystemSetting.Count(x => x.IsDelete != "1") > 0)
                    //    {
                    //        var entity = _dbContext.SystemSetting.FirstOrDefault(x => x.IsDelete != "1");
                    //        entity.PicturePath = picture.PicturePath;
                    //        entity.IsDelete = picture.IsDelete;
                    //        entity.AddTime = picture.AddTime;
                    //        _dbContext.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        var entity = _mapper.Map<ProcessPictureViewModel, SystemSetting>(picture);
                    //        entity.SystemSettingUuid = Guid.NewGuid();
                    //        entity.PicturePath = picture.PicturePath;
                    //        entity.IsDelete = picture.IsDelete;
                    //        entity.AddTime = picture.AddTime;
                    //        _dbContext.SystemSetting.Add(entity);
                    //        _dbContext.SaveChanges();
                    //    }

                    //}
                    data.Code = "200";
                    data.Path = strimage;
                    data.Msg = "上传成功";
                    return data;
                }
                else
                {
                    data.Code = "404";
                    data.Msg = "图片过大";
                    return data;
                }
            }
            else

            {
                data.Code = "404";
                data.Msg = "图片格式错误";
                return data;
            }
        }
    }
}
