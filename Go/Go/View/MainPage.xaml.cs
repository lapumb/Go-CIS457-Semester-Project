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

        public string GetIpInput()
        {
            if (ipInput.Text == null)
                return ""; 

            return ipInput.Text; 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MainPg = this;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}