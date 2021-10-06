namespace Entities.Models
{
    public class Answers : Base
    {
        public int QuestionId { get; set;}
        public string Response { get; set; }
        public bool IsTrue { get; set; }
    }
}
