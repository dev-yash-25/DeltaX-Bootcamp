using System;
using IMDBConsoleApp.Models;
using System.Collections.Generic;

namespace IMDBConsoleApp.Services.Interface
{
    public interface IActorService
    {
        void Add(string name, DateTime dob);
        Actor Get(int id);
        IEnumerable<Actor> Get(IEnumerable<int> ids);
        IEnumerable<Actor> Get();

    }
}
