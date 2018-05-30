using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterBot.Domain;
using TwitterBot.Infrastructure;

namespace TwitterBot.Api.Controllers
{

    [Route("api/[controller]")]
    public class HearthbeatController : Controller
    {
        private readonly TwitterContext _dbMananger;
        private readonly TwitterService _twitterService;
        public HearthbeatController(TwitterContext dbMananger, TwitterService twitterService)
        {
            _dbMananger = dbMananger;
            _twitterService = twitterService;
        }

        [HttpGet("Database")]
        public IActionResult CheckDatabaseConnection()
        {
            if (_dbMananger == null)
                return NotFound();

            try
            {
                _dbMananger.Database.OpenConnection();
                _dbMananger.Database.CloseConnection();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }

            return Ok("Databaseconnection is ok");
        }

        [HttpGet("twitter")]
        public IActionResult CheckTwitterIsUp()
        {
            var exc = _twitterService.IsTwitterUp();
            if (exc == null)
            {
                return Ok("Twitter is up");
            }
            else
            {
                return NotFound("Twitter is down");
            }
        }

        [HttpPost("TwitterHandle")]
        public IActionResult CheckTwitterHandel([FromBody]string handle)
        {
            if (handle == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(handle))
                return BadRequest();

            if (_twitterService.DoesTwitterUserExist(new TwitterProfile(handle)))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
