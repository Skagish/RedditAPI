using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using RedditApi.Models;
using RedditApi.Models.BsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedditApi.Repositories
{
   
    public class ThreadRepository : IThreadRepository
    {
        private readonly RedditContext _context = null;
        public ThreadRepository(IOptions<RedditDbSettings> settings)
        {
            _context = new RedditContext(settings);
        }

        public async Task AddThreads(ThreadsInBsonWrapper item)
        {
            try
            {
                await _context.ThreadsInBson.InsertOneAsync(item);
            }
            catch (Exception ex)
            {
                //manage the exception
                throw ex;
            }
        }

        public async Task<List<ThreadsInBsonWrapper>> GetAllThreads()
        {
            try
            {
                return await _context.ThreadsInBson
                        .Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                //manage the exception
                throw ex;
            }
        }

        public async Task<ThreadsInBsonWrapper> GetThreads(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return await _context.ThreadsInBson
                                .Find(thread => thread.InternalId == internalId)
                                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                //manage the exception
                throw ex;
            }
        }

        public async Task<ICollection<ThreadsInBsonWrapper>> GetThreads(string bodyText, DateTime updatedFrom, long headerSizeLimit)
        {
            try
            {
                var query = _context.ThreadsInBson.Find(thread => thread.ToString().Contains(bodyText) &&
                                       thread.UpdatedOn >= updatedFrom);

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                //manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveAllThreads()
        {
            try
            {
                DeleteResult actionResult
                    = await _context.ThreadsInBson.DeleteManyAsync(new BsonDocument());

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                //manage the exception
                throw ex;
            }
        }

        public async Task<bool> RemoveThreads(string id)
        {
            try
            {
                DeleteResult actionResult
                    = await _context.ThreadsInBson.DeleteOneAsync(
                        Builders<ThreadsInBsonWrapper>.Filter.Eq("Id", id));

                return actionResult.IsAcknowledged
                    && actionResult.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                //manage the exception
                throw ex;
            }
        }

        public async Task<bool> UpdateThreads(string id, string body)
        {
            try
            {
                ThreadsInBsonWrapper item = JsonConvert.DeserializeObject<ThreadsInBsonWrapper>(body);
                ReplaceOneResult actionResult
                    = await _context.ThreadsInBson
                                    .ReplaceOneAsync(n => n.Id.Equals(id)
                                            , item
                                            , new ReplaceOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                //anage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
    }
}
