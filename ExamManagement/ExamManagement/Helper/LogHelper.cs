using Entities.Models;
using ExamManagement.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExamManagement.Helper
{
    public class LogHelper
    {
        public static void Info(string parametres, int? userId)
        {
            InfoLogger(parametres, userId);
        }
        public static void Fatal(Exception ex, string parametres, int? userId)
        {
            ExceptionLogger(ex, parametres, userId);
        }
        public static void Success(string parametres, int? userId)
        {
            SuccessLogger(parametres, userId);
        }

        private static void ExceptionLogger(Exception ex, string parameters, int? userId)
        {
            using (var scope = BaseController._serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                var controllerName = BaseController._actionContext.ActionContext.RouteData.Values["controller"].ToString();
                var actionName = BaseController._actionContext.ActionContext.RouteData.Values["action"].ToString();

                var model = new Log();
                model.Guid = Guid.NewGuid().ToString();
                model.UserId = userId;
                model.PageUrl = controllerName + "/" + actionName;
                model.Parameters = parameters;
                model.ExceptionMessage = ex?.Message;
                model.InnerException = ex?.InnerException?.Message;
                model.InInnerExceptionMessage = ex?.InnerException?.InnerException?.Message;
                model.StackTrace = ex?.StackTrace.ToString();
                model.Method = controllerName;
                model.Action = actionName;
                model.IpAddress = BaseController._actionContext.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
                model.RequestType = BaseController._actionContext.ActionContext.HttpContext.Request.Method;
                model.LogLevel = "FATAL";
                model.CreatedDate = DateTime.Now;


                context.Logs.Add(model);
                context.SaveChanges();
            }
        }
        private static void InfoLogger(string parameters, int? userId)
        {
            using (var scope = BaseController._serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                var controllerName = BaseController._actionContext.ActionContext.RouteData.Values["controller"].ToString();
                var actionName = BaseController._actionContext.ActionContext.RouteData.Values["action"].ToString();

                var model = new Log();
                model.Guid = Guid.NewGuid().ToString();
                model.UserId = userId;
                model.PageUrl = controllerName + "/" + actionName;
                model.Parameters = parameters;
                model.Method = controllerName;
                model.Action = actionName;
                model.IpAddress = BaseController._actionContext.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
                model.RequestType = BaseController._actionContext.ActionContext.HttpContext.Request.Method;
                model.LogLevel = "INFO";
                model.CreatedDate = DateTime.Now;

                context.Logs.Add(model);
                context.SaveChanges();
            }
        }
        private static void SuccessLogger(string parameters, int? userId)
        {
            using (var scope = BaseController._serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ExamManagementContext>();
                var controllerName = BaseController._actionContext.ActionContext.RouteData.Values["controller"].ToString();
                var actionName = BaseController._actionContext.ActionContext.RouteData.Values["action"].ToString();

                var model = new Log();
                model.Guid = Guid.NewGuid().ToString();
                model.UserId = userId;
                model.PageUrl = controllerName + "/" + actionName;
                model.Parameters = parameters;
                model.Method = controllerName;
                model.Action = actionName;
                model.IpAddress = BaseController._actionContext.ActionContext.HttpContext.Connection.RemoteIpAddress.ToString();
                model.RequestType = BaseController._actionContext.ActionContext.HttpContext.Request.Method;
                model.LogLevel = "SUCCESS";
                model.CreatedDate = DateTime.Now;

                dbContext.Logs.Add(model);
                dbContext.SaveChanges();
            }
        }
    }
}
