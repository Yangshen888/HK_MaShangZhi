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
    public class AppMessageController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        public AppMessageController(haikanSDMSContext dbContext, IMapper mapper)
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
        public IActionResult MessageList(dynamic model)
        {
          
            var response = ResponseModelFactory.CreateResultInstance;

            string guid = model.schoolUuid;
            var entity = _dbContext.Train.Where(x => x.IsDelete != "1" && x.SchoolUuid == Guid.Parse(guid)).Select(x => new
            {
                x.Content,
                x.Theme,
                x.Id,
                x.TrainTime,
                x.TrainName,
                x.Speaker,
                x.TrainUuid,
                x.Mnum,
                x.Address,
                x.Anum,
                x.TrainLession

            });

            var query = entity.ToList();
            response.SetData(query);

            return Ok(response);
        }

        [HttpPost]
        public IActionResult MessageshowList(dynamic model)
        {

            var response = ResponseModelFactory.CreateResultInstance;
            string traGuid = model.trainUuid;
            string guid = model.schoolUuid;
            var entity = _dbContext.Train.Where(x => x.IsDelete != "1" && x.SchoolUuid == Guid.Parse(guid) && x.TrainUuid == Guid.Parse(traGuid)).Select(x => new
            {
                x.Content,
                x.Theme,
                x.Id,
                x.TrainTime,
                x.TrainName,
                x.Speaker,
                x.TrainUuid,
                x.Mnum,
                x.Address,
                x.Anum,
                x.TrainLession

            });

            var query = entity.ToList();
            response.SetData(query);

            return Ok(response);
        }
    }
}
