using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Cuisine
{
    [Route("api/v1/cuisine/[controller]/[action]")]
    [ApiController]
    public class AppCuisineController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public AppCuisineController(haikanSDMSContext dbContext, IMapper mapper)
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
        public IActionResult List(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string name = model.cuisineName;
            string type = model.cuisineType;
            string guid = model.schoolUuid;
            var entity = _dbContext.Cuisine.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).Select(x => new
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

            if (!string.IsNullOrEmpty(name))
            {
                entity = entity.Where(x => x.CuisineName.Contains(name));
            }
            if (!string.IsNullOrEmpty(type))
            {
                if (type == "特色菜")
                {
                    entity = entity.OrderByDescending(x => x.LikeNum);
                    var query = entity.ToList().Take(3);
                    response.SetData(query);
                }
                else
                {
                    entity = entity.Where(x => x.CuisineType == type);
                    var query = entity.ToList();
                    response.SetData(query);
                }
            }
            else
            {
                entity = entity.OrderByDescending(x => x.LikeNum);
                var query = entity.ToList();
                response.SetData(query);
            }
            return Ok(response);
        }

        private static string GetIngredient(string Ingredient, haikanSDMSContext _dbContext)
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
                var list = _dbContext.Ingredient.Where(x => uuids.Contains(x.IngredientUuid.ToString())).Select(x => x.FoodName).ToList();
                var names = string.Join(',', list);
                return names;
            }
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Givelike(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.Cuisine.FirstOrDefault(x => x.IsDelete != 1 && x.CuisineUuid == guid);
                query.LikeNum += 1;
                _dbContext.SaveChanges();
                response.SetSuccess("点赞！");
                return Ok(response);
            }
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCuisine(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var query = _dbContext.Cuisine.Where(x => x.CuisineUuid == guid).Select(x => new
            {
                x.Accessory,
                x.CuisineName,
                x.CuisineType,
                x.Price,
                x.Abstract,
                x.LikeNum,
                Ingredient = GetIngredient(x.Ingredient, _dbContext),
                Burdening = GetIngredient(x.Burdening, _dbContext),
                HeatEnergy = GetPurchase(x.Ingredient, x.Burdening, 1, _dbContext),
                Protein = GetPurchase(x.Ingredient, x.Burdening, 2, _dbContext),
                Fat = GetPurchase(x.Ingredient, x.Burdening, 3, _dbContext),
                Saccharides = GetPurchase(x.Ingredient, x.Burdening, 4, _dbContext),
                Va = GetPurchase(x.Ingredient, x.Burdening, 5, _dbContext)
            });
            response.SetData(query);
            return Ok(response);
        }

        private static string GetPurchase(string Ingredient, string Burdening, int type, haikanSDMSContext _dbContext)
        {
            double heat = 0;
            double Protein = 0;
            double Fat = 0;
            double Saccharides = 0;
            double Va = 0;
            bool isChinese = false;
            var num = "";
            if (string.IsNullOrEmpty(Ingredient) && string.IsNullOrEmpty(Burdening))
            {
                return "0";
            }
            else if (string.IsNullOrEmpty(Ingredient))
            {
                foreach (char ch in Burdening)
                {
                    //判断字符ch是否为汉字字符
                    if (ch >= 0x4e00 && ch <= 0x9fbb)
                    {
                        isChinese = true;
                        break;
                    }
                }
                if (!isChinese)
                {
                    var uuids = Burdening.ToUpper().Split(',').ToList();
                    var lists = _dbContext.Ingredient.Where(x => uuids.Contains(x.IngredientUuid.ToString())).Select(x => new { x.HeatEnergy, x.Protein, x.Fat, x.Saccharides, x.Va }).ToList();
                    heat = lists.Sum(x => x.HeatEnergy.Value);
                    Protein = lists.Sum(x => x.Protein.Value);
                    Fat = lists.Sum(x => x.Fat.Value);
                    Saccharides = lists.Sum(x => x.Saccharides.Value);
                    Va = lists.Sum(x => x.Va.Value);
                }
                
                   
                
                
            }
            else if (string.IsNullOrEmpty(Burdening))
            {
                foreach (char ch in Ingredient)
                {
                    //判断字符ch是否为汉字字符
                    if (ch >= 0x4e00 && ch <= 0x9fbb)
                    {
                        isChinese = true;
                        break;
                    }
                }
                if (!isChinese)
                {
                    var uuid = Ingredient.ToUpper().Split(',').ToList();
                    var lists = _dbContext.Ingredient.Where(x => uuid.Contains(x.IngredientUuid.ToString())).Select(x => new { x.HeatEnergy, x.Protein, x.Fat, x.Saccharides, x.Va }).ToList();
                    heat = lists.Sum(x => x.HeatEnergy.Value);
                    Protein = lists.Sum(x => x.Protein.Value);
                    Fat = lists.Sum(x => x.Fat.Value);
                    Saccharides = lists.Sum(x => x.Saccharides.Value);
                    Va = lists.Sum(x => x.Va.Value);
                }
            }
            else
            {
                foreach (char ch in Ingredient+Burdening)
                {
                    //判断字符ch是否为汉字字符
                    if (ch >= 0x4e00 && ch <= 0x9fbb)
                    {
                        isChinese = true;
                        break;
                    }
                }
                if (!isChinese)
                {
                    var uuid = Ingredient.ToUpper().Split(',').ToList();
                    var uuids = Burdening.ToUpper().Split(',').ToList();
                    var list = _dbContext.Ingredient.Where(x => uuid.Contains(x.IngredientUuid.ToString())).Select(x => new { x.HeatEnergy, x.Protein, x.Fat, x.Saccharides, x.Va }).ToList();
                    var lists = _dbContext.Ingredient.Where(x => uuids.Contains(x.IngredientUuid.ToString())).Select(x => new { x.HeatEnergy, x.Protein, x.Fat, x.Saccharides, x.Va }).ToList();
                    double heats = list.Sum(x => x.HeatEnergy.Value);
                    double Proteins = list.Sum(x => x.Protein.Value);
                    double Fats = list.Sum(x => x.Fat.Value);
                    double Saccharidess = list.Sum(x => x.Saccharides.Value);
                    double Vas = list.Sum(x => x.Va.Value);
                    heat = lists.Sum(x => x.HeatEnergy.Value) + heats;
                    Protein = lists.Sum(x => x.Protein.Value) + Proteins;
                    Fat = lists.Sum(x => x.Fat.Value) + Fats;
                    Saccharides = lists.Sum(x => x.Saccharides.Value) + Saccharidess;
                    Va = lists.Sum(x => x.Va.Value) + Vas;
                }
            }
            if (type == 1)
            {
                num = heat.ToString();
            }
            if (type == 2)
            {
                num = Protein.ToString();
            }
            if (type == 3)
            {
                num = Fat.ToString();
            }
            if (type == 4)
            {
                num = Saccharides.ToString();
            }
            if (type == 5)
            {
                num = Va.ToString();
            }
            return num;
        }

    }
}
