using System;
using System.Collections.Generic;
using System.IO.Compression;
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

        [HttpGet("findPetsByType")]
        public List<Pet>? FindPetsByType([FromQuery] string type)
        {
            var findPets = new List<Pet>();
            findPets = pets.FindAll(pet => pet.Type == type);
            return findPets.Count == 0 ? null : findPets;
        }

        [HttpGet("findPetsByPriceRange/{minPrice}_{maxPrice}")]
        public List<Pet>? FindPetsByPriceRange([FromRoute] int minPrice, [FromRoute] int maxPrice)
        {
            var findPetsByPriceRange = pets.FindAll(pet => pet.Price <= maxPrice && pet.Price >= minPrice);
            return findPetsByPriceRange.Count == 0 ? null : findPetsByPriceRange;
        }

        [HttpGet("findPetsByColor")]
        public List<Pet>? FindPetsByColor([FromQuery] string color)
        {
            var findPetsByColor = pets.FindAll(pet => pet.Color == color);
            return findPetsByColor.Count == 0 ? null : findPetsByColor;
        }
    }
}
