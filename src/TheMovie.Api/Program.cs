using TheMovie.Api.Extensions;

namespace TheMovie.Api;

/// <summary>
/// Application entry point for TheMovie API.
/// </summary>
/// <remarks>
/// <para>
/// Bootstraps the web host and delegates configuration to <see cref="TheMovie.Api.Extensions.ProgramExtensions"/>,
/// which registers dependencies and wires up middlewares/endpoints. Keeping Program.cs minimal improves readability
/// and maintainability.
/// </para>
/// </remarks>
internal class Program
{
    /// <summary>
    /// Starts the web application.
    /// </summary>
    /// <param name="args">Command-line arguments passed to the application.</param>
    /// <remarks>
    /// <para>
    /// Boot sequence:
    /// <list type="number">
    /// <item> Creates a <see cref="WebApplicationBuilder"/> from <paramref name="args"/>. </item>
    /// <item> Registers dependencies via <c>builder.AddDependencies()</c> (architecture + infrastructure). </item>
    /// <item> Builds the app and wires middlewares/endpoints via <c>app.AddMiddlewares()</c>. </item>
    /// <item> Runs the host. </item>
    /// </list>
    /// </para>
    /// <para>
    /// Configuration notes:
    /// <list type="bullet"  >
    /// <item> Honors <c>ASPNETCORE_ENVIRONMENT</c> to enable Swagger UI in Development. </item>
    /// <item> Expects connection strings under <c>ConnectionStrings:WriteDatabase</c> and <c>ConnectionStrings:ReadDatabase</c>. </item>
    /// </list>
    /// </para>
    /// <para>
    /// Example:
    /// <code><![CDATA[
    /// set ASPNETCORE_ENVIRONMENT=Development
    /// dotnet run --project src/TheMovie.Api
    /// ]]></code>
    /// </para>
    /// </remarks>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.AddDependencies();

        WebApplication app = builder.Build();
        app.AddMiddlewares();

        app.Run();
    }
}
