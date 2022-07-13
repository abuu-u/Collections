namespace Collections.Api.Models.Collections;

public class DateTimeValueData : IHasFieldId
{
    public int Id { get; set; }
    
    public int FieldId { get; set; }
    
    public DateTime Value { get; set; }
}
