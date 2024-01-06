using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qel.Graph.Dal.Entities;

namespace Qel.Graph.Dal.Configurations;

[DbContext(typeof(DbContextMain))]
public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("Nodes");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();

        builder.Property(e => e.Text)
               .HasMaxLength(128)
               .IsRequired()
               .HasDefaultValue("");
        builder.Property(e => e.Shape)
               .HasMaxLength(128)
               .HasDefaultValue("Ellipse");
        builder.Property(e => e.Label)
               .HasMaxLength(128);
        builder.Property(e => e.Color)
               .HasMaxLength(128)
               .HasDefaultValue("Black");


    }
}
