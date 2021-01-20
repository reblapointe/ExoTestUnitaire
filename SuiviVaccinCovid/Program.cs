using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SuiviVaccinCovid
{
    public class Vaccin
    {
        public int VaccinId { get; set; }
        public DateTime Date { get; set; }
        public string NAMPatient { get; set; }
        public TypeVaccin Type { get; set; }

        public override string ToString()
        {
            return $" Vaccin #{VaccinId} ({Type?.Nom}), adiminstré le {Date} à {NAMPatient}";
        }
    }

    public class TypeVaccin
    {
        public int TypeVaccinId { get; set; }
        public string Nom { get; set; }
    }

    public class VaccinContext : DbContext
    {
        public DbSet<Vaccin> Vaccins { get; set; }
        public DbSet<TypeVaccin> TypesVaccin { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VaccinBD;Trusted_Connection=True;");
    }

    class Program
    {
        static void Main(string[] args)
        {
            VaccinContext context = new VaccinContext();
            if (context.TypesVaccin.Count() == 0)
            {
                context.TypesVaccin.Add(new TypeVaccin { Nom = "Pfizer" });
                context.TypesVaccin.Add(new TypeVaccin { Nom = "Moderna" });
            }
            TypeVaccin pfizer = context.TypesVaccin.Where(p => p.Nom == "Pfizer").FirstOrDefault();
            TypeVaccin moderna = context.TypesVaccin.Where(p => p.Nom == "Moderna").FirstOrDefault();
            Vaccin dose1Mylene = new Vaccin
            {
                Date = DateTime.Today,
                NAMPatient = "LAPM12345678",
                Type = moderna
            };

            Vaccin dose1Gaston = new Vaccin
            {
                Date = new DateTime(2021, 01, 15),
                NAMPatient = "BHEG12345678",
                Type = pfizer
            };
            
            context.Vaccins.Add(dose1Mylene);
            context.Vaccins.Add(dose1Gaston);

            context.SaveChanges();  

            context.Remove(dose1Gaston);
            dose1Mylene.Type = pfizer;

            context.SaveChanges();  

            foreach (Vaccin vaccin in context.Vaccins)
                Console.WriteLine(vaccin);
        }
    }
}
