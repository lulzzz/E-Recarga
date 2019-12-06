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
        }
    }
}