using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Senparc.CO2NET.Trace;
using Senparc.CO2NET.Utilities;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.TenPay.V3;
using System;
using System.IO;
using System.Text;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 测试日志
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Logger()
        {
            //_logger.LogDebug(message: "LogDebug()...");
            _logger.LogInformation(message: "LogInformation()...");
            //_logger.LogWarning(message: "LogWarning()...");
            //_logger.LogError(message: "LogError()...");
            ResponseResultModel response = ResponseModelFactory.CreateResultInstance;
            response.SetSuccess(message: "test logger success");
            return Ok(value: response);
        }

        [HttpGet]
        public string Test1()
        {
            
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }
        [HttpGet]
        public string Test2()
        {
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }
        [HttpGet]
        public string Test3()
        {
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }
        [HttpGet]
        public string Test4()
        {
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }
        [HttpGet]
        public string Test5()
        {
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }
        [HttpGet]
        public string Test6()
        {
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }
        [HttpGet]
        public string Test7()
        {
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }
        [HttpGet]
        public string Test8()
        {
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }
        [HttpGet]
        public string Test9()
        {
            var user = User;
            return "xxxxxxxxxxxxxxx";
        }


        [HttpPost]
        [Consumes("application/xml")]
        [Produces("application/xml")]
        public async System.Threading.Tasks.Task<IActionResult> PayCallBackAsync()
        {
            _logger.LogInformation("产生回调");
            _logger.LogInformation("方法:payCallBackAsync");
            var user = User;
            //var request = Request;
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                _logger.LogInformation(body);
            };
            string result = @"<xml><return_code><![CDATA[SUCCESS]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>";
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(result);
            return Ok(doc);

        }

        [HttpPost]
        [Consumes("application/xml")]
        public IActionResult PayCallBack2()
        {
            _logger.LogInformation("产生回调");
            _logger.LogInformation("方法:paycallback2");
            return Ok();

        }


        private static int? GetInt(ResponseHandler handler, string key)
        {
            var txt = handler.GetParameter(key);
            return int.TryParse(txt, out var num) ? (int?)num : null;
        }

        static string GetTxt(ResponseHandler handler, string key)
        {
            return handler.GetParameter(key);
        }

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="msg"></param>
        private static void Log(string msg)
        {
            var logDir = ServerUtility.ContentRootMapPath($"~/App_Data/PayNotifyUrl/{SystemTime.Now:yyyyMMdd}");
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            var logPath = Path.Combine(logDir, $"{SystemTime.Now:yyyyMMdd}-{SystemTime.Now:HHmmss}-{Guid.NewGuid().ToString("n").Substring(0, 8)}.txt");
            using (var fileStream = System.IO.File.OpenWrite(logPath))
            {
                fileStream.Write(Encoding.Default.GetBytes(msg), 0, Encoding.Default.GetByteCount(msg));
                fileStream.Close();
            }
        }


        /// <summary>
        /// 收到财付通消息后返回的回调通知
        /// </summary>
        /// <param name="return_code"></param>
        /// <param name="return_msg"></param>
        /// <returns></returns>
        private static string PayNotifyXml(string return_code = "SUCCESS", string return_msg = "OK")
        {
            return $@"<xml><return_code><![CDATA[{return_code}]]></return_code><return_msg><![CDATA[{return_msg}]]></return_msg></xml>";
        }

    }
}
