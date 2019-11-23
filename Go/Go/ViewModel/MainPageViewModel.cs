using Go.View;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Go.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            App.MainViewModel = this;
        }

        private bool _running = false;
        public bool Running
        {
            get { return _running; }
            set
            {
                _running = value;
                OnPropertyChanged(nameof(Running));
            }
        }

        public ICommand ToGameCommand => new Command(async () =>
        {
            string[] btns =
                {
                "5",
                "6",
                "7",
                "8",
                "9"
                };

            string result = await App.MainPg.DisplayActionSheet("Select Board Size (NxN)", null, null, btns);
            if (result != null)
            {
                try
                {
                    Running = true;
                    var nextPage = new GamePage(Convert.ToInt32(result));
                    await App.MainPg.Navigation.PushAsync(nextPage);
                    Running = false;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message); 
                }
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
