using _Reddit_API.Models;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;

namespace _Reddit_API.Controllers
{
    public class ClientController : ApiController
    {
        public ClientController()
        {
        }

        // GET: api/Client
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/Client/GetTopThreads")]
        public async Task<ThreadWrapper> GetTopThreads()
        {
            var token = await GetToken();
            var threads = await GetThreadsAsync(token);
            var comments = await GetCommentsAsync(token, threads);
            var products = ReturnProducts(comments);
            return products;
        }

        [HttpPost]
        [Route("api/Client/callback")]
        public bool TokenReceiver(TokenResponse tokenResponse)
        {
            var token = tokenResponse.AccessToken;
            if (token != null)
            {
                return true;
            }
            return false;
        }

        public async Task<string> GetToken()
        {
            try
            {
                var client = new HttpClient();
                var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = "https://www.reddit.com/api/v1/access_token",
                    ClientCredentialStyle = ClientCredentialStyle.AuthorizationHeader,
                    ClientId = "uCVrtPfI3OOkyQ",
                    ClientSecret = "EYfI97NNVmMRXncS9NyYGPnAsUg",
                    GrantType = "client_credentials",
                    Scope = "read"
                });
                var respContent = await response.HttpResponse.Content.ReadAsStringAsync();
                if (response.IsError) throw new Exception(response.Error);
                var token = response.AccessToken;

                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ICollection<Product>> GetThreadsAsync(string token)
        {
            try
            {
                var threadLink = "https://oauth.reddit.com/best.json?limit=5";
                var threads = new List<Product>();

                var client = new HttpClient();
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

        public async Task<List<Product>> GetCommentsAsync(string token, ICollection<Product> list)
        {
            try
            {
                var client = new HttpClient();
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

        public ThreadWrapper ReturnProducts(List<Product> list)
        {
            try
            {
                List<EndProduct> endProducts = new List<EndProduct>();
                for (int i = 0; i < 5; i++)
                {
                    var endProduct = new EndProduct();
                    endProduct.Title = list[i].Title;
                    endProduct.Comments = list[i].Comments;
                    endProducts.Add(endProduct);
                }

                var result = new ThreadWrapper();
                var results = new List<EndProduct>();
                foreach (var product in endProducts)
                {
                    results.Add(product);
                }
                result.Threads = results;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

