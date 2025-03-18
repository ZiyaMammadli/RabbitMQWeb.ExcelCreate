using Microsoft.AspNetCore.Identity;

namespace RabbitMQWeb.ExcelCreate.Models;

public class AppUser:IdentityUser
{
    public string RefreshToken { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime RefreshTokenExpiredDate { get; set; }
    public List<UserFIle> userFIles { get; set; }
}
