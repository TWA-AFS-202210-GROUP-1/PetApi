using System.Collections.Generic;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using PetApi.Controllers;
using PetApi.Models;
using PetApi.Services;
using Xunit;

namespace PetApiTest.ControllerTest;

public class PetsControllerTest
{
    [Fact]
    public async void Should_add_new_pet_to_system_successfully()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();

        var pet = new Pet("TestTom", PetType.Cat, "blue", 150);

        //when
        var response = await httpClient.PostAsJsonAsync("api/pets", pet);
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonConvert.DeserializeObject<Pet>(responseContentString);

        //then
        Assert.Equal(pet.Name, responseContent.Name);
    }

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

    [Fact]
    public void Should_return_pet_when_get_all_pet_by_name_given_pet_service_can_find_pet()
    {
        //given
        var perServiceMock = new Mock<IPetsService>();
        perServiceMock.Setup(_ => _.GetByName(It.IsAny<string>())).Returns(new Pet("Tom", PetType.Cat, "blue", 100));
        var controller = new PetsController(perServiceMock.Object);

        //when
        var result = controller.GetByName("Tom") as ObjectResult;

        //then
        Assert.Equal("Tom", (result?.Value as Pet)?.Name);
    }

    [Fact]
    public void Should_return_not_found_when_get_all_pet_by_name_given_service_can_not_find_pet()
    {
        //given
        var perServiceMock = new Mock<IPetsService>();
        perServiceMock.Setup(_ => _.GetByName(It.IsAny<string>())).Returns(It.IsAny<Pet>());
        var controller = new PetsController(perServiceMock.Object);

        //when
        var result = controller.GetByName("Tom");

        //then
        Assert.True(result is NotFoundResult);
    }

    [Fact]
    public void Should_return_no_content_when_delete_pet_by_name_given_service_can_find_pet()
    {
        //given
        var perServiceMock = new Mock<IPetsService>();
        perServiceMock.Setup(_ => _.DeleteByName(It.IsAny<string>())).Returns(true);
        var controller = new PetsController(perServiceMock.Object);

        //when
        var result = controller.DeleteByName("Tom");

        //then
        Assert.True(result is NoContentResult);
    }

    [Fact]
    public void Should_return_not_found_when_delete_pet_by_name_given_service_can_not_find_pet()
    {
        //given
        var perServiceMock = new Mock<IPetsService>();
        perServiceMock.Setup(_ => _.DeleteByName(It.IsAny<string>())).Returns(false);
        var controller = new PetsController(perServiceMock.Object);

        //when
        var result = controller.DeleteByName("Jack");

        //then
        Assert.True(result is NotFoundResult);
    }

    [Fact]
    public void Should_return_pet_modified_found_when_modify_pet_price_given_service_can_not_find_pet()
    {
        //given
        var perServiceMock = new Mock<IPetsService>();
        perServiceMock.Setup(_ => _.ModifyPetPrice(It.IsAny<string>(), It.IsAny<PetPriceChangeDto>())).Returns(new Pet("Tom", PetType.Cat, "blue", 150));
        var controller = new PetsController(perServiceMock.Object);

        //when
        var result = controller.ModifyPetPrice("Tome", new PetPriceChangeDto { Price = 150 });

        //then
        Assert.True(result is ObjectResult);
    }
}