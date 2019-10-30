using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Go.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
            //start by opening the main page
            Detail = new NavigationPage(new MainPage());
            IsPresented = false;
        }

        private void AboutPage_Clicked(object sender, EventArgs e)
        {
            Detail.Navigation.PushAsync(new AboutPage()); 
            IsPresented = false; 
        }
    }
}