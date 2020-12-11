using Haikan.DebugTools;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Haikan.DBHelper;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

using System.Security.Cryptography;
using System.Linq;

using HaiKanTaskHousekeeperCore.Data;
using HaiKanTaskHousekeeperCore.mData;
using System.Configuration;
using System.Linq.Expressions;
using Senparc.Weixin.TenPay.V3;
using log4net.Appender;

namespace HaiKanTaskHousekeeperWatchDog
{
    public partial class FrmMain : Form
    {
        #region 实例化对象
        //实例化Timer类，设置间隔时间为1秒；  
        System.Timers.Timer t = new System.Timers.Timer(1000);
        System.Timers.Timer t2 = new System.Timers.Timer(1000);

        MSZ context_m = new MSZ();
        MSZ context_m2 = new MSZ();
        MSZDJ context_dj = new MSZDJ();
        List<FoodType> types = new List<FoodType>();
        List<Schinfo> schools = new List<Schinfo>();
        Dictionary<string, int> dic = new Dictionary<string, int>();
        int num = 0;
        int ingcount = 0;

        #endregion
        public FrmMain()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Console.ForegroundColor = ConsoleColor.Green;
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            //var st = DateTime.Now;
            types = context_m.FoodType.ToList();

            //AddLog("看门狗", "" + typeof(FrmMain) + "", "看门狗程序启动");
            LogHelper.WriteLog(typeof(FrmMain), "看门狗程序启动！");
            //到达时间的时候执行事件； 
            t.Elapsed += timerDo_Tick;
            t2.Elapsed += timerDo2_Tick;


            //设置是执行一次（false）还是一直执行(true)；   
            t.AutoReset = true;
            t2.AutoReset = true;

            //是否执行System.Timers.Timer.Elapsed事件；  
            t.Enabled = true;
            t2.Enabled = true;
            //AddLog("看门狗", "" + typeof(FrmMain) + "", "看门狗程序启动完毕！");
            LogHelper.WriteLog(typeof(FrmMain), "看门狗程序启动完毕！");

            //var et = DateTime.Now;
            //var td = et - st;
            //Console.WriteLine(td.ToString("g"));

            //string result = Encrypt("123456", "eda53bf9af5f48749208139abb190231");
        }



        /// <summary>
        /// 查询相应的表，执行相应的硬件控制方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDo_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {


                var time = DateTime.Now;
                Console.WriteLine(time);
                Console.WriteLine(time.Second);



                if (time.Second % 10 != 0)
                {
                    Console.WriteLine("跳出");
                    LogHelper.WriteLog(typeof(FrmMain), "非时间段");
                    return;
                }
                LogHelper.WriteLog(typeof(FrmMain), "计时器事件触发");
                LogHelper.WriteLog(typeof(FrmMain), time.ToString("yyyyy-MM-dd HH:mm:ss:fff"));
                t.Enabled = false;

                schools = context_m.School.Where(x => x.IsDelete == 0).Select(x => new Schinfo
                {
                    ID = x.ID,
                    SchoolUUID = x.SchoolUUID,
                    SchoolName = x.SchoolName
                }).ToList();

                //schools = new List<Schinfo>() { new Schinfo() { ID = 26, SchoolUUID = Guid.Parse("46E110F7-DD05-4BB8-93C4-30F9CB803A94"),SchoolName= "渔浦小学" } };

                for (int n = 0; n < schools.Count; n++)
                {

                    var schname = schools[n].SchoolName;
                    if (!dic.ContainsKey(schname))
                    {
                        dic.Add(schname, 0);
                    }
                    LogHelper.WriteLog(typeof(FrmMain), "循环学校名称:" + schname);

                    var schuuid = schools[n].SchoolUUID;
                    var query_buyers = from wbb in context_dj.world_barn_buyers
                                       join wbu in context_dj.world_barn_users
                                       on wbb.world_barn_user_id equals wbu.id
                                       where wbu.school_name.Contains(schname) && (!wbb.buyer_order_shortname.Contains("教师"))
                                       select wbb.id;
                    var buyersdata = query_buyers.ToList();
                    List<long> buyers = buyersdata.ConvertAll(x => decimal.ToInt64(x));
                    LogHelper.WriteLog(typeof(string), JsonConvert.SerializeObject(buyers));
                    if (buyers.Count > 0)
                    {
                        LogHelper.WriteLog(typeof(FrmMain), "存在学校数据:" + schname);
                        LogHelper.WriteLog(typeof(FrmMain), "学校uuid:" + schuuid);

                        #region 食材同步

                        LogHelper.WriteLog(typeof(FrmMain), "食材同步开始");
                        //var count = context_m.Ingredient.Where(x => x.SchoolUUID == school.SchoolUUID && x.IsDelete == 0).Count();
                        //Console.WriteLine(count);
                        //LogHelper.WriteLog(typeof(FrmMain), "原食材查询(条)" + count);

                        var query = from wbg in context_dj.world_barn_goods
                                    join wbgc in context_dj.world_barn_goods_categories
                                    on wbg.small_class_id equals wbgc.id
                                    select new
                                    {
                                        wbg.id,
                                        wbg.goods_name,
                                        wbg.goods_code,
                                        type = wbgc.name,
                                        wbg.goods_desc,
                                        wbg.unit_code,
                                        wbg.rs_memo,
                                    };
                        var goods = query.ToList();

                        LogHelper.WriteLog(typeof(FrmMain), "食材查询(条)" + goods.Count);
                        if (goods.Count != ingcount)
                        {
                            LogHelper.WriteLog(typeof(FrmMain), "循环开始1");
                            string ty = "";
                            for (int i = 0; i < goods.Count; i++)
                            {
                                var name = goods[i].goods_name;
                                if (context_m.Ingredient.Any(x => x.SchoolUUID == schuuid && x.FoodName == name && x.IsDelete == 0))
                                {
                                    var entity = context_m.Ingredient.FirstOrDefault(x => x.SchoolUUID == schuuid && x.FoodName == name && x.IsDelete == 0);
                                    entity.TypeUUID = types.Find(x => x.Name == goods[i].type).TypeUUID;
                                    if (string.IsNullOrEmpty(entity.Accessory) && (!string.IsNullOrEmpty(goods[i].rs_memo)))
                                    {
                                        //int index = goods[i].rs_memo.LastIndexOf('.');
                                        //string suffix = goods[i].rs_memo.Substring(index);
                                        //string infix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + Guid.NewGuid().ToString();
                                        //string filename = @"C:\inetpub\wwwroot\wwwroot\UploadFiles\LiveShotPicture\" + infix + suffix;
                                        //var check = WriteBytesToFile(filename, GetBytesFromUrl(goods[i].rs_memo));
                                        //if (check)
                                        //{
                                        //    entity.Accessory = infix + suffix;
                                        //}
                                        context_m.SaveChanges();

                                    }
                                    ty = "修改";

                                }
                                else
                                {
                                    Ingredient ingredient = new Ingredient()
                                    {
                                        IngredientUUID = Guid.NewGuid(),
                                        FoodName = goods[i].goods_name,
                                        AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                        AddPeople = "狗",
                                        SchoolUUID = schuuid,
                                        IsDelete = 0,
                                        TypeUUID = types.Find(x => x.Name == goods[i].type).TypeUUID,
                                        HeatEnergy = 0,
                                        Protein = 0,
                                        Fat = 0,
                                        Saccharides = 0,
                                        VA = 0,
                                    };
                                    if (!string.IsNullOrEmpty(goods[i].rs_memo))
                                    {
                                        int index = goods[i].rs_memo.LastIndexOf('.');
                                        string suffix = goods[i].rs_memo.Substring(index);
                                        string infix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + Guid.NewGuid().ToString();
                                        string filename = @"C:\inetpub\wwwroot\wwwroot\UploadFiles\LiveShotPicture\" + infix + suffix;
                                        var check = WriteBytesToFile(filename, GetBytesFromUrl(goods[i].rs_memo));
                                        if (check)
                                        {
                                            ingredient.Accessory = infix + suffix;
                                        }
                                    }
                                    context_m.Ingredient.Add(ingredient);
                                    context_m.SaveChanges();
                                    ty = "添加";


                                }
                                Console.WriteLine("循环1第" + i + "次"+ty);
                                if (i % 50 == 0)
                                {
                                    LogHelper.WriteLog(typeof(FrmMain), "循环1第" + i + "次");
                                }
                            }
                            //context_m.SaveChanges();
                            LogHelper.WriteLog(typeof(FrmMain), "循环结束1");
                        }
                        else
                        {
                            LogHelper.WriteLog(typeof(FrmMain), "总数不变,未进循环1");

                        }
                        //ingcount = goods.Count;
                        


                        //手动填写的食材

                        LogHelper.WriteLog(typeof(FrmMain), "手动食材名称同步");
                        var query3 = context_dj.world_barn_order_items.GroupBy(x => x.goods_name).ToList();
                        LogHelper.WriteLog(typeof(FrmMain), "手动食材名称查询(条)" + query3.Count);

                        LogHelper.WriteLog(typeof(FrmMain), "循环开始2");

                        for (int i = 0; i < query3.Count; i++)
                        {
                            var key = query3[i].Key;
                            Console.WriteLine(query3[i].Key);
                            if (!context_m.Ingredient.Any(x => x.SchoolUUID == schuuid && x.FoodName == key && x.IsDelete == 0))
                            {
                                Ingredient ingredient = new Ingredient()
                                {
                                    IngredientUUID = Guid.NewGuid(),
                                    FoodName = key,
                                    AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                    AddPeople = "狗",
                                    SchoolUUID = schuuid,
                                    IsDelete = 0,

                                };
                                context_m.Ingredient.Add(ingredient);
                                context_m.SaveChanges();

                            }
                            Console.WriteLine("循环2第" + i + "次");
                            if (i % 50 == 0)
                            {
                                LogHelper.WriteLog(typeof(FrmMain), "循环2第" + i + "次");
                            }
                        }
                        //context_m.SaveChanges();

                        LogHelper.WriteLog(typeof(FrmMain), "循环结束2");
                        //}


                        #endregion

                        //同步采购记录
                        LogHelper.WriteLog(typeof(FrmMain), "采购记录同步开始");

                        var query2 = from wboi in context_dj.world_barn_order_items
                                     join wbo in context_dj.world_barn_orders
                                     on wboi.order_id equals wbo.order_id
                                     where wbo.status == 8 && buyers.Contains(wbo.buyer_id)
                                     select new
                                     {
                                         wbo.customer_delivery_date,
                                         wbo.order_id,
                                         wbo.total_amount,
                                         wboi.id,
                                         wboi.goods_name,
                                         wboi.demanded_quantity,
                                         wboi.real_price,
                                         wboi.unit_price,
                                         wboi.subtotal,
                                         wboi.unit,
                                         wboi.note,
                                         wboi.supplier_name,
                                         wbo.buyer_name,
                                     };
                        //二次遍历只遍历之前3个月的数据(可自行修改)
                        if (dic[schname] > 0)
                        {
                            var date = DateTime.Now.AddMonths(-3);
                            query2 = query2.Where(x => x.customer_delivery_date > date);
                        }
                        var records = query2.ToList();

                        LogHelper.WriteLog(typeof(FrmMain), "采购记录查询(条)" + records.Count);

                        LogHelper.WriteLog(typeof(FrmMain), "循环开始3");
                        for (int i = 0; i < records.Count; i++)
                        {
                            var foodname = records[i].goods_name;
                            //Console.WriteLine(foodname);
                            var date = records[i].customer_delivery_date.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            var ing = context_m.Ingredient.Select(x=>new {x.FoodName,x.IngredientUUID,x.ID,x.IsDelete,x.SchoolUUID }).FirstOrDefault(x => x.FoodName == foodname && x.IsDelete == 0&&x.SchoolUUID==schuuid);
                            //if ((foodname == "茭白肉(斤)"||foodname== "冷鲜全精后腿肉片(斤)")&& date=="2020-10-29 08:00:00") 
                            //{
                            //    Console.WriteLine(JsonConvert.SerializeObject(ing));
                            //    Console.WriteLine(!context_m.PurchaseRecord.Any(x => x.IngredientUUID == ing.IngredientUUID && x.PurchaseDate == date && x.IsDelete == 0));
                            //}
                            if (!context_m.PurchaseRecord.Any(x => x.IngredientUUID == ing.IngredientUUID && x.PurchaseDate == date && x.IsDelete == 0&&x.SchoolUUID==schuuid))
                            {
                                PurchaseRecord purchase = new PurchaseRecord()
                                {
                                    PurchaseUUID = Guid.NewGuid(),
                                    IngredientUUID = ing.IngredientUUID,
                                    Supplier = records[i].supplier_name,
                                    PurchaseDate = records[i].customer_delivery_date.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                                    PurchaseNum = records[i].demanded_quantity.ToString(),
                                    Price = (double)records[i].real_price,
                                    State = "1",
                                    SystemUserUUID = records[i].buyer_name,
                                    IsDelete = 0,
                                    AddPeople = "狗",
                                    AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                                    SchoolUUID = schuuid,
                                    Unit = records[i].unit,
                                };
                                context_m.PurchaseRecord.Add(purchase);
                                context_m.SaveChanges();
                                //if ((foodname == "茭白肉(斤)" || foodname == "冷鲜全精后腿肉片(斤)" )&& date == "2020-10-29 08:00:00.0")
                                //{
                                //Console.WriteLine(JsonConvert.SerializeObject(purchase));

                                //}

                            }
                            Console.WriteLine("循环3第" + i + "次");
                            if (i % 50 == 0)
                            {
                                LogHelper.WriteLog(typeof(FrmMain), "循环3第" + i + "次");
                            }
                        }
                        LogHelper.WriteLog(typeof(FrmMain), "循环结束3");
                        var endtime = DateTime.Now;

                        var td = endtime - time;
                        LogHelper.WriteLog(typeof(FrmMain), td.ToString("g"));




                        //try
                        //{
                        //    t.Enabled = true;



                        //    LogHelper.WriteLog(typeof(FrmMain), "看门狗程序完毕！");
                        //}
                        //catch (Exception ex)
                        //{
                        //    t.Enabled = true;
                        //    LogHelper.WriteLog(typeof(FrmMain), "遇到错误：" + ex);
                        //}
                        //if (dic.ContainsKey(schname))
                        //{
                        dic[schname]++;
                        //}
                        //else
                        //{
                        //    dic.Add(schname, 1);
                        //}
                        //num++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FrmMain), "错误:" + ex.Message);
                //t.Enabled = true;
            }






            t.Enabled = true;
        }


        /// <summary>
        /// 根据网路路径获取图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        static public byte[] GetBytesFromUrl(string url)
        {
            byte[] b = { };
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse myResp = myReq.GetResponse();
                Stream stream = myResp.GetResponseStream();
                //int i;
                using (BinaryReader br = new BinaryReader(stream))
                {
                    //i = (int)(stream.Length);
                    b = br.ReadBytes(500000);
                    br.Close();
                }
                myResp.Close();
                return b;
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取图片错误"+ex.Message+url);
                LogHelper.WriteLog(typeof(string), "获取图片错误" + ex.Message + url);
                return b;
            }


        }

        /// <summary>
        /// 将图片字节写入文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        static public bool WriteBytesToFile(string fileName, byte[] content)
        {
            if (content.Length <= 0)
            {
                return false;
            }
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("图片错误："+ex.Message);
                return false;
            }
            finally
            {
                fs.Close();
                w.Close();
            }

        }


        /// <summary>
        /// 微信支付的结果校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerDo2_Tick(object sender, ElapsedEventArgs e)
        {
            
            t2.Enabled = false;
            Console.WriteLine("开始支付查询");
            LogHelper.WriteLog(typeof(string), "开始支付查询");
            try
            {
                var time = DateTime.Now;
                if (time.Second % 3 == 0)
                {
                    context_m2.Dispose();
                    context_m2 = new MSZ();
                    //t.Enabled = false;
                    var list = context_m2.BillState.Where(x => x.State == 0).ToList();
                    if (list.Count == 0)
                    {
                        LogHelper.WriteLog(typeof(string), "无数据跳出支付查询");
                        t2.Enabled = true;
                        return;
                    }
                    else
                    {
                        LogHelper.WriteLog(typeof(FrmMain), "进行支付遍历");
                        for (int i = 0; i < list.Count; i++)
                        {
                            var entity = list[i];
                            string nonceStr = TenPayV3Util.GetNoncestr();
                            TenPayV3OrderQueryRequestData order = new TenPayV3OrderQueryRequestData(entity.Appid, entity.Mch_id, null, nonceStr, entity.BillNum, entity.Key, "MD5");
                            var result = TenPayV3.OrderQuery(order);
                            LogHelper.WriteLog(typeof(FrmMain), "返回结果:" + JsonConvert.SerializeObject(result));
                            var bill = context_m2.BillState.FirstOrDefault(x => x.BillNum == entity.BillNum);
                            if (result.return_code == "SUCCESS" && result.result_code == "SUCCESS" && result.trade_state == "SUCCESS")
                            {
                                LogHelper.WriteLog(typeof(FrmMain), "金额:" + result.cash_fee);
                                
                                if (result.trade_state == "SUCCESS")
                                {
                                    Console.WriteLine("SUCCESS");
                                    bill.State = 1;
                                    Console.WriteLine("保存:"+ context_m2.SaveChanges());
                                    var sbill = context_m2.StudentBill.FirstOrDefault(x => x.StudentBillUUID == bill.SBillUUID.Value);
                                    LogHelper.WriteLog(typeof(FrmMain),"钱:" + sbill.OrderMoney);
                                    LogHelper.WriteLog(typeof(FrmMain),"钱2:" + (decimal.Parse(result.cash_fee)));
                                    LogHelper.WriteLog(typeof(FrmMain),"愿订单时间:" + DateTime.Parse(sbill.OederTime));
                                    
                                    if ((string.IsNullOrEmpty(sbill.OrderNum))||((!string.IsNullOrEmpty(sbill.OrderNum))&&sbill.OrderNum!=result.out_trade_no&& DateTime.Parse(sbill.OederTime) < DateTime.ParseExact(result.time_end,"yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture)))
                                    {
                                        LogHelper.WriteLog(typeof(string), "进入条件,修改数据");
                                        sbill.OrderMoney = sbill.OrderMoney + (decimal.Parse(result.cash_fee) / 100);
                                        sbill.OrderNum = result.out_trade_no;
                                        sbill.OederTime = DateTime.ParseExact(result.time_end, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture).ToString("yyyy-MM-dd HH:mm:ss");
                                        sbill.SystemUserUUID = context_m2.SystemUser.FirstOrDefault(x => x.Wechat == result.openid).SystemUserUUID;
                                    }
                                    

                                }
                                else if (result.trade_state == "REFUND")
                                {
                                    Console.WriteLine("REFUND");

                                    bill.State = 2;
                                }
                                else if (result.trade_state == "NOTPAY")
                                {
                                    Console.WriteLine("NOTPAY");

                                    bill.State = 3;
                                }
                                else if (result.trade_state == "CLOSED")
                                {
                                    Console.WriteLine("CLOSED");

                                    bill.State = 4;
                                }
                                else if (result.trade_state == "REVOKED")
                                {
                                    Console.WriteLine("REVOKED");

                                    bill.State = 5;
                                }
                                else if (result.trade_state == "USERPAYING")
                                {
                                    Console.WriteLine("USERPAYING");

                                    bill.State = 0;
                                }
                                else if (result.trade_state == "PAYERROR")
                                {
                                    Console.WriteLine("PAYERROR");

                                    bill.State = 6;
                                }
                                var num=context_m2.SaveChanges();
                                Console.WriteLine("保存"+num);
                            }
                            //else
                            //{
                            //    bill.State = -1;
                            //    context_m.SaveChanges();
                            //}
                        }
                    }
                }
                else
                {
                    LogHelper.WriteLog(typeof(string), "非时段跳出");
                    t2.Enabled = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.WriteLog(typeof(string), "支付查询错误:" + ex.Message);
            }
            finally
            {
                t2.Enabled = true;
            }

        }


    }

    internal class Schinfo
    {
        public int ID { get; set; }
        public Guid SchoolUUID { get; set; }
        public string SchoolName { get; set; }

    }



}
