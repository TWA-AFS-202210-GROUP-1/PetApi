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
            return Ok(pets);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName([FromQuery]string name)
        {
            var pet = _petService.GetByName(name);
            if (pet == null)
            {
                return NotFound();
            }

            return Ok(pet);
        }
    }
}
