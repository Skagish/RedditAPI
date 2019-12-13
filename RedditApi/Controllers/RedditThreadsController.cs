using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RedditApi.Models;
using RedditApi.Models.BsonModels;
using RedditApi.Models.CommentTree;
using RedditApi.Models.ThreadingTree;
using RedditApi.Repositories;

namespace RedditApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedditThreadsController : ControllerBase
    {
        private readonly IThreadRepository _threadRepo;
        public RedditThreadsController(IThreadRepository threadRepo)
        {
            _threadRepo = threadRepo;
        }
        // GET: api/RedditThreads
        [HttpGet]
        public async Task<List<ThreadWrapper>> GetAllThreads()
        {
            var bsonList = await _threadRepo.GetAllThreads();
            var jsonList = new List<Threads>();
            var jsonThList = new List<ThreadWrapper>();

            for (int i = 0; i < bsonList.Count; i++)
            {
                var json = new Threads();
                var th = new ThreadWrapper();
                var epList = new List<EndProduct>();
                for (int j = 0; j < bsonList[i].ThreadsInBson.Count; j++)
                {
                    var ep = new EndProduct();
                    //json.Id = bsonList[i].Id;
                    ep.Title = bsonList[i].ThreadsInBson[j].Title;
                    ep.Comments = bsonList[i].ThreadsInBson[j].Comments;
                    epList.Add(ep);
                }
                jsonThList.Add(th);
                th.Threads = epList;
                json.threadWrapper = th;
                jsonList.Add(json);
            }
            return jsonThList;
        }

        // GET: api/RedditThreads/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Threads> GetThreads(string id)
        {
            var bson = await _threadRepo.GetThreads(id);
            var json = new Threads();

            for (int i = 0; i < bson.ThreadsInBson.Count; i++)
            {
                json.threadWrapper.Threads[i].Title = bson.ThreadsInBson[i].Title;
                json.threadWrapper.Threads[i].Comments = bson.ThreadsInBson[i].Comments;
            }
            if (json == null)
            {
                return null;
            }
            return json;
        }

        // POST: api/RedditThreads
        [HttpPost]
        public async Task<IActionResult> AddThreads()
        {
            try
            {
                if (await ThreadExists())
                {
                    await _threadRepo.RemoveThreads("1");
                }

                var json = await GetTopThreads();
                var bson = new ThreadsInBsonWrapper();
                var bsonList = new List<ThreadsInBson>();

                for (int i = 0; i < 5; i++)
                {
                    var wrap = new ThreadsInBson();
                    wrap.Title = json.threadWrapper.Threads[i].Title;
                    wrap.Comments = json.threadWrapper.Threads[i].Comments;
                    bsonList.Add(wrap);
                }
                bson.Id = "1";
                bson.ThreadsInBson = bsonList;
                await _threadRepo.AddThreads(bson);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // DELETE: api/RedditThreads/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _threadRepo.RemoveThreads(id);
        }



       private async Task<bool> ThreadExists()
        {
            using var client = new HttpClient();
            var threadLink = "https://localhost:44379/api/RedditThreads/1";
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, threadLink);
            HttpResponseMessage response = await client.SendAsync(request);
            if (response == null)
            {
                return false;
            }
            return true;
        }
        private async Task<Threads> GetTopThreads()
        {
            var token = await GetToken();
            var threads = await GetThreadsAsync(token);
            var comments = await GetCommentsAsync(token, threads);
            var products = ReturnProducts(comments);
            return products;
        }

        private async Task<string> GetToken()
        {
            using var client = new HttpClient();
            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://www.reddit.com/api/v1/access_token",
                ClientCredentialStyle = ClientCredentialStyle.AuthorizationHeader,
                ClientId = "uCVrtPfI3OOkyQ",
                ClientSecret = "EYfI97NNVmMRXncS9NyYGPnAsUg",
                GrantType = "client_credentials",
                Scope = "read"
            });
            if (response.IsError) throw new Exception(response.Error);
            var token = response.AccessToken;
            return token;
        }

        private async Task<ICollection<Product>> GetThreadsAsync(string token)
        {
            try
            {
                using var client = new HttpClient();
                var threadLink = "https://oauth.reddit.com/best.json?limit=5";
                var threads = new List<Product>();

                client.SetBearerToken(token);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, threadLink);
                request.Headers.UserAgent.ParseAdd("Reddit API" + token);
                HttpResponseMessage response = await client.SendAsync(request);
                var resString = response.Content.ReadAsStringAsync().Result;
                Listing deserializedReddit = JsonConvert.DeserializeObject<Listing>(resString);
                foreach (var thread in deserializedReddit.ListingData.Threads)
                {
                    var newProduct = new Product();
                    newProduct.Title = thread.Data.Title;
                    newProduct.Subreddit = thread.Data.Subreddit;
                    threads.Add(newProduct);
                }
                return threads;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<Product>> GetCommentsAsync(string token, ICollection<Product> list)
        {
            try
            {
                using var client = new HttpClient();
                client.SetBearerToken(token);

                var results = new List<Product>();

                foreach (var thread in list)
                {
                    string subreddit = thread.Subreddit;
                    var commentLink = $"https://oauth.reddit.com/r/{subreddit}/comments.json?limit=5";

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, commentLink);
                    request.Headers.UserAgent.ParseAdd("Reddit API" + token);
                    HttpResponseMessage response = await client.SendAsync(request);

                    var resString = response.Content.ReadAsStringAsync().Result;
                    ListingComments deserializedReddit = JsonConvert.DeserializeObject<ListingComments>(resString);

                    ICollection<Comments> coment = new List<Comments>();
                    var th = new Product();
                    th.Title = thread.Title.ToString();

                    foreach (var comment in deserializedReddit.ListingData.Threads)
                    {
                        var com = new Comments();
                        com.Comment = comment.Data.Body;

                        coment.Add(com);
                    }
                    th.Comments = coment;
                    results.Add(th);
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Threads ReturnProducts(List<Product> list)
        {
            try
            {
                var wrap = new ThreadWrapper();
                var result = new Threads();
                List<EndProduct> endProducts = new List<EndProduct>();
                foreach (var item in list)
                {
                    var newList = new List<string>();
                    var endProduct = new EndProduct();
                    endProduct.Title = item.Title;
                    foreach (var comment in item.Comments)
                    {
                        newList.Add(comment.Comment);
                    }
                    endProduct.Comments = newList;
                    endProducts.Add(endProduct);
                    var results = new List<EndProduct>();
                    foreach (var product in endProducts)
                    {
                        results.Add(product);
                    }
                    wrap.Threads = results;
                }
                result.threadWrapper = wrap;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
