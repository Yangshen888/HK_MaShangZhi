using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.MYEntities;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Ledger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Ledger
{
    [Route("api/v1/Ledger/[controller]/[action]")]
    [ApiController]
    public class AllLedgerController : ControllerBase
    {
        private readonly haikanITMContext _dbITMContext;

        public AllLedgerController(haikanITMContext dbITMContext)
        {
            _dbITMContext = dbITMContext;
        }


        /// <summary>
        /// 电子台账列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LedgerList(LedgerQuery lquery)
        {
            //name = "湘湖小学";
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                var school = _dbITMContext.Orgs.FirstOrDefault(x => x.SchoolName == lquery.Kw);
                if (school != null)
                {
                    var query = _dbITMContext.Logs.Where(x => x.OrganizationId == school.OrganizationId).OrderByDescending(x => x.CreatedAt).Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.ObjectId,
                        x.ObjectType,
                        CreatedAt = x.CreatedAt.Value.ToString("yyyy-MM-dd"),
                    });

                    var list = query.Paged(lquery.CurrentPage, lquery.PageSize).ToList(); ;
                    response.SetData(list);
                    return Ok(response);
                }


                return Ok(response);
            }
        }

        /// <summary>
        /// 电子台账详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LedgerInfo(ulong id)
        {

            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                var log = _dbITMContext.Logs.FirstOrDefault(x => x.Id == id);
                if (log.ObjectType == "disinfection")
                {
                    var entity = _dbITMContext.Disinfections.Select(x => new
                    {
                        x.Id,
                        Createddate = x.CreatedDate.Value.ToString("yyyy-MM-dd"),
                        x.TypeName,
                        x.ImgUrls,
                        x.DisinfectionUserName,
                        x.DisinfectionId
                    }).FirstOrDefault(x => x.DisinfectionId == log.ObjectId);
                    var list = _dbITMContext.DisinfectionRecords.Where(x => (ulong)x.DisinfectionId == entity.DisinfectionId).ToList();
                    response.SetData(new { entity, list });
                }
                else if (log.ObjectType == "inspection")
                {
                    var entity = _dbITMContext.Inspections.OrderByDescending(x => x.CreatedAt).Select(x => new
                    {
                        x.CreatedAt,
                        Create = x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.Inspector,
                        x.ShouldCount,
                        x.ActualCount,
                        x.Id,
                        x.InspectionId
                    }).FirstOrDefault(x => x.InspectionId == log.ObjectId);
                    var list = _dbITMContext.InspectionInformation.Where(x => x.InspectionId == entity.InspectionId && x.CheckStatus == "正常").ToList();
                    response.SetData(new { entity, list.Count });

                }
                else if (log.ObjectType == "pesticide")
                {
                    var entity = _dbITMContext.Pesticides.Select(x => new
                    {
                        x.Id,
                        Checked = x.CheckedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.CreateUserId,
                        Created = x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.HandleMeasure,
                        x.HandleMeasures,
                        x.InspectionOrder,
                        x.InspectionOrders,
                        x.InspectionResult,
                        x.InspectionResults,
                        x.Inspector,
                        x.Note,
                        x.OrganizationId,
                        x.Status,
                        x.TestPaper,
                        Updated = x.UpdatedAt == null ? "" : x.UpdatedAt.Value.ToString(""),
                        x.UserName,
                        x.UserOrganizationId,
                        x.Vegetables,
                        x.VegetablesName,
                        x.PesticideId

                    }).FirstOrDefault(x => x.PesticideId == log.ObjectId);
                    response.SetData(entity);

                }
                else if (log.ObjectType == "purchase")
                {
                    var entity = _dbITMContext.Purchases.FirstOrDefault(x => x.PurchaseId == log.ObjectId);
                    response.SetData(entity);
                }
                else if (log.ObjectType == "sample")
                {
                    var entity = _dbITMContext.Samples.Select(x => new
                    {
                        x.Id,
                        x.AuditorId,
                        x.AuditorName,
                        CreatedDate = x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.Del,
                        EliminateDate = x.EliminatedAt == null ? "" : x.EliminatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.EliminateId,
                        x.EliminateName,
                        x.FoodIds,
                        x.FoodName,
                        x.Hours,
                        x.Img,
                        MaturedDate = x.MaturedAt == null ? "" : x.MaturedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.MealTime,
                        x.MealTimeName,
                        x.Note,
                        x.OrganizationId,
                        //SampledDate=(x.MaturedAt.Value.AddDays(x.SampledAt.Value.Day).AddMonths(x.SampledAt.Value.Month).AddSeconds(x.SampledAt.Value.Second)).ToString("yyyy-MM-dd HH:mm:ss"),
                        SampledDate = x.SampledAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.SampleName,
                        x.UpdatedAt,
                        x.Weight,
                        x.SampleId

                    }).FirstOrDefault(x => x.SampleId == log.ObjectId);
                    response.SetData(entity);

                }else if (log.ObjectType=="waste")
                {

                    var entity = _dbITMContext.Wastes.Select(x => new
                    {
                        x.Id,
                        x.WasteId,
                        HandleDate =x.HandleDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.NewHandlerName,
                        x.FullName,
                        x.Receiver,
                        x.ReceiverIdentityCard,
                        x.SwillNumber,
                        x.WasteoilNumber,
                        x.OtherWasteNumber,
                        x.Note,
                        x.Imgs
                    }).FirstOrDefault(x => x.WasteId == log.ObjectId);
                    response.SetData(entity);
                }
                else if (log.ObjectType == "synthesize")
                {
                    var entity = _dbITMContext.Synthesizes.Select(x => new
                    {
                        x.Id,
                        x.SynthesizeId,
                        CreatedAt = x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                        x.SubjectName,
                        x.Reperson,
                        x.ContinueTime,
                        x.DepartmentName,
                        x.Number,
                        x.Introduce,
                        x.Result,
                        x.Imgs
                    }).FirstOrDefault(x => x.SynthesizeId == log.ObjectId);
                    response.SetData(entity);
                }
                return Ok(response);
            }
        }

        [HttpGet]
        public IActionResult GetinspecInfo(string id)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                var query = _dbITMContext.Inspections.Select(x=>new Inspectioninfo
                {
                    Id = x.Id,
                    InspectionId = x.InspectionId,
                    ShouldCount = x.ShouldCount,
                    ActualCount = x.ActualCount,
                    Create = x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss")
                }).FirstOrDefault(x => x.InspectionId.ToString() == id);
                var list = _dbITMContext.InspectionInformation.Where(x => x.InspectionId == query.InspectionId).ToList();
                var count = list.Where(x=>x.CheckStatus=="正常").ToList().Count();
                response.SetData(new { query,list,count});
                return Ok(response);
            }
        }


        [HttpPost]
        public IActionResult InspectionList(InspectionPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbITMContext)
            {
                //var log = _dbITMContext.Logs.FirstOrDefault(x => x.Id == payload.ID);
                //var query = from i in _dbITMContext.Inspections
                //            join ii in _dbITMContext.InspectionInformation 
                //            on i.Id equals ii.InspectionId 
                //            orderby i.CreatedAt descending
                //            select new {
                //                Id= i.Id,
                //                ShouldCount=i.ShouldCount,
                //                ActualCount=i.ActualCount,
                //                Create=i.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                //                UserName=ii.UserName,
                //                CheckStatus=ii.CheckStatus,
                //            };
                ////var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                //var query2 = from i in query
                //             group i by i.Id into g
                //             select new
                //             {
                //                 g.Key,
                //                 Normal= g.ToList()
                //                 .Count(x=>x.CheckStatus=="正常"),
                //             };
                //var list2 = query2.ToList();
                var query3 = _dbITMContext.Inspections.OrderByDescending(x => x.CreatedAt).Select(x => new Inspectioninfo
                {
                    Id = x.Id,
                    InspectionId=x.InspectionId,
                    ShouldCount = x.ShouldCount,
                    ActualCount = x.ActualCount,
                    Create = x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                    //Normal=_dbITMContext.InspectionInformation.Count(y=>y.InspectionId==x.Id&&y.CheckStatus=="正常").ToString()
                });
                var list3 = query3.Paged(payload.CurrentPage, payload.PageSize).ToList();
                for (int i = 0; i < list3.Count; i++)
                {
                    var key = list3[i].Id;
                    list3[i].Normal = _dbITMContext.InspectionInformation.Count(y => y.InspectionId == list3[i].InspectionId && y.CheckStatus == "正常").ToString();
                }



                response.SetData(list3);
                return Ok(response);
            }
        }

        /// <summary>
        /// 测试mysql连接
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult L_test()
        {
            using (_dbITMContext)
            {
                var query = _dbITMContext.Orgs.FirstOrDefault();
                return Ok(query);
            }
        }
    }


    

    internal class Inspectioninfo
    {
        public ulong Id { get; set; }
        public string ShouldCount { get; set; }
        public string ActualCount { get; set; }
        public string Create { get; set; }
        public ulong InspectionId { get; set; }
        public string Normal { get; set; }

    }
}
