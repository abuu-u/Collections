using System.ComponentModel.DataAnnotations;
using Collections.Api.Entities;

namespace Collections.Api.Models.Collections;

public class CreateCollectionRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public int TopicId { get; set; }

    public List<CreateFieldData> Fields { get; set; }

    public string? ImageUrl { get; set; }
}
