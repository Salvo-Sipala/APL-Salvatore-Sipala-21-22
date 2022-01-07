using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using FrontEnd.ViewModels;
using FrontEnd.Models;
using Newtonsoft.Json;
using System.Net.Http;
using FrontEnd.RestControllers;

namespace FrontEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (EntryEmail.Text == null || EntryPassword.Text == null)
            {
                await DisplayAlert("Try again", "Password or Email entered incorrectly", "OK");
            }
            else
            {
                Login log = new Login(EntryEmail.Text, EntryPassword.Text);
                string json = JsonConvert.SerializeObject(log);
                Console.WriteLine("json:" + json);
                HttpResponseMessage response = await LoginRequest.DoLogin(json);
                Console.WriteLine("response:" + response);
                if (response.IsSuccessStatusCode) //200-299
                {
                    string response_content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("response_c:" + response_content);
                    Token token = JsonConvert.DeserializeObject<Token>(response_content);
                    Console.WriteLine("token:" + token);
                    try
                    {
                        await SecureStorage.SetAsync("token", token.MyToken);
                        await SecureStorage.SetAsync("email", EntryEmail.Text);
                        //await Shell.Current.GoToAsync($"///{nameof(SearchPage)}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        await DisplayAlert("Attention!!!", "Something went wrong", "OK");
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadGateway) //502
                    {
                        await DisplayAlert("Try Again!", "No connection with the server", "OK");

                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) //400
                    {
                        await DisplayAlert("Try Again!", "Invalid request", "OK");
                    }
                }
            }
        }

        private async void SignInTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"///{nameof(RegistrationPage)}");
        }

    }
}