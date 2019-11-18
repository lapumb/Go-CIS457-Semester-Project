
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public static MainPage Game { get; set; } = null; 
        public MainPage()
        {
            Game = this; 
            InitializeComponent();
        }
    }
}