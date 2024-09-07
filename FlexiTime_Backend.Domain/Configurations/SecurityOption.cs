namespace FlexiTime_Backend.Domain.Configurations
{
    public class SecurityOption
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
