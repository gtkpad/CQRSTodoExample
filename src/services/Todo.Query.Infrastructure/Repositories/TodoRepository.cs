using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Todo.Query.Domain.Repositories;
using Todo.Query.Infrastructure.Config;

namespace Todo.Query.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{    private readonly IMongoCollection<Domain.Entities.Todo> _eventStoreCollection;
 
     public TodoRepository(IOptions<MongodbConfig> config)
     {
         var mongoClient = new MongoClient(config.Value.ConnectionString);
         var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);
         _eventStoreCollection = mongoDatabase.GetCollection<Domain.Entities.Todo>(nameof(Domain.Entities.Todo));
     }


     public async Task Create(Domain.Entities.Todo todo)
     {
         await _eventStoreCollection.InsertOneAsync(todo);
     }

     public async Task Update(Domain.Entities.Todo todo)
     {
         var update = Builders<Domain.Entities.Todo>.Update
             .Set(x => x.Done, todo.Done)
             .Set(x => x.Name, todo.Name);
         await _eventStoreCollection.UpdateOneAsync(x => x.Id == todo.Id, update);
     }

     public async Task<IList<Domain.Entities.Todo>> GetAll()
     {
         return await _eventStoreCollection.Find(_ => true).ToListAsync();
     }

     public async Task<Domain.Entities.Todo> GetById(Guid id)
     {
            return await _eventStoreCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
     }

     public async Task<IList<Domain.Entities.Todo>> GetUndone()
     {
         return await _eventStoreCollection.Find(x => x.Done == false).ToListAsync();
     }

     public async Task<IList<Domain.Entities.Todo>> GetDone()
     {
            return await _eventStoreCollection.Find(x => x.Done == true).ToListAsync();
     }
}