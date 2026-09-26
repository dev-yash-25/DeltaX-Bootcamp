using System.ComponentModel.DataAnnotations;

using IMDB_API.Helpers;
using IMDB_API.Models.Authentication.Requests;
using IMDB_API.Models.Authentication.Responses;
using IMDB_API.Models.Db;
using IMDB_API.Repository.Interfaces;
using IMDB_API.Services.Interfaces;
using IMDB_API.Validators;
using Microsoft.AspNetCore.Identity;

namespace IMDB_API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IAuthRepository authRepository, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }

        public void SignUp(SignupRequest request)
        {
            ValidationHelper.ValidateNull(request, "Signup Request");

            request.Name = request.Name?.Trim();
            request.EmailId = request.EmailId?.Trim();

            UserValidator.ValidateSignup(request);

            var existingUser = _authRepository.Get(request.EmailId);

            if (existingUser != null)
            {
                throw new ValidationException("Email Id already exists.");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.EmailId,
                Password = request.Password
            };

            _authRepository.Create(user);
        }

        public AuthResponse Login(LoginRequest request)
        {
            ValidationHelper.ValidateNull(request, "Login Request");
         
            request.EmailId = request.EmailId.Trim();

            UserValidator.ValidateLogin(request);

            var user = _authRepository.Get(request.EmailId);

            if (user == null || user.Password != request.Password)
            {
                throw new ValidationException("Invalid credentials.");
            }

            var token = _jwtService.GenerateToken(user);

            return new AuthResponse
            {
                AccessToken = token
            };
        }
    }
}