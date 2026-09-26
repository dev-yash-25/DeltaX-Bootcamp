using IMDB_API.Models.Db;
using IMDB_API.Models.Filters;
using IMDB_API.Repository.Interfaces;
using IMDBSample.Models.Db;
using System.Collections.Generic;
using System.Linq;

namespace IMDB_API.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies = new List<Movie>();
        private readonly Dictionary<int, List<int>> _movieActors = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, List<int>> _movieGenres = new Dictionary<int, List<int>>();

        private int _id = 1;

        public IEnumerable<Movie> Get(MovieFilter filter)
        {
            IEnumerable<Movie> movies = _movies;

            if (filter.Year.HasValue)
            {
                movies = movies.Where(
                    m => m.YearOfRelease == filter.Year.Value);
            }

            return movies;
        }

        public Movie Get(int id)
        {
            return _movies.SingleOrDefault(m => m.Id == id);
        }

        public void Add(Movie movie, List<int> actorIds, List<int> genreIds)
        {
            movie.Id = _id++;

            _movies.Add(movie);

            _movieActors[movie.Id] = actorIds;
            _movieGenres[movie.Id] = genreIds;
        }

        public void Update(int id, Movie movie, List<int> actorIds, List<int> genreIds)
        {
            var existing = _movies.SingleOrDefault(m => m.Id == id);

            existing.Name = movie.Name;
            existing.YearOfRelease = movie.YearOfRelease;
            existing.Plot = movie.Plot;
            existing.ProducerId = movie.ProducerId;
            existing.CoverImage = movie.CoverImage;

            _movieActors[id] = actorIds;
            _movieGenres[id] = genreIds;
        }

        public void Delete(int id)
        {
            var movie = _movies.SingleOrDefault(m => m.Id == id);
            
            _movies.Remove(movie);

            _movieActors.Remove(id);
            _movieGenres.Remove(id);
        }

        public IEnumerable<int> GetActorIds(int movieId)
        {
            return _movieActors[movieId];
        }

        public IEnumerable<int> GetGenreIds(int movieId)
        {
            return _movieGenres[movieId];
        }
    }
}
