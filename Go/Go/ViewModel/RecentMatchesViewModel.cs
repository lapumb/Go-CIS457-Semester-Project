using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Go.ViewModel
{
    public class RecentMatchesViewModel : INotifyPropertyChanged
    {

        /*changed event notifiers */
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
