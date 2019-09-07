namespace Backend.Migrations
{
    using mARkIt.Backend.DataObjects;
    using Microsoft.Azure.Mobile.Server.Tables;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Backend.Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }

        protected override void Seed(Backend.Models.MobileServiceContext context)
        {
            //This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.E.g.

            ////context.TestDataObjects.AddOrUpdate(
            ////    p => p.Id,
            ////  new TestDataObject { Id = "1" , LastKnownLocation = new Point { Latitude = 5, Longitude = 5 } }
            ////  );
        }
    }
}
