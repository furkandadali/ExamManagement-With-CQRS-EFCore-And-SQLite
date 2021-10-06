using Shared;
using System.Collections.Generic;
using System.Net;

namespace ExamManagement.Models
{
    public class SwaggerResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;
        public bool IsSuccess { get; set; } = true;
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string ExtraInfo { get; set; }
    }
    public class ExamDetailResponseModel : SwaggerResponse
    {
        public ExamDetailResponseDTO Result { get; set; } = new ExamDetailResponseDTO();
    }
    public class ExamListResponseModel : SwaggerResponse
    {
        public List<ExamResponseDTO> Result { get; set; } = new List<ExamResponseDTO>();
    }
    public class ArticleResponseModel : SwaggerResponse
    {
        public ArticleDetailResponseDTO Result { get; set; } = new ArticleDetailResponseDTO();
    }
    public class QuestionResponseModel : SwaggerResponse
    {
        public QuestionDetailResponseDTO Result { get; set; } = new QuestionDetailResponseDTO();
    }
    public class AnswerResponseModel : SwaggerResponse
    {
        public AnswerDetailResponseDTO Result { get; set; } = new AnswerDetailResponseDTO();
    }
}
