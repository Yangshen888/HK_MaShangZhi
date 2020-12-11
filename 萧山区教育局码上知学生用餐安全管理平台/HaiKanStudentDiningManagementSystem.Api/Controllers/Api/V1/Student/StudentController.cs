using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Student;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Student
{
    [Route("api/v1/Student/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class StudentController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IWebHostEnvironment _hostEnv;
        public StudentController(haikanSDMSContext dbContext, IMapper mapper, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostEnv = env;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(StudentRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.StudentBill.Where(x => x.IsDeleted == 0);
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw.Trim()))
                {
                    query = query.Where(x => x.StudentName.Contains(payload.Kw.Trim()));
                }
                if (!string.IsNullOrEmpty(payload.kw1.Trim()))
                {
                    query = query.Where(x => x.OrderName.Contains(payload.kw1.Trim()));
                }
                if (!string.IsNullOrEmpty(payload.kw2.Trim()))
                {
                    query = query.Where(x => x.ClassName.Contains(payload.kw2.Trim()));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(x => x.StudentNum);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                string idcard = model.idcardNum;
                string ordername = model.orderName;
                string serialNumber = model.serialNumber;
                string amountPayable = model.amountPayable;
                string orderMoney = model.orderMoney;
                var isserial = _dbContext.StudentBill.FirstOrDefault(x => x.SerialNumber == serialNumber);
                if (isserial!=null)
                {
                    response.SetFailed("该流水号已存在");
                    return Ok(response);
                }
                var iscard = _dbContext.StudentBill.FirstOrDefault(x=>x.IdcardNum==idcard && x.OrderName == ordername);
                if (iscard!=null)
                {
                    response.SetFailed("该学生账单已存在");
                    return Ok(response);
                }
                var entity = new StudentBill();
                entity.StudentBillUuid = Guid.NewGuid();
                entity.SerialNumber = serialNumber;
                entity.StudentName = model.studentName;
                entity.IdcardNum = idcard;
                //string strSex = idcard.Substring(14, 3);
                //if (int.Parse(strSex) % 2 == 0)//性别代码为偶数是女性奇数为男性
                //{
                //    entity.Sex = 2;
                //}
                //else
                //{
                //    entity.Sex = 1;
                //}
                entity.ClassName = model.className;
                entity.OrderName = ordername;
                entity.AmountPayable = model.amountPayable == null ? 0 : model.amountPayable;
                entity.OrderMoney = model.orderMoney == null ? 0 : model.orderMoney;
                entity.IsDeleted = 0;
                entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                _dbContext.StudentBill.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult show(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = _dbContext.StudentBill.FirstOrDefault(x => x.StudentBillUuid == guid);
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.SchoolUuid == null && AuthContextService.CurrentUser.SchoolGuid == null)
            {
                response.SetFailed("请登录学校账号");
                return Ok(response);
            }
            using (_dbContext)
            {
                decimal amountPayable = 0;
                string amoun = model.amountPayable;
                Guid guid = model.studentBillUuid;
                var entity = _dbContext.StudentBill.FirstOrDefault(x => x.StudentBillUuid == guid);
                entity.ClassName = model.className;
                entity.OrderName = model.orderName;
                entity.StudentName = model.studentName;
                if (amoun != null)
                {
                    amountPayable = model.amountPayable;
                }
                entity.AmountPayable = amountPayable;
                var num = _dbContext.SaveChanges();
                if (num > 0)
                {
                    response.SetSuccess("修改成功");
                }
                else
                {
                    response.SetFailed("未修改");
                }
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
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE StudentBill SET IsDeleted=@IsDelete WHERE StudentBillUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
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
                    response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
                    break;
                case "recover":
                    response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
                    break;
                default:
                    break;
            }
            return Ok(response);
        }

        /// <summary>
        /// 导入信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Import(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _hostEnv.WebRootPath + "\\UploadFiles\\ImportExcel";


                //var schoolinfo = _dbContext.SchoolInforManagement.AsQueryable();
                string uploadtitle = "学生账单记录导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                        //if (!dt.Columns.Contains("流水号"))
                        //{
                        //    response.SetFailed("无‘流水号’列");
                        //    return Ok(response);
                        //}
                        if (!dt.Columns.Contains("学生姓名"))
                        {
                            response.SetFailed("无‘学生姓名’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("身份证号"))
                        {
                            response.SetFailed("无‘身份证号’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("班级"))
                        {
                            response.SetFailed("无‘班级’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("支付项目"))
                        {
                            response.SetFailed("无‘支付项目’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("应付金额"))
                        {
                            response.SetFailed("无‘应付金额’列");
                            return Ok(response);
                        }
                        //if (!dt.Columns.Contains("已付金额"))
                        //{
                        //    response.SetFailed("无‘已付金额’列");
                        //    return Ok(response);
                        //}
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var entity = new Entities.StudentBill();
                            entity.StudentBillUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["流水号"].ToString().Trim()))
                            {
                                var SerialNumber = _dbContext.StudentBill.Where(x => x.IsDeleted == 0).FirstOrDefault(x => x.SerialNumber == dt.Rows[i]["流水号"].ToString().Trim());
                                if (SerialNumber == null)
                                {
                                    entity.SerialNumber = dt.Rows[i]["流水号"].ToString().Trim();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行流水号已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["学生姓名"].ToString().Trim()))
                            {
                                entity.StudentName = dt.Rows[i]["学生姓名"].ToString().Trim();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行学生姓名为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["身份证号"].ToString().Trim()))
                            {
                                var student = _dbContext.StudentBill.Where(x => x.IsDeleted == 0).FirstOrDefault(x => x.IdcardNum == dt.Rows[i]["身份证号"].ToString() && x.OrderName == dt.Rows[i]["支付项目"].ToString().Trim());
                                if (student == null)
                                {
                                    //Regex reg = new Regex("^(^[1-9]\\d{7}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])\\d{3}$)|(^[1-9]\\d{5}[1-9]\\d{3}((0\\d)|(1[0-2]))(([0|1|2]\\d)|3[0-1])((\\d{4})|\\d{3}[Xx])$)$");
                                    //if (reg.IsMatch(dt.Rows[i]["身份证号"].ToString()))
                                    //{
                                        entity.IdcardNum = dt.Rows[i]["身份证号"].ToString().Trim();
                                        //string strSex = dt.Rows[i]["身份证号"].ToString().Trim().Substring(14, 3);
                                        //if (int.Parse(strSex) % 2 == 0)//性别代码为偶数是女性奇数为男性
                                        //{
                                        //    entity.Sex = 2;
                                        //}
                                        //else
                                        //{
                                        //    entity.Sex = 1;
                                        //}
                                    //}
                                    //else
                                    //{
                                    //    responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行身份证号格式不正确" + "</p></br>";
                                    //    defaultcount++;
                                    //    continue;
                                    //}
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行学生账单已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行身份证号为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["班级"].ToString().Trim()))
                            {
                                entity.ClassName = dt.Rows[i]["班级"].ToString().Trim();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行班级为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["支付项目"].ToString().Trim()))
                            {
                                entity.OrderName = dt.Rows[i]["支付项目"].ToString().Trim();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行支付项目为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["应付金额"].ToString().Trim()))
                            {
                                entity.AmountPayable =Convert.ToDecimal(dt.Rows[i]["应付金额"].ToString().Trim());
                            }
                            else
                            {
                                entity.AmountPayable = 0;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["已付金额"].ToString().Trim()))
                            {
                                entity.OrderMoney = Convert.ToDecimal(dt.Rows[i]["已付金额"].ToString().Trim());
                            }
                            else
                            {
                                entity.OrderMoney = 0;
                            }
                            entity.SchoolUuid = AuthContextService.CurrentUser.SchoolGuid;
                            entity.IsDeleted = 0;
                            _dbContext.StudentBill.Add(entity);
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
        public IActionResult ExportPass(string ids)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var parameters = ids.Split(",");
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = parameters[i].ToUpper();
                }
                var query = _dbContext.StudentBill.Where(x => x.IsDeleted == 0 && parameters.Contains(x.StudentBillUuid.ToString())).Select(x => new Entities.StudentBill
                {
                    SerialNumber=x.SerialNumber,
                    StudentName = x.StudentName,
                    IdcardNum = x.IdcardNum,
                    ClassName = x.ClassName,
                    OrderName = x.OrderName,
                    AmountPayable = x.AmountPayable,
                    OrderMoney = x.OrderMoney,
                }).ToList();
                string sWebRootFolder = _hostEnv.WebRootPath + "\\UploadFiles\\ImportExcel\\";
                string uploadtitle = "学生账单记录导出" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{sWebRootFolder + uploadtitle}.xlsx";
                //CuisineViewModel model = new CuisineViewModel();
                //model.Demos = query;
                TablesToExcel(query, sFileName);
                response.SetData("\\UploadFiles\\ImportExcel\\" + uploadtitle + ".xlsx");
                return Ok(response);
            }

        }


        public static bool TablesToExcel(List<Entities.StudentBill> query, string filename)
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

            ISheet sheet = workBook.CreateSheet("学生账单表");

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

        private static void CreatSheet(ISheet sheet, List<Entities.StudentBill> query)
        {
            IRow headerRow = sheet.CreateRow(0);
            List<string> list = new List<string>() {
                "流水号","学生姓名","身份证号","班级","支付项目","应付金额","应付金额"
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
                dataRow.CreateCell(0).SetCellValue(row.SerialNumber);
                dataRow.CreateCell(1).SetCellValue(row.StudentName);
                dataRow.CreateCell(2).SetCellValue(row.IdcardNum);
                dataRow.CreateCell(3).SetCellValue(row.ClassName);
                dataRow.CreateCell(4).SetCellValue(row.OrderName);
                dataRow.CreateCell(5).SetCellValue(Convert.ToDouble(row.AmountPayable));
                dataRow.CreateCell(6).SetCellValue(Convert.ToDouble(row.OrderMoney));
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
