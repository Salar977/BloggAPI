
using BloggAPI.Models.Entities;

namespace BloggAPI.IntegrationTests.Controller.TestData;
public class TestUser
{
    public User? User { get; set; }
    public string Base64EncodedUsernamePassword { get; init; } = string.Empty;
}
