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
    }
}
