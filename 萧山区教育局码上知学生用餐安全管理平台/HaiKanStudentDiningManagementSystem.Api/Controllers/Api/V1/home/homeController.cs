using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.home
{
    [Route("api/v1/home/[controller]/[action]")]
    [ApiController]
    public class homeController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;

        public homeController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 价格变动
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult price()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var food = _dbContext.Ingredient.Where(x=>x.IsDelete==0 && x.SchoolUuid== AuthContextService.CurrentUser.SchoolGuid).Select(x=>new { 
                    x.FoodName,
                    OldPrice= OldPurchase(x.IngredientUuid),
                    NewPrice = NewPurchase(x.IngredientUuid),
                    Proportion = (NewPurchase(x.IngredientUuid) - OldPurchase(x.IngredientUuid)) == 0 ? "0" : (NewPurchase(x.IngredientUuid)- OldPurchase(x.IngredientUuid))>0?"+"+(NewPurchase(x.IngredientUuid) - OldPurchase(x.IngredientUuid)):"-"+(NewPurchase(x.IngredientUuid) - OldPurchase(x.IngredientUuid)),
                }).ToList();
                response.SetData(food);
                return Ok(response);
            }
        }

        /// <summary>
        /// 上周价格
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static double OldPurchase(Guid guid)
        {
            using (haikanSDMSContext cc = new haikanSDMSContext())
            {
                double num = 0;
                var olddate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                var query = cc.PurchaseRecord.Where(x => x.PurchaseDate.Substring(0,10) == olddate && x.IsDelete == 0 && x.IngredientUuid==guid);
                if (query.Count()>0)
                {
                    var query1 = query.OrderByDescending(x => x.PurchaseDate).First();
                    num = query1.Price.Value;
                }
                return num;
            }
        }

        /// <summary>
        /// 最近价格
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static double NewPurchase(Guid guid)
        {
            using (haikanSDMSContext cc = new haikanSDMSContext())
            {
                double num = 0;
                var newdate = DateTime.Now.ToString("yyyy-MM-dd");

                var query = cc.PurchaseRecord.Where(x => x.PurchaseDate.Substring(0, 10) == newdate && x.IsDelete == 0 && x.IngredientUuid == guid);
                if (query.Count() > 0)
                {
                    var query1 = query.OrderByDescending(x => x.PurchaseDate).First();
                    num = query1.Price.Value;
                }
                return num;
            }
        }
    }
}
