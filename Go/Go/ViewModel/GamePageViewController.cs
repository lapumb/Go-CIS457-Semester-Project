using Go.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Go.ViewModel
{
    class GamePageViewController : INotifyPropertyChanged
    {
        /// <summary>
        /// GameGrid is a dictionary holding the string value (coordinate) or the button value
        /// </summary>
        public static Dictionary<string, Button> GameGrid { get; set; } = new Dictionary<string, Button>();
        public static int Turn { get; set; } = 0;
        public static GamePageViewController GameController { get; set; } = new GamePageViewController();

        public void Init()
        {
            GameController = this;
            Turn = 0;
        }

        /* UI CONTROLS */
        public ICommand PassCommand => new Command(async () =>
        {
            //pass the users turn
        });

        public ICommand QuitCommand => new Command(async () =>
        {
            bool result = await MainPage.Game.DisplayAlert("Confirm", "Are you sure you want to quit, lil bitch?", "Yes, I'm sure", "Cancel");
            if (result)
            {
                //exit game sequence; close port, disconnect, etc.
                await MainPage.Game.Navigation.PopAsync();
            }
        });

        /*changed event notifiers */
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
