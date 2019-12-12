using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Reddit_API.Models;
using Reddit_API.Models.CommentTree;
using Reddit_API.Models.ThreadingTree;

namespace Reddit_API.Controllers
{
    public class ClientController : Controller
    {
        [HttpGet]
        public async Task<ThreadWrapper> GetTopThreads()
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

        private ThreadWrapper ReturnProducts(List<Product> list)
        {
            try
            {
                var result = new ThreadWrapper();
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
                    result.Threads = results;
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}