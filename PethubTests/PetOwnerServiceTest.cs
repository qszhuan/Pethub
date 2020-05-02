using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Pethub.Services;
using Xunit;

namespace PetHubTests
{
    public class PetOwnerServiceTest
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        public PetOwnerServiceTest()
        {
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(_httpMessageHandlerMock.Object));

            var serviceCollection = new ServiceCollection();
            var serviceCollection1 = serviceCollection.AddScoped<IPetOwnerService, PetOwnerService>();
            serviceCollection1.AddScoped(x => httpClientFactoryMock.Object);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Theory]
        [InlineData(@"[{'name':'Bob', 'gender':'Male', 'age':23, 'pets':[]}]", 0)]
        [InlineData(@"[{'name':'Bob', 'gender':'Male', 'age':23, 'pets':null}]", 0)]
        [InlineData(@"[{'name':'Bob', 'gender':'Female', 'age':23, 'pets':[{'name':'Tom', 'type':'Cat'}]}]", 1)]
        [InlineData(@"[{'name':'Bob', 'gender':'Female', 'age':23, 'pets':[{'name':'Fido', 'type':'Dog'}]}]", 1)]
        [InlineData(@"[{'name':'Bob', 'gender':'Female', 'age':23, 'pets':[{'name':'Tom', 'type':'Cat'},{'name':'Fido', 'type':'Dog'}]}]", 2)]
        public void ShouldGetPetOwners(string data, int petCount)
        {
            SetupData(data);
            
            var petOwnerService = _serviceProvider.GetService<IPetOwnerService>();
            var owners = petOwnerService.GetOwners().Result;
            
            Assert.NotEmpty(owners);
            Assert.Single(owners);
            Assert.Equal(petCount, owners.First().Pets.Count);
        }

        [Fact]
        public void ShouldGetPetAnOwner()
        {
            var data = @"[{'name':'Bob', 'gender':'Male', 'age':23, 'pets':[{'name':'Tom', 'type':'Cat'}]}]";
            SetupData(data);
            var petOwnerService = _serviceProvider.GetService<IPetOwnerService>();
            var owners = petOwnerService.GetOwners().Result;
            
            Assert.NotEmpty(owners);
            Assert.Single(owners);
            
            var petOwner = owners.First();
            Assert.Equal("Bob", petOwner.Name);
            Assert.Equal(Gender.Male, petOwner.Gender);
            Assert.Equal(23, petOwner.Age);
            
            Assert.Single(petOwner.Pets);
            var cat = petOwner.Pets.First();
            Assert.Equal("Tom", cat.Name);
            Assert.Equal(PetType.Cat, cat.Type);
        }

        private void SetupData(string data)
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(data)
                });
        }
    }

}
