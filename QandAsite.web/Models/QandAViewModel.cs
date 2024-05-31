using QandAsite.Data;

namespace QandAsite.web.Models
{
    public class QandAViewModel
    {
        public List<Question> Questions { get; set; }
        public List<Tag> Tags { get; set; }

        public Question Question { get; set; }
    }
}
