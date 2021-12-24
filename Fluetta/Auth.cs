using CmlLib.Core.Auth;
using Newtonsoft.Json;
using System.IO;
using CmlLib.Core.Auth.Microsoft.UI.Wpf;

namespace Fluetta
{
    public class Auth
    {
        public class SessionData
        {
            public MSession Session { get; set; }
            public bool OnlineMode { get; set; }
            public bool IsMicrosoft { get; set; }
        }
        public static bool IsSuccess(string email, string password)
        {
            MLogin login = new();
            MLoginResponse response = login.Authenticate(email, password);
            return response.IsSuccess;
        }
        public static SessionData Authentificate(string username = null, bool isOnlineMode = true, string password = null, bool microsoft = false)
        {
            SessionData mSession = new()
            {
                OnlineMode = isOnlineMode,
                IsMicrosoft = microsoft
            };

            if (isOnlineMode && !microsoft) 
            {
                string email = username;
                string pw = password;

                MLogin login = new();
                MLoginResponse response = login.Authenticate(email, pw);
                mSession.Session = response.Session;
            }
            else if (isOnlineMode && microsoft)
            {
                MicrosoftLoginWindow loginWindow = new();
                var session = loginWindow.ShowLoginDialog();
                if (session != null) mSession.Session = session;
                else return null;
            }
            else
            {
                mSession.Session = MSession.GetOfflineSession(username);
            }
            File.WriteAllText($".{Path.DirectorySeparatorChar}settings{Path.DirectorySeparatorChar}login.json", JsonConvert.SerializeObject(mSession));
            return mSession;
        }
    }
}
