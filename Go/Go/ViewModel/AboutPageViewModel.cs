using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Go.ViewModel
{
    public class AboutPageViewModel : INotifyPropertyChanged
    {
        public AboutPageViewModel()
        {
            App.AboutViewModel = this; 
        }

        /* UI CONTROLS */
        public ICommand LinkClickedCommand => new Command<string>(async (url) =>
        {
            await Launcher.OpenAsync(new Uri(url));
        });

        /*changed event notifiers */
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
