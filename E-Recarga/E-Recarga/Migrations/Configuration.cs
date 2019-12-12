namespace E_Recarga.Migrations
{
    using E_Recarga.Models;
    using E_Recarga.Models.ERecargaModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<E_Recarga.Models.ERecargaModels.ERecargaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(E_Recarga.Models.ERecargaModels.ERecargaDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

           context.AppointmentStatuses
                .AddOrUpdate(x => x.Id,
                Enum.GetValues(typeof(AppointmentStatusEnum))
                               .OfType<AppointmentStatusEnum>()
                               .Select(x => new AppointmentStatus() { Id = x, Name = x.ToString() })
                               .ToArray());


            context.PodTypes
                .AddOrUpdate(x => x.Id,
                Enum.GetValues(typeof(PodTypeEnum))
                                .OfType<PodTypeEnum>()
                                .Select(x => new PodType() { Id = x, Name = x.ToString() })
                                .ToArray());


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            foreach (var Role in Enum.GetValues(typeof(RoleEnum)).OfType<RoleEnum>())
            {
                if (!roleManager.RoleExists(Role.ToString()))
                    context.Roles.AddOrUpdate(new IdentityRole(Role.ToString()));
            }
        }
    }
}
