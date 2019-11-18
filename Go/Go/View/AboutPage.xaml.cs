
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public static AboutPage About { get; set; } = null;
        public AboutPage()
        {
            About = this; 
            InitializeComponent();
        }
    }
}