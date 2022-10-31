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
using static PetApi.Model.Pet;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
    [Fact]
    public async void Should_add_new_pet_to_system_successfully()
    {
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync("api/deleteAllPets");

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
        await httpClient.DeleteAsync("api/deleteAllPets");
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000); // construct data
        var serializeObjectPet = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObjectPet, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        await httpClient.PostAsync("/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("api/getAllPets");
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
        await httpClient.DeleteAsync("api/deleteAllPets");
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000); // construct data
        var serializeObjectPet = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObjectPet, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync("api/getPetByName?Name=Kitty");
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
        await httpClient.DeleteAsync("api/deleteAllPets");
        var petHello = new Pet(name: "Hello", type: "dog", color: "white", price: 1000);
        var petKitty = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObjectPetHello = JsonConvert.SerializeObject(petHello);
        var serializeObjectPetKitty = JsonConvert.SerializeObject(petKitty);
        var postBodyHello = new StringContent(serializeObjectPetHello, Encoding.UTF8, "application/json");
        var postBodyKitty = new StringContent(serializeObjectPetKitty, Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/api/addNewPet", postBodyHello);
        await httpClient.PostAsync("/api/addNewPet", postBodyKitty);
        //when
        var deletePet = await httpClient.DeleteAsync("api/deleteBroughtPet?Name=Kitty");
        // then
        deletePet.EnsureSuccessStatusCode();
        var responseDeletePet = await deletePet.Content.ReadAsStringAsync();
        var responseDeletePetBody = JsonConvert.DeserializeObject<Pet>(responseDeletePet);
        Assert.Equal(petKitty, responseDeletePetBody);

        var deletedPet = await httpClient.GetAsync("api/getPetByName?Name=Kitty");
        Assert.Equal("NoContent", deletedPet.StatusCode.ToString());

        var existPet = await httpClient.GetAsync("api/getPetByName?Name=Hello");
        existPet.EnsureSuccessStatusCode();
        var responseExistPet = await existPet.Content.ReadAsStringAsync();
        var responseExistPetBody = JsonConvert.DeserializeObject<Pet>(responseExistPet);
        Assert.Equal(petHello, responseExistPetBody);
    }
}
