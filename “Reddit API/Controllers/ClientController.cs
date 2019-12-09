using _Reddit_API.Client;
using IdentityModel.Client;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace _Reddit_API.Controllers
{
    public class ClientController : ApiController
    {
        // GET: api/Client
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("api/Client/GetToken")]
        public async Task<string> GetToken()
        {
            var client = new HttpClient();

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = "https://www.reddit.com/api/v1/access_token",

                ClientId = "uCVrtPfI3OOkyQ",
                ClientSecret = "EYfI97NNVmMRXncS9NyYGPnAsUg",
                Scope = "read"
            });
            return Convert.ToString(response);
        }

        [HttpGet]
        [Route("api/Client/Best")]
        public async Task<HttpResponseMessage> Best()
        {
            var client = new HttpClient();
            var topThreads = "http://www.reddit.com/best.json?limit=5";
            var response = await client.GetAsync(topThreads);
            return response;
        }

        // DELETE: api/Client/5
        public void Delete(int id)
        {
        }
    }
}
