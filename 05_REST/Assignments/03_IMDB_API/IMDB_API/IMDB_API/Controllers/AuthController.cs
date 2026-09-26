using IMDB_API.Models.Authentication.Requests;
using IMDB_API.Models.Authentication.Responses;
using IMDB_API.Models.Responses;
using IMDB_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMDB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody]SignupRequest request)
        {
            _authService.SignUp(request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            var response = _authService.Login(request);

            return Ok(new ApiResponse<AuthResponse>
            {
                Data = response
            });
        }
    }
}
