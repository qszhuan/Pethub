using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Pethub.Models;
using Pethub.Services;
using Xunit;

namespace PethubTests
{
    public class PetOwnerServiceTest : TestBase
    {
        public PetOwnerServiceTest()
        {
            _serviceCollection.AddScoped<IPetOwnerService, PetOwnerService>();
            _serviceCollection.AddScoped(x=>new Mock<ILogger<PetOwnerService>>().Object);
            _serviceCollection.AddScoped<IConfiguration>(x=>
            {
                var myConfiguration = new Dictionary<string, string>
                {
                    {"PeopleApi", "http://localhost:5000"}
                };

                var configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(myConfiguration)
                    .Build();
                return configuration;
            });
        }


        [Theory]
        [InlineData(@"[{'name':'Bob', 'gender':'Male', 'age':23, 'pets':[]}]", 0)]
        [InlineData(@"[{'name':'Bob', 'gender':'Male', 'age':23, 'pets':null}]", 0)]
        [InlineData(@"[{'name':'Bob', 'gender':'Female', 'age':23, 'pets':[{'name':'Tom', 'type':'Cat'}]}]", 1)]
        [InlineData(@"[{'name':'Bob', 'gender':'Female', 'age':23, 'pets':[{'name':'Fido', 'type':'Dog'}]}]", 1)]
        [InlineData(@"[{'name':'Bob', 'gender':'Female', 'age':23, 'pets':[{'name':'Tom', 'type':'Cat'},{'name':'Fido', 'type':'Dog'}]}]", 2)]
        public void ShouldGetPetOwners(string data, int petCount)
        {
            SetupHttpData(data);
            
            var petOwnerService = GetServiceProvider().GetService<IPetOwnerService>();
            var owners = petOwnerService.GetOwners().Result;
            
            Assert.NotEmpty(owners);
            Assert.Single(owners);
            Assert.Equal(petCount, owners.First().Pets.Count);
        }

        [Fact]
        public void ShouldGetPetAnOwner()
        {
            var data = @"[{'name':'Bob', 'gender':'Male', 'age':23, 'pets':[{'name':'Tom', 'type':'Cat'}]}]";
            SetupHttpData(data);
            var petOwnerService = GetServiceProvider().GetService<IPetOwnerService>();
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

        
    }

}
