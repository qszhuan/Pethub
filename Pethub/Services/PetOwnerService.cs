using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pethub.Models;

namespace Pethub.Services
{
    public class PetOwnerService : IPetOwnerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PetOwnerService> _logger;
        private readonly IConfiguration _configuration;

        public PetOwnerService(IHttpClientFactory httpClientFactory, 
            ILogger<PetOwnerService> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public Task<List<PetOwner>> GetOwners()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var owners = httpClient.GetAsync(_configuration["PeopleApi"])
                .ContinueWith(action =>
                {
                    try
                    {
                        var jsonSerializerSettings = new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        };
                        var result = action.Result.Content.ReadAsStringAsync().Result;
                        var petOwners = JsonConvert.DeserializeObject<List<PetOwner>>(result, jsonSerializerSettings);
                        return petOwners;
                    }
                    catch (Exception ex)
                    {
                        var contractException = new ContractException(ex.Message, ex.InnerException);
                        _logger.LogError(contractException, contractException.Message);

                        throw contractException;
                    }
                });

            return owners;
        }
    }
}