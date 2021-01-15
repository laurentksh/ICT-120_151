using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Authentication;
using ICT_151.Models.Dto;
using ICT_151.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICT_151.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private IPublicationService PublicationService;
        private IExceptionHandlerService ExceptionHandlerService;

        public PublicationController(IPublicationService publicationService, IExceptionHandlerService exceptionHandlerService)
        {
            PublicationService = publicationService;
            ExceptionHandlerService = exceptionHandlerService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PublicationViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPublication([FromRoute] Guid id)
        {
            try
            {
                var user = await HttpContext.GetUser();
                var result = await PublicationService.GetPublication(id, user?.Id);

                return Ok(result);
            } catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpGet("{id}/replies")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PublicationViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReplies([FromRoute] Guid id)
        {
            try
            {
                var user = await HttpContext.GetUser();
                var result = await PublicationService.GetReplies(id, user?.Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpGet("{id}/reposts")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RepostViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReposts([FromRoute] Guid id)
        {
            try
            {
                var result = await PublicationService.GetReposts(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpGet("{id}/likes")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LikeViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLikes([FromRoute] Guid id)
        {
            try
            {
                var result = await PublicationService.GetLikes(id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateNew([FromBody] PublicationCreateDto dto)
        {
            try
            {
                var user = await HttpContext.GetUser();
                var result = await PublicationService.CreateNew(user.Id, dto);

                return Created($"/api/Publication/{result.Id}", result);
            } catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("{id}/retweet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Retweet([FromRoute] Guid id)
        {
            try
            {
                var user = await HttpContext.GetUser();
                await PublicationService.Repost(user.Id, id);

                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("{id}/like")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Like([FromRoute] Guid id)
        {
            try
            {
                var user = await HttpContext.GetUser();
                await PublicationService.Like(user.Id, id);

                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpDelete("{id}/retweet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnRetweet([FromRoute] Guid id)
        {
            try
            {
                var user = await HttpContext.GetUser();
                await PublicationService.UnRepost(user.Id, id);

                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpDelete("{id}/like")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UnLike([FromRoute] Guid id)
        {
            try
            {
                var user = await HttpContext.GetUser();
                await PublicationService.UnLike(user.Id, id);

                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }
    }
}
