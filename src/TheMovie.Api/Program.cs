
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Swashbuckle.AspNetCore.Filters;
using System.Data;
using System.Reflection;
using TheMovie.Api.Features.Movies;
using TheMovie.Api.Features.Movies.CreateMovie;
using TheMovie.Api.Features.Movies.DeleteMovie;
using TheMovie.Application.Behaviours;
using TheMovie.Application.Interfaces;
using TheMovie.Domain.Interfaces;
using TheMovie.Domain.SeedWork;
using TheMovie.Infrastructure.Persistence;
using TheMovie.Infrastructure.Persistence.Repositories;
using TheMovie.Infrastructure.Services;

namespace TheMovie.Api
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly, includeInternalTypes: true);

            string? writeConnectionString = builder.Configuration.GetConnectionString("WriteDatabase");
            builder.Services.AddDbContext<TheMovieDbContext>(options => options.UseNpgsql(writeConnectionString));

            string? readConnectionString = builder.Configuration.GetConnectionString("ReadDatabase");
            builder.Services.AddScoped<IDbConnection>(serviceProvider => new NpgsqlConnection(readConnectionString));

            builder.Services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<TheMovieDbContext>());

            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<IGenreRepository, GenreRepository>();

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

            WebApplication app = builder.Build();

            string[] supportedCultures = new[] { "en-US", "pt-BR" };

            RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
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

            app.Run();
        }
    }
}
