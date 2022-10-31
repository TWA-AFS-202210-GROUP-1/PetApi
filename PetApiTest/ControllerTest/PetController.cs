using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace PetApi.Controllers
{
    public class PetController
    {
        [Fact]
        public async void Should_add_new_pet_to_system_successfully()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            /*
             * Method: POST
             * URI: /opi/addNewPet
             * Body:
             * {
             *  "name: "Kitty",
             *  "type":"cat",
             *  "color":"white",
             *  "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: "1000");
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("/api/addNewPet", postBody);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async void Should_get_all_pets()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            /*
             * Method: GET
             * URI: /api/getAllPets
             * Body:
             * {
             *  "name: "Kitty",
             *  "type":"cat",
             *  "color":"white",
             *  "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: "1000");
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            //when
            var response = await httpClient.GetAsync("api/getAllPets");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, savedPet[0]);
        }

        [Fact]
        public async void Should_get_pet_by_name()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            /*
             * Method: GET
             * URI: /api/getAllPets
             * Body:
             * {
             *  "name: "Kitty",
             *  "type":"cat",
             *  "color":"white",
             *  "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: "1000");
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            //when
            var response = await httpClient.GetAsync("api/getPet/?name=Kitty");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async void Should_get_item_off_shelf()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            /*
             * Method: GET
             * URI: /api/getAllPets
             * Body:
             * {
             *  "name: "Kitty",
             *  "type":"cat",
             *  "color":"white",
             *  "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: "1000");
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            //when
            var response = await httpClient.DeleteAsync("api/deletePet/?name=Kitty&type=cat&color=white&price=1000");
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var pets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            List<Pet> emptypets = new List<Pet>();
            Assert.Equal(emptypets, pets);
        }

        [Fact]
        public async void Should_modify_the_price()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            /*
             * Method: GET
             * URI: /api/getAllPets
             * Body:
             * {
             *  "name: "Kitty",
             *  "type":"cat",
             *  "color":"white",
             *  "price": 1000
             * }
             */
            var pet = new Pet(name: "Kitty", type: "cat", color: "white", price: "1000");
            var serializeObject = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            //when
            var pet1 = new Pet(name: "Kitty", type: "cat", color: "white", price: "500");
            var serializeObject1 = JsonConvert.SerializeObject(pet1);
            var postBody1 = new StringContent(serializeObject1, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("api/changePetPrice/", postBody1);
            //then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var changedpet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal("500", changedpet.Price);
        }
    }
}