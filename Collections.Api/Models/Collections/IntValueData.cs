namespace Collections.Api.Models.Collections;

public class IntValueData : IHasFieldId
{
    public int Id { get; set; }
    
    public int FieldId { get; set; }
    
    public int Value { get; set; }
}
