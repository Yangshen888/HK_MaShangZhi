using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Quality
{
    [Route("api/v1/Quality/[controller]/[action]")]
    [ApiController]
    public class AppQualityController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public AppQualityController(haikanSDMSContext dbContext, IMapper mapper)
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
        public IActionResult FliesList(dynamic model)
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
            var entity = _dbContext.Quality.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).OrderByDescending(x => x.EffectiveTime).Select(x => new
            {
                x.FlieName,
                x.AddTime,
                Accessory = GetPicture(x.Accessory),
                x.QualityUuid,
                x.SchoolUuid,
                x.JianJie,
                x.EffectiveTime,
                //Content=GetContent(x.Accessory),
            });

            var query = entity.ToList();
            response.SetData(query);

            return Ok(response);
        }

        [HttpGet]
        public IActionResult FlieInfo(Guid? uuid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var entity= _dbContext.Quality.FirstOrDefault(x => x.QualityUuid == uuid);
            response.SetData(entity);
            return Ok(response);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string GetPicture(string content)
        {
            StringBuilder imgsrc = new StringBuilder();
            HtmlDocument doc = new HtmlDocument();
            if (string.IsNullOrEmpty(content))
            {
                return imgsrc.ToString();
            }
            doc.LoadHtml(content);
            var imgnode = doc.DocumentNode.SelectSingleNode("//img");
            if (imgnode != null)
            {
                imgsrc.Append(imgnode.Attributes["src"].Value);
            }
            return imgsrc.ToString();
        }

        /// <summary>
        /// 获取正文
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string GetContent(string content)
        {
            StringBuilder builder = new StringBuilder();
            var htmlDoc = new HtmlDocument();
            if (string.IsNullOrEmpty(content))
            {
                return builder.ToString();
            }
            htmlDoc.LoadHtml(content);

            var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//p");
            if (htmlNodes != null)
            {
                foreach (var node in htmlNodes)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append("\n");
                    }
                    builder.Append(node.InnerText);
                }
            }
            return builder.ToString();
        }
    }
}
