﻿using System;
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
        private readonly IRepository<BotOptions> _options;
        private readonly IRepository<TwitterProfile> _profiles;
        public BotController(IRepository<BotOptions> options, IRepository<TwitterProfile> profiles)
        {
            _options = options;
            _profiles = profiles;
        }

        [HttpGet]
        public IActionResult GetBotOptions()
        {
            var list = _options.GetAll().ToList();

            list.ForEach(option => option.ProfileOccurances.ForEach(occ =>
            {
                occ.BotOptions = null;
                occ.Profile.Words = null;
            }));

            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetTweet(Guid? id)
        {
            var option = _options.Get(new BotOptions() {Id = id});

            if (option == null)
                return NotFound("BotOptions");

            var bot = new Bot(option);

            var tweet = bot.GenerateTweet();

            return Ok(tweet);
        }

        [HttpPost]
        public IActionResult SetProfiles([FromBody] BotOptionApi option)
        {
            if (option == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(option.Name))
                return BadRequest();

            var tempOption = _options.Add(option);

            option.Profiles.ForEach(profile =>
            {
                if (profile.Occurrence == null)
                    profile.Occurrence = 1;

                if (profile.Occurrence > 0)
                {
                    var p = _profiles.Get(profile);

                    if (p != null)
                        tempOption.AddProfile(p, profile.Occurrence.Value);
                }
            });

            _options.Update(tempOption);

            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteBot([FromBody] BotOptionApi option)
        {
            if (option == null)
                return BadRequest();

            if (option.Id == null)
                return BadRequest();

            var dbOption = _options.Get(new BotOptions() {Id = option.Id});

            if (dbOption == null)
                return NotFound();

            try
            {
                _options.Remove(dbOption);
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}
