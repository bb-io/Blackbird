using Apps.Blackbird.Models.Entities;

namespace Apps.Blackbird.Models.Response.Users;

public class ListUsersResponse
{
    public IEnumerable<UserEntity> Users { get; set; }
}