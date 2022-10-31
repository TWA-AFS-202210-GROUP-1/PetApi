using Microsoft.AspNetCore.Mvc;
using PetApi.Controllers;
using System;
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
        public Pet GetPet([FromQuery] string name)
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

        [HttpDelete("deletePet")]
        public List<Pet> DeletePet([FromQuery] string name, string type, string color, string price)
        {
            for (int i = 0; i < pets.Count; i++)
            {
                if (pets[i].Name == name && pets[i].Type == type && pets[i].Color == color && pets[i].Price == price)
                {
                   pets.RemoveAt(i);
                   return pets;
                }
            }

            return pets;
        }

        [HttpPut("changePetPrice")]
        public Pet ChangePetprice(Pet pet)
        {
            for (int i = 0; i < pets.Count; i++)
            {
                if (pets[i].Name == pet.Name && pets[i].Type == pet.Type && pets[i].Color == pet.Color)
                {
                    pets[i].Price = pet.Price;
                    return pets[i];
                }
            }

            return null;
        }

        [HttpGet("getPetByType")]
        public List<Pet> GetPetByType([FromQuery] string type)
        {
            List<Pet> pet_results = new List<Pet>();
            for (int i = 0; i < pets.Count; i++)
            {
                if (pets[i].Type == type)
                {
                    pet_results.Add(pets[i]);
                }
            }

            return pet_results;
        }

        [HttpGet("getPetByPriceRange")]
        public List<Pet> GetPetByPriceRange([FromQuery] string lowprice, string highprice)
        {
            List<Pet> pet_results = new List<Pet>();
            for (int i = 0; i < pets.Count; i++)
            {
                if (Convert.ToDouble(pets[i].Price) < Convert.ToDouble(highprice) && Convert.ToDouble(pets[i].Price) >= Convert.ToDouble(lowprice))
                {
                    pet_results.Add(pets[i]);
                }
            }

            return pet_results;
        }

        [HttpGet("getPetByColor")]
        public List<Pet> GetPetByColor([FromQuery] string color)
        {
            List<Pet> pet_results = new List<Pet>();
            for (int i = 0; i < pets.Count; i++)
            {
                if (pets[i].Color == color)
                {
                    pet_results.Add(pets[i]);
                }
            }

            return pet_results;
        }
    }
}
