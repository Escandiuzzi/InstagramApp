using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaSharper.API;
using InstaSharper.API.Builder;
using InstaSharper.Classes;
using InstaSharper.Classes.Models;
using InstaSharper.Logger;

namespace Instagram_App
{
    public class InstagramManager
    {
        string username;
        string password;

        bool filterVerified;

        private static UserSessionData user;
        private static IInstaApi api;


        public InstagramManager(string username, string password, bool filterVerified) 
        {
            this.username = username;
            this.password = password;

            this.filterVerified = filterVerified;

            LoginAsync();
        }


        public async Task LoginAsync() 
        {
            user = new UserSessionData();

            user.UserName = username;
            user.Password = password;

            api = InstaApiBuilder.CreateBuilder()
                .SetUser(user)
                .UseLogger(new DebugLogger(LogLevel.Exceptions))
                .Build();

            var loginRequest = await api.LoginAsync();

            if (loginRequest.Succeeded)
            {
                Console.WriteLine("\n\nLogged in!\n\n");
                GetAnalysisAsync();
            }
            else
                Console.WriteLine("\n\nError Loggin in!\n" + loginRequest.Info.Message);
        }

        private async Task GetAnalysisAsync()
        {
            var following = await api.GetUserFollowingAsync(username, PaginationParameters.Empty);
            var followers = await api.GetUserFollowersAsync(username, PaginationParameters.Empty);

            List<InstaUserShort> filteredList = new List<InstaUserShort>();

            if (filterVerified)
                filteredList = following.Value.ToList().Where(x => !followers.Value.ToList().Contains(x) && !x.IsVerified).ToList();
            else
                filteredList = following.Value.ToList().Where(x => !followers.Value.ToList().Contains(x)).ToList();

            foreach (var user in filteredList.OrderBy(x => x.UserName))
                Console.WriteLine($"Username: {user.UserName}\nName: {user.FullName}\n");;
        }
    }
}
