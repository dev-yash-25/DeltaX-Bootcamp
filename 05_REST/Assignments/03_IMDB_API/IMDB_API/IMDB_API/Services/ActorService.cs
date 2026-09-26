using AutoMapper;
using IMDB_API.Helpers;
using IMDB_API.Helpers.Validators;
using IMDB_API.Models.Db;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Repository;
using IMDB_API.Repository.Interfaces;
using IMDB_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IMDB_API.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;

        public ActorService(IActorRepository actorRepository, IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }

        public IEnumerable<ActorResponse> Get()
        {
            var actors = _actorRepository.Get();

            return _mapper.Map<IEnumerable<ActorResponse>>(actors);
        }

        public ActorResponse Get(int id)
        {
            ValidationHelper.ValidatePositiveInt(id, "Actor Id");
            var actor = _actorRepository.Get(id);
            ValidationHelper.ValidateNotFound(actor, "id");

            return _mapper.Map<ActorResponse>(actor);
        }

        public IEnumerable<ActorResponse> Get(IEnumerable<int> ids)
        {
            List<int> actorIds = ids.ToList();
            List<Actor> actors = _actorRepository.Get(actorIds).ToList();

            ActorValidator.ValidateIds(actorIds, actors);

            return _mapper.Map<IEnumerable<ActorResponse>>(actors);
        }

        public int Add(ActorRequest request)
        {
            ActorValidator.ValidateRequest(request);

            var actor = _mapper.Map<Actor>(request);
            _actorRepository.Add(actor);

            return actor.Id;
        }

        public void Update(int id, ActorRequest request)
        {
            ValidationHelper.ValidatePositiveInt(id, "Actor Id");
            ActorValidator.ValidateRequest(request);
            Get(id);

            var Actor = _mapper.Map<Actor>(request);
            _actorRepository.Update(id, Actor);
        }

        public void Delete(int id)
        {
            ValidationHelper.ValidatePositiveInt(id, "Actor Id");
            Get(id);

            _actorRepository.Delete(id);
        }
    }
}
