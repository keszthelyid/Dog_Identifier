using Dog_Identifier.Models;

namespace Dog_Identifier.Repository
{
    public interface IDogRepository
    {
        IEnumerable<Dog> GetAllDogs();
        Dog GetDogByName(string name);
    }
}