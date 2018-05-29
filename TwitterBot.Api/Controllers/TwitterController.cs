using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Core.Extensions;
using TwitterBot.Api.Model;
using TwitterBot.Domain;
using TwitterBot.Infrastructure;
using TwitterBot.Infrastructure.Logging;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Api.Controllers
{
    [Route("api/[controller]")]
    public class TwitterController : Controller
    {
        private readonly IRepository<TwitterProfile> _repository;
        private readonly TwitterService _twitterService;
        private readonly TwitterProfileTrainer _trainer;
        private readonly ILogger _logger;

        public TwitterController(IRepository<TwitterProfile> repository, TwitterService twitterService, TwitterProfileTrainer trainer, ILogger logger)
        {
            _repository = repository;
            _twitterService = twitterService;
            _trainer = trainer;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetExistingsProfiles()
        {
            _logger.Log("Accessing all profiles");
            try
            {
                var list = _repository.GetAll();

                list.ForEach(p => p.Words = null);

                _logger.Separator();
                return Ok(list);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]TwitterProfileApi profile)
        {
            _logger.Log("Adding profile to list");
            if (profile == null)
            {
                _logger.Error("No profile recieved");
                _logger.Separator();
                return BadRequest("Sum ting wong");
            }

            if (profile.Name == null)
            {
                _logger.Error("No profilename");
                _logger.Separator();
                return BadRequest("Name not given");
            }

            if (_twitterService.DoesTwitterUserExist(profile) == false)
            {
                _logger.Error("Profile doesn't match a twitter profile");
                _logger.Separator();
                return NotFound("Twitter user does not exist");
            }

            if (_twitterService.ProfileTimeLineHasTweets(profile) == false)
            {
                _logger.Error("Profile doesn't have any tweets");
                _logger.Separator();
                return BadRequest("Twitter user does not have any tweets.");
            }

            TwitterProfile prolife = null;

            try
            {
                prolife = _repository.Add(profile);
                _logger.Log($"Profile {profile.Name} added to database");

            }
            catch (Exception e)
            {
                if (prolife != null)
                    _repository.Remove(prolife);
                _logger.Error("Something went wrong adding to db");
                _logger.Error(e.Message);
                _logger.Separator();
                return BadRequest();
            }

            _logger.Separator();
            return Ok(prolife);
        }

        [HttpPost("trainwithtweet")]
        public IActionResult TrainProfileWithTweet([FromBody](TwitterProfileApi profile, TweetApi tweet) data)
        {
            if (data.profile == null)
                return BadRequest();

            if (data.tweet == null)
                return BadRequest();

            var profile = _repository.Get(data.profile);

            if (profile == null)
                return NotFound();

            profile = _trainer.Train(profile, data.tweet);

            if (profile == null)
                return StatusCode(500);

            _repository.Update(profile);

            return Ok();
        }

        [HttpPost("TrainData")]
        public IActionResult GetTrainData([FromBody]TwitterProfileApi apiprofile)
        {
            if (apiprofile == null)
                return BadRequest();

            if (apiprofile.Name == null)
                return BadRequest();

            var profile = _repository.Get(apiprofile);

            if (profile == null)
                return NotFound();

            var tweets = _twitterService.ListAllTweetsFromProfile(profile);

            if (tweets == null)
                return BadRequest();

            return Ok(tweets);
        }

        [HttpPost("train")]
        public IActionResult TrainProfile([FromBody] List<TwitterProfileApi> profiles)
        {
            if (profiles?.Count == 0)
                return BadRequest("Sum ting wong");
            try
            {

                var targetProfiles = _repository.SearchList((p => profiles.Any(profile => profile.Name == p.Name))).ToList();

                targetProfiles.ForEach(Train);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        private void Train(TwitterProfile profile)
        {
            var tweets = _twitterService.ListAllTweetsFromProfile(profile).ToList();

            tweets.ForEach(tweet =>
            {
                profile = _repository.Get(profile);

                profile = _trainer.Train(profile, tweet);

                _repository.Update(profile);
            });

        }

        [HttpDelete("handle")]
        public IActionResult Delete([FromBody] TwitterProfileApi profile)
        {
            if (profile == null)
            {
                return BadRequest("No profile was given");
            }

            if (!_repository.Exists(profile))
                return BadRequest($"Profile does not exist: {profile.Name}");
            
            var prolife = _repository.Remove(profile);

            if (prolife == null)
                return BadRequest();

            return Ok("Remove complete");
        }

        [HttpPost("GenerateTweet")]
        private Tweet GenerateTweet(BotOptions options)
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

        [HttpGet("TSV")]
        public IActionResult GetTweetsToTsv(string twitterUser, int numberOfTweets)
        {
            _twitterService.tweetCount = numberOfTweets;

            try
            {
                var tweets = _twitterService.ListAllTweetsFromProfile(new TwitterProfile {Name = twitterUser}).ToList();

                var tweetString = $"TwitterId\tCreatedAt\tText\tFavoriteCount\tRetweetCount{Environment.NewLine}";

                for (var index = 0; index < tweets.Count; index++)
                {
                    tweetString += tweets[index].TwitterId + "\t";
                    tweetString += $"{tweets[index].CreatedAt.ToShortDateString()} {tweets[index].CreatedAt.ToShortTimeString()}\t";
                    tweetString += tweets[index].Text + "\t";
                    tweetString += tweets[index].FavoriteCount + "\t";
                    tweetString += tweets[index].RetweetCount;

                    if (index != tweets.Count - 1)
                        tweetString += Environment.NewLine;
                }

                return Ok(tweetString);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}