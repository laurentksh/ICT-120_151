using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ICT_151.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private ILogger<MediaController> Logger;
        private IExceptionHandlerService ExceptionHandlerService;
        private IMediaService MediaService;

        public MediaController(ILogger<MediaController> logger, IExceptionHandlerService exceptionHandlerService)
        {
            Logger = logger;
            ExceptionHandlerService = exceptionHandlerService;
        }

        [HttpGet("default_pp")]
        public async Task<IActionResult> GetDefaultProfilePictureImage()
        {
            try {
                return Ok(/* TODO */);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedia([FromRoute] Guid id)
        {
            try {

                return Ok();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostMedia(IFormFile formFile)
        {
            try {

                return Ok();
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }
    }
}
