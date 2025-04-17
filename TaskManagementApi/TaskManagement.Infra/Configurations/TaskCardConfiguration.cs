using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManagement.Domain;
using TaskManagement.Domain.ValueObjects;

namespace TaskManagement.Infra.Configurations;

internal sealed class TaskCardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("Cards");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Description)
            .HasConversion(
                v => v.ToString(),
                v => CardDescription.Create(v!).Value!)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Responsible)
            .HasConversion(
                v => v.ToString(),
                v => CardResponsible.Create(v!).Value!)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion(new EnumToStringConverter<CardStatus>())
            .IsRequired();

        builder.Property(x => x.CreatedOnUtc)
            .IsRequired();

        builder.Property(x => x.UpdatedOnUtc)
            .IsRequired(false);
    }
}