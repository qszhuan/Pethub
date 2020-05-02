using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pethub.Services
{
    public interface IPetOwnerService
    {
        Task<List<PetOwner>> GetOwners();
    }

    public class PetOwner
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }

        public List<Pet> Pets { get; set; } = new List<Pet>();
    }

    public class Pet
    {
        public string Name { get; set; }
        public PetType Type { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
    public enum PetType
    {
        Dog,
        Cat
    }
}
