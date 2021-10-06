using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using Entities.Models;
using System.Net.Http.Headers;
using ExamManagement.Helper;
using ExamManagement.Models;
using Shared;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ExamManagement.Controllers
{
    [SwaggerTag("This allows you to view statistics of searches performed by your organization.")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class ExamController : BaseController
    {
        private readonly Interfaces.IGetWebPageContent _getWebPageContent;
        public ExamController(ExamManagementContext context, Interfaces.IGetWebPageContent getWebPageContent, IActionContextAccessor actionContext, IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor) : base(context, actionContext, serviceScopeFactory, httpContextAccessor)
        {
            _getWebPageContent = getWebPageContent;
        }

        [HttpPost]
        public IActionResult SetAndUpdateArticles(int articleIndex)
        {
            var result = _getWebPageContent.GetWebPageContent(articleIndex);

            if (result != null && result.Count >=1)
            {

                foreach (var articleItem in result)
                {
                    var auth = AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
                    string[] headerValue = ProjectHelper.ParseAuthorizationHeader(auth);
                    string parameters = string.Empty;
                    string username = headerValue[0];
                    string password = headerValue[1];
                    parameters += "Key: " + articleItem.Value.ArticleKey + "| ";
                    parameters += "Header: " + articleItem.Value.Header + "| ";
                    parameters += "Url: " + articleItem.Value.URL + "| ";
                    parameters += "ShortInfo: " + articleItem.Value.ShortInfo + "| ";
                    parameters += "Content: " + articleItem.Value.Content + "| ";

                    try
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                            var response = new ExamDetailResponseModel();
                            var articleCheckResponse = new ArticleResponseModel();
                            var user = context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);

                            if (user != null)
                            {
                                LogHelper.Info(parameters, user?.Id);
                                
                                var article = new Articles()
                                {
                                    Key = articleItem.Value.ArticleKey,
                                    Content = articleItem.Value.Content,
                                    Header = articleItem.Value.Header,
                                    Url = articleItem.Value.URL,
                                    ShortInfo = articleItem.Value.ShortInfo
                                };

                                articleCheckResponse.Result = context.Articles.Where(m => m.IsDeleted == false && m.Key == article.Key).Select(m => new ArticleDetailResponseDTO()
                                {
                                    Key = m.Key,
                                    Header = m.Header

                                }).FirstOrDefault();

                                if (articleCheckResponse.Result == null)
                                {
                                    context.Articles.Add(article);
                                    context.SaveChanges();
                                    //response.Result = new ArticleDetailResponseDTO()
                                    //{
                                    //TODO : article liste tipinde döngü dışında boş bir liste oluşturup listenin içerisini burada doldurup en son liste tipinde bir response dönebilir.
                                    //};
                                    LogHelper.Success(parameters, user?.Id);
                                }
                            }
                            else
                            {
                                return BadRequestWithCustomError("User Not Found!", "SR1001");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Fatal(ex, parameters, null);
                        return BadRequestWithCustomError("System Error!", "");
                    }
                }
                return OK("Makaleler Başarıyla Kaydedilmiştir.");
            }
            else
            {
                return BadRequestWithCustomError("System Error!", "");
            }
        }

        [HttpPost]
        public IActionResult CreateNewQuestion(CreateQuestionRequestDTO model)
        {
            var auth = AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            string[] headerValue = ProjectHelper.ParseAuthorizationHeader(auth);
            string parameters = string.Empty;
            string username = headerValue[0];
            string password = headerValue[1];
            parameters += "ArticleId: " + model.ArticleId + "| ";
            parameters += "Question: " + model.Question + "| ";
            parameters += "ApiKey: " + username;
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                    var response = new QuestionResponseModel();
                    var user = context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);

                    if (user != null)
                    {
                        LogHelper.Info(parameters, user?.Id);
                        //if(string.IsNullOrEmpty(model.ProductName))
                        //    return BadRequestWithCustomError("Please Fill Product Name!", "SR1001");
                        var product = new Questions()
                        {
                            ArticleId = model.ArticleId,
                            Question = model.Question
                        };
                        context.Questions.Add(product);
                        context.SaveChanges();
                        response.Result = new QuestionDetailResponseDTO()
                        {
                            Id = product.Id,
                            CreatedDate = product.CreatedDate,
                            ArticleId = product.ArticleId,
                            Question = product.Question,
                            IsDeleted = product.IsDeleted,
                            ModifiedDate = product.ModifiedDate
                        };
                        LogHelper.Success(parameters, user?.Id);
                        return OK(product);
                    }
                    else
                    {
                        return BadRequestWithCustomError("User Not Found!", "SR1001");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ex, parameters, null);
                return BadRequestWithCustomError("System Error!", "");
            }
        }

        [HttpPost]
        public IActionResult UpdateQuestion(UpdateQuestionRequestDTO model)
        {
            var auth = AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            string[] headerValue = ProjectHelper.ParseAuthorizationHeader(auth);
            string parameters = string.Empty;
            string username = headerValue[0];
            string password = headerValue[1];
            parameters += "Id: " + model.Id + "| ";
            parameters += "ArticleId: " + model.ArticleId + "| ";
            parameters += "IsDeleted: " + model.IsDeleted + "| ";
            parameters += "Question: " + model.Question + "| ";
            parameters += "ApiKey: " + username;
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                    var response = new QuestionResponseModel();
                    var user = context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);

                    if (user != null)
                    {
                        LogHelper.Info(parameters, user?.Id);

                        var question = context.Questions.FirstOrDefault(m => m.IsDeleted == false && m.Id == model.Id);
                        if (question != null)
                        {
                            question.Question = model.Question;
                            question.ModifiedDate = DateTime.Now;
                            question.IsDeleted = model.IsDeleted;
                            question.ArticleId = model.ArticleId;

                            context.Entry(question).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            context.SaveChanges();

                            response.Result = new QuestionDetailResponseDTO()
                            {
                                Id = question.Id,
                                CreatedDate = question.CreatedDate,
                                ArticleId = question.ArticleId,
                                IsDeleted = question.IsDeleted,
                                ModifiedDate = question.ModifiedDate,
                                Question = question.Question
                            };
                        }
                        else
                        {
                            return BadRequestWithCustomError("Product Not Found!", "PR1001");
                        }

                        LogHelper.Success(parameters, user?.Id);
                        return OK(question);
                    }
                    else
                    {
                        return BadRequestWithCustomError("User Not Found!", "SR1001");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ex, parameters, null);
                return BadRequestWithCustomError("System Error!", "");
            }
        }

        [HttpPost]
        public IActionResult CreateNewAnswer(CreateAnswerRequestDTO model)
        {
            var auth = AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            string[] headerValue = ProjectHelper.ParseAuthorizationHeader(auth);
            string parameters = string.Empty;
            string username = headerValue[0];
            string password = headerValue[1];
            parameters += "QuestionId: " + model.QuestionId + "| ";
            parameters += "IsTrue: " + model.IsTrue + "| ";
            parameters += "Response: " + model.Response + "| ";
            parameters += "ApiKey: " + username;
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                    var response = new AnswerResponseModel();
                    var user = context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);

                    if (user != null)
                    {
                        var answerCount = context.Answers.Where(m => m.IsDeleted == false && m.QuestionId == model.QuestionId).Select(m => new AnswerDetailResponseDTO()
                        {
                            Id = m.Id,
                            CreatedDate = m.CreatedDate

                        }).OrderBy(m => m.Id).ToList();

                        if (answerCount.Count <= 4)
                        {
                            LogHelper.Info(parameters, user?.Id);
                            //if(string.IsNullOrEmpty(model.ProductName))
                            //    return BadRequestWithCustomError("Please Fill Product Name!", "SR1001");
                            var answer = new Answers()
                            {
                                QuestionId = model.QuestionId,
                                Response = model.Response,
                                IsTrue = model.IsTrue,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false
                            };
                            context.Answers.Add(answer);
                            context.SaveChanges();
                            response.Result = new AnswerDetailResponseDTO()
                            {
                                Id = answer.Id,
                                CreatedDate = answer.CreatedDate,
                                Response = answer.Response,
                                IsDeleted = answer.IsDeleted,
                                ModifiedDate = answer.ModifiedDate,
                                IsTrue = answer.IsTrue,
                                QuestionId = answer.QuestionId
                            };
                            LogHelper.Success(parameters, user?.Id);
                            return OK(answer);
                        }
                        else
                        {
                            return BadRequestWithCustomError("Cevap Sayısı Max 4 olabilir.", "");//204 dönmekte fayda var!
                        }

                    }
                    else
                    {
                        return BadRequestWithCustomError("User Not Found!", "SR1001");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ex, parameters, null);
                return BadRequestWithCustomError("System Error!", "");
            }
        }

        [HttpPost]
        public IActionResult UpdateAnswer(UpdateAnswerRequestDTO model)
        {
            var auth = AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            string[] headerValue = ProjectHelper.ParseAuthorizationHeader(auth);
            string parameters = string.Empty;
            string username = headerValue[0];
            string password = headerValue[1];
            parameters += "Id: " + model.Id + "| ";
            parameters += "IsDeleted: " + model.IsDeleted + "| ";
            parameters += "IsTrue: " + model.IsTrue + "| ";
            parameters += "QuestionId: " + model.QuestionId + "| ";
            parameters += "Response: " + model.Response + "| ";
            parameters += "ApiKey: " + username;
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                    var response = new AnswerResponseModel();
                    var user = context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);

                    if (user != null)
                    {
                        LogHelper.Info(parameters, user?.Id);

                        var answer = context.Answers.FirstOrDefault(m => m.IsDeleted == false && m.Id == model.Id);
                        if (answer != null)
                        {
                            answer.Id = model.Id;
                            answer.QuestionId = model.QuestionId;
                            answer.ModifiedDate = DateTime.Now;
                            answer.IsDeleted = model.IsDeleted;
                            answer.IsTrue = model.IsTrue;
                            answer.Response = model.Response;

                            context.Entry(answer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            context.SaveChanges();

                            response.Result = new AnswerDetailResponseDTO()
                            {
                                Id = answer.Id,
                                CreatedDate = answer.CreatedDate,
                                IsTrue = answer.IsTrue,
                                IsDeleted = answer.IsDeleted,
                                ModifiedDate = answer.ModifiedDate,
                                Response = answer.Response,
                                QuestionId = answer.QuestionId
                            };
                        }
                        else
                        {
                            return BadRequestWithCustomError("Product Not Found!", "PR1001");
                        }

                        LogHelper.Success(parameters, user?.Id);
                        return OK(answer);
                    }
                    else
                    {
                        return BadRequestWithCustomError("User Not Found!", "SR1001");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ex, parameters, null);
                return BadRequestWithCustomError("System Error!", "");
            }
        }

        [HttpPost]
        public IActionResult CreateNewExam(NewExamRequestDTO model)
        {
            var auth = AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            string[] headerValue = ProjectHelper.ParseAuthorizationHeader(auth);
            string parameters = string.Empty;
            string username = headerValue[0];
            string password = headerValue[1];
            parameters += "ArticleId: " + model.ArticleId + "| ";
            parameters += "StudentId: " + model.StudentId + "| ";
            parameters += "ApiKey: " + username;

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                    var response = new ExamDetailResponseModel();
                    var user = context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);

                    if (user != null)
                    {
                        LogHelper.Info(parameters, user?.Id);
                        //if(string.IsNullOrEmpty(model.ProductName))
                        //    return BadRequestWithCustomError("Please Fill Product Name!", "SR1001");
                        var exam = new Exams()
                        {
                            StudentId = model.StudentId,
                            ArticleId = model.ArticleId
                        };
                        context.Exams.Add(exam);
                        context.SaveChanges();
                        response.Result = new ExamDetailResponseDTO()
                        {
                            Id = exam.Id,
                            CreatedDate = exam.CreatedDate,
                            ArticleId = exam.ArticleId,
                            StudentId = exam.StudentId,
                            IsDeleted = exam.IsDeleted,
                            ModifiedDate = exam.ModifiedDate,
                            IsDone = exam.IsDone,
                            Score = exam.Score,
                            Answers = exam.Answers
                        };
                        LogHelper.Success(parameters, user?.Id);
                        return OK(exam);
                    }
                    else
                    {
                        return BadRequestWithCustomError("User Not Found!", "SR1001");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ex, parameters, null);
                return BadRequestWithCustomError("System Error!", "");
            }
        }
        
        [HttpPost]
        public IActionResult UpdateExam(UpdateExamRequestDTO model)
        {
            var auth = AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
            string[] headerValue = ProjectHelper.ParseAuthorizationHeader(auth);
            string parameters = string.Empty;
            string username = headerValue[0];
            string password = headerValue[1];
            parameters += "Id: " + model.Id + "| ";
            parameters += "Answers: " + model.Answers + "| ";
            parameters += "Isdeleted: " + model.Isdeleted + "| ";
            parameters += "StudentId: " + model.StudentId + "| ";
            parameters += "QuestionId: " + model.ArticleId + "| ";
            parameters += "ApiKey: " + username;
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                    var response = new ExamDetailResponseModel();
                    var user = context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);
                    Dictionary<int, string> trot = new Dictionary<int, string>();
                    Dictionary<int, string> studentTrot = new Dictionary<int, string>();
                    int count = 1;
                    var studentTrotList = model.Answers?.Split(',')?.ToList();

                    if (studentTrotList != null && studentTrotList.Count >0)
                    {
                        foreach (var trotItem in studentTrotList)
                        {
                            studentTrot.Add(count, trotItem.Split('-').LastOrDefault().ToString());
                            count++;
                        }
                    }

                    count = 1;
                    int score = 0;

                    if (user != null)
                    {
                        LogHelper.Info(parameters, user?.Id);

                        var exam = context.Exams.FirstOrDefault(m => m.IsDeleted == false && m.Id == model.Id);

                        var questions = context.Questions.Where(m => m.IsDeleted == false && m.ArticleId == model.ArticleId).Select(m => new QuestionDetailResponseDTO()
                        {
                            Id = m.Id,
                            CreatedDate = m.CreatedDate

                        }).OrderBy(m => m.Id).ToList();

                        foreach (var questionItem in questions)
                        {
                            var answer = context.Answers.Where(m => m.IsDeleted == false && m.QuestionId == questionItem.Id && m.IsTrue == true).Select(m => new AnswerDetailResponseDTO()
                            {
                                
                                Id = m.Id,
                                Response = m.Response

                            }).OrderBy(m => m.Id).ToList().FirstOrDefault().Response.ToString();
                            trot.Add(count,answer);
                            count++;
                        }

                        for (int i = 1; i < count; i++)
                        {
                            if (studentTrot[i] == trot[i])
                            {
                                score += 25;
                            }
                        }

                        if (exam != null && score != null)
                        {
                            exam.Id = model.Id;
                            exam.IsDeleted = model.Isdeleted;
                            exam.ModifiedDate = DateTime.Now;
                            exam.Answers = model.Answers;
                            exam.ArticleId = model.ArticleId;
                            exam.IsDone = true;
                            exam.StudentId = model.StudentId;
                            exam.Score = score;

                            context.Entry(exam).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            context.SaveChanges();

                            response.Result = new ExamDetailResponseDTO()
                            {
                                Id = exam.Id,
                                CreatedDate = exam.CreatedDate,
                                StudentId = exam.StudentId,
                                IsDeleted = exam.IsDeleted,
                                ModifiedDate = exam.ModifiedDate,
                                IsDone = exam.IsDone,
                                ArticleId = exam.ArticleId,
                                Answers = exam.Answers,
                                Score = exam.Score
                            };
                        }
                        else
                        {
                            return BadRequestWithCustomError("Product Not Found!", "PR1001");
                        }

                        LogHelper.Success(parameters, user?.Id);
                        return OK(exam);
                    }
                    else
                    {
                        return BadRequestWithCustomError("User Not Found!", "SR1001");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Fatal(ex, parameters, null);
                return BadRequestWithCustomError("System Error!", "");
            }
        }


        //[HttpGet]
        //public IActionResult GetExams([Required] int StudentId)
        //{
        //    var auth = AuthenticationHeaderValue.Parse(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
        //    string[] headerValue = ProjectHelper.ParseAuthorizationHeader(auth);
        //    string parameters = string.Empty;
        //    string username = headerValue[0];
        //    string password = headerValue[1];
        //    parameters += "StudenttId: " + StudenttId + "| ";
        //    parameters += "ApiKey: " + username;

        //    try
        //    {
        //        using (var scope = _serviceScopeFactory.CreateScope())
        //        {
        //            var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
        //            var response = new ExamDetailResponseModel();
        //            var user = context.ApiUsers.FirstOrDefault(m => m.IsDeleted == false && m.ApiKey == username && m.ApiPassword == password);

        //            if (user != null)
        //            {
        //                LogHelper.Info(parameters, user?.Id);

        //                response.Result = context.Exams.Where(m => m.IsDeleted == false && m.Id == ProductId).Select(m => new ExamDetailResponseDTO()
        //                {
        //                    Id = m.Id,
        //                    CreatedDate = m.CreatedDate,
        //                    
        //                }).FirstOrDefault();

        //                LogHelper.Success(parameters, user?.Id);
        //                return OK(response);
        //            }
        //            else
        //            {
        //                return BadRequestWithCustomError("User Not Found!", "SR1001");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Fatal(ex, parameters, null);
        //        return BadRequestWithCustomError("System Error!", "");
        //    }
        //}
    }
}

