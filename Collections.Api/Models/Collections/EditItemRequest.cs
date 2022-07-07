namespace Collections.Api.Models.Collections;

public class EditItemRequest : IHasFields
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public List<string> Tags { get; set; }

    public List<IntValueData> IntFields { get; set; }

    public List<BoolValueData> BoolFields { get; set; }

    public List<StringValueData> StringFields { get; set; }

    public List<DateTimeValueData> DateTimeFields { get; set; }
}
