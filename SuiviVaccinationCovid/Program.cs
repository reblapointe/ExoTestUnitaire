
using Microsoft.EntityFrameworkCore;
using SuiviVaccinationCovid.Modele;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuiviVaccinationCovid
{
    // Solution avec délégués
    public class Program
    {
        public Func<DateTime> FournisseurDeDate { get; set; }
        public VaccinationContext Contexte { get; set; }

        public static void Main(string[] _)
        {
            // Injection des dépendances
            Program p = new()
            {
                FournisseurDeDate = () => DateTime.Now,
                Contexte = new VaccinationContext()
            };
            p.Peupler();

            //p.EnregistrerDose(p.CreerNouvelleDose("PILA95032911", p.Contexte.Vaccins.Single(t => t.Nom == "Pfizer")));

            Console.WriteLine("La dose la plus récente : ");
            Console.WriteLine(p.LePlusRecent());

            Console.WriteLine();
            Console.WriteLine("Les types de vaccins : ");
            foreach (var t in p.Contexte.Vaccins)
                Console.WriteLine("  " + t);
            Console.WriteLine();
            Console.WriteLine("Les doses : ");
            foreach (var v in p.Contexte.Doses)
                Console.WriteLine("  " + v);
        }

        public void Peupler()
        {
            if (!Contexte.Vaccins.Any())
            {
                Contexte.Vaccins.Add(new Vaccin { Nom = "Moderna" });
                Contexte.Vaccins.Add(new Vaccin { Nom = "Pfizer" });
                Contexte.Vaccins.Add(new Vaccin { Nom = "AstraZeneca" });
                Contexte.SaveChanges();
            }

            if (!Contexte.Doses.Any())
            {
                Dose dose1Mylene = new()
                {
                    Date = new DateTime(2021, 01, 24),
                    NAMPatient = "LAPM12345678",
                    Vaccin = Contexte.Vaccins.Single(t => t.Nom == "Moderna")
                };

                Dose dose1Gaston = new()
                {
                    Date = new DateTime(2021, 01, 15),
                    NAMPatient = "BHEG12345678",
                    Vaccin = Contexte.Vaccins.Single(t => t.Nom == "Pfizer")
                };

                Contexte.Doses.Add(dose1Mylene);
                Contexte.Doses.Add(dose1Gaston);
                Contexte.SaveChanges();
            }
        }

        public Dose CreerNouvelleDose(string nam, Vaccin vaccin)
        {
            return new Dose
            {
                NAMPatient = nam,
                Vaccin = vaccin,
                Date = FournisseurDeDate()
            };
        }

        public void EnregistrerDose(Dose dose)
        {
            var memePatient = Contexte.Doses.Where(d => d.NAMPatient == dose.NAMPatient);
            if (memePatient.Any() && Math.Abs((dose.Date - memePatient.OrderBy(d => d.Date).Last().Date).Days) < 21)
                throw new ArgumentException("Les doses doivent être espacées d'au moins 21 jours");
            if (memePatient.Count() == 1 && memePatient.First().Vaccin != dose.Vaccin)
                throw new ArgumentException("Les deux premières doses d'un patient doivent être le même type de vaccin");

            Contexte.Doses.Add(dose);
            Contexte.SaveChanges();
        }

        public static Dose LePlusRecent(IQueryable<Dose> doses)
        {
            if (doses.Any())
                return doses.OrderBy(d => d.Date).Last();
            return null;
        }

        public Dose LePlusRecent()
        {
            return LePlusRecent(Contexte.Doses.Include(v => v.Vaccin));
        }
    }
}
