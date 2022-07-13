namespace Collections.Api.Models.Collections;

public class BoolValueData : IHasFieldId
{
    public int Id { get; set; }
    
    public int FieldId { get; set; }
    
    public bool Value { get; set; }
}
