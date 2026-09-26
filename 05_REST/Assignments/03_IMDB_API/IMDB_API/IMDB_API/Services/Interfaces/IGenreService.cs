using IMDB_API.Models.Db;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using System.Collections.Generic;

namespace IMDB_API.Services.Interfaces
{
    public interface IGenreService
    {
        IEnumerable<GenreResponse> Get();
        GenreResponse Get(int id);
        IEnumerable<GenreResponse> Get(IEnumerable<int> ids);
        
        int Add(GenreRequest request);
        void Update(int id, GenreRequest request);
        void Delete(int id);
    }
}