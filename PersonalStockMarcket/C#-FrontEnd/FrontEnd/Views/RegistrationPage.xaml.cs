using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FrontEnd.ViewModels;
using FrontEnd.Models;
using Newtonsoft.Json;
using System.Net.Http;
using FrontEnd.RestControllers;
using Xamarin.Essentials;

namespace FrontEnd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            //BindingContext = new RegistrationViewModel();
        }

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            if (EntryEmail.Text == null || EntryName.Text == null || 
                EntrySurname.Text == null || EntryNickname == null || 
                EntryPassword.Text == null || EntryRepeatPassword == null)
            {
                await DisplayAlert("Try again", "Insert all fields!", "OK");
            }
            else
            {
                if (EntryPassword.Text == EntryRepeatPassword.Text)
                {
                    Registration reg = new Registration(EntryEmail.Text, EntryName.Text, EntrySurname.Text,
                        EntryNickname.Text, EntryPassword.Text, EntryRepeatPassword.Text);

                    string json = JsonConvert.SerializeObject(reg);
                    HttpResponseMessage response = await RegistrationRequest.DoRegistration(json);

                    if (response.IsSuccessStatusCode)
                    {
                        string response_content = await response.Content.ReadAsStringAsync();
                        Token token = JsonConvert.DeserializeObject<Token>(response_content);

                        try
                        {
                            await SecureStorage.SetAsync("token", token.MyToken);
                            await SecureStorage.SetAsync("email", EntryEmail.Text);
                            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
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
                else
                {
                    await DisplayAlert("Try Again!", "Password and Repeat Password don't match!", "OK");
                }
            }
        }

        private async void LoginTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
        }
    }
}