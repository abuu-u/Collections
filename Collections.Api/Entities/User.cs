using System.ComponentModel.DataAnnotations.Schema;

namespace Collections.Api.Entities;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public bool Status { get; set; }

    public bool Admin { get; set; }
    
    [InverseProperty(nameof(Collection.Owner))]
    public List<Collection> Collections { get; set; } 

    public string PasswordHash { get; set; }
}
