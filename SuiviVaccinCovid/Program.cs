using Microsoft.EntityFrameworkCore;
using System;

namespace SuiviVaccinCovid
{
    public class Vaccin
    {
        public int VaccinId { get; set; }
        public DateTime Date { get; set; }
        public string Nom { get; set; }
        public string NAMPatient { get; set; }

        public override string ToString()
        {
            return $" Vaccin #{VaccinId} ({Nom}), adiminstré le {Date} à {NAMPatient}";
        }
    }
    public class VaccinContext : DbContext
    {
        public DbSet<Vaccin> Vaccins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VaccinBD;Trusted_Connection=True;");
    }

    class Program
    {
        static void Main(string[] args)
        {
            Vaccin dose1Mylene = new Vaccin
            {
                Date = DateTime.Today,
                NAMPatient = "LAPM12345678",
                Nom = "Moderna"
            };

            Vaccin dose1Gaston = new Vaccin
            {
                Date = new DateTime(2021, 01, 15),
                NAMPatient = "BHEG12345678",
                Nom = "Pfizer"
            };

            VaccinContext context = new VaccinContext();
            
            context.Vaccins.Add(dose1Mylene);
            context.Vaccins.Add(dose1Gaston);

            context.SaveChanges();  

            context.Remove(dose1Gaston);
            dose1Mylene.Nom = "Pfizer";

            context.SaveChanges();  

            foreach (Vaccin vaccin in context.Vaccins)
                Console.WriteLine(vaccin);


        }
    }
}
