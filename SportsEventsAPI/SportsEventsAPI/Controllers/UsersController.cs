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
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly EventService _eventService;

        public UsersController(UserService userService, EventService eventService)
        {
            _userService = userService;
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult<List<User>> Get() => _userService.Get();

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        [HttpGet("~/api/sports/{sportId}/events/{eventId}/[controller]/{id}")]
        public ActionResult<User> Get(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        [HttpGet("~/api/sports/{sportId}/events/{eventId}/[controller]")]
        [HttpGet("~/api/events/{eventId}/users")]
        public ActionResult<List<User>> GetUsersByRoom(string eventId) => _userService.GetUsersByEvent(eventId);

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (user == null)
                return BadRequest();

            if (user.UserName == null || user.Password == null)
                return BadRequest(new { message = "Username or password not entered" });

            if (_userService.GetByUsername(user.UserName) != null)
                return BadRequest(new { message = "Username already taken" });

            _userService.Create(user);

            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }
        [HttpPost("~/api/sports/{sportId}/events/{eventId}/[controller]")]
        [HttpPost("~/api/events/{eventId}/users")]
        public ActionResult<User> Create(User user, string eventId)
        {
            if (user == null)
                return BadRequest();

            _userService.Create(user);

            Event tempEvent = _eventService.Get(eventId);
            tempEvent.ParticipantIds.Add(user.Id);
            _eventService.Update(eventId, tempEvent);

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id:length(24)}")]
        [HttpPut("~/api/sports/{sportId}/events/{eventId}/users/{id}")]
        public IActionResult Update(string id, User userIn)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            userIn.Id = id;
            _userService.Update(id, userIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        [HttpDelete("~/api/sports/{sportId}/events/{eventId}/[controller]/{id}")]
        public IActionResult Delete(string id)
        {
            var user = _userService.Get(id);

            if (user == null)
            {
                return NotFound();
            }
            _eventService.RemoveParticipant(user.Id);
            _userService.Remove(user.Id);

            return NoContent();
        }
    }
}