using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Authentication;
using ICT_151.Models;
using ICT_151.Models.Dto;
using ICT_151.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ICT_151.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ILogger<MediaController> Logger;
        private readonly IExceptionHandlerService ExceptionHandlerService;
        private readonly IMediaService MediaService;

        public MediaController(ILogger<MediaController> logger, IExceptionHandlerService exceptionHandlerService, IMediaService mediaService)
        {
            Logger = logger;
            ExceptionHandlerService = exceptionHandlerService;
            MediaService = mediaService;
        }

        [HttpGet("default_pp")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDefaultProfilePictureImage()
        {
            try {
                return Ok(await MediaService.GetDefaultProfilePicture());
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMedia([FromRoute] Guid id)
        {
            try {
                var user = await HttpContext.GetUser();

                var result = await MediaService.GetMedia(user?.Id, id);
                return Ok(result);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MediaViewModel))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> UploadMedia([FromForm] CreateMediaDTO mediaDto)
        {
            try {
                var user = await HttpContext.GetUser();

                var result = await MediaService.UploadMedia(user.Id, mediaDto);
                return CreatedAtAction(nameof(GetMedia), new { id = result.Id }, result);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }
    }
}
