using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterBot.Api.Model;
using TwitterBot.Domain;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Api.Controllers
{
    [Route("api/[controller]")]
    public class BotController : Controller
    {
        private IRepository<BotOption> _options;
        private IRepository<TwitterProfile> _profiles;
        public BotController(IRepository<BotOption> options, IRepository<TwitterProfile> profiles)
        {
            _options = options;
            _profiles = profiles;
        }

        [HttpGet]
        public IActionResult GetBotOptions()
        {
            var list = _options.GetAll().ToList();

            list.ForEach(option => option.ProfileOccurances = new List<ProfileOccurrance>());

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

            return Ok(tweet);
        }

        [HttpPost]
        public IActionResult SetProfiles([FromBody] BotOptionApi option)
        {
            if (option == null)
                return BadRequest();

            var tempOption = _options.Add(option);

            option.Profiles.ForEach(profile =>
            {
                var p = _profiles.Get(profile);

                if (p != null)
                    tempOption.AddProfile(p);
            });

            _options.Update(tempOption);

            return Ok();
        }

        private Tweet GenerateTweet()
        {
            return new Tweet();
        }
    }
}
