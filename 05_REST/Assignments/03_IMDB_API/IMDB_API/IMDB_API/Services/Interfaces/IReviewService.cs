using IMDB_API.Models.Db;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using System.Collections.Generic;

namespace IMDB_API.Services.Interfaces
{
    public interface IReviewService
    {
        IEnumerable<ReviewResponse> Get(int movieId);
        ReviewResponse Get(int movieId, int id);

        int Add(int movieId, ReviewRequest request);
        void Update(int movieId, int id, ReviewRequest request);
        void Delete(int movieId, int id);
    }
}
