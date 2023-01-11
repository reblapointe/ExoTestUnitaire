using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SuiviVaccinationCovid.Modele
{
    public class VaccinationContext : DbContext
    {
        public virtual DbSet<Dose> Doses { get; set; }
        public virtual DbSet<Vaccin> Vaccins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VaccinationUT;Trusted_Connection=True;");
    }
}
