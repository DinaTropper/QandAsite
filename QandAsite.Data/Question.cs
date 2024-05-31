namespace QandAsite.Data
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set;}
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }
        public int UserId { get; set; }
        public int Likes { get; set; }

        public List<QuestionTags> QuestionTags { get; set; }
        public List<Answer> Answers { get; set; } = new();


        public User User { get; set; }
    }
}
