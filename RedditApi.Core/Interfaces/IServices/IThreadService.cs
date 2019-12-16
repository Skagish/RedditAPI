using Microsoft.AspNetCore.Mvc;
using RedditApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedditApi.core.Interfaces.IServices
{
    public interface IThreadService
    {
        Task<List<ThreadWrapper>> GetAllThreads();
        Task<Threads> GetThreads(string id);
        Task<ThreadWrapper> AddThreads();
        void Delete(string id);
    }
}
