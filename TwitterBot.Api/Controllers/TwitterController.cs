using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Core.Extensions;
using TwitterBot.Api.Model;
using TwitterBot.Domain;
using TwitterBot.Infrastructure;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Api.Controllers
{
    [Route("api/[controller]")]
    public class TwitterController : Controller
    {
        private readonly IRepository<TwitterProfile> _repository;
        private readonly TwitterService _twitterService;
        private readonly TwitterProfileTrainer _trainer;

        public TwitterController(IRepository<TwitterProfile> repository, TwitterService twitterService, TwitterProfileTrainer trainer)
        {
            _repository = repository;
            _twitterService = twitterService;
            _trainer = trainer;
        }

        [HttpGet]
        public IActionResult GetExistingsProfiles()
        {
            var list = _repository.GetAll();

            list.ForEach(p => p.Words = null);

            return Ok(list);
        }

        [HttpPost("tweet")]
        public IActionResult GetTweet([FromBody] List<TwitterProfileApi> profiles)
        {
            List<TwitterProfile> list = null;
            if (profiles == null || profiles.Count == 0)
                list = _repository.GetAll().ToList();
            else
                list = _repository.SearchList(profile =>
                {
                    foreach (var p in profiles)
                    {
                        if (p.Name == profile.Name)
                            return true;
                    }

                    return false;
                }).ToList();

            return Ok(GenerateTweet(list));
        }

        [HttpPost]
        public IActionResult Post([FromBody] TwitterProfileApi profile)
        {
            if (profile == null)
                return BadRequest("Sum ting wong");

            if (profile.Name == null)
                return BadRequest("Name not given");

            var prolife = _repository.Add(profile);

            return Ok(prolife);
        }

        [HttpPost("train")]
        public IActionResult TrainProfile([FromBody] List<TwitterProfileApi> profile)
        {
            if (profile?.Count == 0)
                return BadRequest("Sum ting wong");

            profile.Select(p => (TwitterProfile)p).ForEach(Train);

            return Ok();
        }

        private void Train(TwitterProfile profile)
        {
            var tweets = _twitterService.ListAllTweetsFromProfile(profile).ToList();

            tweets.ForEach(tweet => _trainer.Train(profile, tweet));
        }

        [HttpDelete("handle")]
        public IActionResult Delete([FromBody] List<TwitterProfileApi> profiles)
        {
            if (profiles == null || profiles.Count == 0)
            {
                return BadRequest("No profile was given");
            }

            foreach (var profile in profiles)
            {
                if (!_repository.Exists(profile))
                    return BadRequest($"Profile does not exist: {profile.Name}");
            }

            profiles.ForEach(profile => _repository.Remove(profile));

            return Ok("Remove complete");
        }

        private string GenerateTweet(IEnumerable<TwitterProfile> profiles)
        {
            var bot = new Bot("test");

            profiles.ForEach(bot.AddProfile);

            return bot.GenerateRandomTweetText();
        }
    }
}