namespace Collections.Api.Models.Users
{
    public class GetPageResponse
    {
        public int PagesCount { get; set; }

        public List<UserData> Users { get; set; }
    }
}