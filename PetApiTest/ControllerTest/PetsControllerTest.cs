using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using PetApi.Controllers;
using PetApi.Models;
using PetApi.Services;
using Xunit;

namespace PetApiTest.ControllerTest;

public class PetsControllerTest
{
    [Fact]
    public void Should_return_all_pets_when_get_all_pets_given_pet_service()
    {
        //given
        var perServiceMock = new Mock<IPetsService>();
        perServiceMock.Setup(_ => _.GetAllPets()).Returns(new List<Pet>()
        {
            new ("Spike", PetType.Dog, "white", 100),
            new ("Tom", PetType.Cat, "blue", 100),
        });
        var controller = new PetsController(perServiceMock.Object);

        //when
        var result = controller.GetAll() as ObjectResult;

        //then
        Assert.Equal(2, (result?.Value as IList<Pet>)?.Count);
    }
}