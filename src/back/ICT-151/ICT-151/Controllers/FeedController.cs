﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICT_151.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        public FeedController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetMainFeed()
        {

            return Ok();
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserFeed([FromRoute] string username)
        {
            
            return Ok();
        }
    }
}
