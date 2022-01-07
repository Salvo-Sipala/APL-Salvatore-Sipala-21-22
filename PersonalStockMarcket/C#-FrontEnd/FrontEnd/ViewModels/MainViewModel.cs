using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FrontEnd.ViewModels;
using FrontEnd.Models;
using FrontEnd.Views;

namespace FrontEnd.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command SignInCommand { get; }

        public MainViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            SignInCommand = new Command(OnSignInClicked);
        }

        private async void OnLoginClicked(object o)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }

        private async void OnSignInClicked(object o)
        {
            await Shell.Current.GoToAsync($"//{nameof(RegistrationPage)}");
        }
    }
}
