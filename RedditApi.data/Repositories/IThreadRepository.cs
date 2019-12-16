using RedditApi.Models.BsonModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditApi.Repositories
{
    public interface IThreadRepository
    {
        Task<List<ThreadsInBsonWrapper>> GetAllThreads();
        Task<ThreadsInBsonWrapper> GetThreads(string id);

        // query after multiple parameters
        Task<ICollection<ThreadsInBsonWrapper>> GetThreads(string bodyText, DateTime updatedFrom, long headerSizeLimit);

        // add new note document
        Task AddThreads(ThreadsInBsonWrapper item);

        // remove a single document
        Task<bool> RemoveThreads(string id);

        // update just a single document
        Task<bool> UpdateThreads(string id, string body);

        // should be used with high cautious
        Task<bool> RemoveAllThreads();
    }
}
