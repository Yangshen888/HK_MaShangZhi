using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Menu
{
    [Route("api/v1/menu/[controller]/[action]")]
    [ApiController]
    
    public class AppWeekMenuController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public AppWeekMenuController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult list(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string date = model.date;
            string time = model.time;
            string guid = model.schoolUuid;
            DateTime time1 = Convert.ToDateTime(time.Split("-")[0]);
            DateTime time2 = Convert.ToDateTime(time.Split("-")[1]);
            var entityschool = _dbContext.NweekMenu.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).ToList();
            if (entityschool.Count > 0)
            {
                var entitytime = entityschool.Find(x => x.Datetimes.Split('-')[0] == time1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == time2.ToString("yyyy/MM/dd"));
                string mon = "";
                string noon = "";
                string night = "";
                if (entitytime != null)
                {
                    if (date == "周一")
                    {
                        mon = entitytime.MonMon;
                        noon = entitytime.MonNoon;
                        night = entitytime.MonNight;
                    }
                    else if (date == "周二")
                    {
                        mon = entitytime.TuesMon;
                        noon = entitytime.TuesNoon;
                        night = entitytime.TuesNight;
                    }
                    else if (date == "周三")
                    {
                        mon = entitytime.WedMon;
                        noon = entitytime.WedNoon;
                        night = entitytime.WedNight;
                    }
                    else if (date == "周四")
                    {
                        mon = entitytime.ThursMon;
                        noon = entitytime.ThursNoon;
                        night = entitytime.ThursNight;
                    }
                    else if (date == "周五")
                    {
                        mon = entitytime.FriMon;
                        noon = entitytime.FriNoon;
                        night = entitytime.FriNight;
                    }
                    else if (date == "周六")
                    {
                        mon = entitytime.SatMon;
                        noon = entitytime.SatNoon;
                        night = entitytime.SatNight;
                    }
                    else
                    {
                        mon = entitytime.SunMon;
                        noon = entitytime.SunNoon;
                        night = entitytime.SunNight;
                    }
                    var monuuid = mon.ToUpper().Split(',').ToList();
                    var monentity = _dbContext.Cuisine.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid) && monuuid.Contains(x.CuisineUuid.ToString())).Select(x => new
                    {
                        x.CuisineName,
                        x.CuisineType,
                        x.Abstract,
                        Ingredient = GetIngredient(x.Ingredient, _dbContext),
                        Burdening = GetIngredient(x.Burdening, _dbContext),
                        x.LikeNum,
                        x.Price,
                        x.CuisineUuid,
                        x.Accessory
                    });
                    var noonuuid = noon.ToUpper().Split(',').ToList();
                    var noonentity = _dbContext.Cuisine.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid) && noonuuid.Contains(x.CuisineUuid.ToString())).Select(x => new
                    {
                        x.CuisineName,
                        x.CuisineType,
                        x.Abstract,
                        Ingredient = GetIngredient(x.Ingredient, _dbContext),
                        Burdening = GetIngredient(x.Burdening, _dbContext),
                        x.LikeNum,
                        x.Price,
                        x.CuisineUuid,
                        x.Accessory
                    });
                    var nightuuid = night.ToUpper().Split(',').ToList();
                    var nightentity = _dbContext.Cuisine.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid) && nightuuid.Contains(x.CuisineUuid.ToString())).Select(x => new
                    {
                        x.CuisineName,
                        x.CuisineType,
                        x.Abstract,
                        Ingredient = GetIngredient(x.Ingredient, _dbContext),
                        Burdening = GetIngredient(x.Burdening, _dbContext),
                        x.LikeNum,
                        x.Price,
                        x.CuisineUuid,
                        x.Accessory
                    });
                    response.SetData(new { monentity, noonentity, nightentity });
                }
                else
                {
                    response.SetSuccess("暂无数据");
                }
            }
            else
            {
                response.SetSuccess("暂无数据");
            }
            return Ok(response);
        }

        [HttpPost]
        public IActionResult MenuDateList1(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string date = model.dateChoosed;
            string mile = model.mileChoosed;
            string datetime = model.date;
            string guid = model.schoolUuid;
            var menu = "";
            var Mon = "";
            var Tues = "";
            var Wed = "";
            var Thurs = "";
            var Fri = "";
            var Sat = "";
            var Sun = "";
            var entityschool = _dbContext.NweekMenu.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).ToList();
            DateTime dt = DateTime.Now;  //当前时间  
            int now = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
            if (now == 0)
            {
                now = 7;
            }
            DateTime startWeek = dt.AddDays(1 - now);  //本周周一  
            DateTime time1 = Convert.ToDateTime(startWeek.ToString("MM/dd"));
            DateTime endWeek = startWeek.AddDays(6);  //本周周日 
            DateTime time2 = Convert.ToDateTime(endWeek.ToString("MM/dd"));
            var entitytime = entityschool.Find(x => x.Datetimes.Split('-')[0] == time1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == time2.ToString("yyyy/MM/dd"));
            if (datetime != "")
            {
                time1 = Convert.ToDateTime(datetime.Split("-")[0]);
                time2 = Convert.ToDateTime(datetime.Split("-")[1]);
                entitytime = entityschool.Find(x => x.Datetimes.Split('-')[0] == time1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == time2.ToString("yyyy/MM/dd"));
                if (entitytime != null)
                {
                    if (entitytime.MonMon != "")
                    {
                        Mon += entitytime.MonMon;
                        menu += entitytime.MonMon;
                    }
                    if (entitytime.MonNoon != "")
                    {
                        Mon += "," + entitytime.MonNoon;
                        menu += "," + entitytime.MonNoon;
                    }
                    if (entitytime.MonNight != "")
                    {
                        Mon += "," + entitytime.MonNight;
                        menu += "," + entitytime.MonNight;
                    }
                    if (entitytime.TuesMon != "")
                    {
                        Tues += entitytime.TuesMon;
                        menu += "," + entitytime.TuesMon;
                    }
                    if (entitytime.TuesNoon != "")
                    {
                        Tues += "," + entitytime.TuesNoon;
                        menu += "," + entitytime.TuesNoon;
                    }
                    if (entitytime.TuesNight != "")
                    {
                        Tues += "," + entitytime.TuesNight;
                        menu += "," + entitytime.TuesNight;
                    }
                    if (entitytime.WedMon != "")
                    {
                        Wed += entitytime.WedMon;
                        menu += "," + entitytime.WedMon;
                    }
                    if (entitytime.WedNoon != "")
                    {
                        Wed += "," + entitytime.WedNoon;
                        menu += "," + entitytime.WedNoon;
                    }
                    if (entitytime.WedNight != "")
                    {
                        Wed += "," + entitytime.WedNight;
                        menu += "," + entitytime.WedNight;
                    }
                    if (entitytime.ThursMon != "")
                    {
                        Thurs += entitytime.ThursMon;
                        menu += "," + entitytime.ThursMon;
                    }
                    if (entitytime.ThursNoon != "")
                    {
                        Thurs += "," + entitytime.ThursNoon;
                        menu += "," + entitytime.ThursNoon;
                    }
                    if (entitytime.ThursNight != "")
                    {
                        Thurs += "," + entitytime.ThursNight;
                        menu += "," + entitytime.ThursNight;
                    }
                    if (entitytime.FriMon != "")
                    {
                        Fri += entitytime.FriMon;
                        menu += "," + entitytime.FriMon;
                    }
                    if (entitytime.FriNoon != "")
                    {
                        Fri += "," + entitytime.FriNoon;
                        menu += "," + entitytime.FriNoon;
                    }
                    if (entitytime.FriNight != "")
                    {
                        Fri += "," + entitytime.FriNight;
                        menu += "," + entitytime.FriNight;
                    }
                    if (entitytime.SatMon != "")
                    {
                        Sat += entitytime.SatMon;
                        menu += "," + entitytime.SatMon;
                    }
                    if (entitytime.SatNoon != "")
                    {
                        Sat += "," + entitytime.SatNoon;
                        menu += "," + entitytime.SatNoon;
                    }
                    if (entitytime.SatNight != "")
                    {
                        Sat += "," + entitytime.SatNight;
                        menu += "," + entitytime.SatNight;
                    }
                    if (entitytime.SunMon != "")
                    {
                        Sun += entitytime.SunMon;
                        menu += "," + entitytime.SunMon;
                    }
                    if (entitytime.SunNoon != "")
                    {
                        Sun += "," + entitytime.SunNoon;
                        menu += "," + entitytime.SunNoon;
                    }
                    if (entitytime.SunNight != "")
                    {
                        Sun += "," + entitytime.SunNight;
                        menu += "," + entitytime.SunNight;
                    }
                }
                else
                {
                    response.SetFailed("暂无数据");
                    return Ok(response);
                }
            }
            else
            {
                response.SetFailed("请选择日期");
                return Ok(response);
            }
            if (date!="" && mile == "")
            {
                menu = "";
                if (date=="周一")
                {
                    menu = Mon;
                }
                if (date == "周二")
                {
                    menu = Tues;
                }
                if (date == "周三")
                {
                    menu = Wed;
                }
                if (date == "周四")
                {
                    menu = Thurs;
                }
                if (date == "周五")
                {
                    menu = Fri;
                }
            }
            else if (mile!="" && date == "")
            {
                menu = "";
                if (mile=="早餐")
                {
                    if (entitytime.MonMon != "")
                        menu = entitytime.MonMon + ",";
                    if (entitytime.TuesMon != "")
                        menu += entitytime.TuesMon + ",";
                    if (entitytime.WedMon != "")
                        menu += entitytime.WedMon + ",";
                    if (entitytime.ThursMon != "")
                        menu += entitytime.ThursMon + ",";
                    if (entitytime.FriMon != "")
                        menu += entitytime.FriMon + ",";
                }
                if (mile == "午餐")
                {
                    if (entitytime.MonNoon != "")
                        menu = entitytime.MonNoon + ",";
                    if (entitytime.TuesNoon != "")
                        menu += entitytime.TuesNoon + ",";
                    if (entitytime.WedNoon != "")
                        menu += entitytime.WedNoon + ",";
                    if (entitytime.ThursNoon != "")
                        menu += entitytime.ThursNoon + ",";
                    if (entitytime.FriNoon != "")
                        menu += entitytime.FriNoon + ",";
                }
                if (mile == "晚餐")
                {
                    if (entitytime.MonNight != "")
                        menu = entitytime.MonNight + ",";
                    if (entitytime.TuesNight != "")
                        menu += entitytime.TuesNight + ",";
                    if (entitytime.WedNight != "")
                        menu += entitytime.WedNight + ",";
                    if (entitytime.ThursNight != "")
                        menu += entitytime.ThursNight + ",";
                    if (entitytime.FriNight != "")
                        menu += entitytime.FriNight + ",";
                }
            }
            else if (mile != "" && date != "")
            {
                menu = "";
                if (date == "周一")
                {
                    if (mile == "早餐")
                    {
                        if (entitytime.MonMon != "")
                            menu = entitytime.MonMon + ",";
                    }
                    if (mile == "午餐")
                    {
                        if (entitytime.MonNoon != "")
                            menu = entitytime.MonNoon + ",";
                    }
                    if (mile == "晚餐")
                    {
                        if (entitytime.MonNight != "")
                            menu = entitytime.MonNight + ",";
                    }
                }
                if (date == "周二")
                {
                    if (mile == "早餐")
                    {
                        if (entitytime.TuesMon != "")
                            menu = entitytime.TuesMon + ",";
                    }
                    if (mile == "午餐")
                    {
                        if (entitytime.TuesNoon != "")
                            menu = entitytime.TuesNoon + ",";
                    }
                    if (mile == "晚餐")
                    {
                        if (entitytime.TuesNight != "")
                            menu = entitytime.TuesNight + ",";
                    }
                }
                if (date == "周三")
                {
                    if (mile == "早餐")
                    {
                        if (entitytime.WedMon != "")
                            menu = entitytime.WedMon + ",";
                    }
                    if (mile == "午餐")
                    {
                        if (entitytime.WedNoon != "")
                            menu = entitytime.WedNoon + ",";
                    }
                    if (mile == "晚餐")
                    {
                        if (entitytime.WedNight != "")
                            menu = entitytime.WedNight + ",";
                    }
                }
                if (date == "周四")
                {
                    if (mile == "早餐")
                    {
                        if (entitytime.ThursMon != "")
                            menu = entitytime.ThursMon + ",";
                    }
                    if (mile == "午餐")
                    {
                        if (entitytime.ThursNoon != "")
                            menu = entitytime.ThursNoon + ",";
                    }
                    if (mile == "晚餐")
                    {
                        if (entitytime.ThursNight != "")
                            menu = entitytime.ThursNight + ",";
                    }
                }
                if (date == "周五")
                {
                    if (mile == "早餐")
                    {
                        if (entitytime.FriMon != "")
                            menu = entitytime.FriMon + ",";
                    }
                    if (mile == "午餐")
                    {
                        if (entitytime.FriNoon != "")
                            menu = entitytime.FriNoon + ",";
                    }
                    if (mile == "晚餐")
                    {
                        if (entitytime.FriNight != "")
                            menu = entitytime.FriNight + ",";
                    }
                }
            }
            var query = _dbContext.Cuisine.Where(x => menu.Contains(x.CuisineUuid.ToString())).Select(x => new
            {
                x.CuisineName,
                x.CuisineType,
                x.Abstract,
                Ingredient = GetIngredient(x.Ingredient, _dbContext),
                Burdening = GetIngredient(x.Burdening, _dbContext),
                x.LikeNum,
                x.Price,
                x.CuisineUuid,
                x.Accessory
            }).ToList();
            if (query.Count() > 0)
            {
                response.SetData(query);
            }
            else
            {
                response.SetFailed("暂无菜品");
            }
            return Ok(response);
        }

        private static List<DateMenu> GetMenus(string date,string week)
        {
            using (haikanSDMSContext cc = new haikanSDMSContext())
            {
                var query = cc.Cuisine.Where(x => date.Contains(x.CuisineUuid.ToString())).Select(x => new DateMenu
                {
                    CuisineName = x.CuisineName,
                    CuisineType = x.CuisineType,
                    Abstract = x.Abstract,
                    Ingredient = GetIngredient(x.Ingredient, cc),
                    Burdening = GetIngredient(x.Burdening, cc),
                    LikeNum = x.LikeNum,
                    Price = x.Price,
                    CuisineUuid = x.CuisineUuid,
                    Accessory = x.Accessory,
                    Date= week
                }).ToList();
                return query;
            }
        }

        [HttpPost]
        public IActionResult MenuDateList(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string date = model.dateChoosed;
            string mile = model.mileChoosed;
            string datetime = model.date;
            string guid = model.schoolUuid;
            var MonMon = new List<DateMenu>();
            var MonNoon = new List<DateMenu>();
            var MonNight = new List<DateMenu>();
            var TuesMon = new List<DateMenu>();
            var TuesNoon = new List<DateMenu>();
            var TuesNight = new List<DateMenu>();
            var WedMon = new List<DateMenu>();
            var WedNoon = new List<DateMenu>();
            var WedNight = new List<DateMenu>();
            var ThursMon = new List<DateMenu>();
            var ThursNoon = new List<DateMenu>();
            var ThursNight = new List<DateMenu>();
            var FriMon = new List<DateMenu>();
            var FriNoon = new List<DateMenu>();
            var FriNight = new List<DateMenu>();
            var SatMon = new List<DateMenu>();
            var SatNoon = new List<DateMenu>();
            var SatNight = new List<DateMenu>();
            var SunMon = new List<DateMenu>();
            var SunNoon = new List<DateMenu>();
            var SunNight = new List<DateMenu>();
            var entityschool = _dbContext.NweekMenu.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).ToList();
            DateTime dt = DateTime.Now;  //当前时间  
            int now = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
            if (now == 0)
            {
                now = 7;
            }
            DateTime startWeek = dt.AddDays(1 - now);  //本周周一  
            DateTime time1 = Convert.ToDateTime(startWeek.ToString("MM/dd"));
            DateTime endWeek = startWeek.AddDays(6);  //本周周日 
            DateTime time2 = Convert.ToDateTime(endWeek.ToString("MM/dd"));
            var entitytime = entityschool.Find(x => x.Datetimes.Split('-')[0] == time1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == time2.ToString("yyyy/MM/dd"));
            if (datetime != "")
            {
                time1 = Convert.ToDateTime(datetime.Split("-")[0]);
                time2 = Convert.ToDateTime(datetime.Split("-")[1]);
                entitytime = entityschool.Find(x => x.Datetimes.Split('-')[0] == time1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == time2.ToString("yyyy/MM/dd"));
                if (entitytime != null && date == "" && mile == "")
                {
                    var menu = new List<dynamic>();
                    MonMon = GetMenus(entitytime.MonMon, time1.ToString("yyyy/MM/dd")+" 周一 早餐");
                    MonNoon = GetMenus(entitytime.MonNoon, time1.ToString("yyyy/MM/dd") + " 周一 午餐");
                    MonNight = GetMenus(entitytime.MonNight, time1.ToString("yyyy/MM/dd") + " 周一 晚餐");
                    TuesMon = GetMenus(entitytime.TuesMon, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 早餐");
                    TuesNoon = GetMenus(entitytime.TuesNoon, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 午餐");
                    TuesNight = GetMenus(entitytime.TuesNight, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 晚餐");
                    WedMon = GetMenus(entitytime.WedMon, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 早餐");
                    WedNoon = GetMenus(entitytime.WedNoon, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 午餐");
                    WedNight = GetMenus(entitytime.WedNight, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 晚餐");
                    ThursMon = GetMenus(entitytime.ThursMon, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 早餐");
                    ThursNoon = GetMenus(entitytime.ThursNoon, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 午餐");
                    ThursNight = GetMenus(entitytime.ThursNight, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 晚餐");
                    FriMon = GetMenus(entitytime.FriMon, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 早餐");
                    FriNoon = GetMenus(entitytime.FriNoon, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 午餐");
                    FriNight = GetMenus(entitytime.FriNight, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 晚餐");
                    SatMon = GetMenus(entitytime.SatMon, time1.AddDays(5).ToString("yyyy/MM/dd") + " 周六 早餐");
                    SatNoon = GetMenus(entitytime.SatNoon, time1.AddDays(5).ToString("yyyy/MM/dd") + " 周六 午餐");
                    SatNight = GetMenus(entitytime.SatNight, time1.AddDays(5).ToString("yyyy/MM/dd") + " 周六 晚餐");
                    SunMon = GetMenus(entitytime.SunMon, time1.AddDays(6).ToString("yyyy/MM/dd") + " 周日 早餐");
                    SunNoon = GetMenus(entitytime.SunNoon, time1.AddDays(6).ToString("yyyy/MM/dd") + " 周日 午餐");
                    SunNight = GetMenus(entitytime.SunNight, time1.AddDays(6).ToString("yyyy/MM/dd") + " 周日 晚餐");
                    menu.Add(MonMon);
                    menu.Add(MonNoon);
                    menu.Add(MonNight);
                    menu.Add(TuesMon);
                    menu.Add(TuesNoon);
                    menu.Add(TuesNight);
                    menu.Add(WedMon);
                    menu.Add(WedNoon);
                    menu.Add(WedNight);
                    menu.Add(ThursMon);
                    menu.Add(ThursNoon);
                    menu.Add(ThursNight);
                    menu.Add(FriMon);
                    menu.Add(FriNoon);
                    menu.Add(FriNight);
                    menu.Add(SatMon);
                    menu.Add(SatNoon);
                    menu.Add(SatNight);
                    menu.Add(SunMon);
                    menu.Add(SunNoon);
                    menu.Add(SunNight);
                    response.SetData(menu);
                }

                else if (date != "" && mile == "")
                {
                    var menu = new List<dynamic>();
                    if (date == "周一")
                    {
                        MonMon = GetMenus(entitytime.MonMon, time1.ToString("yyyy/MM/dd") + " 周一 早餐");
                        MonNoon = GetMenus(entitytime.MonNoon, time1.ToString("yyyy/MM/dd") + " 周一 午餐");
                        MonNight = GetMenus(entitytime.MonNight, time1.ToString("yyyy/MM/dd") + " 周一 晚餐");
                        menu.Add(MonMon);
                        menu.Add(MonNoon);
                        menu.Add(MonNight);
                    }
                    if (date == "周二")
                    {
                        TuesMon = GetMenus(entitytime.TuesMon, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 早餐");
                        TuesNoon = GetMenus(entitytime.TuesNoon, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 午餐");
                        TuesNight = GetMenus(entitytime.TuesNight, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 晚餐");
                        menu.Add(TuesMon);
                        menu.Add(TuesNoon);
                        menu.Add(TuesNight);
                    }
                    if (date == "周三")
                    {
                        WedMon = GetMenus(entitytime.WedMon, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 早餐");
                        WedNoon = GetMenus(entitytime.WedNoon, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 午餐");
                        WedNight = GetMenus(entitytime.WedNight, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 晚餐");
                        menu.Add(WedMon);
                        menu.Add(WedNoon);
                        menu.Add(WedNight);
                    }
                    if (date == "周四")
                    {
                        ThursMon = GetMenus(entitytime.ThursMon, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 早餐");
                        ThursNoon = GetMenus(entitytime.ThursNoon, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 午餐");
                        ThursNight = GetMenus(entitytime.ThursNight, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 晚餐");
                        menu.Add(ThursMon);
                        menu.Add(ThursNoon);
                        menu.Add(ThursNight);
                    }
                    if (date == "周五")
                    {
                        FriMon = GetMenus(entitytime.FriMon, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 早餐");
                        FriNoon = GetMenus(entitytime.FriNoon, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 午餐");
                        FriNight = GetMenus(entitytime.FriNight, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 晚餐");
                        menu.Add(FriMon);
                        menu.Add(FriNoon);
                        menu.Add(FriNight);
                    }
                    response.SetData(menu);
                }
                else if (mile != "" && date == "")
                {
                    var menu = new List<dynamic>();
                    if (mile == "早餐")
                    {
                        MonMon = GetMenus(entitytime.MonMon, time1.ToString("yyyy/MM/dd") + " 周一 早餐");
                        TuesMon = GetMenus(entitytime.TuesMon, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 早餐");
                        WedMon = GetMenus(entitytime.WedMon, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 早餐");
                        ThursMon = GetMenus(entitytime.ThursMon, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 早餐");
                        FriMon = GetMenus(entitytime.FriMon, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 早餐");
                        menu.Add(MonMon);
                        menu.Add(TuesMon);
                        menu.Add(WedMon);
                        menu.Add(ThursMon);
                        menu.Add(FriNight);
                    }
                    if (mile == "午餐")
                    {
                        MonNoon = GetMenus(entitytime.MonNoon, time1.ToString("yyyy/MM/dd") + " 周一 午餐");
                        TuesNoon = GetMenus(entitytime.TuesNoon, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 午餐");
                        WedNoon = GetMenus(entitytime.WedNoon, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 午餐");
                        ThursNoon = GetMenus(entitytime.ThursNoon, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 午餐");
                        FriNoon = GetMenus(entitytime.FriNoon, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 午餐");
                        menu.Add(MonNoon);
                        menu.Add(TuesNoon);
                        menu.Add(WedNoon);
                        menu.Add(ThursNoon);
                        menu.Add(FriNoon);
                    }
                    if (mile == "晚餐")
                    {
                        MonNight = GetMenus(entitytime.MonNight, time1.ToString("yyyy/MM/dd") + " 周一 晚餐");
                        TuesNight = GetMenus(entitytime.TuesNight, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 晚餐");
                        WedNight = GetMenus(entitytime.WedNight, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 晚餐");
                        ThursNight = GetMenus(entitytime.ThursNight, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 晚餐");
                        FriNight = GetMenus(entitytime.FriNight, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 晚餐");
                        menu.Add(MonNight);
                        menu.Add(TuesNight);
                        menu.Add(WedNight);
                        menu.Add(ThursNight);
                        menu.Add(FriNight);
                    }
                    response.SetData(menu);
                }
                else if (mile != "" && date != "")
                {
                    var menu = new List<dynamic>();
                    if (date == "周一")
                    {
                        if (mile == "早餐")
                        {
                            MonMon = GetMenus(entitytime.MonMon, time1.ToString("yyyy/MM/dd") + " 周一 早餐");
                            menu.Add(MonMon);
                        }
                        if (mile == "午餐")
                        {
                            MonNoon = GetMenus(entitytime.MonNoon, time1.ToString("yyyy/MM/dd") + " 周一 午餐");
                            menu.Add(MonNoon);
                        }
                        if (mile == "晚餐")
                        {
                            MonNight = GetMenus(entitytime.MonNight, time1.ToString("yyyy/MM/dd") + " 周一 晚餐");
                            menu.Add(MonNight);
                        }
                    }
                    if (date == "周二")
                    {
                        if (mile == "早餐")
                        {
                            TuesMon = GetMenus(entitytime.TuesMon, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 早餐");
                            menu.Add(TuesNoon);
                        }
                        if (mile == "午餐")
                        {
                            TuesNoon = GetMenus(entitytime.TuesNoon, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 午餐");
                            menu.Add(TuesNoon);
                        }
                        if (mile == "晚餐")
                        {
                            TuesNight = GetMenus(entitytime.TuesNight, time1.AddDays(1).ToString("yyyy/MM/dd") + " 周二 晚餐");
                            menu.Add(TuesNight);
                        }
                    }
                    if (date == "周三")
                    {
                        if (mile == "早餐")
                        {
                            WedMon = GetMenus(entitytime.WedMon, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 早餐");
                            menu.Add(WedMon);
                        }
                        if (mile == "午餐")
                        {
                            WedNoon = GetMenus(entitytime.WedNoon, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 午餐");
                            menu.Add(WedNoon);
                        }
                        if (mile == "晚餐")
                        {
                            WedNight = GetMenus(entitytime.WedNight, time1.AddDays(2).ToString("yyyy/MM/dd") + " 周三 晚餐");
                            menu.Add(WedNight);
                        }
                    }
                    if (date == "周四")
                    {
                        if (mile == "早餐")
                        {
                            ThursMon = GetMenus(entitytime.ThursMon, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 早餐");
                            menu.Add(ThursMon);
                        }
                        if (mile == "午餐")
                        {
                            ThursNoon = GetMenus(entitytime.ThursNoon, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 午餐");
                            menu.Add(ThursNoon);
                        }
                        if (mile == "晚餐")
                        {
                            ThursNight = GetMenus(entitytime.ThursNight, time1.AddDays(3).ToString("yyyy/MM/dd") + " 周四 晚餐");
                            menu.Add(ThursNight);
                        }
                    }
                    if (date == "周五")
                    {
                        if (mile == "早餐")
                        {
                            FriMon = GetMenus(entitytime.FriMon, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 早餐");
                            menu.Add(FriNoon);
                        }
                        if (mile == "午餐")
                        {
                            FriNoon = GetMenus(entitytime.FriNoon, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 午餐");
                            menu.Add(FriNoon);
                        }
                        if (mile == "晚餐")
                        {
                            FriNight = GetMenus(entitytime.FriNight, time1.AddDays(4).ToString("yyyy/MM/dd") + " 周五 晚餐");
                            menu.Add(FriNight);
                        }
                    }
                    response.SetData(menu);
                }
                else
                {
                    response.SetFailed("暂无数据");
                    return Ok(response);
                }
            }
            else
            {
                response.SetFailed("请选择日期");
                return Ok(response);
            }
            //var query = _dbContext.Cuisine.Where(x => menu.Contains(x.CuisineUuid.ToString())).Select(x => new
            //{
            //    x.CuisineName,
            //    x.CuisineType,
            //    x.Abstract,
            //    Ingredient = GetIngredient(x.Ingredient, _dbContext),
            //    Burdening = GetIngredient(x.Burdening, _dbContext),
            //    x.LikeNum,
            //    x.Price,
            //    x.CuisineUuid,
            //    x.Accessory
            //}).ToList();
            //if (query.Count() > 0)
            //{
            //    response.SetData(query);
            //}
            //else
            //{
            //    response.SetFailed("暂无菜品");
            //}
            return Ok(response);
        }

        private static string GetIngredient(string Ingredient, haikanSDMSContext _dbContext)
        {
            using (haikanSDMSContext cc = new haikanSDMSContext())
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
                    var list = cc.Ingredient.Where(x => uuids.Contains(x.IngredientUuid.ToString())).Select(x => x.FoodName).ToList();
                    var names = string.Join(',', list);
                    return names;
                }
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public IActionResult time(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            DateTime dt = DateTime.Now;  //当前时间  
            int now = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
            if (now == 0)
            {
                now = 7;
            }
            DateTime startWeek = dt.AddDays(1 - now);  //本周周一  
            string tdate1 = startWeek.ToString("MM/dd");
            DateTime endWeek = startWeek.AddDays(6);  //本周周日 
            string tdate2 = endWeek.ToString("MM/dd");
            string tdate = tdate1 + "-" + tdate2;
            int lnow = Convert.ToInt32(DateTime.Now.DayOfWeek);
            if (lnow == 0)
            {
                lnow = 7;
            }
            string sdate1 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 7).ToString("MM/dd");
            string sdate2 = DateTime.Now.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) - 7).AddDays(6).ToString("MM/dd");
            string sdate = sdate1 + "-" + sdate2;
            string ndate1 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) + 7).ToString("MM/dd");
            string ndate2 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) + 7).AddDays(6).ToString("MM/dd");
            string ndate = ndate1 + "-" + ndate2;

            string tdatea= startWeek.ToString("yyyy/MM/dd");
            string tdateb = endWeek.ToString("yyyy/MM/dd");
            string tdatec = tdatea + "-" + tdateb;
            string ndatea = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) + 7).ToString("yyyy/MM/dd");
            string ndateb = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) + 7).AddDays(6).ToString("yyyy/MM/dd");
            string ndatec = ndatea + "-" + ndateb;
            int bmon = 0;
            int btues = 0;
            int bwed = 0;
            int bthurs = 0;
            int bfri = 0;
            int bsat = 0;
            int bsun = 0;
            int nmon = 0;
            int ntues = 0;
            int nwed = 0;
            int nthurs = 0;
            int nfri = 0;
            int nsat = 0;
            int nsun = 0;
            var query = _dbContext.NweekMenu.FirstOrDefault(x => x.Datetimes == tdatec && x.SchoolUuid == guid);
            if (query != null)
            {
                if (query.MonMon != "" || query.MonNoon != "" || query.MonNight != "")
                {
                    bmon = 1;
                }
                if (query.TuesMon != "" || query.TuesNoon != "" || query.TuesNight != "")
                {
                    btues = 1;
                }
                if (query.WedMon != "" || query.WedNoon != "" || query.WedNight != "")
                {
                    bwed = 1;
                }
                if (query.ThursMon != "" || query.ThursNoon != "" || query.ThursNight != "")
                {
                    bthurs = 1;
                }
                if (query.FriMon != "" || query.FriNoon != "" || query.FriNight != "")
                {
                    bfri = 1;
                }
                if (query.SatMon != "" || query.SatNoon != "" || query.SatNight != "")
                {
                    bsat = 1;
                }
                if (query.SunMon != "" || query.SunNoon != "" || query.SunNight != "")
                {
                    bsun = 1;
                }
            }
            var query1 = _dbContext.NweekMenu.FirstOrDefault(x => x.Datetimes == ndatec && x.SchoolUuid == guid);
            if (query1 != null)
            {
                if (query1.MonMon != "" || query1.MonNoon != "" || query1.MonNight != "")
                {
                    nmon = 1;
                }
                if (query1.TuesMon != "" || query1.TuesNoon != "" || query1.TuesNight != "")
                {
                    ntues = 1;
                }
                if (query1.WedMon != "" || query1.WedNoon != "" || query1.WedNight != "")
                {
                    nwed = 1;
                }
                if (query1.ThursMon != "" || query1.ThursNoon != "" || query1.ThursNight != "")
                {
                    nthurs = 1;
                }
                if (query1.FriMon != "" || query1.FriNoon != "" || query1.FriNight != "")
                {
                    nfri = 1;
                }
                if (query1.SatMon != "" || query1.SatNoon != "" || query1.SatNight != "")
                {
                    nsat = 1;
                }
                if (query1.SunMon != "" || query1.SunNoon != "" || query1.SunNight != "")
                {
                    nsun = 1;
                }
            }
            response.SetData(new { tdate, sdate, ndate, bmon, btues, bwed, bthurs, bfri, bsat, bsun, nmon, ntues, nwed, nthurs, nfri, nsat, nsun });
            return Ok(response);
        }

        [HttpGet]
        public IActionResult Getweekmenu(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            int now = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));
            if (now == 0)
            {
                now = 7;
            }
            DateTime date1 = DateTime.Now.AddDays(1 - now);
            DateTime date2 = DateTime.Now.AddDays(1 - now).AddDays(6);
            var entityschool = _dbContext.NweekMenu.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).ToList();
            if (entityschool.Count > 0)
            {
                var entitytime = entityschool.FirstOrDefault(x => x.Datetimes.Split('-')[0] == date1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == date2.ToString("yyyy/MM/dd"));
                if (entitytime != null)
                {
                    var menu = "";
                    if (entitytime.MonMon != "")
                        menu += "," + entitytime.MonMon;
                    if (entitytime.MonNoon != "")
                        menu += "," + entitytime.MonNoon;
                    if (entitytime.MonNight != "")
                        menu += "," + entitytime.MonNight;
                    if (entitytime.TuesMon != "")
                        menu += "," + entitytime.TuesMon;
                    if (entitytime.TuesNoon != "")
                        menu += "," + entitytime.TuesNoon;
                    if (entitytime.TuesNight != "")
                        menu += "," + entitytime.TuesNight;
                    if (entitytime.WedMon != "")
                        menu += "," + entitytime.WedMon;
                    if (entitytime.WedNoon != "")
                        menu += "," + entitytime.WedNoon;
                    if (entitytime.WedNight != "")
                        menu += "," + entitytime.WedNight;
                    if (entitytime.ThursMon != "")
                        menu += "," + entitytime.ThursMon;
                    if (entitytime.ThursNoon != "")
                        menu += "," + entitytime.ThursNoon;
                    if (entitytime.ThursNight != "")
                        menu += "," + entitytime.ThursNight;
                    if (entitytime.FriMon != "")
                        menu += "," + entitytime.FriMon;
                    if (entitytime.FriNoon != "")
                        menu += "," + entitytime.FriNoon;
                    if (entitytime.FriNight != "")
                        menu += "," + entitytime.FriNight;
                    if (entitytime.SatMon != "")
                        menu += "," + entitytime.SatMon;
                    if (entitytime.SatNoon != "")
                        menu += "," + entitytime.SatNoon;
                    if (entitytime.SatNight != "")
                        menu += "," + entitytime.SatNight;
                    if (entitytime.SunMon != "")
                        menu += "," + entitytime.SunMon;
                    if (entitytime.SunNoon != "")
                        menu += "," + entitytime.SunNoon;
                    if (entitytime.SunNight != "")
                        menu += "," + entitytime.SunNight;
                    var query = _dbContext.Cuisine.Where(x => menu.Contains(x.CuisineUuid.ToString())).Select(x => new { x.CuisineName, x.Price }).ToList();
                    response.SetData(query);
                }
                else
                {
                    response.SetFailed("暂无数据");
                }
            }
            else
            {
                response.SetFailed("暂无数据");
            }
            return Ok(response);
        }
    }
}
