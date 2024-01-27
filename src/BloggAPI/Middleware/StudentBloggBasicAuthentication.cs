using BloggAPI.Services.interfaces;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BloggAPI.Middleware;

public class StudentBloggBasicAuthentication : IMiddleware
{
    private readonly IUserService _userService;
    private readonly ILogger<StudentBloggBasicAuthentication> _logger;
    private readonly List<Regex> _excludePatterns = new();


    public StudentBloggBasicAuthentication(IUserService userService, ILogger<StudentBloggBasicAuthentication> logger)
    {
        _userService = userService;
        _logger = logger;
        _excludePatterns.Add(new Regex("/api/.*/[Uu]sers/register"));
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        // hvis ruten er register-endepunktet, omgå autentisering
        if (context.Request.Path.StartsWithSegments("/api/v1/users/register")
            && context.Request.Method.Equals("POST"))
        {
            await next(context);
            return;
        }

        try
        {
            if(_excludePatterns.Any(req => req.IsMatch(context.Request.Path.Value!)))
            {
                await next(context);
                return;
            }


            if (!context.Request.Headers.ContainsKey("Authorization"))
                throw new UnauthorizedAccessException("\"Authorization\" mangler i HTTP-Header");

            var authHeader = context.Request.Headers.Authorization;

            //Console.WriteLine(authHeader);

            string base64String = authHeader.ToString().Split(' ')[1];

            string user_password = DecodeBase64String(base64String);

            string[] arr = user_password.Split(":");
            string userName = arr[0];
            string password = arr[1];

            // userName -> slå opp i database !!
            int userId = await _userService.GetAuthenticatedIdAsync(userName, password);

            if( userId == 0 )
            {
                // HTTP 401
                throw new UnauthorizedAccessException($"Ingen tilgang til dette APIen.");
            }

            context.Items["UserId"] = userId;

            await next(context);
        }
        catch (UnauthorizedAccessException ex)
        {
            await Results.Problem(
                title: "Unauthorized: ikke lov til å bruke API",
                statusCode: StatusCodes.Status401Unauthorized,
                detail: ex.Message,
                extensions: new Dictionary<string,Object?>
                {
                    { "traceId", Activity.Current?.Id }
                })
                .ExecuteAsync(context);
        }
    }

    private string DecodeBase64String(string base64String)
    {
        var base64 = Convert.FromBase64String(base64String);
        string base_password = System.Text.Encoding.UTF8.GetString(base64);
        return base_password;
    }
}
