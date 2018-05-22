using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterBot.Domain;

namespace TwitterBot.Api.Controllers
{
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        private Bot _bot;

        public BotController(Bot bot)
        {
            _bot = bot;
        }

        [HttpGet]
        public IActionResult GetTweet()
        {
            var tweet = GenerateTweet();
            return Ok(tweet.Text);
        }

        [HttpPost]
        public IActionResult SetProfiles([FromBody] List<TwitterProfile> profiles)
        {


            return Ok();
        }

        private Tweet GenerateTweet()
        {
            return new Tweet();
        }
    }
}
