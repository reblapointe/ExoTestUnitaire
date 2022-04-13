
using Microsoft.EntityFrameworkCore;
using SuiviVaccinCovid.Modele;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuiviVaccinCovid
{
    // Solution avec délégués
    public class Program
    {
        public Func<DateTime> FournisseurDeDate { get; set; }
        public VaccinContext Contexte { get; set; }

        static void Main(string[] args)
        {
            // Injection des dépendances
            Program p = new()
            {
                FournisseurDeDate = () => DateTime.Now,
                Contexte = new VaccinContext()
            };
            p.Peupler();

            p.EnregistrerVaccin(p.CreerNouveauVaccin("PILA95032911", p.Contexte.TypeVaccins.Single(t => t.Nom == "Pfizer")));

            Vaccin lePlusRecent = p.LePlusRecent();
            Console.WriteLine(lePlusRecent);
        }

        public void Peupler()
        {
            if (!Contexte.TypeVaccins.Any())
            {
                Contexte.TypeVaccins.Add(new TypeVaccin { Nom = "Moderna" });
                Contexte.TypeVaccins.Add(new TypeVaccin { Nom = "Pfizer" });
                Contexte.TypeVaccins.Add(new TypeVaccin { Nom = "AstraZeneca" });
                Contexte.SaveChanges();
            }

            if (!Contexte.Vaccins.Any())
            {
                Vaccin dose1Mylene = new()
                {
                    Date = new DateTime(2021, 01, 24),
                    NAMPatient = "LAPM12345678",
                    Type = Contexte.TypeVaccins.Single(t => t.Nom == "Moderna")
                };

                Vaccin dose1Gaston = new()
                {
                    Date = new DateTime(2021, 01, 15),
                    NAMPatient = "BHEG12345678",
                    Type = Contexte.TypeVaccins.Single(t => t.Nom == "Pfizer")
                };

                Contexte.Vaccins.Add(dose1Mylene);
                Contexte.Vaccins.Add(dose1Gaston);
                Contexte.SaveChanges();
            }
        }

        public Vaccin CreerNouveauVaccin(string nam, TypeVaccin type)
        {
            return new Vaccin
            {
                NAMPatient = nam,
                Type = type,
                Date = FournisseurDeDate()
            };
        }

        public void EnregistrerVaccin(Vaccin vaccin)
        {
            var memePatient = Contexte.Vaccins.Where(v => v.NAMPatient == vaccin.NAMPatient);
            if (memePatient.Any() && Math.Abs((vaccin.Date - memePatient.OrderBy(v => v.Date).Last().Date).Days) < 21)
                throw new ArgumentException("Les doses doivent être séparées par au moins 21 jours");
            if (memePatient.Count() == 1 && memePatient.First().Type != vaccin.Type)
                throw new ArgumentException("Les deux premiers vaccins d'un patient doivent être du même type");

            Contexte.Vaccins.Add(vaccin);
            Contexte.SaveChanges();
        }

        public static Vaccin LePlusRecent(IQueryable<Vaccin> vaccins)
        {
            if (vaccins.Any())
                return vaccins.OrderBy(v => v.Date).Last();
            return null;
        }

        public Vaccin LePlusRecent()
        {
            return LePlusRecent(Contexte.Vaccins.Include(v => v.Type));
        }
    }
}
