using NpgsqlTypes;

namespace Collections.Api.Entities;

public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int ItemId { get; set; }

    public Item Item { get; set; }

    public NpgsqlTsVector SimpleSearchVector { get; set; }
}
