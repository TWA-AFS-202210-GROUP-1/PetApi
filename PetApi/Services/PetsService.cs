using System.Collections.Generic;
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

        public IList<Pet> GetAllPets()
        {
            return _pets;
        }
    }
}
