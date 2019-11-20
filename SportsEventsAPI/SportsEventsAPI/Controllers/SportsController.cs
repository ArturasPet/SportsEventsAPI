using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsEventsAPI.Models;
using SportsEventsAPI.Services;

namespace SportsEventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportsController : ControllerBase
    {
        private readonly SportService _sportService;
        private readonly EventService _eventService;

        public SportsController(SportService sportService, EventService eventService)
        {
            _sportService = sportService;
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult<List<Sport>> Get() => _sportService.Get();

        [HttpGet("{id:length(24)}", Name = "GetSport")]
        //[HttpGet("~/api/sports/{sportId}/[controller]/{id}")]
        public ActionResult<Sport> Get(string id)
        {
            var sport = _sportService.Get(id);

            if (sport == null)
            {
                return NotFound();
            }

            return sport;
        }

        // POST: Sports/Create
        [HttpPost]
        [HttpPost("~/api/sports/{sportId}/[controller]")]
        public ActionResult Create(Sport sport)
        {
            // Check input
            if (sport == null)
                return BadRequest();

            _sportService.Create(sport);

            return CreatedAtRoute("GetSport", new { id = sport.Id.ToString() }, sport);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Sport sportIn)
        {
            var sport = _sportService.Get(id);

            if (sport == null)
            {
                return NotFound();
            }

            if (sportIn == null)
                return BadRequest();
            else
                sportIn.Id = id;

            _sportService.Update(id, sportIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var sport = _sportService.Get(id);

            if (sport == null)
            {
                return NotFound();
            }


            
            _sportService.Remove(sport.Id);

            return NoContent();
        }
    }
}