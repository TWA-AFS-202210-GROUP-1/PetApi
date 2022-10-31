using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Xunit;
using PetApi.Model;
using System.Collections.Generic;

namespace PetApiTest.ControllerTest;

public class PetControllerTest
{
  [Fact]
  public async void Should_add_new_pet_to_system_successfully()
  {
    // given
    var application = new WebApplicationFactory<Program>();
    var httpClient = application.CreateClient();
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
}