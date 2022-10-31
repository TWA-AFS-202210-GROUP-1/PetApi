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
    public IActionResult FindPetByName([FromQuery] string name)
    {
      try
      {
        var pet = pets.First(pet => pet.Name.Equals(name));

        return Ok(pet);
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpGet("findPetsByType")]
    public IActionResult FindPetsByType([FromQuery] string type)
    {
      try
      {
        var foundPets = pets.Where(pet => pet.Type.Equals(type)).ToList();

        return Ok(foundPets);
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpGet("findPetsByColor")]
    public IActionResult FindPetsByColor([FromQuery] string color)
    {
      try
      {
        var foundPets = pets.Where(pet => pet.Color.Equals(color)).ToList();

        return Ok(foundPets);
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpGet("findPetsByPriceRange/priceFrom{start}to{end}")]
    public IActionResult FindPetsByPriceRange([FromRoute] int start, [FromRoute] int end)
    {
      try
      {
        var foundPets = pets.Where(pet => pet.Price >= start && pet.Price <= end).ToList();

        return Ok(foundPets);
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
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
