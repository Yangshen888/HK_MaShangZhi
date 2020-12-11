using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.canteenManagement;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.canteenManageent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.canteenManagement
{
    [Route("api/v1/canteenManagement/[controller]/[action]")]
    [ApiController]
    public class manageMessageController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        private IHostingEnvironment _hostingEnvironment;

        public manageMessageController(haikanSDMSContext dbContext, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
         /// <summary>
        /// 菜品库列表显示
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MessageList(manageMessageRequestpayload payload)
        
        {
            var query = from c in _dbContext.Train
                        where c.IsDelete == "0"
                        select new
                        {
                            c.TrainUuid,
                            c.Theme,
                            //c.SchoolUuid,
                            c.TrainTime,
                            c.TrainName,
                            c.IsDelete,
                            c.Speaker,
                            c.Content,
                            c.SchoolUuid,
                            c.AddTime,
                            c.AddPeople,
                            c.Id,
                            c.Anum,
                            c.Mnum,
                            c.TrainLession,
                            c.Address
                        };

            //if (!string.IsNullOrEmpty(payload.kw))
            //{
            //    query = query.Where(x => x.SchoolUuid == Guid.Parse(payload.kw));
            //}
            if (!string.IsNullOrEmpty(payload.kw1))
            {
                query = query.Where(x => x.Theme.Contains(payload.kw1));
            }
            //判断444
            if (AuthContextService.CurrentUser.SchoolGuid != null)
            {
                query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
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

        /// <summary>
        /// 获取菜品信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult MessageGet(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.Train.FirstOrDefault(x => x.TrainUuid == guid);
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
                var entity = _dbContext.Train.FirstOrDefault(x => x.TrainUuid.ToString() == ids);
                entity.IsDelete = "1";//删除状态
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
                    //    var query1 = _dbContext.Train.FirstOrDefault(x => x.TrainUuid == Guid.Parse(item.Value.ToString()));
                    //    query1.IsDelete = "1";
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
                //var idList = ids.Split(",").ToList();
                ////idList.ForEach(x => {
                ////  _dbContext.Database.ExecuteSqlCommand($"UPDATE DncUser SET IsDeleted=1 WHERE Id = {x}");
                ////});
                //_dbContext.Database.ExecuteSqlCommand($"UPDATE DncUser SET IsDeleted={(int)isDeleted} WHERE Id IN ({ids})");
                //var id = ids.Split(",");
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE Train SET IsDelete=@IsDelete WHERE TrainUUID IN ({0})", parameterNames);
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                _dbContext.Database.ExecuteSqlRaw(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
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
            using (_dbContext)
            {
                var entity = new HaiKanStudentDiningManagementSystem.Api.Entities.Train();
                entity.TrainUuid = Guid.NewGuid();
                entity.Content = model.content;
                entity.Theme = model.theme;
                entity.Address = model.address;
                entity.TrainLession = model.trainLession;
                entity.Anum = model.anum;
                entity.Mnum = model.mnum;
                entity.SchoolUuid = model.schoolUuid;
                entity.Speaker = model.speaker;
                entity.IsDelete = "0";
                entity.TrainName = model.trainName;
                entity.TrainTime = model.trainTime.ToString();
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _dbContext.Train.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }
        /// <summary>
        /// 编辑（保存）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(dynamic model)
        {
            var response = ResponseModelFactory.CreateInstance;

            string guid = model.trainUuid;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Train.FirstOrDefault(x => x.TrainUuid == Guid.Parse(guid));
                entity.Content = model.content;
                entity.Theme = model.theme;
                entity.Speaker = model.speaker;
                entity.TrainName = model.trainName;
                entity.TrainTime = model.trainTime.ToString();
                entity.Address = model.address;
                entity.TrainLession = model.trainLession;
                entity.Mnum = model.mnum;
                entity.Anum = model.anum;
                _dbContext.SaveChanges();
                response.SetSuccess("修改成功");
                return Ok(response);
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
                string uploadtitle = "培训信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
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
                        if (!dt.Columns.Contains("培训主题"))
                        {
                            response.SetFailed("无‘培训主题’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("培训时间"))
                        {
                            response.SetFailed("无‘培训时间’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("培训对象"))
                        {
                            response.SetFailed("无‘培训对象’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("培训地址"))
                        {
                            response.SetFailed("无‘培训地址’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("培训课时"))
                        {
                            response.SetFailed("无‘培训课时’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("上午培训人数"))
                        {
                            response.SetFailed("无‘上午培训人数’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("下午培训人数"))
                        {
                            response.SetFailed("无‘下午培训人数’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("授课人"))
                        {
                            response.SetFailed("无‘授课人’列");
                            return Ok(response);
                        }


                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new Entities.Train();
                            entity.TrainUuid = Guid.NewGuid();
                            if (!string.IsNullOrEmpty(dt.Rows[i]["培训主题"].ToString()))
                            {
                                var a = dt.Rows[i]["培训主题"].ToString();
                                var user = _dbContext.Train.Where(x => x.IsDelete == "0").FirstOrDefault(x => x.Theme == dt.Rows[i]["培训主题"].ToString());

                                if (user == null)
                                {
                                    entity.Theme = dt.Rows[i]["培训主题"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行培训主题已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行培训主题为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            DateTime date;
                            if ((!string.IsNullOrEmpty(dt.Rows[i]["培训时间"].ToString()))&&DateTime.TryParse(dt.Rows[i]["培训时间"].ToString().Trim(),out date))
                            {
                                entity.TrainTime = date.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行培训时间格式有误" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["培训对象"].ToString()))
                            {
                                entity.TrainName = dt.Rows[i]["培训对象"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行培训对象为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["培训地址"].ToString()))
                            {
                                entity.Address = dt.Rows[i]["培训地址"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行培训地址为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["培训课时"].ToString()))
                            {
                                entity.TrainLession = dt.Rows[i]["培训课时"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行培训课时为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["上午培训人数"].ToString()))
                            {
                                entity.Mnum = dt.Rows[i]["上午培训人数"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行上午培训人数为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["下午培训人数"].ToString()))
                            {
                                entity.Anum = dt.Rows[i]["下午培训人数"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行下午培训人数为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["授课人"].ToString()))
                            {
                                entity.Speaker = dt.Rows[i]["授课人"].ToString();
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行授课人为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                            entity.IsDelete = "0";
                            _dbContext.Train.Add(entity);
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
                var query = _dbContext.Train.Where(x => x.IsDelete != "1" && parameters.Contains(x.TrainUuid.ToString())).Select(x => new Entities.Train
                {
                    Theme = x.Theme,
                    TrainTime = x.TrainTime,
                    TrainName = x.TrainName,
                    Address=x.Address,
                    TrainLession=x.TrainLession,
                    Mnum=x.Mnum,
                    Anum=x.Anum,
                    Speaker = x.Speaker,

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
        public static bool TablesToExcel(List<Entities.Train> query, string filename)
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

            ISheet sheet = workBook.CreateSheet("培训信息表");

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

        private static void CreatSheet(ISheet sheet, List<Entities.Train> query)
        {
            IRow headerRow = sheet.CreateRow(0);
            List<string> list = new List<string>() {
                "培训主题","培训时间","培训对象","培训地址","培训课时","上午培训人数","下午培训人数","授课人"
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
                dataRow.CreateCell(0).SetCellValue(row.Theme);
                dataRow.CreateCell(1).SetCellValue(row.TrainTime);
                dataRow.CreateCell(2).SetCellValue(row.TrainName);
                dataRow.CreateCell(3).SetCellValue(row.Address);
                dataRow.CreateCell(4).SetCellValue(row.TrainLession);
                dataRow.CreateCell(5).SetCellValue(row.Mnum);
                dataRow.CreateCell(6).SetCellValue(row.Anum);
                dataRow.CreateCell(7).SetCellValue(row.Speaker);
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
