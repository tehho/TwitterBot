using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterBot.Api.Model;
using TwitterBot.Domain;
using TwitterBot.Infrastructure;

namespace TwitterBot.Api.Controllers
{

    [Route("api/[controller]")]
    public class HeartbeatController : Controller
    {
        private readonly TwitterContext _dbMananger;
        private readonly TwitterService _twitterService;

        public HeartbeatController(TwitterContext dbMananger, TwitterService twitterService)
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
        public async Task<IActionResult> CheckTwitterIsUp()
        {
            var exc = await _twitterService.IsTwitterUp();
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
        public async Task<IActionResult> CheckTwitterHandle([FromBody]TwitterProfileApi handle)
        {
            if (handle == null)
                return BadRequest("No handle given");

            if (string.IsNullOrWhiteSpace(handle.Name))
                return BadRequest("Handle is empty");

            var result = (await _twitterService.DoesTwitterUserExist(handle));

            if (result)
            {
                return Ok("Twitterhandle found");
            }
            else
            {
                return NotFound($"Twitterhandle not found");
            }
        }
    }
}
