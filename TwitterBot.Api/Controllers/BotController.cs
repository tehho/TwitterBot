using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterBot.Domain;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Api.Controllers
{
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        private IRepository<BotOption> _options;
        public BotController(IRepository<BotOption> options)
        {
            _options = options;
        }

        [HttpGet]
        public IActionResult GetBotOptions()
        {
            var list = _options.GetAll().ToList();

            list.ForEach(option => option.ProfileOccurances = null);

            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetTweet(int? id)
        {
            var option = _options.Get(new BotOption() {Id = id});

            if (option == null)
                return NotFound("BotOption");

            var bot = new Bot(option);

            var tweet = bot.GenerateTweet();

            return Ok(tweet.Text);
        }

        [HttpPost]
        public IActionResult SetProfiles([FromBody] BotOption option)
        {
            var optionReturn = _options.Add(option);

            if (optionReturn == null)
                return BadRequest();

            return Ok();
        }

        private Tweet GenerateTweet()
        {
            return new Tweet();
        }
    }
}
