using IMDB_API.Models.Db;
using IMDB_API.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IMDB_API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly List<User> _users = new List<User> ();
        private int _id = 1;
        public void Create(User user)
        {
            user.Id = _id++;
            _users.Add(user);
        }

        public User Get(string email)
        {
            return _users.SingleOrDefault(u => u.Email == email);
        }
    }
}
