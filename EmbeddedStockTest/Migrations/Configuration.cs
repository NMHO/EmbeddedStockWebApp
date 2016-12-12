namespace EmbeddedStockTest.Migrations
{
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EmbeddedStockTest.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EmbeddedStockTest.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var passwordHash = new PasswordHasher();

            var model = new RegisterViewModel { Email = "Admin@iha.dk", Password = "Admin-1234" };

            context.Users.AddOrUpdate(u => u.UserName,
                new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PasswordHash = passwordHash.HashPassword(model.Password),
                    LockoutEnabled = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                });

            
        }     
    }    
}
