using Microsoft.EntityFrameworkCore;
using QandAsite.Data.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QandAsite.Data
{
    public class QandARepo
    {

        private readonly string _connectionString;

        public QandARepo(string connectionString)
        {
            _connectionString = connectionString;
        }


        public List<Question> GetQuestions()
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Questions.Include(q => q.QuestionTags).
                ThenInclude(q => q.Tag).Include(q => q.User)
                .Include(q => q.Answers).
               ToList();
        }
        public List<Question> GetQuestionsForTag(string name)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Questions.Where(c => c.QuestionTags.Any(t => t.Tag.Name.ToLower() == name.ToLower()))
                    .Include(q => q.QuestionTags)
                    .ThenInclude(qt => qt.Tag)
                    .ToList();
        }
        public Tag GetTag(string name)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Tag.FirstOrDefault(t => t.Name == name);
        }
        public List<Answer> GetAnswers(int id)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Answers.Where(a => a.QuestionId == id).ToList();
        }
        public int AddTag(string name)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            Tag t = new() { Name = name };
            ctx.Tag.Add(t);
            ctx.SaveChanges();
            return t.Id;
        }
        public void AddQuestion(Question q, List<string> tag)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            ctx.Questions.Add(q);
            ctx.SaveChanges();
            foreach (string tg in tag)
            {
                Tag t = GetTag(tg);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tg);
                }
                else
                {
                    tagId = t.Id;
                }
                ctx.QuestionTags.Add(new QuestionTags
                {
                    QuestionId = q.Id,
                    TagId = tagId
                });
                
            }
            ctx.SaveChanges();
        }
        public void AddAnswer(Answer a)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            ctx.Add(a);
            ctx.SaveChanges();
        }
 

        public Question ViewBigQuestion(int id)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            return ctx.Questions.
           Include(q => q.QuestionTags).ThenInclude(t => t.Tag)
           .Include(u => u.User).Include(a => a.Answers).ThenInclude(a => a.User)
           .FirstOrDefault(q => q.Id == id);
        }
        public void IncrementLikes(int id)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            ctx.Database.ExecuteSqlInterpolated($"UPDATE Questions SET Likes = Likes + 1 WHERE id = {id}");
            ctx.SaveChanges();
        }
        public int GetLikesCount(int id)
        {
            using var ctx = new QuestionsDataContext(_connectionString);
            var question = ctx.Questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return 0;
            }
            return question.Likes;
        }
    }
}
