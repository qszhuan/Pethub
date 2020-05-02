using System.Collections.Generic;
using System.Threading.Tasks;
using Pethub.Models;

namespace Pethub.Services
{
    public interface IPetOwnerService
    {
        Task<List<PetOwner>> GetOwners();
    }
}
