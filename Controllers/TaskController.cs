using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Task.Interfaces;
using Task.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Task.Services;
using Microsoft.Net.Http.Headers;
// using Task.Interfaces;


namespace Task.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        ITaskService TaskService;
        public TaskController(ITaskService TaskService)
        {
            this.TaskService = TaskService;
        }
        [HttpGet]
     //  [Authorize(Policy = "User")]
        public ActionResult<List<task>> Get()
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            int id=TokenService.ReadToken(token);
            return TaskService.GetAll(id);
        }

        [HttpGet("{id}")]
        public ActionResult<task> Get(int id)
        {
        
            var myTask = TaskService.Get(id);

            if (myTask == null)
                return NotFound();

            return myTask;
        }
        [HttpPost]
        public IActionResult Create(task myTask)
        {
            TaskService.Add(myTask);
            return CreatedAtAction(nameof(Create), new { id = myTask.Id }, myTask);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var myTask = TaskService.Get(id);
            if (myTask is null)
                return NotFound();

            TaskService.Delete(id);

            return Content(TaskService.Count.ToString());
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, task myTask)
        {
            if (id != myTask.IdTask)
                return BadRequest();

            var existingtask = TaskService.Get(id);
            if (existingtask is null)
                return NotFound();

            TaskService.Update(myTask);

            return NoContent();
        }


    }
}