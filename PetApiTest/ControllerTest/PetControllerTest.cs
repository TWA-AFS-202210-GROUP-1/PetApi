using System.Drawing;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
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
}
