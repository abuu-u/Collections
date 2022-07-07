using System.ComponentModel.DataAnnotations;

namespace Collections.Api.Models.Collections;

public class AddItemRequest : IHasFields
{
    [Required]
    public string Name { get; set; }

    public List<string> Tags { get; set; }

    public List<IntValueData> IntFields { get; set; }

    public List<BoolValueData> BoolFields { get; set; }

    public List<StringValueData> StringFields { get; set; }

    public List<DateTimeValueData> DateTimeFields { get; set; }
}
