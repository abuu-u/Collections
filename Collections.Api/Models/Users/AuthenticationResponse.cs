namespace Collections.Api.Models.Users
{
    public class AuthenticationResponse
    {
        public string Name { get; set; }

        public string JwtToken { get; set; }
    }
}