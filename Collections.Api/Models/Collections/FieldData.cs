using Collections.Api.Entities;

namespace Collections.Api.Models.Collections;

public class FieldData
{
    public int Id { get; set; }

    public string Name { get; set; }

    public FieldType FieldType { get; set; }
}
