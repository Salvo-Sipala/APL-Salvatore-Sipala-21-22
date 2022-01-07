using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using FrontEnd.Constants;

namespace FrontEnd.RestControllers
{
    public class LoginRequest
    {
        public static async Task<HttpResponseMessage> DoLogin(string json)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(RestUrl.Base_url + RestUrl.Login_url, content);
            return await Task.FromResult(response);
        }
    }
}
