using Go.Model;
using Go.View;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace Go.ViewModel
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private bool NewAccount { get; set; } = false; 
        public LoginPageViewModel()
        {
            App.LoginViewModel = this; 
        }

        public async void LoginSequence()
        {
            try
            {
                var result = await App.FirebaseClient.GetAllUsers();
                foreach (UserInfo info in result)
                {
                    if (info.Username.Equals(UserInfo.User.Username))
                    {
                        if (info.Password.Equals(UserInfo.User.Password))
                        {
                            try
                            {
                                await App.FirebaseClient.AddRecentMatch(DateTime.Now, "TEST", 500, 7);
                            }
                            catch (Exception e)
                            {
                                Debug.Write("LPVM/LS, caught : " + e.Message);
                                Utilities.Utilities.DisplayMessage("Uh Oh..", "Failed creating recent match log. Please check connection.");
                                return;
                            }

                            App.Master.Detail = new NavigationPage(new MainPage());
                            return;
                        }
                    }
                }
            } 
            catch (Exception e)
            {
                Debug.WriteLine("LPVM/LS, caught : " + e.Message);
                Utilities.Utilities.DisplayMessage("Uh Oh..", "Unexpected error occured. Please check connection.");
                return;
            }

            await App.LoginPg.DisplayAlert("Error", "The username and/or password you entered is not correct. Please retry.", "Okay"); 
        }

        /* UI CONTROLS */
        public ICommand CreateAccountCommand => new Command<string>((url) =>
        {
            App.LoginPg.CreateAccountUi();
            NewAccount = true;
        });

        public ICommand CAGCommand => new Command<string>((url) =>
        {
            if (NewAccount)
            {
                App.Master.Detail = new NavigationPage(new LoginPage());
                return;
            }

            UserInfo.User.Username = "Guest";
            UserInfo.User.Password = string.Empty;
            App.Master.Detail = new NavigationPage(new MainPage());
        });

        public ICommand LoginCommand => new Command<string>(async (url) =>
        {
            if (NewAccount)
            {
                if (UserInfo.User.Username.Length > 0 && UserInfo.User.Password.Length > 0)
                {
                    UserInfo result = null; 
                    if (UserInfo.User.Username.Contains("Guest"))
                        await App.LoginPg.DisplayAlert("No!", "Your username cannot contain the word 'guest'. Please enter something else.", "Okay");

                    try
                    {
                        result = await App.FirebaseClient.GetUser(UserInfo.User.Username);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("LPVM/LC, caught : " + e.Message);
                        Utilities.Utilities.DisplayMessage("Uh Oh..", "Unexpected error occured. Please check connection.");
                        return; 
                    }

                    if (result == null)
                    {
                        try
                        {
                            await App.FirebaseClient.AddUser(UserInfo.User.Username, UserInfo.User.Password);
                            await App.LoginPg.DisplayAlert("Success!", "Account created.", "Okay");
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("LPVM/CAC, Exception Caught when creating account: " + e.Message);
                            await App.LoginPg.DisplayAlert("Uh oh..", "Failed creating account. Please check connection.", "Okay");
                            return;
                        }
                    }
                    else
                    {
                        await App.LoginPg.DisplayAlert("No!", "This username already exists. Please use another.", "Okay");
                        return;
                    }
                }
            }

            LoginSequence(); 
        });

        /*changed event notifiers */
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
