using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Models.New;
using HaiKanStudentDiningManagementSystem.Api.MYDEntities;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.campusnews
{
    [Route("api/v1/campusnewsApp/[controller]/[action]")]
    [ApiController]
    public class campusnewsController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly haikanITMDContext _DdbContext;
        private readonly IMapper _mapper;
        public campusnewsController(haikanSDMSContext dbContext, IMapper mapper, haikanITMDContext DdbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _DdbContext = DdbContext;
        }
        /// <summary>
        /// 查看菜品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult NewsList(dynamic model)
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
            string name = model.headline;
            string guid = model.schoolUuid;
            var entity = _dbContext.SchoolJour.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).Select(x => new
            {
                x.Headline,
                x.Content,
                x.Addtime,
                x.SchoolJourUuid,            
                x.Accessory
            });
            if (!string.IsNullOrEmpty(name))
            {
                entity = entity.Where(x => x.Headline.Contains(name));
            }

            var query = entity.ToList();
                response.SetData(query);
            
            return Ok(response);
        }
        /// <summary>
        /// 查看
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LifeList(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string name = model.headline;
            string guid = model.schoolUuid;
            var entity = _dbContext.SchoolLife.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).Select(x => new
            {
                x.Headline,
                x.SchoollifeUuid,
                x.AddTime,
                x.Accessory
            });
            if (!string.IsNullOrEmpty(name))
            {
                entity = entity.Where(x => x.Headline.Contains(name));
            }

            var query = entity.ToList();
            response.SetData(query);

            return Ok(response);
        }

        [HttpPost]
        public IActionResult SchoolNews2(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string name = model.headline;
            string guid = model.schoolUuid;
            int pageSize = model.pageSize;
            int currentPage = model.currentPage;
            var query = _dbContext.SchoolLife.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid)).Select(x => new
            {
                Headline = x.Headline,
                Uuid = x.SchoollifeUuid,
                AddTime = x.AddTime.Substring(0, 11).Replace("-","/"),
                Accessory = x.Accessory,
                Content = "",
                Type = 1,
                x.Tag,
                x.Digest

            });
            var query2 = _dbContext.SchoolJour.Where(x => x.IsDelete != 1 && x.SchoolUuid == Guid.Parse(guid));
            //var query2_2 = query2.Select(x => new
            //{
            //    x.Headline,
            //    x.SchoolJourUuid,
            //    x.Content,
                
            //}).ToList();
            //for(int i = 0; i < query2_2.Count; i++)
            //{
            //    var p=GetPicture(query2_2[i].Content);
            //    var s=GetContent(query2_2[i].Content);
            //}
            var query2_1=query2
                .Select(x => new
                {
                    Headline = x.Headline,
                    Uuid = x.SchoolJourUuid,
                    AddTime = x.Addtime.Substring(0, 10),
                    Accessory = x.Accessory,
                    //Accessory= GetPicture(x.Content),
                    Content = GetContent(x.Content),
                    Type = 2,
                    x.Tag,
                    x.Digest
                });
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Headline.Contains(name));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query2_1 = query2_1.Where(x => x.Headline.Contains(name));
            }
            var list1 = query.ToList();
            var list2 = query2_1.ToList();
            list1.AddRange(list2);
            var query3 = list1.OrderByDescending(x => x.AddTime).AsQueryable();
            var list = query3.Paged(currentPage, pageSize).ToList();

            //var query3 = query.Union(query2_1);
            //query3 = query3.OrderByDescending(x => x.AddTime);
            //var list = query3.ToList();
            response.SetData(list);

            return Ok(response);
        }

        [HttpPost]
        public IActionResult SchoolNews(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string name = model.headline;
            string guid = model.schoolUuid;
            int pageSize = model.pageSize;
            int currentPage = model.currentPage;
            List<New> news = new List<New>();
            //内
            var query = _dbContext.SchoolNews.Where(x => x.SchoolUuid == Guid.Parse(guid));
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Headline.Contains(name));
            }
            //var query3 = query.OrderByDescending(x => x.AddTime).ThenByDescending(x => x.Id).AsQueryable();
            //var data = query3.Paged(currentPage, pageSize).ToList();
            var list = query.ToList().Select(x => new New
            {
                ID=(int)x.Id,
                Headline=x.Headline,
                Uuid=x.Uuid.ToString(),
                AddTime = x.AddTime.Replace("-", "/"),
                Accessory=x.Accessory,
                //Content= GetContent(x.Content),
                Type=x.Type.ToString(),
                Tag=x.Tag,
                Digest=x.Digest,
                InOut="内"
            }).ToList();
            //外
            var myquery = _dbContext.School.FirstOrDefault(x=>x.SchoolUuid==Guid.Parse(guid));
            var myquery1 = _DdbContext.OfficialAccounts.FirstOrDefault(x=>x.SchoolName==myquery.SchoolName);
            if (myquery1!=null)
            {
                var myquery2 = _DdbContext.Articles.Where(x => x.OfficialAccountId == myquery1.Id);
                if (!string.IsNullOrEmpty(name))
                {
                    myquery2 = myquery2.Where(x => x.Subject.Contains(name));
                }
                //var myquery3 = myquery2.OrderByDescending(x => x.PublishedAt).AsQueryable();
                //var mydata = myquery3.Paged(currentPage, pageSize).ToList();
                news = myquery2.ToList().Select(x => new New
                {
                    ID=(int)x.Id,
                    Uuid = x.Id.ToString(),
                    //Headline = MYSchoolNew(x.Subject, 1),
                    //Digest = MYSchoolNew(x.Subject, 0),
                    Headline = x.Subject,
                    Digest = x.Description,
                    AddTime = x.PublishedAt != null ? x.PublishedAt.Value.ToString("yyyy/MM/dd") : DateTime.Now.ToString("yyyy/MM/dd"),
                    Accessory = "https://img.jiulong.yoruan.com/"+ x.Cover,
                    Type = "2",
                    Tag = "新闻",
                    InOut = "外"
                }).ToList();
            }
            //内外结合
            news.AddRange(list);
            var data = news.AsQueryable().OrderByDescending(x=>x.AddTime).ThenByDescending(x => x.ID).Paged(currentPage, pageSize).ToList();
            response.SetData(data);

            return Ok(response);
        }

        public string MYSchoolNew(string content,int type)
        {
            string data = "";
            if (content.Split("|").Length>1)
            {
                data = content.Split("|")[type];
            }
            else if (content.Split("｜").Length > 1)
            {
                data = content.Split("｜")[type];
            }
            else if (type == 1)
            {
                data = content;
            }
            return data;
        }

        [HttpGet]
        public IActionResult GetMYNew(string guid)
        {
            using (_DdbContext)
            {
                var entity = _DdbContext.Articles.Where(x => x.Id.ToString() == guid).Select(x=>new {
                    headline= x.Subject,
                    Addtime = x.PublishedAt != null ? x.PublishedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    x.Content
                }).ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }





        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string GetPicture(string content)
        {
            StringBuilder imgsrc=new StringBuilder();
            HtmlDocument doc = new HtmlDocument();
            if (string.IsNullOrEmpty(content))
            {
                return imgsrc.ToString();
            }
            doc.LoadHtml(content);
            var imgnode=doc.DocumentNode.SelectSingleNode("//img");
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