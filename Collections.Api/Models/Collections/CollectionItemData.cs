namespace Collections.Api.Models.Collections;

public class CollectionItemData
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<StringValueData> StringValues { get; set; }

    public List<DateTimeValueData> DateTimeValues { get; set; }
}
