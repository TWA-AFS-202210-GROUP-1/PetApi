using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
    [Fact]
    public async void Should_add_new_pet_to_system_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync(requestUri: "/api/deleteAllPets");
        /*
         * Method: post
         * uri: /api/addNewPet
         * body:{
         * "name": "Kitty",
         * "type": "cat",
         * "color" : "white",
         * "price": 1000}
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
        //when
        var repsonse = await httpClient.PostAsync(requestUri: "/api/addNewPet", postBody);
        //then
        repsonse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, repsonse.StatusCode);
    }

    [Fact]
    public async void Should_get_all_pets_of_system_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync(requestUri: "/api/deleteAllPets");
        /*
         * Method: post
         * uri: /api/addNewPet
         * body:{
         * "name": "Kitty",
         * "type": "cat",
         * "color" : "white",
         * "price": 1000}
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
        await httpClient.PostAsync(requestUri: "/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync(requestUri: "/api/getAllPets");
        //then
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async void Should_get_pet_of_system_by_name_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync(requestUri: "/api/deleteAllPets");
        /*
         * Method: post
         * uri: /api/addNewPet
         * body:{
         * "name": "Kitty",
         * "type": "cat",
         * "color" : "white",
         * "price": 1000}
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
        await httpClient.PostAsync(requestUri: "/api/addNewPet", postBody);
        //when
        var response = await httpClient.GetAsync(requestUri: "api/findPetByName?Name=Kitty");
        //then
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var res = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(pet, res);
    }

    [Fact]
    public async void Should_delete_pet_of_system_by_name_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync(requestUri: "/api/deleteAllPets");
        /*
         * Method: post
         * uri: /api/addNewPet
         * body:{
         * "name": "Kitty",
         * "type": "cat",
         * "color" : "white",
         * "price": 1000}
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
        await httpClient.PostAsync(requestUri: "/api/addNewPet", postBody);
        //when
        var response = await httpClient.DeleteAsync(requestUri: "/api/deletePetByName?Name=Kitty");
        //then
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async void Should_update_pet_price_of_system_by_name_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync(requestUri: "/api/deleteAllPets");
        /*
         * Method: post
         * uri: /api/addNewPet
         * body:{
         * "name": "Kitty",
         * "type": "cat",
         * "color" : "white",
         * "price": 1000}
         */
        var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: 1000);
        var serializeObject = JsonConvert.SerializeObject(pet);
        var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
        await httpClient.PostAsync(requestUri: "/api/addNewPet", postBody);
        //when
        var updatedPet = new Pet(name: "Kitty", type: "cat", color: "white", price: 700);
        var updatePetserializeObject = JsonConvert.SerializeObject(pet);
        var updatedPostBody = new StringContent(updatePetserializeObject, Encoding.UTF8, mediaType: "application/json");
        var response = await httpClient.PutAsync(requestUri: "/api/updatePetByName", updatedPostBody);
        //then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async void Should_get_pets_of_system_by_type_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync(requestUri: "/api/deleteAllPets");
        /*
         * Method: post
         * uri: /api/addNewPet
         * body:{
         * "name": "Kitty",
         * "type": "cat",
         * "color" : "white",
         * "price": 1000}
         */
        List<Pet> pets = new List<Pet>()
        {
            new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
            new Pet(name: "Mini", type: "dog", color: "black", price: 500),
            new Pet(name: "Haha", type: "dog", color: "white", price: 800),
        };
        foreach (var pet in pets)
        {
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
            await httpClient.PostAsync(requestUri: "/api/addNewPet", postBody);
        }

        //when
        var response = await httpClient.GetAsync(requestUri: "api/findPetsByType?Type=dog");
        //then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var res = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, res.Count);
    }

    [Fact]
    public async void Should_get_pets_of_system_by_price_range_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync(requestUri: "/api/deleteAllPets");
        /*
         * Method: post
         * uri: /api/addNewPet
         * body:{
         * "name": "Kitty",
         * "type": "cat",
         * "color" : "white",
         * "price": 1000}
         */
        List<Pet> pets = new List<Pet>()
        {
            new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
            new Pet(name: "Mini", type: "dog", color: "black", price: 500),
            new Pet(name: "Haha", type: "dog", color: "white", price: 800),
        };
        foreach (var pet in pets)
        {
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
            await httpClient.PostAsync(requestUri: "/api/addNewPet", postBody);
        }

        //when
        var response = await httpClient.GetAsync(requestUri: "api/findPetsByPriceRange?start=400&&end=600");
        //then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var res = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(500, res[0].Price);
    }

    [Fact]
    public async void Should_get_pets_of_system_by_color_successfully()
    {
        //given
        var application = new WebApplicationFactory<Program>();
        var httpClient = application.CreateClient();
        await httpClient.DeleteAsync(requestUri: "/api/deleteAllPets");
        /*
         * Method: post
         * uri: /api/addNewPet
         * body:{
         * "name": "Kitty",
         * "type": "cat",
         * "color" : "white",
         * "price": 1000}
         */
        List<Pet> pets = new List<Pet>()
        {
            new Pet(name: "Kitty", type: "cat", color: "white", price: 1000),
            new Pet(name: "Mini", type: "dog", color: "black", price: 500),
            new Pet(name: "Haha", type: "dog", color: "white", price: 800),
        };
        foreach (var pet in pets)
        {
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, mediaType: "application/json");
            await httpClient.PostAsync(requestUri: "/api/addNewPet", postBody);
        }

        //when
        var response = await httpClient.GetAsync(requestUri: "api/findPetsByColor?color=white");
        //then
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        var res = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Kitty", res[0].Name);
        Assert.Equal("Haha", res[1].Name);
    }
}
