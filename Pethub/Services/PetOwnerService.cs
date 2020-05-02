using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pethub.Services
{
    public class PetOwnerService : IPetOwnerService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PetOwnerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public Task<List<PetOwner>> GetOwners()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var owners = httpClient.GetAsync("http://agl-developer-test.azurewebsites.net/people.json")
                .ContinueWith(action =>
                {
                    var jsonSerializerSettings = new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    var result = action.Result.Content.ReadAsStringAsync().Result;
                    var petOwners = JsonConvert.DeserializeObject<List<PetOwner>>(result, jsonSerializerSettings);
                    return petOwners;
                });

            return owners;
        }
    }
}