using Microsoft.AspNetCore.Mvc;
using PetApi.Model;

namespace PetApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class PetController : ControllerBase
    {
        [HttpPost("addNewPet")]
        public Pet AddNewPet(Pet pet)
        {
            return pet;
        }
    }
}
