using Dog_Identifier.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dog_Identifier.Logic
{
    public interface IDogLogic
    {
        IEnumerable<Dog> GetDogs();
        Task<DogViewModel> GetDogType(DogViewModel model);
        Task<MixedDogPredictionViewModel> GetMixedDogType(DogViewModel model);
    }
}