using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Xunit;
using PetApi.Model;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
  [Fact]
  public async void Should_add_new_pet_to_system_successfully()
  {
    // given
    var httpClient = await InitializeHttpClient();
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
    var httpClient = await InitializeHttpClient();
    var pets = new List<Pet>
    {
      new Pet("Kitty", "cat", "white", 1000),
      new Pet("Hammer", "dog", "brown", 1500),
    };
    foreach (var pet in pets)
    {
      await CreateTestPet(httpClient, pet);
    }

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
    var httpClient = await InitializeHttpClient();
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
  public async void Should_remove_pet_by_name_from_system_successfully()
  {
    // given
    var httpClient = await InitializeHttpClient();
    var pet = new Pet("Kitty", "cat", "white", 1000);
    await CreateTestPet(httpClient, pet);
    // when
    var deleteResponse = await httpClient.DeleteAsync($"/api/deletePetByName?name={pet.Name}");
    var findPetResponse = await httpClient.GetAsync($"/api/findPetByName?name={pet.Name}");
    // then
    deleteResponse.EnsureSuccessStatusCode();
    Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    Assert.Equal(HttpStatusCode.NotFound, findPetResponse.StatusCode);
  }

  [Fact]
  public async void Should_change_pet_price_successfully()
  {
    // given
    var httpClient = await InitializeHttpClient();
    var pet = new Pet("Kitty", "cat", "white", 1000);
    await CreateTestPet(httpClient, pet);
    var petWithNewPrice = new Pet("Kitty", "cat", "white", 2000);
    var serializedNewPetObject = JsonConvert.SerializeObject(petWithNewPrice);
    var newPostBody = new StringContent(serializedNewPetObject, Encoding.UTF8, "application/json");
    // when
    var response = await httpClient.PutAsync($"/api/changePetProperty?name={petWithNewPrice.Name}", newPostBody);
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(petWithNewPrice.Price, returnedPet.Price);
  }

  [Fact]
  public async void Should_get_pets_by_type_from_system_successfully()
  {
    // given
    var httpClient = await InitializeHttpClient();
    var pets = new List<Pet>
    {
      new Pet("WhiteKitty", "cat", "white", 1000),
      new Pet("PinkKitty", "cat", "pink", 1000),
      new Pet("Hammer", "dog", "brown", 1500),
    };
    foreach (var pet in pets)
    {
      await CreateTestPet(httpClient, pet);
    }

    // when
    var response = await httpClient.GetAsync($"/api/findPetsByType?type={pets[0].Type}");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
    Assert.Equal(pets.Take(2).ToList(), returnedPets);
  }

  [Fact]
  public async void Should_get_pets_by_color_from_system_successfully()
  {
    // given
    var httpClient = await InitializeHttpClient();
    var pets = new List<Pet>
    {
      new Pet("WhiteKitty", "cat", "white", 1000),
      new Pet("PinkKitty", "cat", "pink", 1000),
      new Pet("Hammer", "dog", "white", 1500),
    };
    foreach (var pet in pets)
    {
      await CreateTestPet(httpClient, pet);
    }

    // when
    var response = await httpClient.GetAsync($"/api/findPetsByColor?color={pets[0].Color}");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
    Assert.Equal(new List<Pet> { pets[0], pets[2] }, returnedPets);
  }

  [Fact]
  public async void Should_get_pets_by_price_range_from_system_successfully()
  {
    // given
    var httpClient = await InitializeHttpClient();
    var pets = new List<Pet>
    {
      new Pet("Kitty", "cat", "white", 500),
      new Pet("WhiteKitty", "cat", "white", 1000),
      new Pet("PinkKitty", "cat", "pink", 2000),
      new Pet("Hammer", "dog", "black", 2500),
    };
    foreach (var pet in pets)
    {
      await CreateTestPet(httpClient, pet);
    }

    // when
    var response = await httpClient.GetAsync($"/api/findPetsByPriceRange/priceFrom1000to2000");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
    Assert.Equal(pets.Take(new Range(1, 3)), returnedPets);
  }

  private static async Task<HttpClient> InitializeHttpClient()
  {
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    await httpClient.DeleteAsync("/api/deleteAllPets");

    return httpClient;
  }

  private static async Task<string> CreateTestPet(HttpClient httpClient, Pet pet)
  {
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", postBody);

    return "Test pet created.";
  }
}