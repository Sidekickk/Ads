namespace Ads.Data
{
    using Ads.Models;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public interface IAdsDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        void Dispose();

        int SaveChanges();
    }
}
