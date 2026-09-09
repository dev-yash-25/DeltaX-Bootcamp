using IMDBConsoleApp.Models;
using System.Collections.Generic;
using System.Linq;
using IMDBConsoleApp.Exceptions;
using IMDBConsoleApp.Repository;
using IMDBConsoleApp.Services.Interface;
using IMDBConsoleApp.Models.Request;
using IMDBConsoleApp.Models.Response;
using IMDBConsoleApp.Helpers;

namespace IMDBConsoleApp.Services
{
    internal class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IActorService _actorService;
        private readonly IProducerService _producerService;
        
        public MovieService(IActorService actorService,IProducerService producerService)
        {
            _movieRepository = new MovieRepository();
            _actorService = actorService;
            _producerService = producerService;
        }

        public void Add(MovieRequest movieRequest)
        {
            
            ValidateMovie(movieRequest.Name, movieRequest.YearOfRelease, movieRequest.Plot, movieRequest.ActorIds, movieRequest.ProducerId);

            Movie movie = new Movie
            {
                Name = movieRequest.Name.Trim(),
                YearOfRelease = movieRequest.YearOfRelease,
                Plot = movieRequest.Plot.Trim(),
                ActorIds = movieRequest.ActorIds,
                ProducerId =   movieRequest.ProducerId
            };

            _movieRepository.Add(movie);
        }

        public Movie Get(int id)
        {
            var movie = _movieRepository.Get(id);

            if (movie == null)
                throw new ValidationException("Movie not found.");

            return movie;
        }

        public IEnumerable<MovieResponse> Get() //mapping
        {
            var movies = _movieRepository.Get();

            if (!movies.Any())
                throw new ValidationException("Movie List is Empty");

            return movies
                .Select(movie => new MovieResponse
                {
                    Id = movie.Id,
                    Name = movie.Name,
                    YearOfRelease = movie.YearOfRelease,
                    Plot = movie.Plot,

                    Actors = _actorService.Get(movie.ActorIds).ToList(),
                    Producer = _producerService.Get(movie.ProducerId)
                })
                .ToList();
        }

        public void Delete(int id)
        {
            Get(id);
            _movieRepository.Delete(id);
        }

        private void ValidateMovie(string Name, int YearOfRelease, string Plot, IEnumerable<int> actorIds, int producerId)
        {
            ValidationHelper.ValidateString(Name, "Movie");
            ValidationHelper.ValidateString(Plot, "Plot");

            // Duplicate movie check
            bool movieExists = _movieRepository.Get().Any(
                m => m.Name.ToLower() == Name.ToLower() &&
                m.YearOfRelease == YearOfRelease
                );

            if (movieExists)
            {
                throw new ValidationException("Movie Already Exists");
            }

            _actorService.Get(actorIds);
            _producerService.Get(producerId);
        }
    }
}
