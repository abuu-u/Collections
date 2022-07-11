namespace Collections.Api.Models.Collections;

public class GetCollectionResponse
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int TopicId { get; set; }

    public string? ImageUrl { get; set; }

    public List<FieldData> Fields { get; set; }
}