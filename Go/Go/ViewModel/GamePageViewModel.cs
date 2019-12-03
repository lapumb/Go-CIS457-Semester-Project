using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Go.ViewModel
{
    public class GamePageViewModel : INotifyPropertyChanged
    {
        public GamePageViewModel()
        {
            App.GameViewModel = this; 
        }

        /* UI CONTROLS */
        public ICommand PassCommand => new Command(() =>
        {
            //pass the users turn
            App.GamePg.Game.IncrementTurn();
            Model.Connection.Instance.Send("MOVE " + App.GamePg.Game.Opponent + " " + "-1 -1 " + App.GamePg.Game.Turn.ToString());
            App.GamePg.Game.WaitForUserMove();
        });

        public ICommand QuitCommand => new Command(async () =>
        {
            bool result = await App.MainPg.DisplayAlert("Confirm", "Are you sure you want to quit?", "Yes, I'm sure", "Cancel");
            if (result)
            {
                //exit game sequence; close port, disconnect, etc.
                App.GamePg.Game.GameOver();
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
