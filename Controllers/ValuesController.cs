using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ExcelExport.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExcelExport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("Excel/{id}")]
        public ActionResult ExportToExcel(int id)
        {
            byte[] filecontent;
            try
            {

                var juryLists = from user in GetUsers()
                                where user.Roles.Any(r => r.RoleId == id.ToString())
                                select user;

                var articles = GetArticles().Include(r => r.ArticleReviews).Where(r2 => r2.ArticleReviews.Any()).ToList();

                List<string> cmnList = new List<string>();
                cmnList.Add("Article Title");
                cmnList.Add("Point Avarage");
                var juryListNames = juryLists.OrderBy(x => x.Id).Select(x => "JuryPoints : " + x.FullName).ToList();
                cmnList.AddRange(juryListNames);
                string[] columns = cmnList.ToArray();

                var heading = $"Results";

                var dt = new DataTable();
                foreach (var column in columns)
                {
                    dt.Columns.Add(column, typeof(string));
                }

                foreach (var item in articles)
                {
                    var _obj = new object[] { item.ArticleTitle, item.ArticleReview.OrderBy(x => x.JuryUserId).Select(x => x.ArticlePoint).Average() }.Concat(
                        item.ArticleReviews.OrderBy(x => x.JuryUserId).GroupBy(x => x.JuryUserId).Select(x => x.Average(y => y.ReviewPoint))
                            .Cast<object>()).ToArray();
                    dt.Rows.Add(_obj);
                }
                filecontent = ExcelExportHelper.ExportExcel(dt, heading, true, columns);
            }
            catch (Exception e)
            {
                return RedirectToAction("Details", new { id = id });
            }


            return File(filecontent, ExcelExportHelper.ExcelContentType, "Results.xlsx");
        }


        private IList<ApplicationUser> GetUsers()
        {

            var roles = new List<Role> { new Role { RoleId = "1", Name = "Reviewer" } };

            var users = new List<ApplicationUser> {
                new ApplicationUser { Id = "1", FullName="Reviewer 1", Roles=roles},
                new ApplicationUser { Id = "2", FullName="Reviewer 2", Roles=roles},
                new ApplicationUser { Id = "3", FullName="Reviewer 3", Roles=roles},
                new ApplicationUser { Id = "4", FullName="Reviewer 4", Roles=roles},
                new ApplicationUser { Id = "5", FullName="Reviewer 5", Roles=roles},
                new ApplicationUser { Id = "6", FullName="Reviewer 6", Roles=roles},

            };

            return users;
        }

        private IList<Article> GetArticles()
        {

            var articleReviews1 = new List<ArticleReview>{
                new ArticleReview {ArticleReviewId=1, ArticleId = 1, ReviewPoint=9, ReviewerId="1"},
                new ArticleReview {ArticleReviewId=2, ArticleId = 1, ReviewPoint=8, ReviewerId="3"},
                new ArticleReview {ArticleReviewId=3, ArticleId = 1, ReviewPoint=10, ReviewerId="5"}
            };

            var articleReviews2 = new List<ArticleReview>{
                new ArticleReview {ArticleReviewId=4, ArticleId = 2, ReviewPoint=9, ReviewerId="1"},
                new ArticleReview {ArticleReviewId=5, ArticleId = 2, ReviewPoint=8, ReviewerId="2"},
                new ArticleReview {ArticleReviewId=6, ArticleId = 2, ReviewPoint=10, ReviewerId="3"},
                 new ArticleReview {ArticleReviewId=7, ArticleId = 2, ReviewPoint=8, ReviewerId="4"},
                new ArticleReview {ArticleReviewId=8, ArticleId = 2, ReviewPoint=10, ReviewerId="5"}
            };

            var articles = new List<Article> {
                new Article {ArticleId=1, ArticleTitle ="Article 1", Reviews = articleReviews1},
                new Article {ArticleId=2, ArticleTitle ="Article 2", Reviews = articleReviews2},
                new Article {ArticleId=3, ArticleTitle ="Article 3"}
            };

            return articles;
        }
    }
}
