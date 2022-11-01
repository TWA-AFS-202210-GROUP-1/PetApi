using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PetApi.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace PetApiTest.ControllerTest
{
    public class PetControllerTest
    {
        [Fact]
        public async void Should_add_a_pet_to_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            _ = httpClient.DeleteAsync("/api/deleteAllItem");

            Pet pet = new Pet(name: "Hich", type: "dog", color: "yellow", price: 5000);
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");

            //when
            var response = await httpClient.PostAsync("/api/addNewPet", postBody);
            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(pet, savedPet);
        }

        [Fact]
        public async void Should_get_all_pets_from_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            _ = httpClient.DeleteAsync("/api/deleteAllItem");

            Pet pet = new Pet(name: "Hich", type: "dog", color: "yellow", price: 5000);
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);

            //when
            var response = await httpClient.GetAsync("/api/getAllPets");

            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            //var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);

            var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, allPets[0]);
        }

        [Fact]
        public async void Should_get_one_pet_by_its_name_from_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            _ = httpClient.DeleteAsync("/api/deleteAllItem");

            Pet pet = new Pet(name: "Hich", type: "dog", color: "yellow", price: 5000);
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            string needToFindName = "Hich";

            //when
            var response = await httpClient.GetAsync($"/api/getPetByName?name={needToFindName}");

            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var findPet = JsonConvert.DeserializeObject<Pet>(responseBody);

            //var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, findPet);
        }

        [Fact]
        public async void Should_delete_pet_by_its_name_from_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            _ = httpClient.DeleteAsync("/api/deleteAllItem");

            Pet pet = new Pet(name: "Hich", type: "dog", color: "yellow", price: 5000);
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            string needToDeleteName = "Hich";

            //when
            var response = await httpClient.DeleteAsync($"/api/deletePetByName?name={needToDeleteName}");
            var responseAfterDelete = await httpClient.GetAsync($"/api/getPetByName?name={needToDeleteName}");

            // then
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.InternalServerError, responseAfterDelete.StatusCode);
        }

        [Fact]
        public async void Should_modify_pet_price_by_its_name_from_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            _ = httpClient.DeleteAsync("/api/deleteAllItem");

            Pet pet = new Pet(name: "Hich", type: "dog", color: "yellow", price: 5000);
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            string needToModifyPetName = "Hich";
            int needToModifyPetPrice = 10000;

            //when
            var response = await httpClient.PatchAsync($"/api/updataPetPetByName?name={needToModifyPetName}&price={needToModifyPetPrice}", null);

            // then
            var responseBody = await response.Content.ReadAsStringAsync();
            var updataPet = JsonConvert.DeserializeObject<Pet>(responseBody);
            Assert.Equal(needToModifyPetPrice, updataPet.Price);
        }

        [Fact]
        public async void Should_get_one_pet_by_its_type_from_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            _ = httpClient.DeleteAsync("/api/deleteAllItem");

            Pet pet = new Pet(name: "Hich", type: "dog", color: "yellow", price: 5000);
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            string needToFindType = "dog";

            //when
            var response = await httpClient.GetAsync($"/api/getPetByType?type={needToFindType}");

            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var findPet = JsonConvert.DeserializeObject<List<Pet>>(responseBody);

            //var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, findPet[0]);
        }

        [Fact]
        public async void Should_get_one_pet_by_its_price_range_from_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            _ = httpClient.DeleteAsync("/api/deleteAllItem");

            Pet pet = new Pet(name: "Hich", type: "dog", color: "yellow", price: 5000);
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            int minPrice = 2000;
            int maxPrice = 7000;

            //when
            var response = await httpClient.GetAsync($"/api/getPetByPriceRange?minPrice={minPrice}&maxPrice={maxPrice}");

            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var findPet = JsonConvert.DeserializeObject<List<Pet>>(responseBody);

            //var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, findPet[0]);
        }

        [Fact]
        public async void Should_get_one_pet_by_its_color_from_system()
        {
            //given
            var application = new WebApplicationFactory<Program>();
            var httpClient = application.CreateClient();
            _ = httpClient.DeleteAsync("/api/deleteAllItem");

            Pet pet = new Pet(name: "Hich", type: "dog", color: "yellow", price: 5000);
            var petJson = JsonConvert.SerializeObject(pet);
            var postBody = new StringContent(petJson, Encoding.UTF8, "application/json");
            await httpClient.PostAsync("/api/addNewPet", postBody);
            string needToFindColor = "yellow";

            //when
            var response = await httpClient.GetAsync($"/api/getPetByColor?color={needToFindColor}");

            // then
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var findPet = JsonConvert.DeserializeObject<List<Pet>>(responseBody);

            //var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
            Assert.Equal(pet, findPet[0]);
        }
    }
}
