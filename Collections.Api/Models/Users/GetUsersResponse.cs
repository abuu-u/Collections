namespace Collections.Api.Models.Users
{
    public class GetUsersResponse
    {
        public int PagesCount { get; set; }

        public int UsersCount { get; set; }

        public List<UserData> Users { get; set; }
    }
}