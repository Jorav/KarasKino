using KarasKino.Core.Movies;

namespace KarasKino.Infrastructure.Data.Config;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
  public void Configure(EntityTypeBuilder<Movie> builder)
  {
    builder.HasKey(m => m.Id);

    builder.Property(m => m.Title)
      .IsRequired()
      .HasMaxLength(255);

    builder.Property(m => m.ImdbId)
      .IsRequired()
      .HasMaxLength(20);

    builder.Property(m => m.Genres)
      .HasColumnType("text[]");

    builder.HasIndex(m => m.ImdbId)
           .IsUnique();
  }
}
