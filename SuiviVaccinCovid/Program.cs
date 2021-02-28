using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuiviVaccinCovid
{
    public class Vaccin
    {
        public int VaccinId { get; set; }
        public DateTime Date { get; set; }
        public string NAMPatient { get; set; }
        public string Type { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Vaccin vaccin &&
                   VaccinId == vaccin.VaccinId &&
                   Date == vaccin.Date &&
                   NAMPatient == vaccin.NAMPatient &&
                   Type == vaccin.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(VaccinId, Date, NAMPatient, Type);
        }

        public override string ToString()
        {
            return $" Vaccin #{VaccinId} ({Type}), adiminstré le {Date} à {NAMPatient}";
        }

    }

    public class VaccinContext : DbContext
    {
        public DbSet<Vaccin> Vaccins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=VaccinBD;Trusted_Connection=True;");
    }

    public interface IFournisseurDeDate
    {
        public DateTime Now { get; }
    }

    public class FournisseurDeDate : IFournisseurDeDate
    {
        public DateTime Now { get { return DateTime.Now; } }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Peupler();

            VaccinContext context = new VaccinContext();

            p.AjouterVaccin(context, p.CreerNouveauVaccin(new FournisseurDeDate(), "SIOA95032911", "Pfizer"));

            Vaccin lePlusRecent = p.LePlusRecent(context.Vaccins);
            Console.WriteLine(lePlusRecent);
        }

        public void Peupler()
        {
            VaccinContext context = new VaccinContext();

            Vaccin dose1Mylene = new Vaccin
            {
                Date = new DateTime(2021, 01, 24),
                NAMPatient = "LAPM12345678",
                Type = "Moderna"
            };

            Vaccin dose1Gaston = new Vaccin
            {
                Date = new DateTime(2021, 01, 15),
                NAMPatient = "BHEG12345678",
                Type = "Pfizer"
            };

            context.Vaccins.Add(dose1Mylene);
            context.Vaccins.Add(dose1Gaston);

            context.SaveChanges();
        }

        public Vaccin CreerNouveauVaccin(IFournisseurDeDate fournisseurDate, string nam, string type)
        {
            return new Vaccin
            {
                NAMPatient = nam,
                Type = type,
                Date = fournisseurDate.Now
            };
        }

        public void AjouterVaccin(VaccinContext contexte, Vaccin vaccin)
        {
            var memePatient = contexte.Vaccins.Where(v => v.NAMPatient == vaccin.NAMPatient);
            if (memePatient.Count() > 1)
                throw new ArgumentException("Patient déjà vacciné deux fois");
            if (memePatient.Count() == 1 && memePatient.First().Type != vaccin.Type)
                throw new ArgumentException("Un patient ne peut pas recevoir deux types de vaccins");

            contexte.Add(vaccin);
            contexte.SaveChanges();
        }

        public Vaccin LePlusRecent(IEnumerable<Vaccin> vaccins)
        {
            if (vaccins.Count() != 0)
                return vaccins.OrderBy(v => v.Date).Last();
            return null;
        }
    }
}
