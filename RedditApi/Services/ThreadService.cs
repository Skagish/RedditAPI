using MongoDB.Driver;
using RedditApi.Models;
using RedditApi.Models.IRepo;
using RedditApi.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RedditApi.Services
{
    public class ThreadService
    {
        /*private readonly IThreadRepository _repository;

        public ThreadService(IThreadRepository repository)
        {
            _repository = repository;
            //var client = new MongoClient(settings.ConnectionString);
            //var database = client.GetDatabase(settings.DatabaseName);
            //_Threads = database.GetCollection<Threads>(settings.ThreadsCollectionName);
        }

        public List<Threads> Get() =>
            _repository.Find(Threads => true).ToList();

        public Threads Get(string id) =>
            _repository.Find<Threads>(thread => thread.Id == id).FirstOrDefault();

        public Threads Create(Threads thread)
        {
            _repository.InsertOne(thread);
            return thread;
        }

        public void Update(string id, Threads threadsIn) =>
            _repository.ReplaceOne(thread => thread.Id == id, threadsIn);

        public void Remove(Threads threadIn) =>
            _repository.DeleteOne(thread => thread.Id == threadIn.Id);

        public void Remove(string id) =>
            _repository.DeleteOne(thread => thread.Id == id);
*/
    }
}
