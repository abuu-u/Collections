namespace Collections.Api.Models.Collections;

public class StringValueData : IHasFieldId
{
    public int FieldId { get; set; }
    
    public string Value { get; set; }
}
