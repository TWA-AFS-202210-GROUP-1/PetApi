using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetApi.Models;
using PetApi.Services;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api/pets")]
    public class PetsController : ControllerBase
    {
        private IPetsService _petService;
        public PetsController(IPetsService petService)
        {
            _petService = petService;
        }

        [HttpPost]
        public IActionResult CreatePet([FromBody]Pet pet)
        {
            var createdPet = _petService.CreatePet(pet);
            return Created($"api/pets?name={createdPet.Name}", createdPet);
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var pets = _petService.GetAllPets();
            return Ok(pets);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName([FromRoute]string name)
        {
            var pet = _petService.GetByName(name);
            if (pet == null)
            {
                return NotFound();
            }

            return Ok(pet);
        }

        [HttpDelete("{name}")]
        public IActionResult DeleteByName([FromRoute] string name)
        {
            var hasBeenDelete = _petService.DeleteByName(name);
            if (hasBeenDelete)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPatch("{name}")]
        public IActionResult ModifyPetPrice([FromRoute]string name, [FromBody]PetPriceChangeDto priceChange)
        {
            var pet = _petService.ModifyPetPrice(name, priceChange);
            if (pet == null)
            {
                return NotFound();
            }

            return Ok(pet);
        }

        [HttpGet("type/{type}")]
        public IActionResult GetByType([FromRoute] PetType type)
        {
            var pet = _petService.GetByType(type);
            return Ok(pet);
        }

        [HttpGet("price/from/{from}/to/{to}")]
        public IActionResult GetByPriceRange([FromRoute] double from, [FromRoute] double to)
        {
            var pet = _petService.GetByPriceRange(from, to);
            return Ok(pet);
        }
    }
}
