using CmlLib.Core.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Fluetta
{
    public class Auth
    {
        public class SessionData
        {
            public MSession Session { get; set; }
            public bool OnlineMode { get; set; }
        }
        public static bool IsSuccess(string email, string password)
        {
            MLogin login = new MLogin();
            MLoginResponse response = login.Authenticate(email, password);
            return response.IsSuccess;
        }
        public static SessionData Authentificate(string username, bool isOnlineMode, string password = null)
        {
            SessionData mSession = new SessionData
            {
                OnlineMode = isOnlineMode
            };

            if (isOnlineMode) 
            {
                string email = username;
                string pw = password;

                MLogin login = new MLogin();
                MLoginResponse response = login.Authenticate(email, pw);
                mSession.Session = response.Session;
            }
            else
            {
                mSession.Session = MSession.GetOfflineSession(username);
            }
            File.WriteAllText(@".\login_settings.json", JsonConvert.SerializeObject(mSession));
            return mSession;
        }
    }
}
