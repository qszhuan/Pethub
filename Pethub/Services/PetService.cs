using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Pethub.Models;

namespace Pethub.Services
{
    public class PetService : IPetService
    {
        private readonly IPetOwnerService _petOwnerService;

        public PetService(IPetOwnerService petOwnerService)
        {
            _petOwnerService = petOwnerService;
        }
        public List<PetGroup> GetOwnerGenderGroupedPets(PetType type)
        {
            var owners = _petOwnerService.GetOwners().Result;

            var petGroups = owners.GroupBy(
                    x => x.Gender,
                    owner => owner.Pets,
                    (gender, enumerable) =>
                    {
                        var pets = enumerable.SelectMany(x => x).Where(x => x.Type == type).OrderBy(x=>x.Name).ToList();
                        return new PetGroup()
                        {
                            Key = gender.ToString(),
                            Pets = pets.ToList()
                        };
                    }
                )
                .Where(x => x.Pets.Any())
                .OrderByDescending(x=> x.Key)
                .ToList();

            return petGroups;
        }
    }
}