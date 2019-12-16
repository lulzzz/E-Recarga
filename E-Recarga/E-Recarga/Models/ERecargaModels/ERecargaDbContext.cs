using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace E_Recarga.Models.ERecargaModels
{
    public class ERecargaDbContext : ApplicationDbContext
    {
        public ERecargaDbContext() : base()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ERecargaDbContext>());
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Pod> Pods { get; set; }
        public DbSet<PodType> PodTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppointmentStatus> AppointmentStatuses { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Price> Prices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PodType>()
                .Property(s => s.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<AppointmentStatus>()
                .Property(s => s.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);


            modelBuilder.Entity<Appointment>()
                 .HasRequired(x => x.Company)
                 .WithMany(x => x.Appointments)
                 .HasForeignKey(x => x.CompanyId)
                 .WillCascadeOnDelete(true);

            modelBuilder.Entity<Appointment>()
                 .HasOptional(x => x.Station)
                 .WithMany(x => x.Appointments)
                 .HasForeignKey(x => x.StationId);

            modelBuilder.Entity<Appointment>()
                 .HasOptional(x => x.Pod)
                 .WithMany(x => x.Appointments)
                 .HasForeignKey(x => x.PodId);

            modelBuilder.Entity<Price>()
                 .HasRequired(x => x.Station)
                 .WithMany(x => x.Prices)
                 .HasForeignKey(x => x.StationId)
                 .WillCascadeOnDelete(true);

            modelBuilder.Entity<Station>()
                 .HasMany(x => x.Pods)
                 .WithRequired(x => x.Station)
                 .HasForeignKey(x => x.StationId);

            modelBuilder.Entity<Station>()
                 .HasRequired(x => x.Company)
                 .WithMany(x => x.Stations)
                 .HasForeignKey(x => x.CompanyId)
                 .WillCascadeOnDelete(true);

            modelBuilder.Entity<Employee>()
                 .HasRequired(x => x.Company)
                 .WithMany(x => x.Employees)
                 .HasForeignKey(x => x.CompanyId)
                 .WillCascadeOnDelete(true);

            modelBuilder.Entity<Employee>()
                 .HasOptional(x => x.Station)
                 .WithMany(x => x.Employees)
                 .HasForeignKey(x => x.StationId);

            modelBuilder.Entity<Appointment>()
                .HasRequired(x => x.Status);

            modelBuilder.Entity<Pod>()
                .HasRequired(x => x.PodType);
        }
    }
}