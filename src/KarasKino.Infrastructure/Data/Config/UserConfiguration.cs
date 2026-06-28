using KarasKino.Core.Users;

namespace KarasKino.Infrastructure.Data.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.HasKey(u => u.Id);

    builder.Property(u => u.Email)
      .IsRequired()
      .HasMaxLength(255);

    builder.HasIndex(u => u.Email)
      .IsUnique();

    builder.Property(u => u.Role)
      .HasConversion<string>();

    builder.Property(u => u.CreatedAt)
      .IsRequired();
  }
}
