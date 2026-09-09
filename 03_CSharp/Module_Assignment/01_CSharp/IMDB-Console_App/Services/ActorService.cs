using System;
using IMDBConsoleApp.Helpers;
using IMDBConsoleApp.Models;
using IMDBConsoleApp.Repository;
using IMDBConsoleApp.Exceptions;
using IMDBConsoleApp.Repository.Interface;
using IMDBConsoleApp.Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace IMDBConsoleApp.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        public ActorService()
        {
            _actorRepository = new ActorRepository();
        }

        public void Add(string name, DateTime dob)  
        {
            PersonValidator.ValidatePerson(name,dob);

            _actorRepository.Add(new Actor
            {
                Name = name.Trim(),
                DOB = dob
            });
        }

        public IEnumerable<Actor> Get()
        {
            var actors = _actorRepository.Get();
            ValidationHelper.ValidateList(actors, "Actor List");
            return actors.ToList();
        }

        public Actor Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Actor actor = _actorRepository.Get(id);
            ValidationHelper.ValidateNull(actor, "Actor");
            return actor;
        }

        public IEnumerable<Actor> Get(IEnumerable<int> ids)
        {
            ValidationHelper.ValidateList(ids, "Actor ID's");

            List<int> actorIds = ids.ToList();

            List<Actor> actors = _actorRepository.Get(actorIds).ToList();

            var validActorIds = actors.Select(a => a.Id);

            List<int> invalidActorIds = actorIds
                .Except(validActorIds)
                .ToList();

            if (invalidActorIds.Any())
            {
                throw new ValidationException(
                    $"Invalid Actor Id(s): {string.Join(", ", invalidActorIds)}");
            }

            return actors.ToList();
        }
    }
}
