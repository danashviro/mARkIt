namespace Backend.Migrations
{
    using Backend.DataObjects;
    using Microsoft.Azure.Mobile.Server.Tables;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Backend.Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }

        protected override void Seed(Backend.Models.MobileServiceContext context)
        {
            //This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.E.g.

            //context.Users.AddOrUpdate(
            //  p => p.Id,
            //  new User { First_Name = "User_A", Last_Name ="Test", Email="111@111.com" },
            //  new User { First_Name = "User_B", Last_Name = "Test", Email = "222@222.com" },
            //  new User { First_Name = "User_C", Last_Name = "Test", Email = "333@333.com" }
            //);
        }
    }
}
