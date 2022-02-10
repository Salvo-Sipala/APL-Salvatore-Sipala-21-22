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
    public partial class StockInfoPage : ContentPage
    {
        public List<Stock> FollowStocks { get; set; }
        public StockInfoPage()
        {
            InitializeComponent();
        }

        private void OnFollowButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string symbol = (string)button.CommandParameter;
            Stock follow_stock = FollowStocks.Find(FollowStocks => FollowStocks.Symbol == symbol);

        }
    }
}