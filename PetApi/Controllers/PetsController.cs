using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetApi.Models;
using PetApi.Services;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetsController : ControllerBase
    {
        private IPetsService _petService;
        public PetsController(IPetsService petService)
        {
            _petService = petService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var pets = _petService.GetAllPets();
            return new ObjectResult(pets);
        }
    }
}
