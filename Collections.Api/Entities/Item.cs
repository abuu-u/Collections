using NpgsqlTypes;

namespace Collections.Api.Entities;

public enum FixedFields
{
    Name = -1
}

public class Item
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<Tag> Tags { get; set; }

    public List<Comment> Comments { get; set; }

    public List<Like> Likes { get; set; }

    public Collection Collection { get; set; }

    public List<IntValue> IntValues { get; set; }

    public List<StringValue> StringValues { get; set; }

    public List<BoolValue> BoolValues { get; set; }

    public List<DateTimeValue> DateTimeValues { get; set; }

    public NpgsqlTsVector SimpleSearchVector { get; set; }
}
