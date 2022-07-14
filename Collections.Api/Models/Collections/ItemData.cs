namespace Collections.Api.Models.Collections;

public class ItemData
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<StringValueData> StringValues { get; set; }

    public List<DateTimeValueData> DateTimeValues { get; set; }

    public List<BoolValueData> BoolValues { get; set; }

    public List<IntValueData> IntValues { get; set; }
}
