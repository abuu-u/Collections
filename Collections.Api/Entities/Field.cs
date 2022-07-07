using Collections.Api.Models.Collections;
using NpgsqlTypes;

namespace Collections.Api.Entities;

public enum FieldType
{
    String,
    MultiLineString,
    Int,
    Bool,
    DateTime,
}

public class Field : IHasId
{
    public int Id { get; set; }

    public string Name { get; set; }

    public FieldType FieldType { get; set; }

    public int CollectionId { get; set; }

    public Collection Collection { get; set; }

    public NpgsqlTsVector SimpleSearchVector { get; set; }
}
