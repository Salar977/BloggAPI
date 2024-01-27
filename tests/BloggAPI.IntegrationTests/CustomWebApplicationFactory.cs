using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using BloggAPI.Repository.interfaces;

namespace BloggAPI.IntegrationTests;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public CustomWebApplicationFactory()
    {
        UserRespostoryMock = new Mock<IUserRepository>();
    }

    public Mock<IUserRepository> UserRespostoryMock { get; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(UserRespostoryMock.Object);
        });
    }
}
