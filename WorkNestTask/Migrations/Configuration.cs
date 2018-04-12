namespace WorkNestTask.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WorkNestTask.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WorkNestTask.Models.TaskDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WorkNestTask.Models.TaskDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(store);
            new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "Alice" },
                new ApplicationUser { UserName = "Bob" }
            }
            .ForEach(u => userManager.Create(u, "12345_Zz"));

            context.SaveChanges();
        }
    }
}
