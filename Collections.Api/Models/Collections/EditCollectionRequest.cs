using System.ComponentModel.DataAnnotations;

namespace Collections.Api.Models.Collections;

public class EditCollectionRequest
{
    [Required]
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? TopicId { get; set; }

    public List<EditCollectionFieldData> Fields { get; set; }

    public string? ImageUrl { get; set; }
}