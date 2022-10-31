using System.Collections.Generic;
using PetApi.Models;

namespace PetApi.Services;

public interface IPetsService
{
    public IList<Pet> GetAllPets();
}