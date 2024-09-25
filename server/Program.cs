using Microsoft.AspNetCore.SignalR;
using DotNetEnv;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Env.Load();
        Console.WriteLine($"ASPNETCORE_URLS: {Environment.GetEnvironmentVariable("ASPNETCORE_URLS")}");
        builder.Services.AddSignalR();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed((host) => true)); // Allow any origin
        });
        var app = builder.Build();
        var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
        if (!string.IsNullOrEmpty(urls))
        {
            app.Urls.Add(urls);
        }

        app.UseCors("AllowAll");
        app.MapGet("/", () => "Hello World!");
        app.MapHub<ChatHub>("/chathub");

        app.Run();
    }
}
