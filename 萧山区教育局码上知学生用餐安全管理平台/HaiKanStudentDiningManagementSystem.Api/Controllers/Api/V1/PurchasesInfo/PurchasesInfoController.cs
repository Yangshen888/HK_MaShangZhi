using System;
using System.Data;
using System.Linq;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.MYEntities;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.PurchasesInfo;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.PurchasesInfo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.PurchasesInfo
{
    [Route("api/v1/PurchasesInfo/[controller]/[action]")]
    [ApiController]

    public class PurchasesInfoController : ControllerBase
    {
        private readonly haikanITMContext _dbITMContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostingEnvironment;


        public PurchasesInfoController(haikanITMContext dbITMContext, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _dbITMContext = dbITMContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

    

        /// <summary>
        ///  列表显示
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetList(PurchasesInfoRequestPayload payload)
        {
           
            
            var query = from c in _dbITMContext.Purchases
                        join o in _dbITMContext.Orgs
                        on c.OrganizationId equals (int)o.OrganizationId
                       
                        select new
                        {
                            o.SchoolName,
                            c.Id,
                            c.OrganizationId,
                            c.OrganizationName,
                            c.Register,
                            c.RegisterDate,
                            c.PurchaseUser,
                            c.PurchaseDate,
                            c.Type,
                            c.Supplier,
                            c.Status,
                            c.TicketImgs,
                            c.Note,
                        };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                query = query.Where(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
            }
            query = query.OrderByDescending(x => x.Id);
            var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
            var totalCount = query.Count();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list, totalCount);
            return Ok(response);
        }


        /// <summary>
        /// 获取当前行信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetfoGet(string Id)
        {
            using (_dbITMContext)
            {
                var query = _dbITMContext.Purchases.FirstOrDefault(x=>x.Id.ToString()==Id);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 人员列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UsersList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = from i in _dbITMContext.Users
                         where i.SysUserId != null
                         select new
                         {
                             i.OrganizationId,
                             i.SysUserId,
                             UserId = i.UserId.ToString(),
                             i.Name
                         };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (ene != null)
                {
                    entity = entity.Where(x => x.OrganizationId == ene.OrganizationId);
                }
            }
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 品种列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TypesList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = _dbITMContext.Types.Where(x => x.Type == "cpzl").Select(x => new { TypeId = x.TypeId.ToString(), x.KeyValue });
            var query = entity.ToList().Distinct();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCreate(PurchasesInfoViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var entity = new HaiKanStudentDiningManagementSystem.Api.MYEntities.Purchases();
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                entity.OrganizationId = (int)ene.OrganizationId;
                entity.OrganizationName = ene.Name;
                entity.Register = model.Register;
                entity.RegisterUserId = model.RegisterUserId;
                entity.RegisterDate = DateTime.Parse(model.RegisterDate).ToString("yyyy-MM-dd");
                entity.PurchaseUser = model.PurchaseUser;
                entity.PurchaseDate = DateTime.Parse(model.PurchaseDate).ToString("yyyy-MM-dd");
                entity.Type = model.Type;
                entity.Types = model.Types;
                entity.Supplier = model.Supplier;
                entity.Status = "0";
                entity.TicketImgs = model.TicketImgs;
                entity.Note = model.Note;
                _dbITMContext.Purchases.Add(entity);
                _dbITMContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }




        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEdit(PurchasesInfoViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var entity = _dbITMContext.Purchases.FirstOrDefault(x => x.Id == model.Id);
                entity.Register = model.Register;
                entity.RegisterUserId = model.RegisterUserId;
                entity.RegisterDate = DateTime.Parse(model.RegisterDate).ToString("yyyy-MM-dd");
                entity.PurchaseUser = model.PurchaseUser;
                entity.PurchaseDate = DateTime.Parse(model.PurchaseDate).ToString("yyyy-MM-dd");
                entity.Type = model.Type;
                entity.Types = model.Types;
                entity.Supplier = model.Supplier;
                entity.TicketImgs = model.TicketImgs;
                entity.Note = model.Note;
                _dbITMContext.SaveChanges();
                response.SetSuccess("修改成功");
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
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = UpdateIsDelete(ids);
            return Ok(response);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(string ids)
        {

            using (_dbITMContext)
            {
                //MySqlContext context = new MySqlContext();
                //var response = ResponseModelFactory.CreateInstance;
                //var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                //foreach (var item in parameters)
                //{
                //    var query1 = _dbITMContext.SystemMenu.FirstOrDefault(x => x.SystemMenuUuid == Guid.Parse(item.Value.ToString()));
                //    query1.IsDeleted = 0;
                //    _dbITMContext.SaveChanges();
                //}
                //return response;
                
                var sql = string.Format("DELETE FROM purchases WHERE id IN (" +ids+")" );
                _dbITMContext.Database.ExecuteSqlRaw(sql);
                var response = ResponseModelFactory.CreateInstance;
                return response;
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
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = UpdateIsDelete(ids);
                    break;
            }
            return Ok(response);
        }

        [HttpPost]
        public IActionResult aList(PesticideRequestPayload payload)
        {
            var query = from p in _dbITMContext.PurchaseInformation
                        join o in _dbITMContext.Purchases
                        on p.PurchaseId equals o.PurchaseId
                        where o.Id.ToString() == payload.Kw
                        select new
                        {
                            p.Id,
                            p.FoodName,
                            p.UnitName,
                            CreatedAt = Convert.ToDateTime(p.CreatedAt).ToString("yyyy-MM-dd HH:mm:ss"),
                            p.ModelName,
                            p.Number,
                            p.OrganizationId
                        };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                query = query.Where(x => x.OrganizationId == ene.OrganizationId);
            }
            query = query.OrderByDescending(x => x.Id);
            var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
            var totalCount = query.Count();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list, totalCount);
            return Ok(response);
        }

        /// <summary>
        /// 单位列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PartList()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity = from i in _dbITMContext.Partners
                         where i.TypeId==35
                         select new
                         {
                             i.OrganizationId,
                             i.PartnerId,
                             i.ProvideId,
                             i.FullName
                         };
            if (!string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolName))
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                if (ene != null)
                {
                    entity = entity.Where(x => x.OrganizationId == ene.OrganizationId);
                }
            }
            var query = entity.ToList();
            response.SetData(query);
            return Ok(response);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult aCreate(PurchaseInformationViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                var enet = _dbITMContext.Purchases.FirstOrDefault(x=>x.Id== model.Pid);
                var entity = new MYEntities.PurchaseInformation();
                entity.OrganizationId = ene.OrganizationId;
                entity.Status = 0;
                entity.OwnerPurchaseId = model.Pid;
                entity.OrganizationId = ene.OrganizationId;
                entity.PurchaseId = enet.PurchaseId;
                entity.FoodName = model.FoodName;
                entity.UnitName = model.UnitName;
                entity.ModelName = model.ModelName;
                entity.Number = model.Number;
                entity.Supplier = model.Supplier;
                entity.SupplierId = model.SupplierId;
                entity.CreateUserId = model.CreateUserId;
                entity.CreatedAt = DateTime.Now;
                entity.ProductionDate = model.ProductionDate;
                entity.ExpireDate = model.ExpireDate;
                _dbITMContext.PurchaseInformation.Add(entity);
                _dbITMContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取当前行信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ashow(string Id)
        {
            using (_dbITMContext)
            {
                var query = _dbITMContext.PurchaseInformation.FirstOrDefault(x => x.Id.ToString() == Id);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult aEdit(PurchaseInformationViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            if (AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbITMContext)
            {
                var ene = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == AuthContextService.CurrentUser.SchoolName);
                var entity = _dbITMContext.PurchaseInformation.FirstOrDefault(x => x.Id == model.Id);
                entity.FoodName = model.FoodName;
                entity.UnitName = model.UnitName;
                entity.ModelName = model.ModelName;
                entity.Number = model.Number;
                entity.Supplier = model.Supplier;
                entity.SupplierId = model.SupplierId;
                entity.CreateUserId = model.CreateUserId;
                entity.CreatedAt = DateTime.Now;
                entity.ProductionDate = model.ProductionDate;
                entity.ExpireDate = model.ExpireDate;
                _dbITMContext.SaveChanges();
                response.SetSuccess("修改成功");
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
        public IActionResult aDelete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = aUpdateIsDelete(ids);
            return Ok(response);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel aUpdateIsDelete(string ids)
        {

            using (_dbITMContext)
            {
                var sql = string.Format("DELETE FROM PurchaseInformation WHERE id IN (" + ids + ")");
                _dbITMContext.Database.ExecuteSqlRaw(sql);
                var response = ResponseModelFactory.CreateInstance;
                return response;
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
        public IActionResult aBatch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = aUpdateIsDelete(ids);
                    break;
            }
            return Ok(response);
        }
        #region 导入导出

        ///// <summary>
        ///// 导入信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult GetImport(IFormFile excelfile)
        //{
        //    var response = ResponseModelFactory.CreateInstance;
        //    using (_dbITMContext)
        //    {
        //        DateTime beginTime = DateTime.Now;

        //        string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel";


        //        //var schoolinfo = _dbITMContext.SchoolInforManagement.AsQueryable();
        //        string uploadtitle = "餐饮信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
        //        string sFileName = $"{uploadtitle}.xlsx";
        //        FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
        //        //string conStr = ConnectionStrings.DefaultConnection;
        //        string responsemsgsuccess = "";
        //        string responsemsgrepeat = "";
        //        string responsemsgdefault = "";
        //        int successcount = 0;
        //        int repeatcount = 0;
        //        int defaultcount = 0;
        //        string today = DateTime.Now.ToString("yyyy-MM-dd");
        //        try
        //        {
        //            //把excelfile中的数据复制到file中
        //            using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
        //            {
        //                excelfile.CopyTo(fs);
        //                fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
        //            }
        //            DataTable dt = Haikan3.Utils.ExcelTools.ExcelToDataTable(file.ToString(), "Sheet1", true);

        //            if (dt == null || dt.Rows.Count == 0)
        //            {
        //                response.SetFailed("表格无数据");
        //                return Ok(response);
        //            }
        //            else
        //            {
        //                if (!dt.Columns.Contains("店名"))
        //                {
        //                    response.SetFailed("无‘店名’列");
        //                    return Ok(response);
        //                }
        //                for (int i = 0; i < dt.Rows.Count; i++)
        //                {

        //                    var entity = new HaiKanStudentDiningManagementSystem.Api.MYEntities.Purchases();
        //                    entity.CanyinUuid = Guid.NewGuid();
        //                    if (!string.IsNullOrEmpty(dt.Rows[i]["店名"].ToString()))
        //                    {
        //                        entity.CanyinName = dt.Rows[i]["店名"].ToString();
        //                    }
        //                    else
        //                    {
        //                        responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行店名为空" + "</p></br>";
        //                        defaultcount++;
        //                        continue;
        //                    }
        //                    if (!string.IsNullOrEmpty(dt.Rows[i]["所处位置"].ToString()))
        //                    {
        //                        entity.CanyinAddress = dt.Rows[i]["所处位置"].ToString();
        //                    }
        //                    if (!string.IsNullOrEmpty(dt.Rows[i]["店主"].ToString()))
        //                    {
        //                        entity.Fuzeren = dt.Rows[i]["店主"].ToString();
        //                    }
        //                    if (!string.IsNullOrEmpty(dt.Rows[i]["联系电话"].ToString()))
        //                    {
        //                        entity.Phone = dt.Rows[i]["联系电话"].ToString();
        //                    }
        //                    Regex reg1 = new Regex("^(正常营业|暂停营业)$");
        //                    if (reg1.IsMatch(dt.Rows[i]["状态"].ToString()))
        //                    {
        //                        entity.Staues = dt.Rows[i]["状态"].ToString();
        //                    }
        //                    else
        //                    {
        //                        responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行状态不正确" + "</p></br>";
        //                        defaultcount++;
        //                        continue;
        //                    }
        //                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
        //                    entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
        //                    //entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
        //                    //entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
        //                    entity.IsDeleted = 0;
        //                    _dbITMContext.Purchases.Add(entity);
        //                    _dbITMContext.SaveChanges();
        //                    successcount++;
        //                }
        //            }
        //            responsemsgsuccess = "<p style='color:green'>导入成功:" + successcount + "条</p></br>" + responsemsgsuccess;
        //            responsemsgrepeat = "<p style='color:orange'>重复需手动修改数据:" + repeatcount + "条</p></br>" + responsemsgrepeat;
        //            responsemsgdefault = "<p style='color:red'>导入失败:" + defaultcount + "条</p></br>" + responsemsgdefault;

        //            DateTime endTime = DateTime.Now;
        //            TimeSpan useTime = endTime - beginTime;
        //            string taketime = "导入时间" + useTime.TotalSeconds.ToString() + "秒  ";
        //            response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
        //            {
        //                time = taketime,
        //                successmsg = responsemsgsuccess
        //                ,
        //                repeatmsg = responsemsgrepeat,
        //                defaultmsg = responsemsgdefault
        //            })));
        //            return Ok(response);
        //        }
        //        catch (Exception ex)
        //        {

        //            response.SetFailed(ex.Message);
        //            return Ok(response);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 导出信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public IActionResult ExportPass(string ids)
        //{
        //    var response = ResponseModelFactory.CreateResultInstance;
        //    using (_dbITMContext)
        //    {
        //        if (ids != null)
        //        {
        //            var parameters = ids.Split(",");
        //            for (int i = 0; i < parameters.Length; i++)
        //            {
        //                parameters[i] = parameters[i].ToUpper();
        //            }
        //            var query1 = _dbITMContext.Purchases.Where(x => x.IsDeleted != 1 && parameters.Contains(x.CanyinUuid.ToString())).Select(x => new HaiKanStudentDiningManagementSystem.Api.MYEntities.Purchases
        //            {
        //                Id = x.Id,
        //                CanyinName = x.CanyinName,
        //                Phone = x.Phone,
        //                CanyinAddress = x.CanyinAddress,
        //                Fuzeren = x.Fuzeren,
        //                Staues = x.Staues,
        //            });
        //            var query = query1.OrderByDescending(x => x.Id).ToList();
        //            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel\\";
        //            string uploadtitle = "餐饮信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
        //            string sFileName = $"{sWebRootFolder + uploadtitle}.xlsx";
        //            //CuisineViewModel model = new CuisineViewModel();
        //            //model.Demos = query;
        //            TablesToExcel(query, sFileName);
        //            response.SetData("\\UploadFiles\\ImportUserInfoExcel\\" + uploadtitle + ".xlsx");
        //            return Ok(response);
        //        }
        //        else
        //        {
        //            var query1 = _dbITMContext.Purchases.Where(x => x.IsDeleted != 1).Select(x => new HaiKanStudentDiningManagementSystem.Api.MYEntities.Purchases
        //            {
        //                Id = x.Id,
        //                CanyinName = x.CanyinName,
        //                Phone = x.Phone,
        //                CanyinAddress = x.CanyinAddress,
        //                Fuzeren = x.Fuzeren,
        //                Staues = x.Staues,
        //            });
        //            var query = query1.OrderByDescending(x => x.Id).ToList();
        //            string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel\\";
        //            string uploadtitle = "餐饮信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
        //            string sFileName = $"{sWebRootFolder + uploadtitle}.xlsx";
        //            //CuisineViewModel model = new CuisineViewModel();
        //            //model.Demos = query;
        //            TablesToExcel(query, sFileName);
        //            response.SetData("\\UploadFiles\\ImportUserInfoExcel\\" + uploadtitle + ".xlsx");
        //            return Ok(response);
        //        }


        //    }

        //}





        //public static bool TablesToExcel(List<HaiKanStudentDiningManagementSystem.Api.MYEntities.Purchases> query, string filename)
        //{
        //    MemoryStream ms = new MemoryStream();

        //    IWorkbook workBook;
        //    //IWorkbook workBook=WorkbookFactory.Create(filename);
        //    string suffix = filename.Substring(filename.LastIndexOf(".") + 1, filename.Length - filename.LastIndexOf(".") - 1);
        //    if (suffix == "xls")
        //    {
        //        workBook = new HSSFWorkbook();
        //    }
        //    else
        //        workBook = new XSSFWorkbook();

        //    ISheet sheet = workBook.CreateSheet(" 表");

        //    CreatSheet(sheet, query);


        //    workBook.Write(ms);
        //    try
        //    {
        //        SaveToFile(ms, filename);
        //        ms.Flush();
        //        return true;
        //    }
        //    catch
        //    {
        //        ms.Flush();
        //        throw;
        //    }

        //}

        //private static void CreatSheet(ISheet sheet, List<HaiKanStudentDiningManagementSystem.Api.MYEntities.Purchases> query)
        //{
        //    IRow headerRow = sheet.CreateRow(0);
        //    List<string> list = new List<string>() {
        //        "店名","所处位置","店主","联系电话","状态"
        //    };

        //    //表头
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        headerRow.CreateCell(i).SetCellValue(list[i]);
        //    }

        //    int rowIndex = 1;
        //    foreach (var row in query)
        //    {
        //        IRow dataRow = sheet.CreateRow(rowIndex);
        //        dataRow.CreateCell(0).SetCellValue(row.CanyinName);
        //        dataRow.CreateCell(1).SetCellValue(row.CanyinAddress);
        //        dataRow.CreateCell(2).SetCellValue(row.Fuzeren);
        //        dataRow.CreateCell(3).SetCellValue(row.Phone);
        //        dataRow.CreateCell(4).SetCellValue(row.Staues);

        //        rowIndex++;
        //    }
        //}
        //private static void SaveToFile(MemoryStream ms, string fileName)
        //{
        //    using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        //    {
        //        byte[] data = ms.ToArray();         //转为字节数组 
        //        fs.Write(data, 0, data.Length);     //保存为Excel文件
        //        fs.Flush();
        //        data = null;
        //    }
        //}
        #endregion
    }
}
