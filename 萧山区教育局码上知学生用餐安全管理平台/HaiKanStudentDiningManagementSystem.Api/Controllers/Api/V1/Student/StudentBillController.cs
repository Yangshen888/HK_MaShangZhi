using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using AutoMapper;
using HaiKan3.Utils;
using HaiKanStudentDiningManagementSystem.Api.Configurations;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Pay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Senparc.CO2NET.HttpUtility;
using Senparc.Weixin;
using Senparc.Weixin.TenPay.V3;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Student
{
    [Route("api/v1/student/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class StudentBillController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        //private IWebHostEnvironment _hostingEnvironment;
        private static readonly string Appid = "wx0bf342f51437ca67";
        private static readonly string Secret = "b58718e16c0d2736f3959587ccd4f24f";
        //private static string Mch_id = "";


        public StudentBillController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment hostingEnvironment, ILogger<StudentBillController> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            //_hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="idcard"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetBillInfo(string idcard, Guid? suid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var query = _dbContext.StudentBill.Where(x => x.IdcardNum == idcard && x.IsDeleted == 0 && x.OrderMoney < x.AmountPayable && x.AmountPayable > 0 && x.SchoolUuid == suid);
            var list = query.Select(x => new
            {
                x.StudentBillUuid,
                x.OrderName,
                x.AmountPayable,
                x.OrderMoney
            }).ToList();
            var entity = query.FirstOrDefault();
            if (entity != null)
            {
                var ent = new { entity.StudentName, entity.IdcardNum, entity.ClassName, entity.StudentNum, entity.Sex };
                response.SetData(new { ent, list });
            }
            else
            {
                response.SetData(null);
            }

            return Ok(response);
        }


        [HttpPost]
        public IActionResult GetBillList(string idcard)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            return Ok(response);

        }

        /// <summary>
        /// 统一下单(生成预支付交易单,返回信息后在小程序中调起支付)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Unifiedorder(UnidiedorderData udata)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateResultInstance;

                var stu = _dbContext.StudentBill.FirstOrDefault(x => x.StudentBillUuid == udata.BillGuid);


                if (stu == null)
                {
                    response.SetFailed("未查找到对应缴费信息");
                    return Ok(response);
                }
                if (stu.OrderMoney >= stu.AmountPayable)
                {
                    response.SetFailed("已缴费");
                    return Ok(response);
                }

                //时间戳
                string timeStamp = TenPayV3Util.GetTimestamp();
                //随机字符串
                string nonceStr = TenPayV3Util.GetNoncestr();
                string appid = "wx0bf342f51437ca67";

                //获取学校绑定商户信息
                var school = _dbContext.School.FirstOrDefault(x => x.SchoolUuid == udata.Guid);
                if (school == null)
                {
                    response.SetFailed("未查找到对应学校");
                    return Ok(response);
                }
                if (school.Yard == null || school.Secretkey == null)
                {
                    response.SetFailed("未查找到对应学校商户信息");
                    return Ok(response);
                }


                //商户号
                string mch_id = school.Yard;//"1600884893";
                //商户支付秘钥
                string partnerKey = AES.AesDecrypt(school.Secretkey, HaiKan3.Utils.AES.Key);//"ew6QCdWiDfcif902EbC07dh0icTuM5le";
                //签名
                string sign = "";
                string sign_type = "MD5";

                //商品描述
                string body = udata.Body;
                //商户订单号
                string out_trade_no = "";
                //标价金额(单位:分)
                int total_fee = udata.Totalfee;
                //终端IP
                //string spbill_create_ip = "183.158.56.51"; //Request.HttpContext.Connection.RemoteIpAddress.ToString();
                string spbill_create_ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                _logger.LogInformation("ip:" + spbill_create_ip);
                //通知地址
                string notify_url = "http://msz-b.jiulong.yoruan.com/test/PayCallBack";
                //string notify_url = "http://msz-b.jiulong.yoruan.com/api/v1/student/StudentBill/PayCallBack";
                //交易类型
                string trade_type = "JSAPI";
                //预支付id
                string prepayId = "";
                //微信调用支付的签名
                string paySign = "";
                //用户openid
                string openid = udata.Openid;
                Store_Info info = new Store_Info()
                {
                    address = "xxxxxx",
                    area_code = "330185",
                    id = "MSZzf" + appid,
                    name = "码上知支付商城",
                };
                TenPayV3UnifiedorderRequestData_SceneInfo sceneInfo = new TenPayV3UnifiedorderRequestData_SceneInfo(false);
                sceneInfo.store_info = info;

                //生成订单号
                out_trade_no = DateTime.Now.ToString("yyyyMMddHHmmss") + TenPayV3Util.BuildRandomStr(14);
                _logger.LogInformation("订单号:" + out_trade_no);


                TenPayV3UnifiedorderRequestData requestData = new TenPayV3UnifiedorderRequestData(appid, mch_id, body, out_trade_no, total_fee, spbill_create_ip, notify_url, Senparc.Weixin.TenPay.TenPayV3Type.JSAPI, openid, partnerKey, nonceStr, null, DateTime.Now, DateTime.Now.AddHours(2), null, null, "CNY", null, null, false, sceneInfo, null);

                var urlFormat = ReurnPayApiUrl("https://api.mch.weixin.qq.com/{0}pay/unifiedorder");
                var data = requestData.PackageRequestHandler.ParseXML();//获取XML
                _logger.LogInformation("xml:" + data);
                var str = PostXmlMethod.PostXmla(urlFormat, data);
                _logger.LogInformation("postxml:" + str);

                DataSet ds = new DataSet();
                StringReader stream = new StringReader(str);//读取字符串为数据量
                XmlTextReader reader = new XmlTextReader(stream);//对XML的数据流的只进只读访问
                ds.ReadXml(reader);//把数据读入DataSet

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["return_code"].ToString() == "SUCCESS")
                    {
                        _logger.LogInformation("成功");
                        _logger.LogInformation("timeStamp:" + timeStamp + "; nonceStr:" + nonceStr + "; prepayId:" + ds.Tables[0].Rows[0]["prepay_id"].ToString() + "; paySign:" + ds.Tables[0].Rows[0]["sign"].ToString() + "; partnerKey:" + partnerKey);
                        response.SetData(new { appid = ds.Tables[0].Rows[0]["appid"].ToString(), timeStamp, nonceStr, prepayId = ds.Tables[0].Rows[0]["prepay_id"].ToString(), sign_type, paySign = ds.Tables[0].Rows[0]["sign"].ToString(), sjcode = Electroniccode.getString(32), key = partnerKey, outtradeno = out_trade_no });
                        BillState bill = new BillState()
                        {
                            BillUuid = Guid.NewGuid(),
                            BillNum = out_trade_no,
                            Appid = appid,
                            MchId = mch_id,
                            Money = total_fee,
                            Key = partnerKey,
                            SbillUuid = udata.BillGuid,
                        };
                        _logger.LogInformation("订单记录:" + JsonConvert.SerializeObject(bill));
                        _dbContext.BillState.Add(bill);
                        _dbContext.SaveChanges();
                        return Ok(response);
                    }
                    else
                    {
                        response.SetFailed(ds.Tables[0].Rows[0]["return_msg"].ToString());
                        _logger.LogInformation("异常");
                        _logger.LogInformation(ds.Tables[0].Rows[0]["return_msg"].ToString());
                        return Ok(response);
                    }
                }
                else
                {
                    response.SetFailed("订单信息为空");
                    return Ok(response);
                }

            }

        }


        /// <summary>
        /// 返回可用的微信支付地址（自动判断是否使用沙箱）
        /// </summary>
        /// <param name="urlFormat">如：<code>https://api.mch.weixin.qq.com/{0}pay/unifiedorder</code></param>
        /// <returns></returns>
        private static string ReurnPayApiUrl(string urlFormat)
        {
            return string.Format(urlFormat, Senparc.Weixin.Config.UseSandBoxPay ? "sandboxnew/" : "");
        }


        public class OrderData
        {

            public string idcard { get; set; }
            public decimal OrderMoney { get; set; }
            public string SerialNumber { get; set; }
            public string OrderNum { get; set; }
            public Guid? SystemUserUuid { get; set; }
        }

        [HttpPost]
        public IActionResult BillSuccess(OrderData model)
        {
            using (_dbContext)
            {
                var response = ResponseModelFactory.CreateResultInstance;

                var query = _dbContext.StudentBill.FirstOrDefault(x => x.IdcardNum == model.idcard);

                if (query == null)
                {
                    response.SetFailed("查询学生信息失败");
                    return Ok(response);
                }
                if (query.OrderNum != model.OrderNum && DateTime.Parse(query.OederTime) < DateTime.Now)
                {
                    query.OrderMoney = query.OrderMoney + model.OrderMoney;
                    query.SerialNumber = model.SerialNumber;
                    query.OrderNum = model.OrderNum;
                    query.SystemUserUuid = model.SystemUserUuid;
                    query.OederTime = DateTime.Now.ToString();
                    _dbContext.SaveChanges();
                }



                return Ok(response);
            }


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
    }
}
