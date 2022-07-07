using Microsoft.EntityFrameworkCore;

namespace Collections.Api.Entities;

[Index(nameof(Value))]
public class BoolValue
{
    public int Id { get; set; }

    public bool Value { get; set; }

    public int FieldId { get; set; }

    public Field Field { get; set; }

    public Item Item { get; set; }
}
