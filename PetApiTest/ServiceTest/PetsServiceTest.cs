using PetApi.Models;
using PetApi.Services;
using Xunit;

namespace PetApiTest.ServiceTest
{
    public class PetsServiceTest
    {
        [Fact]
        public void Should_return_pet_when_create_pet_given_pet()
        {
            //given
            var service = new PetsService();

            //when
            var result = service.CreatePet(new Pet("TestJack", PetType.Cat, "Black", 200));

            //then
            Assert.Equal("TestJack", result.Name);
        }

        [Fact]
        public void Should_return_pets_when_get_all_pets_given_no()
        {
            //given
            var service = new PetsService();

            //when
            var result = service.GetAllPets();

            //then
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Should_return_pet_when_get_by_name_given_name_is_exist()
        {
            //given
            var service = new PetsService();

            //when
            var result = service.GetByName("Tom");

            //then
            Assert.NotNull(result);
            Assert.Equal("Tom", result.Name);
        }

        [Fact]
        public void Should_return_pet_when_get_by_name_given_name_is_not_exist()
        {
            //given
            var service = new PetsService();

            //when
            var result = service.GetByName("Jack");

            //then
            Assert.Null(result);
        }

        [Fact]
        public void Should_return_true_when_delete_by_name_given_name_is_exist()
        {
            //given
            var service = new PetsService();

            //when
            var result = service.DeleteByName("Tom");

            //then
            Assert.True(result);
        }

        [Fact]
        public void Should_return_null_when_get_by_name_given_name_is_not_exist()
        {
            //given
            var service = new PetsService();

            //when
            var result = service.DeleteByName("Jack");

            //then
            Assert.False(result);
        }

        [Fact]
        public void Should_return_pet_when_modify_pet_given_pet_is_exist()
        {
            //given
            var service = new PetsService();

            //when
            var result = service.ModifyPetPrice("Tom", new PetPriceChangeDto { Price = 150 });

            //then
            Assert.NotNull(result);
            Assert.Equal( 150, result.Price);
        }

        [Fact]
        public void Should_return_pet_when_modify_pet_given_name_is_not_exist()
        {
            //given
            var service = new PetsService();

            //when
            var result = service.ModifyPetPrice("Jack", new PetPriceChangeDto { Price = 150 });

            //then
            Assert.Null(result);
        }
    }
}
