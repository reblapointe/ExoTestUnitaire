
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
        public VaccinationContext Contexte { get; set; }

        public static void Main(string[] _)
        {
            // Injection des dépendances
            Program p = new()
            {
                Contexte = new VaccinationContext()
            };
            p.Peupler();

            p.EnregistrerDose("PILA95032911", p.Contexte.Vaccins.Single(t => t.Nom == "Pfizer"));

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

        public void EnregistrerDose(string nam, Vaccin vaccin)
        {
            Dose dose = new()
            {
                NAMPatient = nam,
                Vaccin = vaccin,
                Date = DateTime.Now
            };

            Contexte.Doses.Add(dose);
            Contexte.SaveChanges();
        }

        public static Dose LePlusRecent(IQueryable<Dose> doses)
        {
            return doses.OrderBy(d => d.Date).Last();
        }

        public Dose LePlusRecent()
        {
            return LePlusRecent(Contexte.Doses.Include(v => v.Vaccin));
        }
    }
}
