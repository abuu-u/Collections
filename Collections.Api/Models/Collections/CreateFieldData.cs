using Collections.Api.Entities;

namespace Collections.Api.Models.Collections;

public class CreateFieldData
{
    public string Name { get; set; }

    public FieldType FieldType { get; set; }
}
