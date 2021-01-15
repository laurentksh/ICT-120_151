using ICT_151.Authentication;
using ICT_151.Data;
using ICT_151.Models.Dto;
using ICT_151.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService UserService;
        private IExceptionHandlerService ExceptionHandlerService;

        public UserController(IUserService userService, IExceptionHandlerService exceptionHandlerService)
        {
            UserService = userService;
            ExceptionHandlerService = exceptionHandlerService;
        }

        [HttpGet("{identifier}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSummaryViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromRoute] string identifier)
        {
            try
            {
                UserSummaryViewModel result = null;

                if (Guid.TryParse(identifier, out Guid id)) {
                    result = await UserService.GetUser(id);
                } else {
                    result = await UserService.GetUser(identifier);
                }

                return Ok(result);
            } catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSessionViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate([FromBody] AuthUserDto authBody)
        {
            try {
                var result = await UserService.AuthenticateUser(authBody, HttpContext.Connection.RemoteIpAddress);

                return Ok(result);
            } catch (Exception ex) {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("new")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNew(CreateUserDto dto)
        {
            try {
                var result = await UserService.CreateNew(dto);

                return Created($"/api/User/{result.Username}", result);
            } catch (Exception ex) {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete(Guid userId)
        {
            try {
                var user = await HttpContext.GetUser();

                await UserService.Delete(user.Id, userId);

                return Ok();
            } catch (Exception ex) {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Follow(Guid userId, Guid toFollowUserId)
        {
            try {
                await UserService.Follow(userId, toFollowUserId);

                return Ok();
            } catch (Exception ex) {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("unfollow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnFollow(Guid userId, Guid toUnFollowUserId)
        {
            try {
                await UserService.UnFollow(userId, toUnFollowUserId);

                return Ok();
            } catch (Exception ex) {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }
    }
}
