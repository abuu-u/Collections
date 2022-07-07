namespace Collections.Api.Models.Collections;

public class LatestItemData
{
    public int Id { get; set; }

    public string Name { get; set; }

    public LatestCollectionData Collection { get; set; }
}
