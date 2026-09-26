using AutoMapper;
using IMDB_API.Helpers;
using IMDB_API.Helpers.Validators;
using IMDB_API.Models.Db;
using IMDB_API.Models.Requests;
using IMDB_API.Models.Responses;
using IMDB_API.Repository.Interfaces;
using IMDB_API.Services.Interfaces;
using System.Collections.Generic;

namespace IMDB_API.Services
{
    public class ProducerService : IProducerService
    {
        private readonly IProducerRepository _producerRepository;
        private readonly IMapper _mapper;

        public ProducerService(IProducerRepository producerRepository, IMapper mapper)
        {
            _producerRepository = producerRepository;
            _mapper = mapper;
        }

        public IEnumerable<ProducerResponse> Get()
        {
            var producers = _producerRepository.Get();
            return _mapper.Map<IEnumerable<ProducerResponse>>(producers);
        }

        public ProducerResponse Get(int id)
        {
            ValidationHelper.ValidatePositiveInt(id, "Producer Id");
            var producer = _producerRepository.Get(id);
            ValidationHelper.ValidateNotFound(producer, "Producer");

            return _mapper.Map<ProducerResponse>(producer);
        }

        public int Add(ProducerRequest request)
        {
            ProducerValidator.ValidateRequest(request);

            var producer = _mapper.Map<Producer>(request);
            _producerRepository.Add(producer);

            return producer.Id;
        }

        public void Update(int id, ProducerRequest request)
        {
            ValidationHelper.ValidatePositiveInt(id, "Producer Id");
            ProducerValidator.ValidateRequest(request);

            Get(id);

            var producer = _mapper.Map<Producer>(request);
            _producerRepository.Update(id, producer);
        }

        public void Delete(int id)
        {
            ValidationHelper.ValidatePositiveInt(id, "Producer Id");
            Get(id);

            _producerRepository.Delete(id);
        }
    }
}
