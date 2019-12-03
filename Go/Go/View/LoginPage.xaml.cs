using Go.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        public void CreateAccountUi()
        {
            loginBtn.Text = "Create Account";
            continueAsGuestBtn.Text = "Cancel";
            createAccountLabel.IsVisible = false;
        }

        /**
         *  LIFECYCLE
         */
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.LoginPg = this;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.LoginPg = null;
        }

        /**
         *  UI CONTROL
         */
        private void UsernameEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserInfo.User.Username = e.NewTextValue;
        }

        private void PasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserInfo.User.Password = e.NewTextValue;
        }
    }
}