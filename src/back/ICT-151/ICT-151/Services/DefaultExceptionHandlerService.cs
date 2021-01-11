using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ICT_151.Services
{
    public interface IExceptionHandlerService
    {
        public IActionResult Handle(Exception exception, object context = null);
    }
    
    public class DefaultExceptionHandlerService : IExceptionHandlerService
    {
        public IActionResult Handle(Exception exception, object context = null)
        {
            switch (exception) {
                case FormatException:
                case ArgumentException:
                    return new BadRequestResult();
                case WrongCredentialsException:
                    return new UnauthorizedResult();
                case UserNotFoundException:
                case DataNotFoundException:
                    return new NotFoundResult();
                case NotImplementedException:
                    return new StatusCodeResult(StatusCodes.Status501NotImplemented);
                case NullReferenceException:
                case Exception:
                default:
                    return new StatusCodeResult(500);
            }
        }
    }
}
