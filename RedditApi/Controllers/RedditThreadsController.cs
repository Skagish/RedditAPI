using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RedditApi.core.Interfaces.IServices;
using RedditApi.Models;

namespace RedditApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedditThreadsController : ControllerBase
    {
        private readonly IThreadService _threadService;
        public RedditThreadsController(IThreadService threadService)
        {
            _threadService = threadService;
        }
        // GET: api/RedditThreads
        [HttpGet]
        public async Task<List<ThreadWrapper>> GetAllThreads()
        {
            return await _threadService.GetAllThreads();
        }

        // GET: api/RedditThreads/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetThreads(string id)
        {

            var threads = await _threadService.GetThreads(id);
            if (threads == null)
            {
                return NotFound($"Object Does not Exist With ID : {id}");
            }
            else
            {
                return Ok(threads);
            }

        }

        // POST: api/RedditThreads
        [HttpPost]
        public async Task<IActionResult> AddThreads()
        {
            await _threadService.AddThreads();
            return Ok();
        }

        // DELETE: api/RedditThreads/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _threadService.Delete(id);
        }
    }
}
