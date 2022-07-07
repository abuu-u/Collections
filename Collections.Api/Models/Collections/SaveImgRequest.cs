using System.ComponentModel.DataAnnotations;

namespace Collections.Api.Models.Collections;

public class SaveImageRequest
{
    [Required]
    public IFormFile Image { get; set; }
}