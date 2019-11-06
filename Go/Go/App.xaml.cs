using Xamarin.Forms;
using Go.View; 

namespace Go
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MasterPage();
        }
    }
}
