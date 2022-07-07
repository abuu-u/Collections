using Collections.Api.Entities;

namespace Collections.Api.Models.Collections;

public class GetCollectionItemsRequest
{
    public int? SortFieldId { get; set; }

    public FieldType? SortFieldType { get; set; }

    public string? FilterName { get; set; }

    public List<string>? FilterTags { get; set; }
}
