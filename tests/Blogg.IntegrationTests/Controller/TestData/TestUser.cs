

using Blogg.Models.Entities;

namespace Blogg.IntegrationTests.Controller.TestData;
public class TestUser
{
    public User? User { get; set; }
    public string Base64EncodedUsernamePassword { get; init; } = string.Empty;
}
