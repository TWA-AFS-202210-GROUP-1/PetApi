using System.Collections.Generic;
using PetApi.Models;

namespace PetApi.Services;

public interface IPetsService
{
    public Pet CreatePet(Pet pet);
    public IList<Pet> GetAllPets();
    public Pet? GetByName(string name);
    public bool DeleteByName(string name);
    public Pet? ModifyPetPrice(string name, PetPriceChangeDto priceChange);
    public IList<Pet>? GetByType(PetType name);
    public IList<Pet>? GetByPriceRange(double from, double to);
}