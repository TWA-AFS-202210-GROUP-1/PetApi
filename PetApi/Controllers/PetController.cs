using Microsoft.AspNetCore.Mvc;
using PetApi.Controllers;
using System.Collections.Generic;

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

        [HttpGet("getPet")]
        public Pet GetAllPets([FromQuery] string name)
        {
            foreach (var pet in pets)
            {
                if (pet.Name == name)
                {
                    return pet;
                }
            }

            return null;
        }
    }
}
