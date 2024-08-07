using Microsoft.AspNetCore.Identity;

namespace PlantShopAPI.Models;

public class CustomUser : IdentityUser
{
    public override string? UserName { get; set; }
    public string? Password { get; set; }

    public string? Role {  get; set; }
}
