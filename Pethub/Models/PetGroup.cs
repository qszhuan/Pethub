using System.Collections.Generic;

namespace Pethub.Models
{
    public class PetGroup
    {
        public string Key { get; set; }

        public List<Pet> Pets { get; set; }
    }
}