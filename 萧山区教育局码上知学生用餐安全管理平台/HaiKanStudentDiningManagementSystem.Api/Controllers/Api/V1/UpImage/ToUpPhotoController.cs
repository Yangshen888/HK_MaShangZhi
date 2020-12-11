using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.UpImage
{
    [Route("api/v1/UpImage/[controller]/[action]")]
    [ApiController]
    public class ToUpPhotoController : ControllerBase
    {
        private IWebHostEnvironment _hostEnv;

        public ToUpPhotoController(IWebHostEnvironment env)
        {
            _hostEnv = env;
        }


        /// <summary>
        /// 图片对接上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpPhoto([FromForm] string scene, [FromForm] string groupType)
        {
            var resp = ResponseModelFactory.CreateInstance;
            string url = @"https://www.hangzhouyq.cn/zsaImg/fastDfs/uploadFastNew2";

            string fileName = "";
            IFormFileCollection files = Request.Form.Files;
            if (files == null || files.Count() <= 0)
            {
                resp.SetFailed("请上图片文件");
                return Ok(resp);
            }
            var allowType = new string[] { "image/jpeg", "image/png", "image/jpg" };

            byte[] fileContentByte = new byte[0]; // 文件内容二进制
            if (files.Any(c => allowType.Contains(c.ContentType)))
            {
                fileName = files[0].FileName;
                fileContentByte = new byte[files[0].Length];
                var stream = files[0].OpenReadStream();
                stream.Read(fileContentByte);
                stream.Close();

            }
            else
            {
                resp.SetFailed("请上传正确格式的图片");
                return Ok(resp);
            }


            #region 定义请求体中的内容 并转成二进制

            string boundary = "ceshi";
            string Enter = "\r\n";
            string modelIdStr = "--" + boundary + Enter + "Content-Disposition: form-data; name=\"scene\"" + Enter + Enter + scene + Enter;
            string fileContentStr = "--" + boundary + Enter + "Content-Type:application/octet-stream" + Enter + "Content-Disposition: form-data; name=\"file\"; filename=\"" + fileName + "\"" + Enter + Enter;
            string updateTimeStr = Enter + "--" + boundary + Enter + "Content-Disposition: form-data; name=\"groupType\"" + Enter + Enter + groupType + Enter + "--" + boundary + "--";
            //string encryptStr = Enter + "--" + boundary + Enter + "Content-Disposition: form-data; name=\"encrypt\"" + Enter + Enter+ encrypt + Enter + "--" + boundary + "--";


            var modelIdStrByte = Encoding.UTF8.GetBytes(modelIdStr);//modelId所有字符串二进制
            var fileContentStrByte = Encoding.UTF8.GetBytes(fileContentStr);//fileContent一些名称等信息的二进制（不包含文件本身）
            var updateTimeStrByte = Encoding.UTF8.GetBytes(updateTimeStr);//updateTime所有字符串二进制
            //var encryptStrByte = Encoding.UTF8.GetBytes(encryptStr);//encrypt所有字符串二进制
            #endregion


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "multipart/form-data;boundary=" + boundary;

            Stream myRequestStream = request.GetRequestStream();//定义请求流

            #region 将各个二进制 安顺序写入请求流 modelIdStr -> (fileContentStr + fileContent) -> uodateTimeStr -> encryptStr

            myRequestStream.Write(modelIdStrByte, 0, modelIdStrByte.Length);
            myRequestStream.Write(fileContentStrByte, 0, fileContentStrByte.Length);
            myRequestStream.Write(fileContentByte, 0, fileContentByte.Length);
            myRequestStream.Write(updateTimeStrByte, 0, updateTimeStrByte.Length);
            //myRequestStream.Write(encryptStrByte, 0, encryptStrByte.Length);
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();//发送

            Stream myResponseStream = response.GetResponseStream();//获取返回值
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

            string retString = myStreamReader.ReadToEnd();
            var imgvm = Newtonsoft.Json.JsonConvert.DeserializeObject<HaiKanStudentDiningManagementSystem.Api.ViewModels.UpImgViewModel>(retString);
            if (imgvm.msg == "操作成功")
            {
                resp.SetData("http://www.hangzhouyq.cn:85" + imgvm.data);
            }
            else
            {
                resp.SetFailed("获取失败");
            }
            myStreamReader.Close();
            myResponseStream.Close();
            return Ok(resp);
        }
    }
}
