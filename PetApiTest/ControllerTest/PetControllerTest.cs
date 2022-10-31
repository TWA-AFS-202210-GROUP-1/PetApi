using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Xunit;
using PetApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
  [Fact]
  public async void Should_add_new_pet_to_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var pet = new Pet("Kitty", "cat", "white", 1000);
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    // when
    var response = await httpClient.PostAsync("/api/addNewPet", postBody);
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(pet, returnedPet);
  }

  [Fact]
  public async void Should_get_all_pets_from_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var pets = new List<Pet>
    {
      new Pet("Kitty", "cat", "white", 1000),
      new Pet("Hammer", "dog", "brown", 1500),
    };
    var serializedCatObject = JsonConvert.SerializeObject(pets[0]);
    var serializedDogObject = JsonConvert.SerializeObject(pets[1]);
    var catPostBody = new StringContent(serializedCatObject, Encoding.UTF8, "application/json");
    var dogPostBody = new StringContent(serializedDogObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", catPostBody);
    await httpClient.PostAsync("/api/addNewPet", dogPostBody);

    // when
    var response = await httpClient.GetAsync("/api/getAllPets");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
    Assert.Equal(pets, allPets);
  }

  [Fact]
  public async void Should_get_pet_by_name_from_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    var pet = new Pet("Kitty", "cat", "white", 1000);
    await CreateTestPet(httpClient, pet);
    // when
    var response = await httpClient.GetAsync($"/api/findPetByName?name={pet.Name}");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(pet, returnedPet);
  }

  [Fact]
  public async void Should_delete_pet_by_name_from_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    var pet = new Pet("Kitty", "cat", "white", 1000);
    await CreateTestPet(httpClient, pet);
    // when
    var response = await httpClient.DeleteAsync("/api/deletePetByName?name=Kitty");
    // then
    response.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
  }

  [Fact]
  public async void Should_change_pet_price_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    var pet = new Pet("Kitty", "cat", "white", 1000);
    await CreateTestPet(httpClient, pet);
    var newPet = new Pet("Kitty", "cat", "white", 2000);
    var serializedNewPetObject = JsonConvert.SerializeObject(newPet);
    var newPostBody = new StringContent(serializedNewPetObject, Encoding.UTF8, "application/json");
    // when
    var response = await httpClient.PutAsync($"/api/changePetProperty?name={newPet.Name}", newPostBody);
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(newPet, returnedPet);
  }

  [Fact]
  public async void Should_get_pet_by_type_from_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    var pet = new Pet("Kitty", "cat", "white", 1000);
    await CreateTestPet(httpClient, pet);
    // when
    var response = await httpClient.GetAsync($"/api/findPetByType?type={pet.Type}");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(pet, returnedPet);
  }

  [Fact]
  public async void Should_get_pet_by_color_from_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    var pet = new Pet("Kitty", "cat", "white", 1000);
    await CreateTestPet(httpClient, pet);
    // when
    var response = await httpClient.GetAsync($"/api/findPetByColor?color={pet.Color}");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(pet, returnedPet);
  }

  [Fact]
  public async void Should_get_pet_by_price_range_from_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    var pet = new Pet("Kitty", "cat", "white", 1000);
    await CreateTestPet(httpClient, pet);
    // when
    var response = await httpClient.GetAsync($"/api/findPetByPriceRange/priceFrom0to2000");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(pet, returnedPet);
  }

  private static async Task<string> CreateTestPet(HttpClient httpClient, Pet pet)
  {
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", postBody);

    return "Test pet created.";
  }
}