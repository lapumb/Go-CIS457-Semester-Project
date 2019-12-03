using Go.Model;
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