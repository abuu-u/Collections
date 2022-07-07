using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace Collections.Api.Entities;

[Index(nameof(Value))]
public class IntValue
{
    public int Id { get; set; }

    public int Value { get; set; }

    public int FieldId { get; set; }

    public Field Field { get; set; }

    public Item Item { get; set; }
}
