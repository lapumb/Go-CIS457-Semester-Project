
using Firebase.Database;
using Go.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            App.MainPg = this; 
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing(); 
            //await App.FirebaseClient.AddRecentMatch(DateTime.Now, "test", 55, 0);
        }
    }
}