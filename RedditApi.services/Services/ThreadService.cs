using RedditApi.core.Interfaces.IServices;
using RedditApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedditApi.Repositories;
using RedditApi.Models.BsonModels;
using System;
using System.Net.Http;
using IdentityModel.Client;
using RedditApi.Models.ThreadingTree;
using Newtonsoft.Json;
using RedditApi.Models.CommentTree;
using RedditApi.Services.Services;

namespace RedditApi.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IThreadRepository _threadRepo;
        private readonly IMyLogger _logger;

        public ThreadService(IThreadRepository threadRepo, IMyLogger logger)
        {
            _threadRepo = threadRepo;
            _logger = logger;

        }
        public async Task<ThreadWrapper> AddThreads()
        {
            try
            {
                _logger.Logger().Info("Adding Object to DataBase");
                if (await ThreadExists("1"))
                {
                    await _threadRepo.RemoveThreads("1");
                }

                var json = await GetTopThreads();
                var bson = new ThreadsInBsonWrapper();
                var bsonList = new List<ThreadsInBson>();

                for (int i = 0; i < 5; i++)
                {
                    var wrap = new ThreadsInBson();
                    wrap.Title = json.ThreadWrapper.Threads[i].Title;
                    wrap.Comments = json.ThreadWrapper.Threads[i].Comments;
                    bsonList.Add(wrap);
                }
                bson.Id = "1";
                bson.ThreadsInBson = bsonList;
                await _threadRepo.AddThreads(bson);
                _logger.Logger().Info("Added Object to DataBase");
                return json.ThreadWrapper;
            }
            catch (Exception e)
            {
                _logger.Logger().Fatal(e, "Failed To Communicate To Database.");
                throw;
            }
        }

        public async void Delete(string id)
        {
            try
            {
                _logger.Logger().Info("Deleting Threads List by ID: {id}", id);
                await _threadRepo.RemoveThreads(id);
                if (_threadRepo.GetThreads(id) == null)
                {
                    _logger.Logger().Warn("Failed To Delete Object From Database.");
                }
            }
            catch (Exception e)
            {
                _logger.Logger().Fatal(e, "Failed To Communicate To Database. {e}");

                throw e;
            }
        }

        public async Task<List<ThreadWrapper>> GetAllThreads()
        {
            try
            {
                _logger.Logger().Info("Returning All Threads Lists");
                var bsonList = await _threadRepo.GetAllThreads();
                var jsonList = new List<Threads>();
                var jsonThList = new List<ThreadWrapper>();

                if (bsonList.Count == 0)
                {
                    var list = new List<ThreadWrapper>();
                    list.Add(await AddThreads());
                    return list;
                }

                for (int i = 0; i < bsonList.Count; i++)
                {
                    if (await ThreadExists(bsonList[i].Id))
                    {
                        TimeSpan ts = DateTime.Now.ToLocalTime() - bsonList[i].UpdatedOn.ToLocalTime();
                        if (ts.TotalMinutes < 5 || bsonList != null)
                        {
                            var json = new Threads();
                            var th = new ThreadWrapper();
                            var epList = new List<EndProduct>();
                            for (int j = 0; j < 5; j++)
                            {
                                var ep = new EndProduct();
                                ep.Title = bsonList[i].ThreadsInBson[j].Title;
                                ep.Comments = bsonList[i].ThreadsInBson[j].Comments;
                                epList.Add(ep);
                            }
                            jsonThList.Add(th);
                            th.Threads = epList;
                            json.ThreadWrapper = th;
                            jsonList.Add(json);
                        }
                        else
                        {
                            _logger.Logger().Info("Updating Threads List");
                            var list = new List<ThreadWrapper>();
                            list.Add(await AddThreads());
                            return list;
                        }
                    }
                }
                return jsonThList;
            }
            catch (Exception e)
            {
                _logger.Logger().Fatal(e, "Failed To Get All Thread Lists And Update If Needed. {e}");
                throw;
            }
        }

        public async Task<Threads> GetThreads(string id)
        {
            try
            {
                _logger.Logger().Info("Returning Threads List by ID: {id}", id);
                var bson = await _threadRepo.GetThreads(id);
                var json = new Threads();
                var threadW = new ThreadWrapper();
                var list = new List<EndProduct>();
                if (bson != null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var prod = new EndProduct();
                        prod.Title = bson.ThreadsInBson[i].Title;
                        prod.Comments = bson.ThreadsInBson[i].Comments;
                        list.Add(prod);
                    }
                    threadW.Threads = list;
                    json.ThreadWrapper = threadW;
                    json.UpdatedOn = bson.UpdatedOn;
                    return json;
                }
                else
                {
                    _logger.Logger().Warn("Object Not Found In Database");
                    return null;
                }

            }
            catch (Exception e)
            {
                _logger.Logger().Fatal(e, "Failed To Aquire Response List From Database. {e}");
                throw;
            }
        }

        private async Task<bool> ThreadExists(string id)
        {
            var response = await GetThreads(id);
            if (response == null)
            {
                return false;
            }
            return true;
        }
        private async Task<Threads> GetTopThreads()
        {
            _logger.Logger().Info("Returning Top Threads From Reddit");
            var token = await GetToken();
            var threads = await GetThreadsAsync(token);
            var comments = await GetCommentsAsync(token, threads);
            var products = ReturnProducts(comments);
            return products;
        }

        private async Task<string> GetToken()
        {
            try
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
            catch (Exception)
            {
                _logger.Logger().Fatal("Failed To Aquire Tokken For Reddit Registration");
                throw;
            }
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
                _logger.Logger().Fatal("Failed To Return Title & Subreddit From Reddit");
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
                _logger.Logger().Fatal("Failed To Return Comments From Reddit");
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
                result.ThreadWrapper = wrap;
                return result;
            }
            catch (Exception)
            {
                _logger.Logger().Fatal("Failed To Return Product From Reddit");
                throw;
            }
        }
    }
}
