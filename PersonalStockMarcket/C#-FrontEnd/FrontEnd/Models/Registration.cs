using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FrontEnd.Models
{
    public class Registration
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("password_confirm")]
        public string Password_confirm { get; set; }

        public Registration(string email, string name, string surname, string username, string password, string password_confirm)
        {
            Email = email;
            Name = name;
            Surname = surname;
            Username = username;
            Password = password;
            Password_confirm = password_confirm;
        }
    }
}
