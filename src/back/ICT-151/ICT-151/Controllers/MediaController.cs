using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICT_151.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private IExceptionHandlerService ExceptionHandlerService;

        public MediaController(IExceptionHandlerService exceptionHandlerService)
        {
            ExceptionHandlerService = exceptionHandlerService;
        }

        [HttpGet("image/default_pp")]
        public async Task<IActionResult> GetDefaultProfilePictureImage()
        {
            try
            {
                return Ok(/* TODO */);
            } catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage([FromRoute] Guid id)
        {
            try
            {

                return Ok();
            } catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("image")]
        public async Task<IActionResult> PostImage(IFormFile formFile)
        {
            
            return Ok();
        }

        [HttpGet("video/{id}")]
        public async Task<IActionResult> GetVideo([FromRoute] Guid id)
        {
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }

        [HttpPost("video")]
        public async Task<IActionResult> PostVideo(IFormFile formFile)
        {

            return Ok();
        }
    }
}
