using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Authentication;
using ICT_151.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ICT_151.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private ILogger<FeedController> Logger { get; set; }
        private readonly IFeedService FeedService;
        private readonly IExceptionHandlerService ExceptionHandlerService;

        public FeedController(ILogger<FeedController> logger, IFeedService feedService, IExceptionHandlerService exceptionHandlerService)
        {
            Logger = logger;
            FeedService = feedService;
            ExceptionHandlerService = exceptionHandlerService;
        }

        /// <summary>
        /// Get public publications.
        /// </summary>
        /// <param name="amount">Amount of publications to return.</param>
        /// <param name="positionId">Can be null, where to begin returning publications from.</param>
        /// <returns>List&lt;<see cref="Models.Dto.PublicationViewModel"/>&gt;</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.Dto.PublicationViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMainFeed([FromQuery] int amount, [FromQuery] Guid? positionId)
        {
            try {
                var user = await HttpContext.GetUser();
                var result = await FeedService.GetMainFeed(amount, positionId, user?.Id);

                return Ok(result);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }

        }

        /// <summary>
        /// Get the publications of a user.
        /// </summary>
        /// <param name="identifier">Can be either a User Id (GUID) or a username.</param>
        /// <param name="amount">Amount of publications to return.</param>
        /// <param name="positionId">Can be null, where to begin returning publications from.</param>
        /// <returns>List&lt;<see cref="Models.Dto.PublicationViewModel"/>&gt;</returns>
        [HttpGet("{identifier}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Models.Dto.PublicationViewModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserFeed([FromRoute] string identifier, [FromQuery] int amount, [FromQuery] Guid? positionId)
        {
            try {
                var user = await HttpContext.GetUser();
                List<Models.Dto.PublicationViewModel> result = null;

                if (Guid.TryParse(identifier, out Guid id)) {
                    result = await FeedService.GetFeed(id, amount, positionId, user?.Id);
                } else {
                    result = await FeedService.GetFeed(identifier, amount, positionId, user?.Id);
                }

                return Ok(result);
            } catch (Exception ex) {
                Logger.LogWarning(ex, "An error occured: " + ex.Message ?? "undefined");
                return ExceptionHandlerService.Handle(ex, Request);
            }
        }
    }
}