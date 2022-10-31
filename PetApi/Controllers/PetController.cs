using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
using System.Collections.Generic;

namespace PetApi.Controllers
{
  [ApiController]
  [Route("api")]
  public class PetController
  {
    private static readonly List<Pet> pets = new ();

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
  }
}
