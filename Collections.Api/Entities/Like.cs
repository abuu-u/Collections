namespace Collections.Api.Entities;

public class Like
{
    public int Id { get; set; }

    public User Author { get; set; }

    public Item Item { get; set; }
}
