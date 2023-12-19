using Dog_Identifier.Data;
using Dog_Identifier.Models;
using MongoDB.Driver;

namespace Dog_Identifier.Repository
{
    public class DogRepository : IDogRepository
    {
        private DogDbContext db;

        public DogRepository(DogDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<Dog> GetAllDogs()
        {
            FilterDefinition<Dog> filterDefinition = Builders<Dog>.Filter.Empty;
            return db.Dogs.Find(filterDefinition).ToList();
        }

        public Dog GetDogByName(string name)
        {
            FilterDefinition<Dog> filterDefinition = Builders<Dog>.Filter.Eq(x => x.Name, name);
            return db.Dogs.Find(filterDefinition).FirstOrDefault();
        }
    }
}
