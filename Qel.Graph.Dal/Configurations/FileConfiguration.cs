using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Qel.Graph.Dal.Entities;
using File = Qel.Graph.Dal.Entities.File;

namespace Qel.Graph.Dal.Configurations;

[DbContext(typeof(DbContextMain))]
public class FileConfiguration : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder.ToTable("Files");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .ValueGeneratedNever();

        builder.Property(e => e.Path)
               .HasMaxLength(128)
               .IsRequired();        
        builder.Property(e => e.Json)
               .HasMaxLength(128)
               .IsRequired();

        builder.Property(e => e.CreationDateTime)
               .IsRequired(false);
        builder.Property(e => e.ModifyDateTime)
               .IsRequired(false);
        builder.Property(e => e.IsDeleted)
               .HasDefaultValue(false)
               .IsRequired();
    }
}
