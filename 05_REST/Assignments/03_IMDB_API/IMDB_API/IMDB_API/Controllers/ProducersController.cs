using IMDB_API.Models.Db;
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
    public class ProducersController : ControllerBase
    {
        private readonly IProducerService _producerService;
        public ProducersController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var producers = _producerService.Get();

            return Ok(new ApiResponse<IEnumerable<ProducerResponse>>
            {
                Data = producers
            });
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var producer = _producerService.Get(id);

            return Ok(new ApiResponse<ProducerResponse>
            {
                Data = producer
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody]ProducerRequest request)
        {
            var id = _producerService.Add(request);
            return CreatedAtAction(
                nameof(Get),
                new { id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody]ProducerRequest request)
        {
            _producerService.Update(id, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _producerService.Delete(id);
            return Ok();
        }
    }
}
