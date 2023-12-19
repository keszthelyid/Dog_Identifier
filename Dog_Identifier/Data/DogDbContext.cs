using Dog_Identifier.Models;
using MongoDB.Driver;

namespace Dog_Identifier.Data
{
    public class DogDbContext
    {
        // The application uses MongoDB

        public IMongoCollection<Dog> Dogs { get; set; }
        public DogDbContext()
        {
            string connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;
            string databaseName = MongoUrl.Create(connectionstring).DatabaseName;
            MongoClient mongoClient = new MongoClient(connectionstring);
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            Dogs = database.GetCollection<Dog>("Dogs");
        }
        
    }
}
