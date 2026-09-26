using AutoMapper;
using IMDB_API.Helpers;
using IMDB_API.Helpers.Validators;
using IMDB_API.Models.Db;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Repository;
using IMDB_API.Repository.Interfaces;
using IMDB_API.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IMDB_API.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;
        
        public GenreService(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public IEnumerable<GenreResponse> Get()
        {
            var genres = _genreRepository.Get();
            return _mapper.Map<IEnumerable<GenreResponse>>(genres);
        }


        public GenreResponse Get(int id)
        {
            ValidationHelper.ValidatePositiveInt(id, "Genre Id");
            var genre = _genreRepository.Get(id);
            ValidationHelper.ValidateNotFound(genre, "Genre");

            return _mapper.Map<GenreResponse>(genre);
        }

        public IEnumerable<GenreResponse> Get(IEnumerable<int> ids)
        {
            List<int> genreIds = ids.ToList();
            List<Genre> genres = _genreRepository.Get(genreIds).ToList();

            GenreValidator.ValidateIds(genreIds, genres);
       
            return _mapper.Map<IEnumerable<GenreResponse>>(genres);
        }

        public int Add(GenreRequest request)
        {
            GenreValidator.ValidateRequest(request);

            var genre = _mapper.Map<Genre>(request);
            _genreRepository.Add(genre);

            return genre.Id;
        }

        public void Update(int id, GenreRequest request)
        {
            ValidationHelper.ValidatePositiveInt(id, "Genre Id");
            GenreValidator.ValidateRequest(request);
            Get(id);

            var genre = _mapper.Map<Genre>(request);
            _genreRepository.Update(id, genre);
        }

        public void Delete(int id)
        {
            ValidationHelper.ValidatePositiveInt(id, "Genre Id");
            Get(id);

            _genreRepository.Delete(id);
        }
    }
}
