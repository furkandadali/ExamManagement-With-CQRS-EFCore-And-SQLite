namespace Shared
{
    public class NewExamRequestDTO
    {
        public int ArticleId { get; set; }
        public int StudentId { get; set; }
    }

    public class UpdateExamRequestDTO
    {
        public int Id { get; set; }
        public string Answers { get; set; }
        public bool Isdeleted { get; set; }
        public int StudentId { get; set; }
        public int ArticleId { get; set; }
    }

    public class CreateQuestionRequestDTO
    {
        public int ArticleId { get; set; }
        public string Question { get; set; }
    }

    public class UpdateQuestionRequestDTO
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public bool IsDeleted { get; set; }
        public string Question { get; set; }
    }

    public class CreateAnswerRequestDTO
    {
        public string Response { get; set; }
        public int QuestionId { get; set; }
        public bool IsTrue { get; set; }
    }

    public class UpdateAnswerRequestDTO
    {
        public int Id { get; set; }
        public string Response { get; set; }
        public bool IsDeleted { get; set; }
        public int QuestionId { get; set; }
        public bool IsTrue { get; set; }
    }
}
