namespace E_Recarga.Migrations
{
    using E_Recarga.App_Code;
    using E_Recarga.Models;
    using E_Recarga.Models.ERecargaModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<E_Recarga.Models.ERecargaModels.ERecargaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private void ClearDB(ERecargaDbContext context)
        {
            context.Database.Delete();
            context.Database.Create();


            context.Users.ToList().RemoveAll(x => x.Id == x.Id);
            context.Stations.ToList().RemoveAll(x => x.Id == x.Id);
            context.Appointments.ToList().RemoveAll(x => x.Id == x.Id);
            context.Employees.ToList().RemoveAll(x => x.Id == x.Id);
            context.Companies.ToList().RemoveAll(x => x.Id == x.Id);
            context.Pods.ToList().RemoveAll(x => x.Id == x.Id);
            context.Prices.ToList().RemoveAll(x => x.Id == x.Id);
        }

        private void AddEnumsToDB(ERecargaDbContext context)
        {
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
        }

        private void AddRolesToDB(ERecargaDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            foreach (var Role in Enum.GetValues(typeof(RoleEnum)).OfType<RoleEnum>())
            {
                if (!roleManager.RoleExists(Role.ToString()))
                    context.Roles.AddOrUpdate(new IdentityRole(Role.ToString()));
            }
        }

        private void AddCompaniesAndStationsToDB(ERecargaDbContext context)
        {
            context.Companies.AddOrUpdate(x => x.Id,
                new Company() { Id = 1, Name = "Galp Energy", Email = "GalpEnergy@galp.pt", PhoneNumber = "912345678" },
                new Company() { Id = 2, Name = "EDP ON", Email = "edpOn@edp.pt", PhoneNumber = "922345678" },
                new Company() { Id = 3, Name = "Hiberdrola Energy", Email = "Hiber@drola.pt", PhoneNumber = "932345678" }
                );

            context.Stations.AddOrUpdate(x => x.Id,
                new Station() { Id = 1, BuildingNumber = 1, ComercialName = "Station A", CompanyId = 1, Parish = "Santo Antonio dos Olivais", Region = "Coimbra", PostalCode = "1111-222", StreetName = "Rua da macumba" },
                new Station() { Id = 2, BuildingNumber = 2, ComercialName = "Station B", CompanyId = 1, Parish = "Chao da forca", Region = "Serta", PostalCode = "1111-222", StreetName = "Rua do Carasco" },
                new Station() { Id = 3, BuildingNumber = 3, ComercialName = "Station C", CompanyId = 1, Parish = "Portela", Region = "Porto", PostalCode = "1111-269", StreetName = "Rua das portinhas" },
                new Station() { Id = 4, BuildingNumber = 4, ComercialName = "Station D", CompanyId = 1, Parish = "Amadora", Region = "Setubal", PostalCode = "1345-222", StreetName = "Rua dos assassinos" },
                new Station() { Id = 5, BuildingNumber = 5, ComercialName = "Station E", CompanyId = 1, Parish = "Vale das Flores", Region = "Coimbra", PostalCode = "1234-222", StreetName = "Rua da emaculada" }
                );

            context.Stations.AddOrUpdate(x => x.Id,
                new Station() { Id = 6, BuildingNumber = 2, ComercialName = "Station A", CompanyId = 2, Parish = "Santo Antonio dos Olivais", Region = "Coimbra", PostalCode = "1111-222", StreetName = "Rua da macumba" },
                new Station() { Id = 7, BuildingNumber = 3, ComercialName = "Station B", CompanyId = 2, Parish = "Chao da forca", Region = "Serta", PostalCode = "1111-222", StreetName = "Rua do Carasco" },
                new Station() { Id = 8, BuildingNumber = 4, ComercialName = "Station C", CompanyId = 2, Parish = "Portela", Region = "Porto", PostalCode = "1111-269", StreetName = "Rua das portinhas" },
                new Station() { Id = 9, BuildingNumber = 5, ComercialName = "Station D", CompanyId = 2, Parish = "Amadora", Region = "Setubal", PostalCode = "1345-222", StreetName = "Rua dos assassinos" },
                new Station() { Id = 10, BuildingNumber = 6, ComercialName = "Station E", CompanyId = 2, Parish = "Vale das Flores", Region = "Coimbra", PostalCode = "1234-222", StreetName = "Rua da emaculada" }
                );

            context.Stations.AddOrUpdate(x => x.Id,
                new Station() { Id = 11, BuildingNumber = 3, ComercialName = "Station A", CompanyId = 3, Parish = "Santo Antonio dos Olivais", Region = "Coimbra", PostalCode = "1111-222", StreetName = "Rua da macumba" },
                new Station() { Id = 12, BuildingNumber = 4, ComercialName = "Station B", CompanyId = 3, Parish = "Chao da forca", Region = "Serta", PostalCode = "1111-222", StreetName = "Rua do Carasco" },
                new Station() { Id = 13, BuildingNumber = 5, ComercialName = "Station C", CompanyId = 3, Parish = "Portela", Region = "Porto", PostalCode = "1111-269", StreetName = "Rua das portinhas" },
                new Station() { Id = 14, BuildingNumber = 6, ComercialName = "Station D", CompanyId = 3, Parish = "Amadora", Region = "Setubal", PostalCode = "1345-222", StreetName = "Rua dos assassinos" },
                new Station() { Id = 15, BuildingNumber = 7, ComercialName = "Station E", CompanyId = 3, Parish = "Vale das Flores", Region = "Coimbra", PostalCode = "1234-222", StreetName = "Rua da emaculada" }
                );
        }


        private void AddPodsAndPricesToDB(ERecargaDbContext context)
        {
            Random gen = new Random();
            int podId = 1;
            for (int i = 1; i < 16; i++)
            {
                int podIdentifier = 1;
                for (int j = 1; j < 11; j++)
                {
                    context.Pods.AddOrUpdate(x => x.Id,
                        new Pod() { Id = podId++, isActive = true, StationId = i, PodId = PodTypeEnum.Normal, Identifier = podIdentifier++ });
                }

                for (int j = 1; j < 11; j++)
                {
                    context.Pods.AddOrUpdate(x => x.Id,
                        new Pod() { Id = podId++, isActive = true, StationId = i, PodId = PodTypeEnum.Fast, Identifier = podIdentifier++ });
                }

                foreach (var price in ScheduleGenerator.GeneratePrices(gen.Next(5,15), gen.Next(10, 25)))
                {
                    price.StationId = i;
                    context.Prices.Add(price);
                }
            }
        }


        private void AddUsersToDB(ERecargaDbContext context)
        {
            //Create Users
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);

            var admins = new List<ApplicationUser>()
            {
                new ApplicationUser {Name = "Joana Admin", Email = "joana@admin.pt", UserName = "joana@admin.pt"},
                new ApplicationUser {Name = "Wallace Admin", Email = "wallace@admin.pt", UserName = "wallace@admin.pt"}
            };

            var companyManagers = new List<Employee>()
            {
                new Employee {Name = "Joana Manager", Email = "joana@manager.pt", UserName = "joana@manager.pt", CompanyId = 1},
                new Employee {Name = "Wallace Manager", Email = "wallace@manager.pt", UserName = "wallace@manager.pt", CompanyId = 2},
                new Employee {Name = "Kiara Manager", Email = "Kiara@manager.pt", UserName = "Kiara@manager.pt", CompanyId = 3}
            };

            var workers = new List<Employee>();
            workers.Add(new Employee { Name = $"Wallace Worker", Email = $"Wallace1@worker.pt", UserName = $"Wallace1@worker.pt", CompanyId = 1, StationId = 1 });
            workers.Add(new Employee { Name = $"Wallace Worker", Email = $"Wallace2@worker.pt", UserName = $"Wallace2@worker.pt", CompanyId = 1, StationId = 2 });
            workers.Add(new Employee { Name = $"Wallace Worker", Email = $"Wallace3@worker.pt", UserName = $"Wallace3@worker.pt", CompanyId = 1, StationId = 3 });
            workers.Add(new Employee { Name = $"Wallace Worker", Email = $"Wallace4@worker.pt", UserName = $"Wallace4@worker.pt", CompanyId = 1, StationId = 4 });

            workers.Add(new Employee { Name = $"Joana Worker", Email = $"Joana1@worker.pt", UserName = $"Joana1@worker.pt", CompanyId = 2, StationId = 6 });
            workers.Add(new Employee { Name = $"Joana Worker", Email = $"Joana2@worker.pt", UserName = $"Joana2@worker.pt", CompanyId = 2, StationId = 7 });
            workers.Add(new Employee { Name = $"Joana Worker", Email = $"Joana3@worker.pt", UserName = $"Joana3@worker.pt", CompanyId = 2, StationId = 8 });
            workers.Add(new Employee { Name = $"Joana Worker", Email = $"Joana4@worker.pt", UserName = $"Joana4@worker.pt", CompanyId = 2, StationId = 9 });
            workers.Add(new Employee { Name = $"Joana Worker", Email = $"Joana5@worker.pt", UserName = $"Joana5@worker.pt", CompanyId = 2, StationId = 10 });

            var commonUsers = new List<ApplicationUser>()
            {
                new ApplicationUser() { Name = "Joana User", Email = "joana@user.pt", UserName = "joana@user.pt" },
                new ApplicationUser() { Name = "Wallace User", Email = "wallace@user.pt", UserName = "wallace@user.pt" },
            };

            string password = "q1w2e3r4.";
            //Add Users and roles
            foreach (var user in admins)
            {
                var res = manager.Create(user, password);
                if (res.Succeeded)
                    manager.AddToRole(user.Id, nameof(RoleEnum.Administrator));
            }

            //foreach (var user in companyManagers)
            //{
            //    var res = manager.Create(user, password);
            //    if (res.Succeeded)
            //        manager.AddToRole(user.Id, nameof(RoleEnum.CompanyManager));
            //}

            //foreach (var user in workers)
            //{
            //    var res = manager.Create(user, password);
            //    if (res.Succeeded)
            //        manager.AddToRole(user.Id, nameof(RoleEnum.Employee));
            //}

            //foreach (var user in commonUsers)
            //{
            //    var res = manager.Create(user, password);
            //    if (res.Succeeded)
            //        manager.AddToRole(user.Id, nameof(RoleEnum.User));
            //}
        }


        private void AddAppointmentsToDB(ERecargaDbContext context)
        {
            Random generator = new Random();
            DateTime end = DateTime.Now;
            List<AppointmentStatusEnum> appointmentStatuses = Enum.GetValues(typeof(AppointmentStatusEnum)).OfType<AppointmentStatusEnum>().ToList();
            var users = context.Users.ToList();

            for (DateTime i = DateTime.Now.AddDays(-5); i < end; i = i.AddDays(1))
            {
                for(int quant = generator.Next(15, 25); quant > 0; quant--)
                {
                    var tempA = i.AddMinutes(generator.Next(30, 800));
                    var tempB = i.AddMinutes(generator.Next(30, 800));
                    var tempC = i.AddMinutes(generator.Next(30, 800));
                    var tempD = i.AddMinutes(generator.Next(30, 800));

                    context.Appointments.AddOrUpdate(x => x.Id,
                        new Appointment()
                        {
                            StationId = generator.Next(1, 16),
                            PodId = generator.Next(1, context.Pods.Count()),
                            AppointmentStatusId = AppointmentStatusEnum.Completed,
                            Cost = generator.Next(5, 200),
                            CompanyId = 1,
                            UserId = users.ElementAt(generator.Next(1, context.Users.Count())).Id,
                            Start = tempA,
                            End = tempA.AddMinutes(generator.Next(60, 300)),
                        },
                        
                        new Appointment()
                        {
                            StationId = generator.Next(1, 16),
                            PodId = generator.Next(1, context.Pods.Count()),
                            AppointmentStatusId = AppointmentStatusEnum.Completed,
                            Cost = generator.Next(5, 200),
                            CompanyId = 2,
                            UserId = users.ElementAt(generator.Next(1, context.Users.Count())).Id,
                            Start = tempC,
                            End = tempC.AddMinutes(generator.Next(60, 300)),
                        }
                    );
                }
            }
        }


        protected override void Seed(ERecargaDbContext context)
        {
            ClearDB(context);
            AddEnumsToDB(context);
            AddRolesToDB(context);

            //AddCompaniesAndStationsToDB(context);
            //AddPodsAndPricesToDB(context);
            AddUsersToDB(context);

            //AddAppointmentsToDB(context);
        }
    }
}