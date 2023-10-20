using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LasPollasHermanas.Shared.Models;

namespace LasPollasHermanas.Server.Models
{
    public class DildoConfiguration : IEntityTypeConfiguration<Dildo>
    {
        public void Configure(EntityTypeBuilder<Dildo> builder)
        {
            // Fluent API
            builder.Property(dildo => dildo.Price)
            .HasPrecision(18,2);
        }
    }
}