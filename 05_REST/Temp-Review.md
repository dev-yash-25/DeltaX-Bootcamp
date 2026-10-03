## Optimizing Movie Response

### 1. MovieDto.cs
```csharp
using System;

namespace IMDB_API.Models.Dtos
{
    public class MovieDto
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public string CoverImage { get; set; }

        public int ProducerId { get; set; }
        public string ProducerName { get; set; }
        public string ProducerBio { get; set; }
        public DateTime ProducerDOB { get; set; }
        public string ProducerGender { get; set; }

        public int? ActorId { get; set; }   //nullable (if movie has no actors)
        public string ActorName { get; set; }
        public string ActorBio { get; set; }
        public DateTime? ActorDOB { get; set; }
        public string ActorGender { get; set; }

        public int? GenreId { get; set; }
        public string GenreName { get; set; }
    }
}
```

### 2. MovieRepository.cs
```csharp
public async Task<IEnumerable<MovieDto>> Get(MovieFilter filter)
{
    const string query = @"
        SELECT
            M.Id AS MovieId,
            M.Name AS MovieName,
            M.YearOfRelease,
            M.Plot,
            M.CoverImage,

            P.Id AS ProducerId,
            P.Name AS ProducerName,
            P.Bio AS ProducerBio,
            P.DateOfBirth AS ProducerDOB,
            P.Gender AS ProducerGender,

            A.Id AS ActorId,
            A.Name AS ActorName,
            A.Bio AS ActorBio,
            A.DateOfBirth AS ActorDOB,
            A.Gender AS ActorGender,

            G.Id AS GenreId,
            G.Name AS GenreName

        FROM Foundation.Movies M

        INNER JOIN Foundation.Producers P
            ON P.Id = M.ProducerId

        LEFT JOIN Foundation.Actor_Movies AM
            ON AM.MovieId = M.Id

        LEFT JOIN Foundation.Actors A
            ON A.Id = AM.ActorId

        LEFT JOIN Foundation.Genre_Movies GM
            ON GM.MovieId = M.Id

        LEFT JOIN Foundation.Genres G
            ON G.Id = GM.GenreId

        WHERE (@Year IS NULL OR M.YearOfRelease = @Year)

        ORDER BY M.Id";

    return await QueryAsync<MovieDto>(
        query,
        new { Year = filter.Year });
}
```

### 3. MovieService.cs
```csharp
// Get filtered
public async Task<IEnumerable<MovieResponse>> Get(MovieFilter filter)
{
    ValidateFilter.ValidateMovie(filter);

    var movies = await _movieRepository.Get(filter);

    return movies
        .GroupBy(movie => movie.MovieId)
        .Select(group => MapMovieDtoResponse(group))
        .ToList();
}

// Get by Id
public async Task<MovieResponse> Get(int id)
{
    ValidationHelper.ValidatePositiveInt(id, "Movie Id");

    var movie = await _movieRepository.Get(id);
    ValidationHelper.ValidateNotFound(movie, "Movie");

    return await MapMovieResponse(movie);
}

// Mapper func for single movie fetching (old)
private async Task<MovieResponse> MapMovieResponse(Movie movie)
{
    var response = _mapper.Map<MovieResponse>(movie);

    response.Producer = await _producerService.Get(movie.ProducerId);

    var actorIds = await _movieRepository.GetActorIds(movie.Id);
    var genreIds = await _movieRepository.GetGenreIds(movie.Id);

    response.Actors = await _actorService.Get(actorIds);
    response.Genres = await _genreService.Get(genreIds);

    return response;
}

// Mappper for Get filtered
// Getting singular values, binding to object -> final response
private MovieResponse MapMovieDtoResponse(IGrouping<int, MovieDto> group)
{
    var first = group.First();

    return new MovieResponse
    {
        Id = first.MovieId,
        Name = first.MovieName,
        YearOfRelease = first.YearOfRelease,
        Plot = first.Plot,
        CoverImage = first.CoverImage,

        Producer = new ProducerResponse
        {
            Id = first.ProducerId,
            Name = first.ProducerName,
            Bio = first.ProducerBio,
            DOB = first.ProducerDOB,
            Gender = first.ProducerGender
        },

        Actors = group
            .Where(x => x.ActorId.HasValue)
            .GroupBy(x => x.ActorId)
            .Select(x =>
            {
                var actor = x.First();

                return new ActorResponse
                {
                    Id = actor.ActorId.Value,
                    Name = actor.ActorName,
                    Bio = actor.ActorBio,
                    DOB = actor.ActorDOB.Value,
                    Gender = actor.ActorGender
                };
            })
            .ToList(),

        Genres = group
            .Where(x => x.GenreId.HasValue)
            .GroupBy(x => x.GenreId)
            .Select(x =>
            {
                var genre = x.First();

                return new GenreResponse
                {
                    Id = genre.GenreId.Value,
                    Name = genre.GenreName
                };
            })
            .ToList()
    };
}

```


<br>

---

<br>

## Old - GetFiltered
```csharp
public async Task<IEnumerable<Movie>> Get(MovieFilter filter)
{
    const string query = @"
        SELECT
            Id,
            Name,
            YearOfRelease,
            Plot,
            CoverImage,
            ProducerId
        FROM Foundation.Movies
        WHERE (@Year IS NULL OR YearOfRelease = @Year)";

    return await QueryAsync(
        query,
        new
        {
            Year = filter.Year
        });
}
```
