using AutoMapper;
using IMDB_API.Helpers;
using IMDB_API.Helpers.Validators;
using IMDB_API.Models.Filters;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Repository;
using IMDB_API.Repository.Interfaces;
using IMDB_API.Services.Interfaces;
using IMDBSample.Models.Db;
using System.Collections.Generic;
using System.Linq;

namespace IMDB_API.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        private readonly IActorService _actorService;
        private readonly IProducerService _producerService;
        private readonly IGenreService _genreService;

        public MovieService(
            IMovieRepository movieRepository,
            IActorService actorService, 
            IProducerService producerService,
            IGenreService genreService,
            IMapper mapper)
        {
            _movieRepository = movieRepository;
            _actorService = actorService;
            _producerService = producerService;
            _genreService = genreService;
            _mapper = mapper;
        }

        public IEnumerable<MovieResponse> Get(MovieFilter filter)
        {
            if (filter.Year.HasValue)
            {
                ValidationHelper.ValidatePositiveInt(filter.Year.Value, "Movie Year");
            }

            var movies = _movieRepository.Get(filter);

            var responses = new List<MovieResponse>();

            foreach (var movie in movies)
            {
                responses.Add(MapMovieResponse(movie));
            }

            return responses;
        }

        public MovieResponse Get(int id)
        {
            ValidationHelper.ValidatePositiveInt(id, "Movie Id");

            var movie = _movieRepository.Get(id);
            ValidationHelper.ValidateNotFound(movie, "Movie");

            return MapMovieResponse(movie);
        }

        public int Add(MovieRequest request)
        {
            MovieValidator.ValidateRequest(request);

            _producerService.Get(request.ProducerId);

            var actorIds = request.ActorIds.ToList() ?? new List<int>();
            var genreIds = request.GenreIds.ToList() ?? new List<int>();

            _actorService.Get(actorIds);
            _genreService.Get(genreIds);

            var movie = _mapper.Map<Movie>(request);
            _movieRepository.Add(movie, actorIds, genreIds);

            return movie.Id;
        }

        public void Update(int id, MovieRequest request)
        {
            ValidationHelper.ValidatePositiveInt(id, "Movie Id");
            MovieValidator.ValidateRequest(request);
            Get(id);

            _producerService.Get(request.ProducerId);

            var actorIds = request.ActorIds?.ToList() ?? new List<int>();
            var genreIds = request.GenreIds?.ToList() ?? new List<int>();

            _actorService.Get(actorIds);
            _genreService.Get(genreIds);

            var movie = _mapper.Map<Movie>(request);
            _movieRepository.Update(id, movie, actorIds, genreIds);
        }

        public void Delete(int id)
        {
            ValidationHelper.ValidatePositiveInt(id, "Movie Id");
            Get(id);

            _movieRepository.Delete(id);
        }

        private MovieResponse MapMovieResponse(Movie movie)
        {
            var response = _mapper.Map<MovieResponse>(movie);
            response.Producer = _producerService.Get(movie.ProducerId);

            response.Actors = _actorService.Get(_movieRepository.GetActorIds(movie.Id));
            response.Genres = _genreService.Get(_movieRepository.GetGenreIds(movie.Id));

            return response;
        }
    }
}
