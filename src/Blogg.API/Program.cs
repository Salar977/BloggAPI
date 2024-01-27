using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Blogg.Data;
using Blogg.Extensions;
using Blogg.Middleware;

public class Program
{

	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		// Rate Limiter - fixed
		builder.Services.AddRateLimiter(rateLimiterOptions =>
		{
			rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
			{
				options.PermitLimit = 1;
				options.Window = TimeSpan.FromSeconds(5);
				options.QueueLimit = 0;
				rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
			});
		});

		// Extention Methods
		builder.AddSwaggerWithBasicAuthentication();

		builder.RegisterMappers();
		builder.RegisterRepositories();
		builder.RegisterServices();


		builder.Services.AddScoped<StudentBloggBasicAuthentication>();
		builder.Services.AddTransient<GlobalExceptionMiddleware>();

		builder.Services.AddValidatorsFromAssemblyContaining<Program>();
		builder.Services.AddFluentValidationAutoValidation(config => config.DisableDataAnnotationsValidation = false);

		builder.Services.AddDbContext<StudentBloggDbContext>(options =>
		{
			options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
			new MySqlServerVersion(new Version(8, 0)));
		});

		// viktig!!! Serilog Logger configuration
		builder.Host.UseSerilog((context, configuration) =>
		{
			configuration.ReadFrom.Configuration(context.Configuration);
		});

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseRateLimiter();

		app.UseMiddleware<GlobalExceptionMiddleware>();
		app.UseMiddleware<StudentBloggBasicAuthentication>();

		app.UseSerilogRequestLogging();
		app.UseHttpsRedirection();
		app.UseAuthorization();
		app.MapControllers()
			.RequireRateLimiting("fixed");

		app.Run();
	}
}