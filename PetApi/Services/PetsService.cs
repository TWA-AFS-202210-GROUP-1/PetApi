using System.Collections.Generic;
using System.Linq;
using PetApi.Models;

namespace PetApi.Services
{
    public class PetsService : IPetsService
    {
        private IList<Pet> _pets;

        public PetsService()
        {
            _pets = new List<Pet>()
            {
                new ("Spike", PetType.Dog, "white", 100),
                new ("Tom", PetType.Cat, "blue", 100),
            };
        }

        public Pet CreatePet(Pet pet)
        {
            _pets.Add(pet);
            return pet;
        }

        public IList<Pet> GetAllPets()
        {
            return _pets;
        }

        public Pet? GetByName(string name)
        {
            return _pets.FirstOrDefault(_ => _.Name.Equals(name));
        }

        public bool DeleteByName(string name)
        {
            var pet = GetByName(name);
            if (pet != null)
            {
                return _pets.Remove(pet);
            }
            
            return false;
        }

        public Pet? ModifyPetPrice(string name, PetPriceChangeDto priceChange)
        {
            var pet = GetByName(name);
            if (pet != null)
            {
                pet.Price = priceChange.Price;
            }

            return pet;
        }
    }
}
