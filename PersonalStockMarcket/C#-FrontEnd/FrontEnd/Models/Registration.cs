using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FrontEnd.Models
{
    public class Registration
    {
        [JsonProperty("email")]
        private string email;

        [JsonProperty("name")]
        private string name;

        [JsonProperty("surname")]
        private string surname;

        [JsonProperty("nickname")]
        private string nickname;

        [JsonProperty("password")]
        private string password;

        [JsonProperty("password_confirm")]
        private string password_confirm;

        public Registration(string email, string name, string surname, string nickname, string password, string password_confirm)
        {
            this.email = email;
            this.name = name;
            this.surname = surname;
            this.nickname = nickname;
            this.password = password;
            this.password_confirm = password_confirm;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }
        public string Name
        {
            get => name;
            set => name = value;
        }
        public string Surname
        {
            get => surname;
            set => surname = value;
        }
        public string Nickname
        {
            get => nickname;
            set => nickname = value;
        }
        public string Password
        {
            get => password;
            set => password = value;
        }
        public string Password_confirm
        {
            get => password_confirm;
            set => password_confirm = value;
        }

    }
}
