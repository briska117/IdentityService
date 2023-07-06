namespace IdentityService.Configuration
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public int AccessExpiration { get; set; }

    }
}
