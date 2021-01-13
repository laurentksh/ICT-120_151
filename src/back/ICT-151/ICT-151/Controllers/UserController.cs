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

        [HttpGet("{username}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSummaryViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromRoute] string username)
        {
            try
            {

                return Ok();
            } catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex);
            }
        }

        [HttpPost("auth")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.UserSessionViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authenticate([FromBody] AuthUserDto authBody)
        {
            try {
                var result = await UserService.AuthenticateUser(authBody, HttpContext.Connection.RemoteIpAddress);

                return Ok(result);
            } catch (Exception ex) {
                return ExceptionHandlerService.Handle(ex);
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

                return Created($"/api/User/{result.Username}", null);
            } catch (Exception ex) {
                return ExceptionHandlerService.Handle(ex);
            }
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(Guid userId)
        {
            try {
                await UserService.Delete(userId);

                return Ok();
            } catch (Exception ex) {
                return ExceptionHandlerService.Handle(ex);
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
                return ExceptionHandlerService.Handle(ex);
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
                return ExceptionHandlerService.Handle(ex);
            }
        }
    }
}
