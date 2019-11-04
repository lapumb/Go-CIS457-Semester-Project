using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private async void ContinueToGame_Clicked(object sender, EventArgs e)
        {
            string[] btns =
            {
                "5",
                "6",
                "7",
                "8",
                "9",
                "11",
                "13",
                "15",
                "19"
            };

            string result = await DisplayActionSheet("Select Board Size (NxN)", null, null, btns); 
            await Navigation.PushAsync(new GamePage(Convert.ToInt32(result)));
        }
    }
}