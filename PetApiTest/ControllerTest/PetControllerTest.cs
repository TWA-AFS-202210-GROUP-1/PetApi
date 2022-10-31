using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Xunit;
using PetApi.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
    var savedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(pet, savedPet);
  }

  [Fact]
  public async void Should_get_all_pets_from_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var pet = new Pet("Kitty", "cat", "white", 1000);
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", postBody);
    // when
    var response = await httpClient.GetAsync("/api/getAllPets");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var allPets = JsonConvert.DeserializeObject<List<Pet>>(responseBody);
    Assert.Equal(pet, allPets[0]);
  }

  [Fact]
  public async void Should_get_pet_by_name_from_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var pet = new Pet("Kitty", "cat", "white", 1000);
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", postBody);
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
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var pet = new Pet("Kitty", "cat", "white", 1000);
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", postBody);
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
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var pet = new Pet("Kitty", "cat", "white", 1000);
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", postBody);
    var newPet = new Pet("Kitty", "cat", "white", 2000);
    var serializedNewPetObject = JsonConvert.SerializeObject(newPet);
    var newPostBody = new StringContent(serializedNewPetObject, Encoding.UTF8, "application/json");
    // when
    var response1 = await httpClient.GetAsync($"/api/findPetByName?name={newPet.Name}");
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
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var pet = new Pet("Kitty", "cat", "white", 1000);
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", postBody);
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
    await httpClient.DeleteAsync("/api/deleteAllPets");
    var pet = new Pet("Kitty", "cat", "white", 1000);
    var serializedObject = JsonConvert.SerializeObject(pet);
    var postBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
    await httpClient.PostAsync("/api/addNewPet", postBody);
    // when
    var response = await httpClient.GetAsync($"/api/findPetByColor?color={pet.Color}");
    // then
    response.EnsureSuccessStatusCode();
    var responseBody = await response.Content.ReadAsStringAsync();
    var returnedPet = JsonConvert.DeserializeObject<Pet>(responseBody);
    Assert.Equal(pet, returnedPet);
  }
}