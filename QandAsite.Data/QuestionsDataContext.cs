using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Net.Mime.MediaTypeNames;

namespace QandAsite.Data
{
    public class QuestionsDataContext : DbContext
    {
        private readonly string _connectionString;

        public QuestionsDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //set up composite primary key
            modelBuilder.Entity<QuestionTags>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<QuestionTags> QuestionTags { get; set; }
    }
}
