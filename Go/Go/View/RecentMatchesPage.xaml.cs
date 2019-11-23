
using Go.Model;
using Go.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
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
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
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
            }
        }

        private void RecentMatchesList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}