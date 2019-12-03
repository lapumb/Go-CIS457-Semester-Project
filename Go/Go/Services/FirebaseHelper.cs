using Firebase.Database;
using Firebase.Database.Query;
using Go.Model;
using System;
using System.Collections.Generic;
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

        public async Task<RecentMatch> GetRecentMatch(DateTime date)
        {
            var allPersons = await GetAllRecentMatches();
            await firebase
              .Child(UserInfo.User.Username).Child("Recent Matches")
              .OnceAsync<RecentMatch>();
            return allPersons.Where(a => a.Date == date).FirstOrDefault();
        }

        public async Task AddRecentMatch(DateTime date, string opponent, int playerScore, int opponentScore)
        {
            await firebase
              .Child(UserInfo.User.Username).Child("Recent Matches")
              .PostAsync(new RecentMatch() { Date = date, PlayedAgainst = opponent, PlayerScore = playerScore, OpponentScore = opponentScore });
        }

        public async Task DeleteRecentMatch(DateTime date)
        {
            var deleteRecent = (await firebase
              .Child(UserInfo.User.Username).Child("Recent Matches")
              .OnceAsync<RecentMatch>()).Where(a => a.Object.Date == date).FirstOrDefault();
            await firebase.Child("Recent Matches").Child(deleteRecent.Key).DeleteAsync();
        }

        /**
         *  USER INFORMATION
         */
        public async Task<List<UserInfo>> GetAllUsers()
        {
            return (await firebase
              .Child("Users")
              .OnceAsync<UserInfo>()).Select(item => new UserInfo
              {
                  Username = item.Object.Username,
                  Password = item.Object.Password
              }).ToList();
        }

        public async Task<UserInfo> GetUser(string username)
        {
            var users = await GetAllUsers();
            await firebase
              .Child("Users")
              .OnceAsync<UserInfo>();
            return users.Where(a => a.Username.Equals(username)).FirstOrDefault();
        }

        public async Task AddUser(string username, string password)
        {
            await firebase
              .Child("Users")
              .PostAsync(new UserInfo() { Username = username, Password = password });
        }

        public async Task DeleteUser(string username)
        {
            var user = (await firebase
              .Child("Users")
              .OnceAsync<UserInfo>()).Where(a => a.Object.Username.Equals(username)).FirstOrDefault();
            await firebase.Child("Users").Child(user.Key).DeleteAsync();
        }
    }
}
