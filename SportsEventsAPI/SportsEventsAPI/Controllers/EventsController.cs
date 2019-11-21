using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SportsEventsAPI.Models;
using SportsEventsAPI.Services;

namespace SportsEventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult<List<Event>> Get() => _eventService.Get();

        [HttpGet("{id:length(24)}", Name = "GetEvent")]
        [HttpGet("~/api/sports/{sportId}/[controller]/{id}")]
        public ActionResult<Event> Get(string id)
        {
            var @event = _eventService.Get(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        [HttpGet("~/api/sports/{sportId}/[controller]")]
        public ActionResult<List<Event>> GetEventsBySport(string sportId) => 
            _eventService.GetEventsBySport(sportId);

        // POST: Events/Create
        [HttpPost]
        [HttpPost("~/api/sports/{sportId}/[controller]")]
        public ActionResult Create(Event @event, string sportId)
        {
            if (@event == null)
                return BadRequest();

            @event.SportId = sportId;
            @event.ParticipantIds = new List<string>();

            _eventService.Create(@event);

            return CreatedAtRoute("GetEvent", new { id = @event.Id.ToString() }, @event);
        }

        [HttpPut("{id:length(24)}")]
        [HttpPut("~/api/sports/{sportId}/[controller]/{id}")]
        public IActionResult Update(string id, Event eventIn)
        {
            var @event = _eventService.Get(id);

            if (@event == null)
            {
                return NotFound();
            }

            if (eventIn == null)
                return BadRequest();
            else
                eventIn.Id = id;

            _eventService.Update(id, eventIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [HttpDelete("~/api/sports/{sportId}/[controller]/{id}")]
        public IActionResult Delete(string id)
        {
            var @event = _eventService.Get(id);

            if (@event == null)
            {
                return NotFound();
            }

            _eventService.Remove(@event.Id);

            return NoContent();
        }
    }
}