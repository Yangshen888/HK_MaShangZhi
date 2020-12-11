using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.canteenManagement
{
    [Route("api/v1/canteenManagement/[controller]/[action]")]
    [ApiController]
    public class AppMoneyController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        public AppMoneyController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// 查看菜品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MoneyList(dynamic model)
        {
            //using (_dbContext)
            //{
            //    var entity = from p in _dbContext.SchoolJour
            //                 where p.IsDelete != 1
            //                 select new
            //                 {
            //                     p.Headline,
            //                     p.Content,
            //                     p.Accessory,
            //                     p.SchoolJourUuid,
            //                     p.SchoolUuid,
            //                 };
            //    var response = ResponseModelFactory.CreateInstance;

            //    var query = entity.ToList();
            //    response.SetData(query);
            //    return Ok(response);
            //}
            var response = ResponseModelFactory.CreateResultInstance;

            string guid = model.schoolUuid;
            var entity = _dbContext.ElectronicBill.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).Select(x => new
            {
                x.PurchaseTime,
                x.Supplier,
                x.Phone,
                x.CuisineName,
                x.Specification,
                x.Quantity,
                x.ProducedTime,
                x.ExpirationTime,
                x.Rt,
                x.ElectronicUuid



            });

            var query = entity.ToList();
            response.SetData(query);

            return Ok(response);
        }
        [HttpPost]
        public IActionResult MoneyshowList(dynamic model)
        {

            var response = ResponseModelFactory.CreateResultInstance;
            string traGuid = model.electronicUuid;
            string guid = model.schoolUuid;
            var entity = _dbContext.ElectronicBill.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid) && x.ElectronicUuid == Guid.Parse(traGuid)).Select(x => new
            {
                x.PurchaseTime,
                x.Supplier,
                x.Phone,
                x.CuisineName,
                x.Specification,
                x.Quantity,
                x.ProducedTime,
                x.ExpirationTime,
                x.Rt,
                x.ElectronicUuid

            });

            var query = entity.ToList();
            response.SetData(query);

            return Ok(response);
        }
    }
}
