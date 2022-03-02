using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Essentials;
using FrontEnd.Constants;

namespace FrontEnd.RestControllers
{
    public class StockRequest
    {
        public static async Task<HttpResponseMessage> GetStockBySymbol(string json_symbol)
        {
            string token;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json_symbol, Encoding.UTF8, "application/json");

            try
            {
                token = await SecureStorage.GetAsync("token");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            HttpResponseMessage response = await client.PutAsync(RestUrl.Base_url + "/stock_search", content);
            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> GetMoreInfo(string json_symbol)
        {
            string token;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(json_symbol, Encoding.UTF8, "application/json");

            try
            {
                token = await SecureStorage.GetAsync("token");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            HttpResponseMessage response = await client.PutAsync(RestUrl.Base_url + "/get_more_info", content);
            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> FollowStock(string followed_stock)
        {
            string token;
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(followed_stock, Encoding.UTF8, "application/json");

            try
            {
                token = await SecureStorage.GetAsync("token");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            HttpResponseMessage response = await client.PutAsync(RestUrl.Base_url + "/follow_stock", content);
            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> GetUserFavouritesStocks()
        {
            string token;
            HttpClient client = new HttpClient();

            try
            {
                token = await SecureStorage.GetAsync("token");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);
            HttpResponseMessage response = await client.GetAsync(RestUrl.Base_url + "/get_user_favourites_stocks");
            return await Task.FromResult(response);
        }

    }
}
