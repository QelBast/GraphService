using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Qel.Graph.Dal.Entities.UserData;

namespace Qel.Graph.Dal.Configurations.UserData;

[DbContext(typeof(DbContextMain))]
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
               .ValueGeneratedNever();

        builder.Property(e => e.Name)
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
