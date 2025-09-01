using TheMovie.Api.Extensions;

namespace TheMovie.Api;

internal class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.AddDependencies();

        WebApplication app = builder.Build();
        app.AddMiddlewares();

        app.Run();
    }
}
