namespace Ads.WebApi.App_Start
{
    using System.Data.Entity;
    using Ads.Data;
    using Ads.Data.Migrations;

    public static class DatabaseConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AdsDbContext, Configuration>());
        }
    }
}