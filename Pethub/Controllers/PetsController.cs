using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pethub.Models;
using Pethub.Services;

namespace Pethub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ILogger<PetsController> _logger;
        private readonly IPetService _petService;

        public PetsController(ILogger<PetsController> logger, IPetService petService)
        {
            _logger = logger;
            _petService = petService;
        }

        [HttpGet]
        public IEnumerable<PetGroup> GetOwnerGenderGroupedPets(PetType type)
        {
            _logger.LogDebug($"{nameof(GetOwnerGenderGroupedPets)} by {type}");
            return _petService.GetOwnerGenderGroupedPets(type);
        }
    }
}