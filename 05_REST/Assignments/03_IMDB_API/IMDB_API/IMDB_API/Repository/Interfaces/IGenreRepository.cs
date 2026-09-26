using IMDB_API.Models.Db;
using System.Collections;
using System.Collections.Generic;

namespace IMDB_API.Repository.Interfaces
{
    public interface IGenreRepository
    {        
        IEnumerable<Genre> Get();
        Genre Get(int id);
        IEnumerable<Genre> Get(IEnumerable<int> ids);

        void Add(Genre genre);
        void Update(int id, Genre genre);
        void Delete(int id);
    }
}
