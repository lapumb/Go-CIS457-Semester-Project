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
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MainPg = this;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.MainPg = null;
        }
    }
}