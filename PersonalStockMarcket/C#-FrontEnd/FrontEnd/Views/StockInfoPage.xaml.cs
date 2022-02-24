using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FrontEnd.Models;
using FrontEnd.RestControllers;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace FrontEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockInfoPage : ContentPage
    {
        public List<Stock> Page_stock { get; set; }
        public StockInfoPage(List<Stock> page_stock)
        {
            Page_stock = page_stock;
            InitializeComponent();

            // passing list of stock to binding to the collection, to fill the page
            InfoCollection.ItemsSource = new List<Stock>(Page_stock);
        }


        private async void OnFollowButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string symbol = (string)button.CommandParameter;
            string email = await SecureStorage.GetAsync("email");
            FollowedStock followedStock = new FollowedStock(email, symbol);
            //FollowedStock followedStock = new FollowedStock(email, Page_stock[0], symbol);

            string json = JsonConvert.SerializeObject(followedStock);
            HttpResponseMessage response = await StockRequest.FollowStock(json);

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                await DisplayAlert("Done!", "Stock Followed!", "OK");
                //FavouritesStock = JsonConvert.DeserializeObject<List<Stock>>(response_content);
                //await Navigation.PushAsync(new FavouritesStockPage(SearchedStock));
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                {
                    await DisplayAlert("Try Again!", "No connection with the server", "OK");

                }
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Try Again!", "Invalid request", "OK");
                }
            }
        }
    }
}