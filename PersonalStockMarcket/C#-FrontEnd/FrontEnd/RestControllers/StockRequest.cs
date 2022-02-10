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
        public static async Task<HttpResponseMessage> GetStockBySymbol(string symbol)
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
            HttpResponseMessage response = await client.GetAsync(RestUrl.Base_url + "/stock_search");
            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> GetMoreInfo()
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
            HttpResponseMessage response = await client.GetAsync(RestUrl.Base_url + "/stock_selected");
            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> FollowStock()
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
            HttpResponseMessage response = await client.GetAsync(RestUrl.Base_url + "/follow_stock");
            return await Task.FromResult(response);
        }

        public static async Task<HttpResponseMessage> GetFavouritesStocks()
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
            HttpResponseMessage response = await client.GetAsync(RestUrl.Base_url + "/get_favourites_stocks");
            return await Task.FromResult(response);
        }

    }
}
