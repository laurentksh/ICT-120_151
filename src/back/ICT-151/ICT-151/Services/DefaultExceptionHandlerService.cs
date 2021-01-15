using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace ICT_151.Services
{
    public interface IExceptionHandlerService
    {
        public IActionResult Handle(Exception exception, HttpRequest request);
    }
    
    public class DefaultExceptionHandlerService : IExceptionHandlerService
    {
        public IActionResult Handle(Exception exception, HttpRequest request)
        {
            var problem = GetProblem(exception);
            ProblemDetails problemDetails = new ProblemDetails
            {
                Type = $"/Problems/{problem.problemType}",
                Title = problem.httpStatus.ToString(),
                Status = (int)problem.httpStatus,
                Detail = problem.problemMsg,
                Instance = request?.Path
            };

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }

        private static (HttpStatusCode httpStatus, string problemType, string problemMsg) GetProblem(Exception exception)
        {
            var exMsg = exception.Message ?? "Unspecified";

            return exception switch
            {
                FormatException or ArgumentException => (HttpStatusCode.BadRequest, "BadRequest", exMsg),
                WrongCredentialsException => (HttpStatusCode.Unauthorized, "Unauthorized", exMsg),
                UserNotFoundException or DataNotFoundException => (HttpStatusCode.NotFound, "NotFound", exMsg),
                ForbiddenException => (HttpStatusCode.Forbidden, "Forbidden", exMsg),
                DataAlreadyExistsException => (HttpStatusCode.BadRequest, "Forbidden", exMsg),
                NotImplementedException => (HttpStatusCode.NotImplemented, "NotImplemented", exMsg),
                NullReferenceException => (HttpStatusCode.InternalServerError, "InternalServerError", "Unspecified server error"),
                _ => (HttpStatusCode.InternalServerError, "InternalServerError", "Unspecified server error")
            };
        }
    }
}
