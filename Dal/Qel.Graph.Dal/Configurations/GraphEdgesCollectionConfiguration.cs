using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qel.Graph.Dal.Entities;

namespace Qel.Graph.Dal.Configurations;

[DbContext(typeof(DbContextMain))]
public class GraphEdgesCollectionConfiguration : IEntityTypeConfiguration<GraphEdgesCollection>
{
    public void Configure(EntityTypeBuilder<GraphEdgesCollection> builder)
    {
        builder.ToTable("GraphEdgesCollections");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .ValueGeneratedNever();

        builder.HasOne(e => e.Edge)
       .WithMany()
       .HasForeignKey(e => e.EdgeId)
       .IsRequired(false);
    }
}