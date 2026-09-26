using IMDB_API.Models.Filters;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace IMDB_Controllers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] MovieFilter filter)
        {
            var movies = _movieService.Get(filter);
            
            return Ok(new ApiResponse<IEnumerable<MovieResponse>>
            {
                Data = movies
            });
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var movie = _movieService.Get(id);

            return Ok(new ApiResponse<MovieResponse>
            {
                Data = movie
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody]MovieRequest request)
        {
            var id = _movieService.Add(request);
            return CreatedAtAction(
                nameof(Get),
                new { id = id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]MovieRequest request)
        {
            _movieService.Update(id, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _movieService.Delete(id);
            return Ok();
        }
    }
}


