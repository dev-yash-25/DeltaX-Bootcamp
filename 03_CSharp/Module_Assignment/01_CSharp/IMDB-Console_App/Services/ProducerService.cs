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
    internal class ProducerService : IProducerService
    {
        private readonly IProducerRepository _producerRepository;

        public ProducerService()
        {
            _producerRepository = new ProducerRepository();   
        }

        public void Add(string name, DateTime dob)
        {
            PersonValidator.ValidatePerson(name, dob);

            _producerRepository.Add(
            new Producer
            {
                Name = name.Trim(),
                DOB = dob
            });
        }

        public IEnumerable<Producer> Get()
        {
            var producers = _producerRepository.Get();
            ValidationHelper.ValidateList(producers, "Producer List");
            return producers.ToList();
        }

        public Producer Get(int id)
        {
            Producer producer = _producerRepository.Get(id);
            ValidationHelper.ValidateNull(producer, "Producer");
            return producer;
        }
    }
}
