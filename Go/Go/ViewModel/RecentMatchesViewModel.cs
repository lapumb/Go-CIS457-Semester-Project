using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Go.ViewModel
{
    public class RecentMatchesViewModel : INotifyPropertyChanged
    {
        public RecentMatchesViewModel()
        {
            App.RecentsViewModel = this;
        }

        /*changed event notifiers */
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
