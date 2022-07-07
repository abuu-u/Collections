using NpgsqlTypes;

namespace Collections.Api.Entities;

public class Comment
{
    public int Id { get; set; }

    public string Text { get; set; }

    public User Author { get; set; }

    public int ItemId { get; set; }

    public Item Item { get; set; }

    public NpgsqlTsVector SimpleSearchVector { get; set; }
}
