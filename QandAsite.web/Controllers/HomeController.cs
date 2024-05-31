using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QandAsite.Data;
using QandAsite.web.Models;
using System.Diagnostics;

namespace QandAsite.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult Index()
        {
            QandARepo repo = new(_connectionString);
            QandAViewModel vm = new()
            {
                Questions = repo.GetQuestions(),
              
            };

            return View(vm);
        }

        [Authorize]
        public IActionResult AskQuestion()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AskQuestion(Question q,List<string> tag)
        {
            QandARepo repo = new(_connectionString);
            UserRepo repo2 = new(_connectionString);
            q.Answers = repo.GetAnswers(q.Id);
            q.UserId = repo2.GetByEmail(User.Identity.Name).Id;
            q.DatePosted = DateTime.Now;
            repo.AddQuestion(q, tag);
            return Redirect($"/question/questionbig?id={q.Id}");
        }

        public IActionResult IsLiked(int id)
        {
            var ids = HttpContext.Session.Get<List<int>>("ids");
            var liked = ids != null && ids.Contains(id);
            return Json(liked);
        }

        [HttpPost]
        [Authorize]
        public void IncrementLikes(int id)
        {
            var ids = HttpContext.Session.Get<List<int>>("ids") ?? new List<int>();
            ids.Add(id);
            HttpContext.Session.Set("ids", ids);
            QandARepo repo = new(_connectionString);
            repo.IncrementLikes(id);
           

        }
        public IActionResult GetLikes(int id)
        {
            QandARepo repo = new(_connectionString);
            int count = repo.GetLikesCount(id);
            return Json(count);
        }



    }
}
