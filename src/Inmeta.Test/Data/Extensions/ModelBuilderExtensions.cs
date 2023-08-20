using Microsoft.EntityFrameworkCore;
using Inmeta.Test.Data.Abstractions;

namespace Inmeta.Test.Data.Extensions
{
    internal static class ModelBuilderExtensions
    {
        internal static void ConfigureProperties<T>(this ModelBuilder modelBuilder)
            where T : class, IIdentifiable, IAuditable
        {
            modelBuilder.Entity<T>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<T>()
                .Property(e => e.CreatedAt)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<T>()
                .Property(e => e.ModifiedAt)
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
