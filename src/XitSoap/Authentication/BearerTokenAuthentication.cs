namespace HodStudio.XitSoap.Authentication
{
    public class BearerTokenAuthentication : IAuthentication
    {
        public BearerTokenAuthentication(string token)
        {
            Token = token;
        }

        public string Token { get; set; }

        public string AuthenticationHeader => $"Bearer {Token}";
    }
}
