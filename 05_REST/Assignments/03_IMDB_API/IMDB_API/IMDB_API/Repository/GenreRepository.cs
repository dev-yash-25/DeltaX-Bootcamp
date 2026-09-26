using IMDB_API.Models.Db;
using IMDB_API.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace IMDB_API.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly List<Genre> _genresList = new List<Genre>();
        private int _id = 1;

        public IEnumerable<Genre> Get()
        {
            return _genresList;
        }

        public Genre Get(int id)
        {
            var genre = _genresList.SingleOrDefault(g => g.Id == id);
            return genre;
        }

        public IEnumerable<Genre> Get(IEnumerable<int> genreIds)
        {
            var genres = _genresList
                .Where(genre => genreIds.Contains(genre.Id))
                .ToList();

            return genres;
        }

        public void Add(Genre genre)
        {
            genre.Id = _id++;
            _genresList.Add(genre);
        }

        public void Update(int id, Genre genre)
        {
            var existingGenre = _genresList.SingleOrDefault(g => g.Id == id);
            existingGenre.Name = genre.Name;
        }

        public void Delete(int id)
        {
            var genre = _genresList.SingleOrDefault(g => g.Id == id);
            _genresList.Remove(genre);
        }
    }
}
