namespace Collections.Api.Models.Collections;

public interface IHasFields
{
    public List<IntValueData> IntFields { get; set; }

    public List<BoolValueData> BoolFields { get; set; }

    public List<StringValueData> StringFields { get; set; }

    public List<DateTimeValueData> DateTimeFields { get; set; }
}
