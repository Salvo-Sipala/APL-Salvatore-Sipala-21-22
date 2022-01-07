using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FrontEnd.Models
{
    public class Token
    {
        [JsonProperty("token")]
        private string myToken;

        public Token(string token)
        {
            myToken = token;
        }
        public string MyToken
        {
            get => myToken;
            set => myToken = value;
        }
    }
}
