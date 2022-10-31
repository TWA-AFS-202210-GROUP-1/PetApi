using System.Collections.Generic;
using PetApi.Models;

namespace PetApi.Services;

public interface IPetsService
{
    public IList<Pet> GetAllPets();
    public Pet? GetByName(string name);
    public bool DeleteByName(string name);
    public Pet? ModifyPetPrice(string name, PetPriceChangeDto priceChange);
}