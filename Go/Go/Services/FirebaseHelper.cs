using Firebase.Database;
using Firebase.Database.Query;
using Go.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Services
{
    public class FirebaseHelper
    {
        //connect to firebase
        public FirebaseClient firebase = new FirebaseClient("https://go-xamarin.firebaseio.com/");

        /**
         *  RECENT MATCH
         */
        public async Task<List<RecentMatch>> GetAllRecentMatches()
        {
            try
            {
                return (await firebase
                  .Child(UserInfo.User.Username).Child("Recent Matches")
                  .OnceAsync<RecentMatch>()).Select(item => new RecentMatch
                  {
                      Date = item.Object.Date,
                      PlayedAgainst = item.Object.PlayedAgainst,
                      PlayerScore = item.Object.PlayerScore,
                      OpponentScore = item.Object.OpponentScore,
                  }).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("FH/GARM, Exception caught - " + e.Message);
            }

            return null;
        }

        public async Task<RecentMatch> GetRecentMatch(DateTime date)
        {
            try
            {
                var allPersons = await GetAllRecentMatches();
                await firebase
                  .Child(UserInfo.User.Username).Child("Recent Matches")
                  .OnceAsync<RecentMatch>();
                return allPersons.Where(a => a.Date == date).FirstOrDefault();
            } 
            catch (Exception e)
            {
                Debug.WriteLine("FH/GRM, caught : " + e.Message); 
            }

            return null; 
        }

        public async Task AddRecentMatch(DateTime date, string opponent, int playerScore, int opponentScore)
        {
            try
            {
                await firebase
                  .Child(UserInfo.User.Username).Child("Recent Matches")
                  .PostAsync(new RecentMatch() { Date = date, PlayedAgainst = opponent, PlayerScore = playerScore, OpponentScore = opponentScore });
            }
            catch (Exception e)
            {
                Debug.WriteLine("FH/ARM, Caught : " + e.Message);
            }
        }

        public async Task DeleteRecentMatch(DateTime date)
        {
            try
            {
                var deleteRecent = (await firebase
                  .Child(UserInfo.User.Username).Child("Recent Matches")
                  .OnceAsync<RecentMatch>()).Where(a => a.Object.Date == date).FirstOrDefault();
                await firebase.Child("Recent Matches").Child(deleteRecent.Key).DeleteAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("FH/DRM, Caught : " + e.Message);
            }
        }

        /**
         *  USER INFORMATION
         */
        public async Task<List<UserInfo>> GetAllUsers()
        {
            try
            {
                return (await firebase
                  .Child("Users")
                  .OnceAsync<UserInfo>()).Select(item => new UserInfo
                  {
                      Username = item.Object.Username,
                      Password = item.Object.Password
                  }).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine("FH/GAU, caught : " + e.Message);
            }

            return null; 
        }

        public async Task<UserInfo> GetUser(string username)
        {
            try
            {
                var users = await GetAllUsers();
                await firebase
                  .Child("Users")
                  .OnceAsync<UserInfo>();
                return users.Where(a => a.Username.Equals(username)).FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine("FH/GU, caught : " + e.Message);
                return null; 
            }
        }

        public async Task AddUser(string username, string password)
        {
            try
            {
                await firebase
                  .Child("Users")
                  .PostAsync(new UserInfo() { Username = username, Password = password });
            }
            catch (Exception e)
            {
                Debug.WriteLine("FH/AD, caught : " + e.Message); 
            }
        }

        public async Task DeleteUser(string username)
        {
            try
            {
                var user = (await firebase
                  .Child("Users")
                  .OnceAsync<UserInfo>()).Where(a => a.Object.Username.Equals(username)).FirstOrDefault();
                await firebase.Child("Users").Child(user.Key).DeleteAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine("FH/DU, caught : " + e.Message); 
            }
        }
    }
}
