using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tweetinvi.Core.Extensions;
using TwitterBot.Api.Model;
using TwitterBot.Domain;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Api.Controllers
{
    [Route("api/[controller]")]
    public class TwitterController : Controller
    {
        private readonly IRepository<TwitterProfile> _repository;
        private readonly TwitterService twitterService;


        public TwitterController(IRepository<TwitterProfile> repository, TwitterService twitterService)
        {
            _repository = repository;
            this.twitterService = twitterService;
        }
        
        [HttpGet]
        public IActionResult GetExistingsProfiles() 
        {
            var list = _repository.GetAll();

            return Ok(list);
        }
        
        [HttpPost("tweet")]
        public IActionResult GetTweet([FromBody]List<TwitterProfileApi> profiles)
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
        public IActionResult Post([FromBody]TwitterProfileApi profile) // TODO: Validera att namnet finns/är en giltig profil
        {
            if (profile == null)
                return BadRequest("Sum ting wong");

            if (profile.Name == null)
                return BadRequest("Name not given");

            var tweetService = new TwitterService(null
                , new Token
                {
                    Key = "GjMrzt4a9YJqKXRTNKjLN2CVi",
                    Secret = "w3koS8pDXMxDscBZnT7VFgGFeoNgv0qxgUa5YYcvrv2WoysfRD"
                },
                new Token()
                {
                    Key = "998554298735845382-cHyJyzufzzSUzceD79y8zb0IkbfrPxi",
                    Secret = "B72OlpxIme0yz3ZHRVw0mCMDxKukXTcNuOvhD9d0ySCX8"
                });

            if (tweetService.DoesTwitterUserExist(profile) == false)
            {
                return BadRequest("Twitter user does not exist");
            }

            if (tweetService.ProfileTimeLineHasTweets(profile) == false)
            {
                return BadRequest("Twitter user does not have any tweets.");
            }


            var prolife = _repository.Add(profile);

            TrainAsync(prolife);

            return Ok(prolife);
        }

        private async Task TrainAsync(TwitterProfile profile)
        {
            await Task.Run(() =>
            {
                var tweets = twitterService.ListAllTweetsFromProfile(profile).ToList();

                tweets.ForEach(profile.TrainFromText);
            });
        }

        [HttpDelete("handle")]
        public IActionResult Delete([FromBody]List<TwitterProfileApi> profiles)
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
