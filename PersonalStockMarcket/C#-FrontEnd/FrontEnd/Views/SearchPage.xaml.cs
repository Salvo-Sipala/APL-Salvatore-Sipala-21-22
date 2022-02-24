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
    public partial class SearchPage : ContentPage
    {
        public List<Stock> SearchedStock { get; set; }

        public Stock OneStock { get; set; }
        public SearchPage()
        {
            InitializeComponent();
        }
        private async void OnSearchPressed(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            string json = JsonConvert.SerializeObject(searchBar.Text);
            HttpResponseMessage response = await StockRequest.GetStockBySymbol(json);

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                SearchedStock = JsonConvert.DeserializeObject<List<Stock>>(response_content);
                Console.WriteLine(SearchedStock.ToString());
                SearchCollectionView.ItemsSource = SearchedStock;
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

        private async void OnButtonStockPressed(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string stock_to_search = (string)button.CommandParameter;
            string json = JsonConvert.SerializeObject(stock_to_search);
            // Find -> cerca un elemento che soddisfi le condizioni definite nel predicato specificato e restituisce
            // la prima occorrenza all'interno dell'intero oggetto list<t>.
            // Stock button_stock = SearchedStock.Find(SearchedStock => SearchedStock.Symbol == stock_to_search);
            HttpResponseMessage response = await StockRequest.GetStockBySymbol(json);

            if (response.IsSuccessStatusCode)
            {
                string response_content = await response.Content.ReadAsStringAsync();
                SearchedStock = JsonConvert.DeserializeObject<List<Stock>>(response_content);
                //await Shell.Current.GoToAsync($"///{nameof(StockInfoPage(button_stock))}");
                await Navigation.PushAsync(new StockInfoPage(SearchedStock));
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