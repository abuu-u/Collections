namespace Collections.Api.Entities;

public class Like
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public User Author { get; set; }

    public int ItemId { get; set; }
    
    public Item Item { get; set; }
}
