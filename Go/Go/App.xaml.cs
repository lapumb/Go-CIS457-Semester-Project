using Xamarin.Forms;
using Go.View;
using Go.ViewModel;
using Go.Services;
using Go.Model;
using System;
using System.Diagnostics;

namespace Go
{
    public partial class App : Application
    {
        public static FirebaseHelper FirebaseClient { get; } = new FirebaseHelper();
        public static MainPage MainPg { get; set; } = null;
        public static AboutPage AboutPg { get; set; } = null;
        public static GamePage GamePg{ get; set; } = null;
        public static LoginPage LoginPg { get; set; } = null;
        public static RecentMatchesPage RecentsPg { get; set; } = null;
        public static MainPageViewModel MainViewModel { get; set; } = null;
        public static AboutPageViewModel AboutViewModel { get; set; } = null;
        public static GamePageViewModel GameViewModel { get; set; } = null;
        public static RecentMatchesViewModel RecentsViewModel { get; set; } = null;
        public static LoginPageViewModel LoginViewModel { get; set; } = null;
        public static MasterPage Master { get; set; } = null; 
        public App()
        {
            InitializeComponent();
            MainPage = new MasterPage();
            
        }

        /// <summary>
        /// Anytime "OnSleep" is called, close the socket. 
        /// We do not want to keep a connection alive if we are running in background
        /// </summary>
        protected override void OnSleep()
        {
            base.OnSleep();
            try
            {
                Connection.Instance.ClosingSequence(GamePg.Game.Opponent);
            }
            catch (Exception e)
            {
                Debug.WriteLine("App/OnSleep, Caught : " + e.Message); 
            }
        }
    }
}