using IMDB_API.Models.Db;
using IMDB_API.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IMDB_API.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly List<Review> _reviewList = new List<Review>();
        private int _id = 1;

        public IEnumerable<Review> Get(int movieId)
        {
            return _reviewList.Where(r => r.MovieId == movieId);
        }

        public Review Get(int movieId, int id)
        {
            var review = _reviewList.SingleOrDefault(
            r => r.MovieId == movieId && 
                 r.Id == id);

            return review;
        }

        public void Add(int movieId, Review review)
        {
            review.Id = _id++;
            review.MovieId = movieId;
            _reviewList.Add(review);
        }

        public void Update(int movieId, int id, Review review)
        {
            var existingReview = _reviewList.SingleOrDefault(r =>
            r.Id == id &&
            r.MovieId == movieId);

            existingReview.Message = review.Message;
        }

        public void Delete(int movieId, int id)
        {
            var review = _reviewList.SingleOrDefault(r => 
            r.Id == id &&
            r.MovieId == movieId);

            _reviewList.Remove(review);
        }
    }
}
