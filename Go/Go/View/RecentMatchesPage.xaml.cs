
using Go.Model;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecentMatchesPage : ContentPage
    {
        private ObservableCollection<RecentMatch> RecentMatches { get; set; } = new ObservableCollection<RecentMatch>(); 
        public RecentMatchesPage()
        {
            InitializeComponent();
            Init();
            Debug.WriteLine(RecentMatches.Count);
            recentMatchesList.ItemsSource = RecentMatches;

            if (string.IsNullOrEmpty(UserInfo.User.Username) || UserInfo.User.Username.Contains("Guest") || RecentMatches.Count==0)
            {
                Utilities.Utilities.DisplayMessage("Uh Oh..", "You must log in to view recent matches.");
                recentMatchesList.IsVisible = false;
                noRecentsAvailableLabel.IsVisible = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.RecentsPg = this;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.RecentsPg = null;
        }

        private async void Init()
        {
            try
            {
                var list = await App.FirebaseClient.GetAllRecentMatches();
                foreach (RecentMatch match in list)
                {
                    RecentMatches.Add(match);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("RMP/OA : " + e.Message);
                Utilities.Utilities.DisplayMessage("Uh Oh..", "Recent matches are not available. Please check connection.");
                return;
            }
        }
    }
}