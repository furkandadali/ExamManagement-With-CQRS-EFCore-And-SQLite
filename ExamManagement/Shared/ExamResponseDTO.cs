using System;

namespace Shared
{
    public class ExamResponseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProductName { get; set; }
    }

    public class ExamDetailResponseDTO
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Answers { get; set; }
        public int ArticleId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDone { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Score { get; set; }
        public int StudentId { get; set; }
    }

    public class ArticleDetailResponseDTO
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Header { get; set; }
        public string Url { get; set; }
        public string ShortInfo { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class QuestionDetailResponseDTO
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string Question { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class AnswerDetailResponseDTO
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Response { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsTrue { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
