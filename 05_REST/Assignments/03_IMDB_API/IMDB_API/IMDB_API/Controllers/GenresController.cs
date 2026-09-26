using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace IMDB_Controllers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var genres = _genreService.Get();
            return Ok(new ApiResponse<IEnumerable<GenreResponse>>
            {
                Data = genres
            });
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var genre = _genreService.Get(id); 
            
            return Ok(new ApiResponse<GenreResponse>
            {
                Data = genre
            });
        }

        [HttpPost]
        public IActionResult Add(GenreRequest request)
        {
            var id = _genreService.Add(request);

            return CreatedAtAction(
                nameof(Get),
                new { id = id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, GenreRequest request)
        {
            _genreService.Update(id, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _genreService.Delete(id);
            return Ok();
        }
    }
}
