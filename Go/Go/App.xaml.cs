﻿using Xamarin.Forms;
using Go.View;
using Go.ViewModel;
using Go.Services;
using Go.Model;

namespace Go
{
    public partial class App : Application
    {
        public static FirebaseHelper FirebaseClient { get; } = new FirebaseHelper();
        public static MainPage MainPg { get; set; } = null;
        public static AboutPage AboutPg { get; set; } = null;
        public static GamePage GamePg{ get; set; } = null;
        public static RecentMatchesPage RecentsPg { get; set; } = null;
        public static MainPageViewModel MainViewModel { get; set; } = null;
        public static AboutPageViewModel AboutViewModel { get; set; } = null;
        public static GamePageViewModel GameViewModel { get; set; } = null;
        public static RecentMatchesViewModel RecentsViewModel { get; set; }
        public App()
        {
            InitializeComponent();
            MainPage = new MasterPage();
        }
    }
}