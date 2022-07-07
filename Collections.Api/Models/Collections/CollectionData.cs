namespace Collections.Api.Models.Collections;

public class CollectionResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Topic { get; set; }

    public List<FieldData> Fields { get; set; }

    public string? ImgUrl { get; set; }
}