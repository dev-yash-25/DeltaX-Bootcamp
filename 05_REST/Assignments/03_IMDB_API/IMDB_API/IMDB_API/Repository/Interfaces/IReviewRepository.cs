using IMDB_API.Models.Db;
using System.Collections.Generic;

namespace IMDB_API.Repository.Interfaces
{
    public interface IReviewRepository
    {
        IEnumerable<Review> Get(int movieId);
        Review Get(int movieId, int id);

        void Add(int movieId, Review review);
        void Update(int movieId, int id, Review review);
        void Delete(int movieId, int id);
    }
}
