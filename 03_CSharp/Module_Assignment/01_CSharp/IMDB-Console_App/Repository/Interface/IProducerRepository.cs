using IMDBConsoleApp.Models;
using System;
using System.Collections.Generic;

namespace IMDBConsoleApp.Repository.Interface
{
    public interface IProducerRepository
    {
        void Add(Producer producer);
        IEnumerable<Producer> Get();
        Producer Get(int id);
    }
}
