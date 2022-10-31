using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
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

    [Fact]
    public async void Should_add_new_pet_to_system_successfully()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();

        var pet = new Pet("TestTom", PetType.Dog, "blue", 150);
        var petString = SerializeObject(pet);
        //when
        var response = await httpClient.PostAsJsonAsync("api/pets", pet);
        var responseContentString = await response.Content.ReadAsStringAsync();

        //then
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(petString, responseContentString);
    }

    [Fact]
    public async void Should_return_all_pet_when_get_by_type_given_cat()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();
        var cat = new Pet("TestTom", PetType.Cat, "blue", 150);
        var dog = new Pet("TestJack", PetType.Dog, "white", 100);
        await httpClient.DeleteAsync("api/pets/all");
        await httpClient.PostAsJsonAsync("api/pets", cat);
        await httpClient.PostAsJsonAsync("api/pets", dog);
        var petString = SerializeObject(new List<Pet> { cat, dog });

        //when
        var response = await httpClient.GetAsync($"api/pets/all");
        var responseContentString = await response.Content.ReadAsStringAsync();

        //then
        Assert.Equal(petString, responseContentString);
    }

    [Fact]
    public async void Should_return_tom_cat_when_get_by_type_given_cat()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();
        var cat = new Pet("TestTom", PetType.Cat, "blue", 150);
        var dog = new Pet("TestJack", PetType.Dog, "white", 100);
        await httpClient.DeleteAsync("api/pets/all");
        await httpClient.PostAsJsonAsync("api/pets", cat);
        await httpClient.PostAsJsonAsync("api/pets", dog);
        var petString = SerializeObject(cat);

        //when
        var response = await httpClient.GetAsync($"api/pets/TestTom");
        var responseContentString = await response.Content.ReadAsStringAsync();

        //then
        Assert.Equal(petString, responseContentString);
    }

    [Fact]
    public async void Should_return_all_cat_when_get_by_type_given_cat()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();
        var cat = new Pet("TestTom", PetType.Cat, "blue", 150);
        var dog = new Pet("TestJack", PetType.Dog, "white", 100);
        await httpClient.DeleteAsync("api/pets/all");
        await httpClient.PostAsJsonAsync("api/pets", cat);
        await httpClient.PostAsJsonAsync("api/pets", dog);
        var petString = SerializeObject(new List<Pet> { cat });

        //when
        var response = await httpClient.GetAsync($"api/pets/type/{nameof(PetType.Cat)}");
        var responseContentString = await response.Content.ReadAsStringAsync();

        //then
        Assert.Equal(petString, responseContentString);
    }

    [Fact]
    public async void Should_return_no_content_when_delete_by_name_given_name()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();
        var cat = new Pet("TestTom", PetType.Cat, "blue", 150);
        await httpClient.DeleteAsync("api/pets/all");
        await httpClient.PostAsJsonAsync("api/pets", cat);

        //when
        var response = await httpClient.DeleteAsync($"api/pets/TestTom");

        //then
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async void Should_return_1_pet_when_get_by_priceRange_given_120_160()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();
        var pet = new Pet("TestTom", PetType.Dog, "blue", 150);
        await httpClient.DeleteAsync("api/pets/all");
        await httpClient.PostAsJsonAsync("api/pets", pet);
        var petString = SerializeObject(new List<Pet> { pet });

        //when
        var response = await httpClient.GetAsync("api/pets/price/from/120/to/160");
        var responseContentString = await response.Content.ReadAsStringAsync();

        //then
        Assert.Equal(petString, responseContentString);
    }

    [Fact]
    public async void Should_return_1_pet_when_get_by_color_given_blue()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();
        var pet = new Pet("TestTom", PetType.Dog, "blue", 150);
        await httpClient.DeleteAsync("api/pets/all");
        await httpClient.PostAsJsonAsync("api/pets", pet);
        var petString = SerializeObject(new List<Pet> { pet });

        //when
        var response = await httpClient.GetAsync($"api/pets/color/blue");
        var responseContentString = await response.Content.ReadAsStringAsync();

        Assert.Equal(petString, responseContentString);
    }

    [Fact]
    public async void Should_return_modified_pet_when_modify_given_cat()
    {
        //given
        var app = new WebApplicationFactory<Program>();
        var httpClient = app.CreateClient();
        var cat = new Pet("TestTom", PetType.Cat, "blue", 150);
        await httpClient.DeleteAsync("api/pets/all");
        await httpClient.PostAsJsonAsync("api/pets", cat);
        var petString = SerializeObject(new Pet("TestTom", PetType.Cat, "blue", 100));
        var patchDto = new PetPriceChangeDto() { Price = 100 };
        var patchString = SerializeObject(patchDto);

        //when
        var response = await httpClient.PatchAsync("api/pets/TestTom", new StringContent(patchString, Encoding.UTF8, "application/json"));
        var responseContentString = await response.Content.ReadAsStringAsync();

        //then
        Assert.Equal(petString, responseContentString);
    }

    private static string SerializeObject(object obj)
    {
        return JsonConvert.SerializeObject(obj, new StringEnumConverter(new CamelCaseNamingStrategy()));
    }
}