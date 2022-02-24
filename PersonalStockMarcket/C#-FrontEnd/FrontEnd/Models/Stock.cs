using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FrontEnd.Models
{
    public class Stock
    {
        [JsonProperty("symbol")]
        private string symbol;

        [JsonProperty("shortName")]
        private string shortName;

        [JsonProperty("regularMarketPrice")]
        private double regularMarketPrice;

        [JsonProperty("regularMarketChange")]
        private double regularMarketChange;

        [JsonProperty("regularMarketChangePercent")]
        private double regularMarketChangePercent;

        public Stock()
        {

        }
        public Stock(string symbol, string shortName,  double regularMarketPrice, double regularMarketChange,
            double regularMarketChangePercent)
        {
            this.symbol = symbol;
            this.shortName = shortName;
            this.regularMarketPrice = regularMarketPrice;
            this.regularMarketChange = regularMarketChange;
            this.regularMarketChangePercent = regularMarketChangePercent;
        }

        public Stock(string symbol, string shortName)
        {
            this.symbol = symbol;
            this.shortName = shortName;
        }
        public string Symbol
        {
            get => symbol;
            set => symbol = value;
        }
        public string Shortname
        {
            get => shortName;
            set => shortName = value;
        }
        public double RegularMarketPrice
        {
            get => regularMarketPrice;
            set => regularMarketPrice = value;
        }
        public double RegularMarketChange
        {
            get => Math.Round(regularMarketChange,2);
            set => regularMarketChange = value;
        }
        public double RegularMarketChangePercent
        {
            get => Math.Round(regularMarketChangePercent, 2);
            set => regularMarketChangePercent = value;
        }
    }
}
