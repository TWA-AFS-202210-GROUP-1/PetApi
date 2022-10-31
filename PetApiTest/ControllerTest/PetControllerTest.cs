using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using System.Collections.Generic;
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
        var responseBody = await repsonse.Content.ReadAsStringAsync();
        var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
        Assert.Equal(pet, savedPet);
    }
}
