using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pethub.Graphql;
using Pethub.Models;
using Pethub.Services;
using Xunit;

namespace PethubTests
{
    public class PetOwnerQueryTests: TestBase
    {
        private readonly Mock<IPetOwnerService> _petOwnerServiceMock = new Mock<IPetOwnerService>();

        public PetOwnerQueryTests()
        {
            _serviceCollection.AddScoped<PetOwnerQuery>();
            _serviceCollection.AddScoped<ISchema>(x => new Schema()
            {
                Query = x.GetService<PetOwnerQuery>()
            });
            _serviceCollection.AddScoped(x => _petOwnerServiceMock.Object);
        }

        [Fact]
        public void ShouldGetOwnerInfo()
        {
            _petOwnerServiceMock.Setup(x => x.GetOwners())
                .Returns(Task.FromResult(new List<PetOwner>
                {
                    new PetOwner()
                    {
                        Name = "Bob",
                        Gender = Gender.Male,
                        Age = 12,
                        Pets = new List<Pet>
                        {
                            new Pet()
                            {
                                Name = "Tom",
                                Type = Pethub.Models.PetType.Cat
                            }
                        }
                    }
                }));

            var schema = GetServiceProvider().GetService<ISchema>();

            var json =  schema.ExecuteAsync(_ =>
            {
                _.Query = @"{ owner(name:""Bob""){name age gender} }";
            });

            var deserializeObject = JsonConvert.DeserializeObject<JObject>(json.Result);
            Assert.Equal(12, deserializeObject["data"]["owner"]["age"]);
            Assert.Equal("Male", deserializeObject["data"]["owner"]["gender"]);
            Assert.Equal("Bob", deserializeObject["data"]["owner"]["name"]);
        }

        [Fact]
        public void ShouldGetOwners()
        {
            _petOwnerServiceMock.Setup(x => x.GetOwners())
                .Returns(Task.FromResult(new List<PetOwner>
                {
                    new PetOwner()
                    {
                        Name = "Bob",
                        Gender = Gender.Male,
                        Age = 12,
                        Pets = new List<Pet>
                        {
                            new Pet()
                            {
                                Name = "Tom",
                                Type = Pethub.Models.PetType.Cat
                            }
                        }
                    }
                }));

            var schema = GetServiceProvider().GetService<ISchema>();

            var json = schema.ExecuteAsync(_ =>
            {
                _.Query = @"{ owners{name age gender pets(type:""Cat""){name type}} }";
            });

            var deserializeObject = JsonConvert.DeserializeObject<JObject>(json.Result);
            Assert.Equal(12, deserializeObject["data"]["owners"][0]["age"]);
            Assert.Equal("Male", deserializeObject["data"]["owners"][0]["gender"]);
            Assert.Equal("Bob", deserializeObject["data"]["owners"][0]["name"]);
            Assert.Equal("Tom", deserializeObject["data"]["owners"][0]["pets"][0]["name"]);
            Assert.Equal("Cat", deserializeObject["data"]["owners"][0]["pets"][0]["type"]);
        }

        [Fact]
        public void ShouldGetCatsGroupedByOwnerGender()
        {
            _petOwnerServiceMock.Setup(x => x.GetOwners())
                .Returns(Task.FromResult(new List<PetOwner>
                {
                    new PetOwner()
                    {
                        Name = "Bob",
                        Gender = Gender.Male,
                        Age = 12,
                        Pets = new List<Pet>
                        {
                            new Pet()
                            {
                                Name = "Tom",
                                Type = Pethub.Models.PetType.Cat
                            }
                        }
                    },
                    new PetOwner()
                    {
                        Name = "Sam",
                        Gender = Gender.Male,
                        Age = 24,
                        Pets = new List<Pet>
                        {
                            new Pet()
                            {
                                Name = "Jim",
                                Type = Pethub.Models.PetType.Cat
                            }
                        }
                    },
                    new PetOwner()
                    {
                        Name = "Jen",
                        Gender = Gender.Female,
                        Age = 24,
                        Pets = new List<Pet>
                        {
                            new Pet()
                            {
                                Name = "Tobby",
                                Type = Pethub.Models.PetType.Cat
                            }
                        }
                    }

                }));

            var schema = GetServiceProvider().GetService<ISchema>();

            var query = @"
{
  petGroups(key: ""gender"") {
            key
            pets(type: ""Cat"") {
                name
            }
        }
    }

";
            var json = schema.ExecuteAsync(_ =>
            {
                _.Query = query;
            });

            var deserializeObject = JsonConvert.DeserializeObject<JObject>(json.Result);
            Assert.Equal("Male", deserializeObject["data"]["petGroups"][0]["key"]);
            Assert.Equal("Jim", deserializeObject["data"]["petGroups"][0]["pets"][0]["name"]);
            Assert.Equal("Tom", deserializeObject["data"]["petGroups"][0]["pets"][1]["name"]);
            Assert.Equal("Female", deserializeObject["data"]["petGroups"][1]["key"]);
            Assert.Equal("Tobby", deserializeObject["data"]["petGroups"][1]["pets"][0]["name"]);
        }
    }
}
