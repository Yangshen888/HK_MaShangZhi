using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.canteenManagement;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Quality;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.canteenManageent;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Quality;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Quality
{
    [Route("api/v1/canteenManagement/[controller]/[action]")]
    [ApiController]

    public class moneyController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IHostingEnvironment _hostingEnvironment;
        public moneyController(haikanSDMSContext dbContext, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// 校园新闻列表展示
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MoneyList(moneyRequestpayload payload)
        {
            using (_dbContext)
            {
                var guid = payload.schoolguid;
                var query = from p in _dbContext.ElectronicBill
                            where p.IsDelete == 0
                            select new
                            {
                                p.ElectronicUuid,
                                p.Supplier,
                                p.Phone,
                                p.IsDelete,
                                p.PurchaseTime,
                                p.CuisineName,
                                p.Specification,
                                p.Quantity,
                                p.ProducedTime,
                                p.ExpirationTime,
                                p.Rt,
                                p.AddTime,
                                p.SchoolUuid,
                                p.Id
                            };
                if (!string.IsNullOrEmpty(payload.kw))
                {
                    query = query.Where(x => x.CuisineName.Contains(payload.kw));
                }
                //判断444
                if (!string.IsNullOrEmpty(payload.schoolguid))
                {
                    query = query.Where(x => x.SchoolUuid == Guid.Parse(payload.schoolguid));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderByDescending(x => x.Id);
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
        public IActionResult Create(moneyViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            //if (string.IsNullOrEmpty(AuthContextService.CurrentUser.SchoolUuid))
            //{
            //    response.SetFailed("");
            //    return Ok(response);
            //}
            using (_dbContext)
            {

                var entity = new HaiKanStudentDiningManagementSystem.Api.Entities.ElectronicBill();
                entity.ElectronicUuid = Guid.NewGuid();
                entity.IsDelete = 0;
                entity.PurchaseTime = model.PurchaseTime;
                entity.Phone = model.Phone;
                entity.Supplier = model.Supplier;
                entity.CuisineName = model.CuisineName;
                entity.ProducedTime = model.ProducedTime;
                entity.Specification = model.Specification;
                entity.Quantity = model.Quantity;
                entity.SchoolUuid = model.SchoolUuid;
                entity.ExpirationTime = model.ExpirationTime;
                entity.Rt = model.RT;
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dbContext.ElectronicBill.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的信息
        /// </summary>
        /// <param name="model">视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult MoneyEdit(moneyViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _dbContext.ElectronicBill.FirstOrDefault(x => x.ElectronicUuid == model.ElectronicUUID);
                if (entity == null)
                {
                    response.SetFailed("不存在");
                    return Ok(response);
                }

                entity.PurchaseTime = model.PurchaseTime;
                entity.Phone = model.Phone;
                entity.Supplier = model.Supplier;
                entity.CuisineName = model.CuisineName;
                entity.ProducedTime = model.ProducedTime;
                entity.Specification = model.Specification;
                entity.Quantity = model.Quantity;
                entity.ExpirationTime = model.ExpirationTime;
                entity.Rt = model.RT;

                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }


        //获取信息
        [HttpGet]
        public IActionResult MoneyGet(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.ElectronicBill.Where(x => x.ElectronicUuid == guid && x.IsDelete != 1).ToList();
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
                var entity = _dbContext.ElectronicBill.FirstOrDefault(x => x.ElectronicUuid.ToString() == ids);
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
                    //    var query1 = _dbContext.ElectronicBill.FirstOrDefault(x => x.ElectronicUuid == Guid.Parse(item.Value.ToString()));
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
                var sql = string.Format("UPDATE ElectronicBill SET IsDelete=@IsDelete WHERE ElectronicUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }
        /// <summary>
        /// 导入菜品信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Import(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\Import";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "电子账台信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{uploadtitle}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                //string conStr = ConnectionStrings.DefaultConnection;
                string responsemsgsuccess = "";
                string responsemsgrepeat = "";
                string responsemsgdefault = "";
                int successcount = 0;
                int repeatcount = 0;
                int defaultcount = 0;
                string today = DateTime.Now.ToString("yyyy-MM-dd");
                try
                {
                    //把excelfile中的数据复制到file中
                    using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
                    {
                        excelfile.CopyTo(fs);
                        fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
                    }
                    DataTable dt = Haikan3.Utils.ExcelTools.ExcelToDataTable(file.ToString(), "Sheet1", true);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        response.SetFailed("表格无数据");
                        return Ok(response);
                    }
                    else
                    {
                        if (!dt.Columns.Contains("进货时间"))
                        {
                            response.SetFailed("无‘进货时间’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("供货商联系方式"))
                        {
                            response.SetFailed("无‘供货商联系方式’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("供货商"))
                        {
                            response.SetFailed("无‘供货商’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("食品名称"))
                        {
                            response.SetFailed("无‘食品名称’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("规格"))
                        {
                            response.SetFailed("无‘规格’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("数量"))
                        {
                            response.SetFailed("无‘数量’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("生产日期"))
                        {
                            response.SetFailed("无‘生产日期’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("保质期"))
                        {
                            response.SetFailed("无‘保质期’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("保存方式"))
                        {
                            response.SetFailed("无‘保存方式’列");
                            return Ok(response);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new Entities.ElectronicBill();
                            entity.ElectronicUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["进货时间"].ToString()))
                            {
                                var a = dt.Rows[i]["进货时间"].ToString();
                                var user = _dbContext.Train.Where(x => x.IsDelete == "0").FirstOrDefault(x => x.Theme == dt.Rows[i]["进货时间"].ToString());
                                DateTime date;
                                if (user == null&&DateTime.TryParse(dt.Rows[i]["进货时间"].ToString(),out date))
                                {
                                    entity.PurchaseTime = date.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行进货时间格式错误" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行进货时间为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["供货商联系方式"].ToString()))
                            {
                                entity.Phone = dt.Rows[i]["供货商联系方式"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行供货商联系方式为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["供货商"].ToString()))
                            {
                                entity.Supplier = dt.Rows[i]["供货商"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行供货商为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["食品名称"].ToString()))
                            {
                                entity.CuisineName = dt.Rows[i]["食品名称"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "食品名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["规格"].ToString()))
                            {
                                entity.Specification = dt.Rows[i]["规格"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "规格名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["数量"].ToString()))
                            {
                                entity.Quantity = dt.Rows[i]["数量"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "数量名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["生产日期"].ToString()))
                            {
                                entity.ProducedTime = dt.Rows[i]["生产日期"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "生产日期名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["保质期"].ToString()))
                            {
                                entity.ExpirationTime = dt.Rows[i]["保质期"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "保质期名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["保存方式"].ToString()))
                            {
                                entity.Rt = dt.Rows[i]["保存方式"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "保存方式名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                            entity.IsDelete = 0;
                            entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                            //var query = _dbContext.ElectronicBill.Where(x => x.PurchaseTime == entity.PurchaseTime && x.Phone == entity.Phone && x.Supplier == entity.Supplier && x.CuisineName == entity.CuisineName && x.Specification == entity.Specification && x.Quantity == entity.Quantity&& x.ProducedTime == entity.ProducedTime);
//&& x.ExpirationTime == entity.ExpirationTime && x.Rt == entity.Rt);
                            if (_dbContext.ElectronicBill.Any(x => x.PurchaseTime == entity.PurchaseTime && x.Phone == entity.Phone && x.Supplier == entity.Supplier && x.CuisineName == entity.CuisineName && x.Specification == entity.Specification && x.Quantity == entity.Quantity && x.ProducedTime == entity.ProducedTime && x.ExpirationTime == entity.ExpirationTime && x.Rt == entity.Rt&&x.SchoolUuid==entity.SchoolUuid))
                            {
                                responsemsgdefault += "<p style='color:orange'>" + "第" + (i + 2) + "条数据已存在" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            _dbContext.ElectronicBill.Add(entity);
                            _dbContext.SaveChanges();
                            successcount++;
                        }
                    }
                    responsemsgsuccess = "<p style='color:green'>导入成功:" + successcount + "条</p></br>" + responsemsgsuccess;
                    responsemsgrepeat = "<p style='color:orange'>重复需手动修改数据:" + repeatcount + "条</p></br>" + responsemsgrepeat;
                    responsemsgdefault = "<p style='color:red'>导入失败:" + defaultcount + "条</p></br>" + responsemsgdefault;


                    DateTime endTime = DateTime.Now;
                    TimeSpan useTime = endTime - beginTime;
                    string taketime = "导入时间" + useTime.TotalSeconds.ToString() + "秒  ";
                    response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
                    {
                        time = taketime,
                        successmsg = responsemsgsuccess
                        ,
                        repeatmsg = responsemsgrepeat,
                        defaultmsg = responsemsgdefault
                    })));
                    return Ok(response);
                }
                catch (Exception ex)
                {

                    response.SetFailed(ex.Message);
                    return Ok(response);
                }
            }
        }
        /// <summary>
        /// 导出菜品信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Export(string ids)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var parameters = ids.Split(",");
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = parameters[i].ToUpper();
                }
                var query = _dbContext.ElectronicBill.Where(x => x.IsDelete != 1 && parameters.Contains(x.ElectronicUuid.ToString())).Select(x => new Entities.ElectronicBill
                {
                    PurchaseTime = x.PurchaseTime,
                    Phone = x.Phone,
                    Supplier = x.Supplier,
                    CuisineName = x.CuisineName,
                    Specification = x.Specification,
                    Quantity = x.Quantity,
                    ProducedTime = x.ProducedTime,
                    ExpirationTime = x.ExpirationTime,
                    Rt = x.Rt,

                }).ToList();
                string sWebRootFolder = _hostingEnvironment.WebRootPath + "\\UploadFiles\\Import\\";
                string uploadtitle = "培训信息信息导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{sWebRootFolder + uploadtitle}.xlsx";
                //CuisineViewModel model = new CuisineViewModel();
                //model.Demos = query;
                TablesToExcel(query, sFileName);
                response.SetData("\\UploadFiles\\Import\\" + uploadtitle + ".xlsx");
                return Ok(response);
            }

        }
        public static bool TablesToExcel(List<Entities.ElectronicBill> query, string filename)
        {
            MemoryStream ms = new MemoryStream();

            IWorkbook workBook;
            //IWorkbook workBook=WorkbookFactory.Create(filename);
            string suffix = filename.Substring(filename.LastIndexOf(".") + 1, filename.Length - filename.LastIndexOf(".") - 1);
            if (suffix == "xls")
            {
                workBook = new HSSFWorkbook();
            }
            else
                workBook = new XSSFWorkbook();

            ISheet sheet = workBook.CreateSheet("电子账单表");

            CreatSheet(sheet, query);


            workBook.Write(ms);
            try
            {
                SaveToFile(ms, filename);
                ms.Flush();
                return true;
            }
            catch
            {
                ms.Flush();
                throw;
            }

        }

        private static void CreatSheet(ISheet sheet, List<Entities.ElectronicBill> query)
        {
            IRow headerRow = sheet.CreateRow(0);
            List<string> list = new List<string>() {
                "进货时间","供货商联系方式","供货商","食品名称","规格","数量","生产日期","保质期","保存方式"
            };

            //表头
            for (int i = 0; i < list.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(list[i]);
            }
            //foreach (var column in list)
            //    headerRow.CreateCell(column.Ordinal).SetCellValue(column);//If Caption not set, returns the ColumnName value
            int rowIndex = 1;
            foreach (var row in query)
            {
                IRow dataRow = sheet.CreateRow(rowIndex);
                dataRow.CreateCell(0).SetCellValue(row.PurchaseTime);
                dataRow.CreateCell(1).SetCellValue(row.Phone);
                dataRow.CreateCell(2).SetCellValue(row.Supplier);
                dataRow.CreateCell(3).SetCellValue(row.CuisineName);
                dataRow.CreateCell(4).SetCellValue(row.Specification);
                dataRow.CreateCell(5).SetCellValue(row.Quantity);
                dataRow.CreateCell(6).SetCellValue(row.ProducedTime);
                dataRow.CreateCell(7).SetCellValue(row.ExpirationTime);
                dataRow.CreateCell(8).SetCellValue(row.Rt);
                //foreach (DataColumn column in table.Columns)
                //{
                //    dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                //}
                rowIndex++;
            }
        }
        private static void SaveToFile(MemoryStream ms, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();         //转为字节数组 
                fs.Write(data, 0, data.Length);     //保存为Excel文件
                fs.Flush();
                data = null;
            }
        }
    }
}
