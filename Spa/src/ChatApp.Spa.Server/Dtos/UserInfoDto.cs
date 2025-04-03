using System.Text.Json.Serialization;

namespace ChatApp.Spa.Server.Dtos;

public class UserInfoDto
{
    public string Id { get; set; }
    public string Nickname { get; set; }
    public Uri AvatarUrl { get; set; }
}
