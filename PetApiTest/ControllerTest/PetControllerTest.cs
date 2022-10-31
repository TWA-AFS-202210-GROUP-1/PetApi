using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using Xunit;
using Xunit.Sdk;
using static PetApi.Model.Pet;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
    [Fact]
    public async void Should_add_new_pet_to_system_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");

        /*
         * Method: POST
         * URI: /api/addNewPet
         * Body:
         * {
         *  "name": "Kitty",
         *  "type": "cat",
         *  "color": "white",
         *  "price":100,
         *}
         */

        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000); // construct data
        var serializeObjectPet = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObjectPet, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/api/addNewPet", postBody);
        // then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet, savedPet);
    }

    [Fact]
    public async void Should_get_all_pets_of_system_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000); // construct data
        var serializeObjectPet = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObjectPet, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        await httpClient.PostAsync("/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("/api/getAllPets");
        // then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(pet, allPets[1]);
    }

    [Fact]
    public async void Should_get_kitty_of_system_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000); // construct data
        var serializeObjectPet = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObjectPet, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("/api/getPetByName?Name=Kitty");
        // then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var targetPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet, targetPet);
    }

    [Fact]
    public async void Should_delete_brought_pet_of_system()
    {
        // given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var petKitty = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObjectPetKitty = JsonConvert.SerializeObject(petKitty);
        var postBodyKitty = new StringContent(serializeObjectPetKitty, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBodyKitty);
        //when
        var deletePet = await httpClient.DeleteAsync("/api/deleteBroughtPet?Name=Kitty");
        // then
        var responseDeletePet = await deletePet.Content.ReadAsStringAsync();
        var responseDeletePetBody = JsonConvert.DeserializeObject<Pet>(responseDeletePet);
        Assert.Equal(petKitty, responseDeletePetBody);
        Assert.Equal("OK", deletePet.StatusCode.ToString());

        var deletedPet = await httpClient.GetAsync("/api/getPetByName?Name=Kitty");
        Assert.Equal("NoContent", deletedPet.StatusCode.ToString());
    }

    [Fact]
    public async void Should_return_null_when_can_not_find_delete_pet_name()
    {
        // given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var petKitty = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObjectPetKitty = JsonConvert.SerializeObject(petKitty);
        var postBodyKitty = new StringContent(serializeObjectPetKitty, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBodyKitty);
        //when
        var response = await httpClient.DeleteAsync("/api/deleteBroughtPet?Name=Hello");
        // then
        Assert.Equal("NoContent", response.StatusCode.ToString());
    }

    [Fact]
    public async void Should_modify_the_price_of_a_pet()
    {
        // given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var petKitty = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObjectPetKitty = JsonConvert.SerializeObject(petKitty);
        var postBody = new StringContent(serializeObjectPetKitty, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);

        var modifiedPet = new Pet(name: "Kitty", type: "cat", color: "white", price: 700);
        var serializeObjectPet = JsonConvert.SerializeObject(modifiedPet);
        var modifiedPostBody = new StringContent(serializeObjectPet, Encoding.UTF8, "application/json");
        //when
        var responseMessage = await httpClient.PutAsync("/api/modifyPetByNameAndPrice?Name=Kitty&&Price=700", modifiedPostBody);
        // then
        responseMessage.EnsureSuccessStatusCode();
        var responseBody = await responseMessage.Content.ReadAsStringAsync();
        var targetPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(modifiedPet, targetPet);
    }

    [Fact]
    public async void Should_return_pets_by_type()
    {
        // given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("/api/deleteAllPets");
        var pets = new List<Pet>()
        {
            new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
            new Pet(name: "Doggie", type: "dog", color: "yellow", price: 900),
            new Pet(name: "Hello", type: "cat", color: "blue", price: 800),
        };

        foreach (var pet in pets)
        {
            var serializeObjectPet = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObjectPet, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
        }

        // when
        var responseMessage = await httpClient.GetAsync("api/findPetsByType?Type=cat");

        // then
        var findPets = new List<Pet>()
        {
            new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
            new Pet(name: "Hello", type: "cat", color: "blue", price: 800),
        };

        responseMessage.EnsureSuccessStatusCode();
        var responseBody = await responseMessage.Content.ReadAsStringAsync();
        var targetPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(findPets, targetPets);
    }

    [Fact]
    public async void Get_pets_by_price_range()
    {
        // given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("api/deleteAllPets");
        var pets = new List<Pet>()
        {
            new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
            new Pet(name: "Doggie", type: "dog", color: "yellow", price: 900),
            new Pet(name: "Hello", type: "cat", color: "blue", price: 800),
        };

        foreach (var pet in pets)
        {
            var serializeObjectPet = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObjectPet, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
        }

        // when
        var priceRange = new List<int>() { 900, 1000 };
        var serializeObjectPriceRange = JsonConvert.SerializeObject(priceRange);
        var postBodyKitty = new StringContent(serializeObjectPriceRange, Encoding.UTF8, "application/json");
        var responseMessage = await httpClient.GetAsync("/api/findPetsByPriceRange/900_1000");

        // then
        var findPets = new List<Pet>()
        {
            new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
            new Pet(name: "Doggie", type: "dog", color: "yellow", price: 900),
        };

        responseMessage.EnsureSuccessStatusCode();
        var responseBody = await responseMessage.Content.ReadAsStringAsync();
        var targetPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(findPets, targetPets);
    }
}
