using IMDBConsoleApp.Models;
using System;
using System.Collections.Generic;

namespace IMDBConsoleApp.Repository.Interface
{
    public interface IActorRepository
    {
        void Add(Actor actor);
        IEnumerable<Actor> Get();
        Actor Get(int id);
        IEnumerable<Actor> Get(IEnumerable<int> ids);
    }
}
