using PetApi.Services;
using Xunit;

namespace PetApiTest.ServiceTest
{
    public class PetsServiceTest
    {
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
    }
}
