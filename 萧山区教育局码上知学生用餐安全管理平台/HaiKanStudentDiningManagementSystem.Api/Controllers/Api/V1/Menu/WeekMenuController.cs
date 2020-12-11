using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Menu
{
    [Route("api/v1/menu/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class WeekMenuController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public WeekMenuController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        /// <summary>
        /// 菜品列表
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult cuisineList(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbContext.Cuisine.Where(x => x.IsDelete != 1);
            if (guid != "null")
            {
                entity = entity.Where(x => x.SchoolUuid == Guid.Parse(guid));
            }
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult WeekMenuCreate(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = new Entities.NweekMenu();
                string schoolUuid = model.schoolUuid;
                if (schoolUuid == null)
                {
                    response.SetFailed("请登录学校账号");
                    return Ok(response);
                }
                string week = model.week;
                if (week == "上周菜单")
                {
                    response.SetFailed("上周菜单不可修改！");
                    return Ok(response);
                }
                if (week == "本周菜单")
                {
                    int now = Convert.ToInt32(DateTime.Now.DayOfWeek.ToString("d"));
                    if (now == 0)
                    {
                        now = 7;
                    }
                    DateTime date1 = DateTime.Now.AddDays(1 - now);
                    DateTime date2 = DateTime.Now.AddDays(1 - now).AddDays(6);
                    var entityschool = _dbContext.NweekMenu.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(schoolUuid)).ToList();
                    if (entityschool.Count > 0)
                    {
                        var entitytime = entityschool.Find(x => x.Datetimes.Split('-')[0] == date1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == date2.ToString("yyyy/MM/dd"));
                        if (entitytime != null)
                        {
                            response.SetFailed("菜单已存在");
                            return Ok(response);
                        }
                    }
                    entity.Datetimes = date1.ToString("yyyy/MM/dd") + "-" + date2.ToString("yyyy/MM/dd");
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                }
                else
                {
                    int now = Convert.ToInt32(DateTime.Now.DayOfWeek);
                    if (now == 0)
                    {
                        now = 7;
                    }
                    DateTime date1 = DateTime.Now.AddDays(Convert.ToInt32(1 - now) + 7);
                    DateTime date2 = DateTime.Now.AddDays(Convert.ToInt32(1 - now) + 7).AddDays(6);
                    var entityschool = _dbContext.NweekMenu.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(schoolUuid)).ToList();
                    if (entityschool.Count > 0)
                    {
                        var entitytime = entityschool.Find(x => x.Datetimes.Split('-')[0] == date1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == date2.ToString("yyyy/MM/dd"));
                        if (entitytime != null)
                        {
                            response.SetFailed("菜单已存在");
                            return Ok(response);
                        }
                    }
                    entity.Datetimes = date1.ToString("yyyy/MM/dd") + "-" + date2.ToString("yyyy/MM/dd");
                    entity.AddTime = DateTime.Now.AddDays(Convert.ToInt32(1 - Convert.ToInt32(DateTime.Now.DayOfWeek)) + 8).ToString("yyyy-MM-dd");
                }
                
                entity.NweekMenuUuid = Guid.NewGuid();
                entity.MonMon = model.monMon;
                entity.MonNoon = model.monNoon;
                entity.MonNight = model.monNight;
                entity.TuesMon = model.tuesMon;
                entity.TuesNoon = model.tuesNoon;
                entity.TuesNight = model.tuesNight;
                entity.WedMon = model.wedMon;
                entity.WedNoon = model.wedNoon;
                entity.WedNight = model.wedNight;
                entity.ThursMon = model.thursMon;
                entity.ThursNoon = model.thursNoon;
                entity.ThursNight = model.thursNight;
                entity.FriMon = model.friMon;
                entity.FriNoon = model.friNoon;
                entity.FriNight = model.friNight;
                entity.SatMon = model.satMon;
                entity.SatNoon = model.satNoon;
                entity.SatNight = model.satNight;
                entity.SunMon = model.sunMon;
                entity.SunNoon = model.sunNoon;
                entity.SunNight = model.sunNight;
                entity.SchoolUuid = model.schoolUuid;
                entity.AddPeople = model.addPeople;
                entity.IsDelete = 0;
                _dbContext.NweekMenu.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <param name="time"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Getweekmenu(string time, string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            if (guid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            DateTime time1 = Convert.ToDateTime(time.Split("至")[0]);
            DateTime time2 = Convert.ToDateTime(time.Split("至")[1]);
            var entityschool = _dbContext.NweekMenu.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).ToList();
            if (entityschool.Count > 0)
            {
                var entitytime = entityschool.Find(x => x.Datetimes.Split('-')[0] == time1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == time2.ToString("yyyy/MM/dd"));
                response.SetData(entitytime);
            }
            else
            {
                response.SetFailed("暂无数据");
                return Ok(response);
            }
            //var entity = _dbContext.NweekMenu.FirstOrDefault(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid) && Convert.ToDateTime(x.AddTime) < Convert.ToDateTime(time2) && Convert.ToDateTime(x.AddTime) > Convert.ToDateTime(time1));
            
            return Ok(response);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult WeekMenuEdit(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                string time = model.time;
                string schoolUuid = model.schoolUuid;
                string week = model.week;
                if (schoolUuid == null)
                {
                    response.SetFailed("请登录学校账号");
                    return Ok(response);
                }
                if (week == "上周菜单")
                {
                    response.SetFailed("上周菜单不可修改！");
                    return Ok(response);
                }
                DateTime time1 = Convert.ToDateTime(time.Split("至")[0]);
                DateTime time2 = Convert.ToDateTime(time.Split("至")[1]);
                var entityschool = _dbContext.NweekMenu.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(schoolUuid)).ToList();
                var entity = entityschool.Find(x => x.Datetimes.Split('-')[0] == time1.ToString("yyyy/MM/dd") && x.Datetimes.Split('-')[1] == time2.ToString("yyyy/MM/dd"));
                if (entity == null)
                {
                    var query = WeekMenuCreate(model);
                    response.SetData(query);
                    return Ok(response);
                }
                int iscunzai = model.iscunzai;
                if (iscunzai==0)
                {
                    if (model.monMon == "" && entity.MonMon!="")
                    {
                        iscunzai = 1;
                    }
                    if (model.monNoon == "" && entity.MonNoon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.monNight == "" && entity.MonNight != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.tuesMon == "" && entity.TuesMon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.tuesNoon == "" && entity.TuesNoon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.tuesNight == "" && entity.TuesNight != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.wedMon == "" && entity.WedMon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.wedNoon == "" && entity.WedNoon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.wedNight == "" && entity.WedNight != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.thursMon == "" && entity.ThursMon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.thursNoon == "" && entity.ThursNoon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.thursNight == "" && entity.ThursNight != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.friMon == "" && entity.FriMon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.friNoon == "" && entity.FriNoon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.friNight == "" && entity.FriNight != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.satMon == "" && entity.SatMon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.satNoon == "" && entity.SatNoon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.satNight == "" && entity.SatNight != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.sunMon == "" && entity.SunMon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.sunNoon == "" && entity.SunNoon != "")
                    {
                        iscunzai = 1;
                    }
                    if (model.sunNight == "" && entity.SunNight != "")
                    {
                        iscunzai = 1;
                    }
                    if (iscunzai == 1)
                    {
                        response.SetData(iscunzai);
                        return Ok(response);
                    }
                }
                entity.MonMon = model.monMon;
                entity.MonNoon = model.monNoon;
                entity.MonNight = model.monNight;
                entity.TuesMon = model.tuesMon;
                entity.TuesNoon = model.tuesNoon;
                entity.TuesNight = model.tuesNight;
                entity.WedMon = model.wedMon;
                entity.WedNoon = model.wedNoon;
                entity.WedNight = model.wedNight;
                entity.ThursMon = model.thursMon;
                entity.ThursNoon = model.thursNoon;
                entity.ThursNight = model.thursNight;
                entity.FriMon = model.friMon;
                entity.FriNoon = model.friNoon;
                entity.FriNight = model.friNight;
                entity.SatMon = model.satMon;
                entity.SatNoon = model.satNoon;
                entity.SatNight = model.satNight;
                entity.SunMon = model.sunMon;
                entity.SunNoon = model.sunNoon;
                entity.SunNight = model.sunNight;
                _dbContext.SaveChanges();
                response.SetSuccess("编辑成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 时间
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult time()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            DateTime dt = DateTime.Now;  //当前时间  
            int now = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
            if (now == 0)
            {
                now = 7;
            }
            DateTime startWeek = dt.AddDays(1 - now);  //本周周一  
            string tdate1 = startWeek.ToString("yyyy-MM-dd");
            DateTime endWeek = startWeek.AddDays(6);  //本周周日 
            string tdate2 = endWeek.ToString("yyyy-MM-dd");
            string tdate = tdate1 + "至" + tdate2;
            int lnow = Convert.ToInt32(DateTime.Now.DayOfWeek);
            if (lnow == 0)
            {
                lnow = 7;
            }
            string sdate1 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 7).ToString("yyyy-MM-dd");
            string sdate2 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 7).AddDays(6).ToString("yyyy-MM-dd");
            string sdate = sdate1 + "至" + sdate2;
            string ndate1 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) + 7).ToString("yyyy-MM-dd");
            string ndate2 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) + 7).AddDays(6).ToString("yyyy-MM-dd");
            string ndate = ndate1 + "至" + ndate2;
            response.SetData(new { tdate, sdate, ndate });
            return Ok(response);
        }


    }
}
