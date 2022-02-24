using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FrontEnd.Models
{
    public class FollowedStock
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        //[JsonProperty("stock")]
        //public Stock Stock { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        public FollowedStock(string email,string symbol)
        {
            Email = email;
            Symbol = symbol;
        }

        //public FollowedStock(string email, Stock stock, string symbol)
        //{
        //    Email = email;
        //    Stock = stock;
        //    Symbol = symbol;
        //}
    }
}
