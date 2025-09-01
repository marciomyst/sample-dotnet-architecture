
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Swashbuckle.AspNetCore.Filters;
using System.Data;
using System.Reflection;
using TheMovie.Application.Behaviours;
using TheMovie.Domain.Interfaces;
using TheMovie.Domain.SeedWork;
using TheMovie.Infrastructure.Persistence.Repositories;
using TheMovie.Infrastructure.Persistence;
using TheMovie.Application.Interfaces;
using TheMovie.Infrastructure.Services;
using TheMovie.Api.Features.Movies;

namespace TheMovie.Api.Extensions;

/// <summary>
/// Extension methods that organize application startup configuration for Program.cs.
/// </summary>
/// <remarks>
/// <para>
/// Splits the Program bootstrap into cohesive steps: dependency registration (architecture and infrastructure)
/// and middleware/endpoint wiring. This keeps Program.cs concise and easier to test and maintain.
/// </para>
/// <para>
/// Usage example:
/// <code><![CDATA[
/// var builder = WebApplication.CreateBuilder(args)
///     .AddDependencies();
/// var app = builder.Build()
///     .AddMiddlewares();
/// app.Run();
/// ]]></code>
/// </para>
/// </remarks>
public static class ProgramExtensions
{
    /// <summary>
    /// Registers application dependencies (architecture + infrastructure) into the DI container.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The same <see cref="WebApplicationBuilder"/> for chaining.</returns>
    /// <remarks>
    /// <para>
    /// Architecture registration includes: localization (Resources path), MediatR (Application assembly), FluentValidation
    /// (validators + ValidationBehaviour pipeline), in-memory caching and Swagger (XML comments + example filters).
    /// </para>
    /// <para>
    /// Infrastructure registration includes: EF Core DbContext (write model), a scoped <see cref="IDbConnection"/> for reads
    /// (Dapper), repositories for aggregates, and mapping <see cref="IUnitOfWork"/> to the DbContext implementation.
    /// </para>
    /// </remarks>
    public static WebApplicationBuilder AddDependencies(this WebApplicationBuilder builder)
    {
        AddArchitectureDependencies(builder);
        AddInfrastructureDependencies(builder);

        return builder;
    }

    /// <summary>
    /// Registers infrastructure services such as DbContext, read connections, repositories and unit of work.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <remarks>
    /// <para>
    /// Expects connection strings under configuration keys:
    /// <list type="bullet">
    /// <item><description><c>ConnectionStrings:WriteDatabase</c> — used by <see cref="TheMovieDbContext"/> via EF Core.</description></item>
    /// <item><description><c>ConnectionStrings:ReadDatabase</c> — used to construct a scoped <see cref="IDbConnection"/> for query handlers.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Also wires <see cref="IUnitOfWork"/> to the DbContext and registers repositories for aggregates.
    /// </para>
    /// </remarks>
    private static void AddInfrastructureDependencies(WebApplicationBuilder builder)
    {
        var writeConnectionString = builder.Configuration.GetConnectionString("WriteDatabase");
        builder.Services.AddDbContext<TheMovieDbContext>(options => options.UseNpgsql(writeConnectionString));

        var readConnectionString = builder.Configuration.GetConnectionString("ReadDatabase");
        builder.Services.AddScoped<IDbConnection>(serviceProvider => new NpgsqlConnection(readConnectionString));

        builder.Services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<TheMovieDbContext>());

        builder.Services.AddScoped<IMovieRepository, MovieRepository>();
        builder.Services.AddScoped<IGenreRepository, GenreRepository>();
    }

    /// <summary>
    /// Registers cross-cutting application services: MediatR, validation, caching, localization and Swagger.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <remarks>
    /// <para>
    /// MediatR scans the Application assembly for handlers; FluentValidation registers validators (including internal types)
    /// and adds a validation pipeline behavior. Swagger is configured with XML comments and Swashbuckle example filters.
    /// </para>
    /// <para>
    /// Localization resolves resources from the configured <c>Resources</c> path.
    /// </para>
    /// </remarks>
    private static void AddArchitectureDependencies(WebApplicationBuilder builder)
    {
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly, includeInternalTypes: true);

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<ICacheService, MemoryCacheService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.ExampleFilters();

            string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            options.IncludeXmlComments(xmlPath);
        });

        builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        builder.Services.AddAuthorization();

    }

    /// <summary>
    /// Configures middlewares and maps endpoints.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>The same <see cref="WebApplication"/> for chaining.</returns>
    /// <remarks>
    /// <para>
    /// Applies request localization for supported cultures, maps all Movies endpoints, enables Swagger in development,
    /// and adds HTTPS redirection and authorization.
    /// </para>
    /// </remarks>
    public static WebApplication AddMiddlewares(this WebApplication app)
    {
        string[] supportedCultures = ["en-US", "pt-BR"];

        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseRequestLocalization(localizationOptions);

        app.MapMovieEndpoints();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        return app;
    }
}
