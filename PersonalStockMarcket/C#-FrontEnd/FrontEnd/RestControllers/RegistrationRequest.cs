using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using FrontEnd.Constants;

namespace FrontEnd.RestControllers
{
    public class RegistrationRequest
    {
        public static async Task<HttpResponseMessage> DoRegistration(string json)
        {
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(RestUrl.Base_url + RestUrl.Registration_url, content);
            return await Task.FromResult(response);
        }
    }
}
