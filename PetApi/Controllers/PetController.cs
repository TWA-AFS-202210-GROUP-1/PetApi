using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController : ControllerBase
    {
        private static List<Pet> pets = new List<Pet>();
        [HttpPost("addNewPet")]
        public IActionResult AddNewPet(Pet pet)
        {
            pets.Add(pet);
            return Ok();
        }

        [HttpGet("getAllPets")]
        public IActionResult GetAllPets()
        {
            return Ok(pets);
        }

        [HttpGet("findPetByName")]
        public IActionResult FindPetByName([FromQuery] string name)
        {
            Pet pet = pets.FirstOrDefault(pet => pet.Name == name);
            if (pet == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pet);
            }
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets([FromQuery] string name)
        {
            pets.Clear();
        }

        [HttpDelete("deletePetByName")]
        public IActionResult DeletePetByName([FromQuery] string name)
        {
            Pet pet = pets.FirstOrDefault(pet => pet.Name == name);
            if (pets.Remove(pet))
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("updatePetByName")]
        public IActionResult UpdatePetByName([FromBody] Pet updatedPet)
        {
            Pet pet = pets.FirstOrDefault(pet => pet.Name == updatedPet.Name);
            if (pet == null)
            {
                return NotFound();
            }
            else
            {
                pet.Price = updatedPet.Price;
                return Ok();
            }
        }

        [HttpGet("findPetsByType")]
        public IActionResult FindPetsByType([FromQuery] string type)
        {
            List<Pet> targetPets = pets.FindAll(i => i.Type == type);
            if (targetPets.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(targetPets);
            }
        }

        [HttpGet("findPetsByPriceRange")]
        public IActionResult FindPetsByPriceRange([FromQuery] string start, [FromQuery] string end)
        {
            List<Pet> targetPets = pets.FindAll(i => i.Price >= int.Parse(start) && i.Price <= int.Parse(end));
            if (targetPets.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(targetPets);
            }
        }
    }
}
