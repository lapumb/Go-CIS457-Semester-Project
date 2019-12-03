using Go.Model;
using Go.View;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Go.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private static IPEndPoint EndPoint { get; set; } = null;
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

        /// <summary>
        /// ToGameCommand is a binded command and has several steps
        ///     1. establish network end point (ie where / what socket we want to connect to)
        ///     2. when a user selects a grid size, user will attemp to connect to the ALREADY RUNNING central server
        ///     3a. connection will attempt for five seconds, then timeout. on connection, the grid page will be instantiated
        ///     3b. if a connection cannot be established, an error msg will pop up and user will not be navigated to game page
        /// </summary>
        public ICommand ToGameCommand => new Command(async () =>
        {
            //TODO: needs to be your own IP address
            EndPoint = Utilities.Utilities.SetIpAddress("35.40.133.71");     //192.168.1.7
            //EndPoint = Utilities.Utilities.GetNetwork(); 
            Debug.WriteLine("Set Netowrk EndPoint (Connection) to : " + EndPoint.ToString()); 
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
                    var gamePage = new GamePage(Convert.ToInt32(result))
                    {
                        Client = new TcpClient()
                    };
                    try
                    {
                        Debug.WriteLine("Connecting...");
                        gamePage.Client.ConnectAsync(EndPoint.Address, EndPoint.Port).Wait(TimeSpan.FromMilliseconds(5000));
                        if (gamePage.Client.Connected)
                        {
                            Running = false;                      
                            Debug.WriteLine("Connected to server.");
                            Connection.Instance.Client = gamePage.Client;
                            Connection.Instance.Send("CONNECT Guest");
                            string opponent = Connection.Instance.Receive();
                            Debug.WriteLine(opponent);
                            string[] opp = opponent.Split();

                            gamePage.OpponentSet(opp[0]);
                            Running = true; 
                            await App.MainPg.Navigation.PushAsync(gamePage);
                            Running = false; 
                        }
                        else
                        {
                            Running = false; 
                            Debug.WriteLine("Could not connect to server");
                            await App.MainPg.DisplayAlert("Uh Oh..", "We could not connect you to the game server. " +
                                "Please confirm your network connection.", "Okay", "Else");
                        }
                    }
                    catch (Exception e)
                    {
                        Running = false; 
                        Debug.WriteLine("Exception while trying to connect to server, " + e.Message);
                        await App.MainPg.DisplayAlert(e.Message, "We could not connect you to the game server. " +
                            "Please confirm your network connection.", "Okay");
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("caught general exception, " + e.Message);
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
