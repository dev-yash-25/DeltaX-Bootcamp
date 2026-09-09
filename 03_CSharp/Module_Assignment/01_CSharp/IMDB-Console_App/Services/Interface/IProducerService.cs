using System;
using IMDBConsoleApp.Models;
using System.Collections.Generic;

namespace IMDBConsoleApp.Services.Interface
{
    public interface IProducerService
    {
        void Add(string name, DateTime dob);
        Producer Get(int id);
        IEnumerable<Producer> Get();
    }
}
