using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheMovie.Domain.Aggregates.GenreAggregate;

namespace TheMovie.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the Entity Framework Core mapping for the <see cref="Genre"/> aggregate root.
/// </summary>
/// <remarks>
/// <para>
/// This configuration maps <see cref="Genre"/> to the <c>Genres</c> table, sets the primary key, and applies required
/// constraints and maximum length to the <c>Name</c> property. A unique index is created on <c>Name</c> to enforce
/// business-level uniqueness at the database level.
/// </para>
/// <para>
/// Domain events are not persisted; <see cref="Genre.DomainEvents"/> is ignored by EF Core.
/// </para>
/// <para>
/// Usage example:
/// <code>
/// protected override void OnModelCreating(ModelBuilder modelBuilder)
/// {
///     modelBuilder.ApplyConfiguration(new GenreConfiguration());
/// }
/// </code>
/// </para>
/// </remarks>
public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    /// <summary>
    /// Applies the EF Core configuration for the <see cref="Genre"/> entity.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the <see cref="Genre"/> entity.</param>
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(g => g.Name).IsUnique();

        builder.Ignore(g => g.DomainEvents);
    }
}
