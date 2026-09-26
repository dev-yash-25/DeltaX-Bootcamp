using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IMDB_Controllers.Controllers
{
    [Authorize]
    [Route("api/movies/{movieId}/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult Get(int movieId)
        {
            var reviews = _reviewService.Get(movieId);

            return Ok(new ApiResponse<IEnumerable<ReviewResponse>>
            {
                Data = reviews
            });
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int movieId, int id)
        {
            var review = _reviewService.Get(movieId, id);

            return Ok(new ApiResponse<ReviewResponse>
            {
                Data = review
            });
        }

        [HttpPost]
        public IActionResult Add(int movieId, ReviewRequest request)
        {
            var id = _reviewService.Add(movieId, request);
            return CreatedAtAction(
                nameof(Get),
                new { movieId = movieId, id = id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int movieId, int id, ReviewRequest request)
        {
            _reviewService.Update(movieId, id, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int movieId, int id)
        {
            _reviewService.Delete(movieId, id);
            return Ok();
        }
    }
}
