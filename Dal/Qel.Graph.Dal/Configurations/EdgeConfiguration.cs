using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qel.Graph.Dal.Entities;

namespace Qel.Graph.Dal.Configurations;

[DbContext(typeof(DbContextMain))]
public class EdgeConfiguration : IEntityTypeConfiguration<Edge>
{
    public void Configure(EntityTypeBuilder<Edge> builder)
    {
        builder.ToTable("Edges");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();

        builder.Property(e => e.Color)
               .HasMaxLength(128)
               .HasDefaultValue("Red");
        builder.Property(e => e.Label)
                .HasMaxLength(128)
                .HasDefaultValue("")
                .IsRequired();

        builder.HasOne(e => e.FromNode)
                .WithMany()
                .HasForeignKey(e => e.FromNodeId)
                .IsRequired(false);
        builder.HasOne(e => e.ToNode)
                .WithMany()
                .HasForeignKey(e => e.ToNodeId)
                .IsRequired(false);

        builder.HasOne(e => e.File)
               .WithMany(e => e.Edges)
               .HasForeignKey(e => e.FileId)
               .IsRequired();
    }
}
