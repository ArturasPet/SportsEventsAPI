using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsEventsAPI.Models;
using SportsEventsAPI.Services;

namespace SportsEventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly EventService _eventService;

        public EventsController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public ActionResult<List<Event>> Get() => _eventService.Get();

        [HttpGet("{id:length(24)}", Name = "GetEvent")]
        //[HttpGet("~/api/sports/{sportId}/[controller]/{id}")]
        public ActionResult<Event> Get(string id)
        {
            var @event = _eventService.Get(id);

            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        // POST: Events/Create
        [HttpPost]
        public ActionResult Create(Event @event)
        {
            _eventService.Create(@event);

            return CreatedAtRoute("GetEvent", new { id = @event.Id.ToString() }, @event);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Event eventIn)
        {
            var @event = _eventService.Get(id);

            if (@event == null)
            {
                return NotFound();
            }

            _eventService.Update(id, eventIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
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

        //// GET: Events
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Events/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// POST: Events/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Events/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Events/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //    // GET: Events/Delete/5
        //    public ActionResult Delete(int id)
        //    {
        //        return View();
        //    }

        //    // POST: Events/Delete/5
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Delete(int id, IFormCollection collection)
        //    {
        //        try
        //        {
        //            // TODO: Add delete logic here

        //            return RedirectToAction(nameof(Index));
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }
    }
}