using System.Collections.Generic;

namespace Pethub.Models
{
    public class PetOwner
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }

        public List<Pet> Pets { get; set; } = new List<Pet>();
    }
}