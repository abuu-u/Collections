using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace Collections.Api.Entities;

public class Collection
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int TopicId { get; set; }

    public Topic Topic { get; set; }

    public string? ImageUrl { get; set; }

    public int OwnerId { get; set; }
    
    [ForeignKey(nameof(OwnerId))]
    [InverseProperty(nameof(User.Collections))]
    public User Owner { get; set; }

    public List<Field> Fields { get; set; }

    public List<Item> Items { get; set; }

    public NpgsqlTsVector SimpleSearchVector { get; set; }
}
