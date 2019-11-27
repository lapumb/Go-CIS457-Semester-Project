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
            App.Master.IsGestureEnabled = true; 
            InitializeComponent();
        }
    }
}