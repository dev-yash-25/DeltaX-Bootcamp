using IMDB_API.Models.Filters;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using System.Collections;
using System.Collections.Generic;

namespace IMDB_API.Services.Interfaces
{
    public interface IMovieService
    {
        IEnumerable<MovieResponse> Get(MovieFilter filter);
        MovieResponse Get(int id);

        int Add(MovieRequest request);
        void Update(int id, MovieRequest request);
        void Delete(int id);
    }
}
