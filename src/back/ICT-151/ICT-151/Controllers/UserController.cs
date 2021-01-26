using ICT_151.Authentication;
using ICT_151.Data;
using ICT_151.Models.Dto;
using ICT_151.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private ILogger<UserController> Logger;
        private IUserService UserService;
        private IExceptionHandlerService ExceptionHandlerService;

        public UserController(ILogger<UserController> logger, IUserService userService, IExceptionHandlerService exceptionHandlerService)
        {
            Logger = logger;
            UserService = userService;
            ExceptionHandlerService = exceptionHandlerService;
        }

        [HttpGet("get/{identifier}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSummaryViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUser([FromRoute] string identifier)
        {
            try {
                UserSummaryViewModel result = null;

                if (Guid.TryParse(identifier, out Guid id)) {
                    result = await UserService.GetUser(id);
                } else {
                    result = await UserService.GetUser(identifier);
                }
                
                return Ok(result);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
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
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpGet("sessions")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserSessionSummaryViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSessions()
        {
            try {
                var user = await HttpContext.GetUser();
                var result = await UserService.GetSessions(user.Id);

                return Ok(result);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpDelete("sessions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ClearSessions([FromQuery] bool allSessions = true)
        {
            try {
                var user = await HttpContext.GetUser();

                if (allSessions)
                    await UserService.ClearSessions(user.Id);
                else
                    await UserService.ClearSessions(user.Id, HttpContext.Connection.RemoteIpAddress);

                return Ok();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpDelete("sessions/{sessionId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ClearSession([FromRoute] string sessionId)
        {
            try {
                var user = await HttpContext.GetUser();
                await UserService.ClearSession(user.Id, Guid.Parse(sessionId));

                return Ok();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("new")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNew([FromBody] CreateUserDto dto)
        {
            try {
                var result = await UserService.CreateNew(dto, HttpContext.Connection.RemoteIpAddress);

                return Created($"/api/User/{result.User.Username}", result);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
        {
            try {
                var user = await HttpContext.GetUser();
                var result = await UserService.Update(user.Id, dto, HttpContext.Connection.RemoteIpAddress);

                return Ok(result);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpDelete("{userId}")]
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
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("{userId}/follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Follow(Guid userId)
        {
            try {
                var user = await HttpContext.GetUser();
                await UserService.Follow(user.Id, userId);

                return Ok();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpDelete("{userId}/follow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnFollow(Guid userId)
        {
            try {
                var user = await HttpContext.GetUser();
                await UserService.UnFollow(user.Id, userId);

                return Ok();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("{userId}/block")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Block(Guid userId)
        {
            try {
                var user = await HttpContext.GetUser();
                await UserService.Block(user.Id, userId);

                return Ok();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpDelete("{userId}/block")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnBlock(Guid userId)
        {
            try {
                var user = await HttpContext.GetUser();
                await UserService.UnBlock(user.Id, userId);

                return Ok();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }
    }
}
