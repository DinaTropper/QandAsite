using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QandAsite.Data;
using QandAsite.web.Models;
using System.Net.Sockets;

namespace QandAsite.web.Controllers
{
    public class QuestionController : Controller
    {
        private readonly string _connectionString;
        public QuestionController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        public IActionResult QuestionBig(int id)
        {
            QandARepo repo = new(_connectionString);
            QandAViewModel vm = new()
            {
                Question = repo.ViewBigQuestion(id)

            };
            return View(vm);
        }

        public IActionResult QuestionsByTag(string name)
        {
            QandARepo repo = new(_connectionString);
            QandAViewModel vm = new()
            {
                Questions = repo.GetQuestionsForTag(name)

            };
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddAnswer(Answer a, int Qid)
        {
            QandARepo repo = new(_connectionString);
            UserRepo repo2 = new(_connectionString);

            a.Date = DateTime.Now;
            a.UserId = repo2.GetByEmail(User.Identity.Name).Id;
            a.QuestionId = Qid;
            repo.AddAnswer(a);

            return Redirect($"/home/index");
        }


    }
}
