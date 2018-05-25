﻿using System;
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
        public BotController()
        {
        }

        [HttpGet]
        public IActionResult GetTweet(int? id)
        {
            var option = _options.Get(new BotOption() {Id : id});

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
