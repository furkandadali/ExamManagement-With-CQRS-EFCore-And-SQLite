namespace Entities.Models
{
    public class Exams : Base
    {
        public int Score { get; set; }
        public bool IsDone { get; set; }
        public string Answers { get; set; }
        public int ArticleId { get; set; }
        public int StudentId { get; set; }
    }
}
