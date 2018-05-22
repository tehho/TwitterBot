using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TwitterBot.Api.Model;
using TwitterBot.Domain;
using TwitterBot.Infrastructure.Repository;

namespace TwitterBot.Api.Controllers
{
    [Route("api/[controller]")]
    public class TwitterController : Controller
    {
        private readonly IRepository<TwitterProfile> _repository;

        public TwitterController(IRepository<TwitterProfile> repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public IActionResult GetExistingsProfiles()
        {
            return Ok(_repository.GetAll());
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

        private string GenerateTweet(IEnumerable<TwitterProfile> profiles)
        {
            return "Test";
        }
        
        [HttpPost]
        public IActionResult Post([FromBody]TwitterProfileApi profile)
        {
            if (profile == null)
                return BadRequest();

            var prolife = _repository.Add(profile);

            return Ok(prolife);
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
    }
}
