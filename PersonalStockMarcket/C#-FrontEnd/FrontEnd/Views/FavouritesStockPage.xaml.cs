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


namespace FrontEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouritesStockPage : ContentPage
    {
        public List<FollowedStock> FollowedStocks { get; set; }
        public FavouritesStockPage()
        {
            InitializeComponent();
            FillPage();
        }

        private async void FillPage()
        {
            HttpResponseMessage response = await StockRequest.GetUserFavouritesStocks();

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                FollowedStocks = JsonConvert.DeserializeObject<List<FollowedStock>>(response_content);
                FollowedCollection.ItemsSource = FollowedStocks;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadGateway)
                {
                    await DisplayAlert("Attention!!!", "No connection with the server", "OK");

                }
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    await DisplayAlert("Try Again!", "Invalid request", "OK");
                }
            }
        }
    }
}