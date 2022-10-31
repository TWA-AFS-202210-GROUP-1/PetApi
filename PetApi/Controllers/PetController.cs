using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController
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
        public Pet GetPetByName([FromQuery] string name)
        {
            return pets.First(x => x.Name.Equals(name));
        }

        [HttpDelete("deleteAllItem")]
        public void DeleteAllItem()
        {
            pets.Clear();
        }

        [HttpDelete("deletePetByName")]
        public void DeletePetByName([FromQuery] string name)
        {
            pets.Remove(pets.Single(r => r.Name.Equals(name)));
        }

        [HttpPatch("updataPetPetByName")]
        public Pet UpdataPetPetByName([FromQuery] string name, int price)
        {
            pets.Single(r => r.Name.Equals(name)).Price = price;
            return pets.Single(r => r.Name.Equals(name));
        }

        [HttpGet("getPetByType")]
        public List<Pet> GetPetByType([FromQuery] string type)
        {
            return pets.FindAll(x => x.Type.Equals(type));
        }

        [HttpGet("getPetByPriceRange")]
        public List<Pet> GetPetByPriceRange([FromQuery] int minPrice, int maxPrice)
        {
            return pets.FindAll(x => x.Price >= minPrice && x.Price <= maxPrice);
        }

        [HttpGet("getPetByColor")]
        public List<Pet> GetPetByColor([FromQuery] string color)
        {
            return pets.FindAll(x => x.Color.Equals(color));
        }
    }
}
