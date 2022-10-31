using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PetApi.Model;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController : ControllerBase
    {
        private static List<Pet> pets = new List<Pet>();
        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            pets.Add(pet);
            return pet;
        }

        [HttpGet("getAllPets")]
        public List<Pet> GetAllPets()
        {
            return pets;
        }

        [HttpGet("getPetByName")]
        public Pet? FindPetByName([FromQuery] string name)
        {
            return pets.FirstOrDefault(pet => pet.Name == name);
        }

        [HttpDelete("deleteAllPets")]
        public void DeleteAllPets()
        {
            pets.Clear();
        }

        [HttpDelete("deleteBroughtPet")]
        public Pet? DeleteBroughtPet([FromQuery] string name)
        {
            foreach (var pet in pets.Where(pet => pet.Name == name))
            {
                pets.Remove(pet);
                return pet;
            }

            return null;
        }

        [HttpPut("modifyPetByNameAndPrice")]
        public Pet? ModifyPetByNameAndPrice([FromQuery] string name, [FromQuery]int price, Pet modifiedPet)
        {
            foreach (var pet in pets.Where(pet => modifiedPet.Name == name))
            {
                pet.Price = price;
                return pet;
            }

            return null;
        }
    }
}
