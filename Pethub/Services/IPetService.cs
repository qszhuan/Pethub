using System.Collections.Generic;
using Pethub.Models;

namespace Pethub.Services
{
    public interface IPetService
    {
        List<PetGroup> GetOwnerGenderGroupedPets(PetType type);
    }
}