using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pethub.Models;

namespace Pethub.Services
{
    public class PetOwnerService : IPetOwnerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PetOwnerService> _logger;

        public PetOwnerService(IHttpClientFactory httpClientFactory, ILogger<PetOwnerService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public Task<List<PetOwner>> GetOwners()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var owners = httpClient.GetAsync("http://agl-developer-test.azurewebsites.net/people.json")
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