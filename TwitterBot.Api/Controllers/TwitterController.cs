using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterBot.Api.Model;

namespace TwitterBot.Api.Controllers
{
    [Route("api/[controller]")]
    public class TwitterController : Controller
    {

        [HttpGet]
        public IActionResult GetExistingsProfiles()
        {
            return Ok(new [] { "value1", "value2" });
        }
        
        [HttpPost("tweet")]
        public IActionResult GetTweet([FromBody]List<TwitterProfileApi> profiles)
        {
            if (profiles == null || profiles.Count == 0)
                return Ok("Using full list, Not Implimented Exception lol");

            return Ok("Using specificc list, Needs bot to be able to work");
        }
        
        [HttpPost]
        public IActionResult Post([FromBody]TwitterProfileApi profile)
        {
            if (profile == null)
                return BadRequest();

            return Ok();
        }
        
        [HttpDelete("handle")]
        public IActionResult Delete([FromBody]List<TwitterProfileApi> profiles)
        {
            return Ok("test");
        }
    }
}
