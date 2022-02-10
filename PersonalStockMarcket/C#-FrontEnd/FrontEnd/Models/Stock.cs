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

        [JsonProperty("preMarketPrice")]
        private double preMarketPrice;

        [JsonProperty("preMarketChange")]
        private double preMarketChange;

        [JsonProperty("preMarketChangePercent")]
        private double preMarketChangePercent;

        [JsonProperty("regularMarketPrice")]
        private double regularMarketPrice;

        [JsonProperty("regularMarketChange")]
        private double regularMarketChange;

        [JsonProperty("regularMarketChangePercent")]
        private double regularMarketChangePercent;

        public Stock(string symbol, string shortName, double preMarketPrice, double preMarketChange,
            double preMarketChangePercent, double regularMarketPrice, double regularMarketChange,
            double regularMarketChangePercent)
        {
            this.symbol = symbol;
            this.shortName = shortName;
            this.preMarketPrice = preMarketPrice;
            this.preMarketChange = preMarketChange;
            this.preMarketChangePercent = preMarketChangePercent;
            this.regularMarketPrice = regularMarketPrice;
            this.regularMarketChange = regularMarketChange;
            this.regularMarketChangePercent = regularMarketChangePercent;
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
        public double PreMarketPrice
        {
            get => preMarketPrice;
            set => preMarketPrice = value;
        }
        public double PreMarketChange
        {
            get => preMarketChange;
            set => preMarketChange = value;
        }
        public double PreMarketChangePercent
        {
            get => preMarketChangePercent;
            set => preMarketChangePercent = value;
        }
        public double RegularMarketPrice
        {
            get => regularMarketPrice;
            set => regularMarketPrice = value;
        }
        public double RegularMarketChange
        {
            get => regularMarketChange;
            set => regularMarketChange = value;
        }
        public double RegularMarketChangePercent
        {
            get => regularMarketChangePercent;
            set => regularMarketChangePercent = value;
        }

    }
}
