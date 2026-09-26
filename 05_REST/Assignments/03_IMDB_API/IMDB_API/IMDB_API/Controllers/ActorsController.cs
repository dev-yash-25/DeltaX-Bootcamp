using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace IMDB_Controllers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var actors = _actorService.Get();

            return Ok(new ApiResponse<IEnumerable<ActorResponse>>
            {
                Data = actors
            });
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var actor = _actorService.Get(id);

            return Ok(new ApiResponse<ActorResponse>
            {
                Data = actor
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody]ActorRequest request)
        {
            int id = _actorService.Add(request);

            return CreatedAtAction(
                nameof(Get),
                new { id });
        }

        [HttpPut("{id:int}")]   
        public IActionResult Update(int id, [FromBody]ActorRequest request)
        {
            _actorService.Update(id, request);

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _actorService.Delete(id);
            return Ok();
        }
    }
}
