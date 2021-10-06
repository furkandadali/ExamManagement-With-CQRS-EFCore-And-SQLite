using System;
using System.Net;

namespace Shared
{
    public class Response
    {

        public HttpStatusCode HttpStatusCode { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public object Result { get; set; }

        public static Response GetExceptionResponse(Exception ex)
        {
            if (ex is NullReferenceException)
                return new Response
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "9000",
                    ErrorMessage = "Null Reference Exception"
                };

            else
                return new Response
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorCode = "9999",
                    ErrorMessage = "System Error"
                };

        }

        public static Response GetErrorResponse(HttpStatusCode httpStatusCode, string errorCode, string errorMessage)
        {

            return new Response
            {
                IsSuccess = false,
                HttpStatusCode = httpStatusCode,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage
            };

        }

        public static Response GetResponse(HttpStatusCode httpStatusCode, object result)
        {

            return new Response
            {
                IsSuccess = true,
                HttpStatusCode = httpStatusCode,
                Result = result
            };

        }

        public static Response GetResponse(HttpStatusCode httpStatusCode)
        {

            return new Response
            {
                IsSuccess = true,
                HttpStatusCode = httpStatusCode,
                Result = null
            };

        }
    }

}
