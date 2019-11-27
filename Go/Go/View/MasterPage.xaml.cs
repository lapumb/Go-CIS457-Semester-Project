using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
            App.Master = this; 
            //start by opening the main page
            Detail = new NavigationPage(new MainPage());
            IsPresented = false;
        }

        private void AboutPage_Clicked(object sender, EventArgs e)
        {
            Detail.Navigation.PushAsync(new AboutPage()); 
            IsPresented = false; 
        }

        private void MatchHistory_Clicked(object sender, EventArgs e)
        {
            Detail.Navigation.PushAsync(new RecentMatchesPage());
            IsPresented = false;
        }
    }
}