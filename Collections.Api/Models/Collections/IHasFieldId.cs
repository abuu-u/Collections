namespace Collections.Api.Models.Collections;

public interface IHasFieldId
{
    public int Id { get; set; }
    
    public int FieldId { get; set; }
}
