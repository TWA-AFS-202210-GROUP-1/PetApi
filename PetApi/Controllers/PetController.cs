using Microsoft.AspNetCore.Mvc;
using PetApi.Model;
using System.Collections.Generic;
using System.Linq;

namespace PetApi.Controllers
{
  [ApiController]
  [Route("api")]
  public class PetController : ControllerBase
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

    [HttpGet("findPetByName")]
    public Pet FindPetByName([FromQuery] string name)
    {
      return pets.First(pet => pet.Name.Equals(name));
    }

    [HttpGet("findPetByType")]
    public Pet FindPetByType([FromQuery] string type)
    {
      return pets.First(pet => pet.Type.Equals(type));
    }

    [HttpGet("findPetByColor")]
    public Pet FindPetByColor([FromQuery] string color)
    {
      return pets.First(pet => pet.Color.Equals(color));
    }

    [HttpPut("changePetProperty")]
    public Pet ChangePetProperty([FromQuery] string name, Pet newPet)
    {
      var oldPet = pets.First(pet => pet.Name.Equals(name));
      oldPet.Type = newPet.Type;
      oldPet.Color = newPet.Color;
      oldPet.Price = newPet.Price;

      return oldPet;
    }

    [HttpDelete("deletePetByName")]
    public IActionResult DeletePet([FromQuery] string name)
    {
      pets.Remove(pets.First(pet => pet.Name.Equals(name)));

      return NoContent();
    }

    [HttpDelete("deleteAllPets")]
    public IActionResult DeleteAllPets()
    {
      pets.Clear();

      return NoContent();
    }
  }
}
