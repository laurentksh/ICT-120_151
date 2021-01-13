using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICT_151.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage([FromRoute] Guid id)
        {

            return Ok();
        }

        [HttpGet("video/{id}")]
        public async Task<IActionResult> GetVideo([FromRoute] Guid id)
        {

            return Ok();
        }
    }
}
