using IMDB_API.Models.Requests;

namespace IMDB_API.Helpers.Validators
{
    public class ReviewValidator
    {
        public static void ValidateRequest(int movieId, ReviewRequest request)
        {
            ValidationHelper.ValidateNull(request, "Genre Request");
            ValidationHelper.ValidatePositiveInt(movieId, "Movie Id");
            ValidationHelper.ValidateString(request.Message, "Genre Name");
        }
    }
}
