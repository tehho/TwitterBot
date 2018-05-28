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
        public IActionResult GetTweet([FromQuery] List<TwitterProfileApi> profiles)
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

            //TODO GenerateTweet()

            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody]TwitterProfileApi profile) 
        {
            if (profile == null)
                return BadRequest("Sum ting wong");

            if (profile.Name == null)
                return BadRequest("Name not given");

            if (_twitterService.DoesTwitterUserExist(profile) == false)
            {
                return BadRequest("Twitter user does not exist");
            }

            if (_twitterService.ProfileTimeLineHasTweets(profile) == false)
            {
                return BadRequest("Twitter user does not have any tweets.");
            }

            TwitterProfile prolife;

            _twitterService.UpdateProfileImage(_twitterService.SaveProfileImageToServer(profile));

            try
            {
                prolife = _repository.Add(profile);

                Train(prolife);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(prolife);
        }

        [HttpPost("train")]
        public IActionResult TrainProfile([FromBody] List<TwitterProfileApi> profiles)
        {
            if (profiles?.Count == 0)
                return BadRequest("Sum ting wong");
            
            var targetProfiles = _repository.SearchList((p => profiles.Any(profile => profile.Name == p.Name ))).ToList();

            targetProfiles.ForEach(Train);

            return Ok();    
        }

        private void Train(TwitterProfile profile)
        {
            var tweets = _twitterService.ListAllTweetsFromProfile(profile).ToList();

            tweets.ForEach(tweet => profile = _trainer.Train(profile, tweet));

            _repository.Update(profile);
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

        [HttpPost("GenerateTweet")]
        private Tweet GenerateTweet(BotOption options)
        {
            var bot = new Bot(options);

            return bot.GenerateTweet();
        }

        [HttpPost("PostToTwitter")]
        public IActionResult PostToTwitter([FromBody] Tweet tweet)
        {
            if (tweet == null)
                return BadRequest("Sum ting wong");

            if (tweet.Text == null)
                return BadRequest("No body in tweet");

            if (!_twitterService.PublishTweet(tweet))
            {
                return StatusCode(500);
            }

            return Ok();
        }
    }
}