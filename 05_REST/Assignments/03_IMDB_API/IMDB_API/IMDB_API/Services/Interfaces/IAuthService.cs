using IMDB_API.Models.Authentication.Requests;
using IMDB_API.Models.Authentication.Responses;

namespace IMDB_API.Services.Interfaces
{
    public interface IAuthService
    {
        void SignUp(SignupRequest request);
        AuthResponse Login(LoginRequest request);
    }
}
