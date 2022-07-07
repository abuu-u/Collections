namespace Collections.Api.Models.Users;

public class CreateCollectionRequest
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    
    public Topic Topic { get; set; }
    
    public string ImgUrl { get; set; }
    
    public User Owner { get; set; }
    
    public List<Field> Fields { get; set; }
    
    public IFormFile? Img { get; set; }
}