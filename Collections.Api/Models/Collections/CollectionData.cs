namespace Collections.Api.Models.Collections;

public class CollectionData
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int TopicId { get; set; }

    public List<FieldData> Fields { get; set; }

    public string? ImageUrl { get; set; }
}