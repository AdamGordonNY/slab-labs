namespace SlabLabs.Api.Models;

public class User
{
    required public string Id { get; set; }
    required public DateTime CreatedAt { get; set; }
    required public string Password { get; set; }
    required public string Email { get; set; }
    public string City { get; set; } = "";
    public string Address { get; set; } = "";
    public string State { get; set; } = "";
    public string ZipCode { get; set; } = "";
    public string PhoneNumber { get; set; } = "";

}