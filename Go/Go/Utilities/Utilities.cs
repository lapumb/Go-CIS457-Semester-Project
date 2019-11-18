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

        /**
        * check if the device is an iPhone, specifically
        */
        public static bool DeviceIsIphone()
        {
            return (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS);
        }


        /**
         * check if device is an android, specifically
         */
        public static bool DeviceIsAndroid()
        {
            return (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android); 
        }

        /**
         * check if device is a UWP, specifically
         */
         public static bool DeviceIsUWP()
        {
            return (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.UWP);
        }


        /**
         * returns 0 if the device is an iPhone, 1 for an Android, -1 for other
         */
        public static int DeviceType()
        {
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.UWP)
                return 2;
            else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
                return 1;
            else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
                return 0;
            else
                return -1;
        }
    }
}
