
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            App.AboutPg = this; 
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.AboutPg = this;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.AboutPg = null;
        }
    }
}