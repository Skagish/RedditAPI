using _Reddit_API.Client;
using _Reddit_API.Models;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace _Reddit_API.Controllers
{
    public class ClientController : ApiController
    {
        private static readonly XmlObjectSerializer Serializer = new DataContractJsonSerializer(typeof(Thread));
        // GET: api/Client
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/Client/GetTopThreads")]
        public async Task<HttpResponseMessage> GetTopThreads()
        {
            var token = await GetToken();
            var threads = GetThreadsAsync(token);
            return await threads;
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
                //var custom = response.Json.TryGetString("title");
                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetThreadsAsync(string token)
        {
            var threadLink = "https://oauth.reddit.com/best.json?limit=5";
            var commentLink = "https://oauth.reddit.com/r/pics/comments.json?limit=2";
            var list = new List<Thread>();

            var client = new HttpClient();
            client.SetBearerToken(token);
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, threadLink);
            request.Headers.UserAgent.ParseAdd("Reddit API" + token);
            HttpResponseMessage response = await client.SendAsync(request);

            



            return response;
        }

        /*public async Task<ICollection<string>> GetComments(List<Thread> threadList)
        {
            for (int i = 0; i < threadList.Count; i++)
            {
                var commentLink = "https://oauth.reddit.com/r/{subreddit}/comments/{thread_id";
                var client = new HttpClient();
                var newToken = await GetToken();
                client.SetBearerToken(newToken);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, commentLink);
                request.Headers.UserAgent.ParseAdd("Reddit API" + newToken);
                HttpResponseMessage response = await client.SendAsync(request);
            }
        }*/
    }
}

