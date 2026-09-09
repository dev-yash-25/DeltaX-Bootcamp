using IMDBConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IMDBConsoleApp.Repository 
{
    internal class MovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies = new List<Movie>();
        private int _id = 1;

        public IEnumerable<Movie> Get()
        {
           return _movies.ToList();
        }

        public Movie Get(int id)
        {
            return _movies.SingleOrDefault(m => m.Id == id);
        }

        public void Add(Movie movie)
        {
            movie.Id = _id++;
            _movies.Add(movie);
        }

        public void Delete(int id)
        {
            var movie = Get(id);
            _movies.Remove(movie);
        }
    }
}
