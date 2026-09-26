using IMDB_API.Exceptions;
using IMDB_API.Models.Db;
using IMDB_API.Models.Requests;
using System.Collections.Generic;
using System.Linq;

namespace IMDB_API.Helpers.Validators
{
    public class GenreValidator
    {
        public static void ValidateRequest(GenreRequest request)
        {
            ValidationHelper.ValidateNull(request, "Genre Request");
            ValidationHelper.ValidateString(request.Name, "Genre Name");
        }

        public static void ValidateIds(IEnumerable<int> genreIds, IEnumerable<Genre> genres)
        {
            ValidationHelper.ValidateList(genreIds, "Genre ID List");
            var validGenreIds = genres.Select(g => g.Id);

            var invalidGenreIds = genreIds
                .Except(validGenreIds)
                .ToList();

            if (invalidGenreIds.Any())
            {
                throw new ValidationException(
                     $"Invalid Genre Ids : {string.Join(", ", invalidGenreIds)}");
            }

        }
    }
}
