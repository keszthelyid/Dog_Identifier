using Dog_Identifier.Data;
using Dog_Identifier.Logic;
using Dog_Identifier.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dog_Identifier.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DogController : ControllerBase
    {
        IDogLogic Logic;
        public DogController(IDogLogic logic)
        {
            Logic = logic;
        }

        [HttpGet]
        public IEnumerable<Dog> GetDogs()
        {
            return Logic.GetDogs();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<DogViewModel> GetDogType([FromBody] DogViewModel model)
        {      
            return await Logic.GetDogType(model); 
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<MixedDogPredictionViewModel> GetMixedDogType([FromBody] DogViewModel model)
        {
            return await Logic.GetMixedDogType(model);
        }
    }
}
