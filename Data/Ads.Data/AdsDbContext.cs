namespace Ads.Data
{
    using Ads.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class AdsDbContext : IdentityDbContext<User> , IAdsDbContext
    {
        public AdsDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Advertisement> Ads { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Town> Towns { get; set; }

        public static AdsDbContext Create()
        {
            return new AdsDbContext();
        }
    }
}
