using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheMovie.Domain.Aggregates.MovieAggregate;

namespace TheMovie.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the Entity Framework Core mapping for the <see cref="Movie"/> aggregate root.
/// </summary>
/// <remarks>
/// <para>
/// This configuration maps <see cref="Movie"/> to the <c>Movies</c> table, establishes the primary key,
/// and applies constraints for required fields and maximum lengths. It also configures an explicit column type for
/// the monetary value <c>Price</c> and a value conversion for the <c>Rating</c> enumeration to its underlying <c>int</c>.
/// </para>
/// <para>
/// Indexes are created on <c>Title</c> and <c>GenreId</c> to speed up lookups; <c>Title</c> is non-unique here to allow
/// remakes and similarly named entries, while business-level uniqueness (if required) should be enforced in the domain/application layer.
/// </para>
/// <para>
/// Domain events are not persisted; <see cref="Movie.DomainEvents"/> is ignored by EF Core to keep the persistence model clean.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// protected override void OnModelCreating(ModelBuilder modelBuilder)
/// {
///     modelBuilder.ApplyConfiguration(new MovieConfiguration());
/// }
/// </code>
/// </para>
/// </remarks>
public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    /// <summary>
    /// Applies the EF Core configuration for the <see cref="Movie"/> entity.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the <see cref="Movie"/> entity.</param>
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable("Movies");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Synopsis)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(m => m.ReleaseYear)
            .IsRequired();

        builder.Property(m => m.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(m => m.GenreId)
            .IsRequired();

        builder.Property(m => m.Rating)
            .IsRequired()
            .HasConversion<int>();

        builder.HasIndex(m => m.Title);
        builder.HasIndex(m => m.GenreId);

        builder.Ignore(m => m.DomainEvents);
    }
}
