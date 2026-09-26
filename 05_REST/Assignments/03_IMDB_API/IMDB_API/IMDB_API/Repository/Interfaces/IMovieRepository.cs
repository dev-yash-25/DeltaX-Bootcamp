using IMDB_API.Models.Db;
using IMDB_API.Models.Filters;
using IMDBSample.Models.Db;
using System.Collections.Generic;

namespace IMDB_API.Repository.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> Get(MovieFilter filter);
        Movie Get(int id);

        void Add(Movie movie, List<int> actorIds, List<int> genreIds);
        void Update(int id, Movie movie, List<int> actorIds, List<int> genreIds);
        void Delete(int id);

        IEnumerable<int> GetActorIds(int movieId);
        IEnumerable<int> GetGenreIds(int movieId);
    }
}
