using System.Threading.Tasks;

namespace Go.Utilities
{
    class Utilities
    {
        /**
         * statically displays a dialog popup
         */
        public static async void DisplayMessage(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "OK");
        }


        /**
         * statically displays a dialog popup
         */
        public static async Task DisplayMessageAsync(string title, string message)
        {
            await App.Current.MainPage.DisplayAlert(title, message, "OK");
        }
    }
}
