namespace FlexiTime_Backend.Utilities.Settings
{
    public class EmailSettings
    {
        public string Host { get; }
        public int Port { get; }
        public bool UseTlsIfAvailable { get; }
        public string UserName { get; }
        public string Password { get; }

        public EmailSettings(string host, int port, string userName, string password, bool useTlsIfAvailable)
        {
            Host = host;
            Port = port;
            UserName = userName;
            Password = password;
            UseTlsIfAvailable = useTlsIfAvailable;
        }
    }
}
