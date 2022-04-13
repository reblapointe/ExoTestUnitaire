using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SuiviVaccinCovid.Modele
{
    public class VaccinContext : DbContext
    {
        public virtual DbSet<Vaccin> Vaccins { get; set; }
        public virtual DbSet<TypeVaccin> TypeVaccins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VaccinBDT;Trusted_Connection=True;");
    }
}
