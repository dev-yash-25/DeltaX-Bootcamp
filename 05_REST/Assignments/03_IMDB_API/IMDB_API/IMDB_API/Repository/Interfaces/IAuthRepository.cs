using IMDB_API.Models.Db;

namespace IMDB_API.Repository.Interfaces
{
    public interface IAuthRepository
    {
        User Get(string email);
        void Create(User user);
    }
}
