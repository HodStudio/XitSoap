using System;
using System.Text;

namespace HodStudio.XitSoap.Authentication
{
    public class BasicAuthentication : IAuthentication
    {
        public BasicAuthentication(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; private set; }
        internal string Password { get; private set; }

        private string _authenticationHeader { get; set; }
        public string AuthenticationHeader
        {
            get
            {
                if (string.IsNullOrEmpty(_authenticationHeader))
                {
                    var authData = string.Format("{0}:{1}", Username, Password);
                    _authenticationHeader = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(authData))}";
                }
                return _authenticationHeader;
            }
        }
    }
}
