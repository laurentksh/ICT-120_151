using ICT_151.Models;
using ICT_151.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ICT_151.Authentication
{
    public static class HttpContextExtension
    {
        public async static Task<User> GetUser(this HttpContext httpContext)
        {
            var repository = httpContext.RequestServices.GetService(typeof(IUserRepository)) as IUserRepository;

            if (repository == null)
                throw new NullReferenceException("Couldn't get repository from Service Provider");

            if (httpContext.User.Identity.IsAuthenticated)
                return await repository.GetBaseUser(Guid.Parse(httpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value));
            else
                return null;
        }
    }
}
