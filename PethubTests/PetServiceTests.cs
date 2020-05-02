using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Pethub.Models;
using Pethub.Services;
using Xunit;

namespace PethubTests
{
    public class PetServiceTests : TestBase
    {
        private readonly Mock<IPetOwnerService> _petOwnerServiceMock = new Mock<IPetOwnerService>();

        public PetServiceTests()
        {
            _serviceCollection.AddScoped<IPetService, PetService>();
            _serviceCollection.AddScoped(x => _petOwnerServiceMock.Object);
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
                                Type = PetType.Cat
                            }
                        }
                    }
                }));

            var petService = GetServiceProvider().GetService<IPetService>();
            var groupedCat = petService.GetOwnerGenderGroupedPets(PetType.Cat);

            Assert.Single(groupedCat);
            var group = groupedCat.First();
            Assert.Equal("Male", group.Key);
            
            Assert.Single(group.Pets);
            var cat = @group.Pets.First();
            Assert.Equal("Tom", cat.Name);
            Assert.Equal(PetType.Cat, cat.Type);
        }

        [Fact]
        public void ShouldGetNoCatsGroupedByOwnerGender()
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
                                Name = "Joe",
                                Type = PetType.Dog
                            }
                        }
                    }
                }));

            var petService = GetServiceProvider().GetService<IPetService>();
            var groupedCat = petService.GetOwnerGenderGroupedPets(PetType.Cat);

            Assert.Empty(groupedCat);
        }

        [Fact]
        public void ShouldGetCatsGroupedByOwnerGenderWithNameInAlphabeticalOrder()
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
                                Name = "Simba",
                                Type = PetType.Cat
                            }
                        }
                    },
                    new PetOwner()
                    {
                        Name = "Sam",
                        Gender = Gender.Male,
                        Age = 12,
                        Pets = new List<Pet>
                        {
                            new Pet()
                            {
                                Name = "Garfield",
                                Type = PetType.Cat
                            }
                        }
                    },
                    new PetOwner()
                    {
                        Name = "Jennifer",
                        Gender = Gender.Female,
                        Age = 12,
                        Pets = new List<Pet>
                        {
                            new Pet()
                            {
                                Name = "Tabby",
                                Type = PetType.Cat
                            },
                            new Pet()
                            {
                                Name = "Jim",
                                Type = PetType.Cat
                            }
                        }
                    }
                }));

            var petService = GetServiceProvider().GetService<IPetService>();
            var groupedCat = petService.GetOwnerGenderGroupedPets(PetType.Cat);

            Assert.Equal(2, groupedCat.Count);

            var catGroup1 = groupedCat.First();
            var catGroup2 = groupedCat.Last();
            Assert.Equal("Male", catGroup1.Key);
            Assert.Equal("Female", catGroup2.Key);

            Assert.Equal(2, catGroup1.Pets.Count);
            Assert.Equal(new List<string>{ "Garfield", "Simba" }, catGroup1.Pets.Select(x=>x.Name).ToList());

            Assert.Equal(2, catGroup2.Pets.Count);
            Assert.Equal(new List<string> { "Jim", "Tabby" }, catGroup2.Pets.Select(x => x.Name).ToList());
        }
    }
}