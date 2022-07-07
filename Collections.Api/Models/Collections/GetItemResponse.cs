namespace Collections.Api.Models.Collections;

public class GetItemResponse
{
    public string Name { get; set; }

    public List<string> Tags { get; set; }

    public List<CommentData> Comments { get; set; }

    public int LikesCount { get; set; }

    public bool Like { get; set; }

    public List<FieldData> Fields { get; set; }

    public List<IntValueData> IntValues { get; set; }

    public List<BoolValueData> BoolValues { get; set; }

    public List<StringValueData> StringValues { get; set; }

    public List<DateTimeValueData> DateTimeValues { get; set; }
}
