
using SuiviVaccinCovid.Modele;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuiviVaccinCovid
{
    public class Program
    {
        public Func<DateTime> FournisseurDeDate { get; set; }
        public VaccinContext Contexte { get; set; }

        static void Main(string[] args)
        {
            // Injection des dépendances
            Program p = new Program
            {
                FournisseurDeDate = () => DateTime.Now,
                Contexte = new VaccinContext()
            };
            p.Peupler();

            p.EnregistrerVaccin(p.CreerNouveauVaccin("SIOA95032911", "Pfizer"));

            Vaccin lePlusRecent = p.LePlusRecent();
            Console.WriteLine(lePlusRecent);
        }

        public void Peupler()
        {
            if (Contexte.Vaccins.Count() == 0)
            {
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

                Contexte.Vaccins.Add(dose1Mylene);
                Contexte.Vaccins.Add(dose1Gaston);
                Contexte.SaveChanges();
            }
        }

        public Vaccin CreerNouveauVaccin(string nam, string type)
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
            if (memePatient.Count() > 1)
                throw new ArgumentException("Patient déjà vacciné deux fois");
            if (memePatient.Count() == 1 && memePatient.First().Type != vaccin.Type)
                throw new ArgumentException("Un patient ne peut pas recevoir deux " +
                    "types de vaccins");

            Contexte.Vaccins.Add(vaccin);
            Contexte.SaveChanges();
        }

        public Vaccin LePlusRecent(IEnumerable<Vaccin> vaccins)
        {
            if (vaccins.Count() != 0)
                return vaccins.OrderBy(v => v.Date).Last();
            return null;
        }

        public Vaccin LePlusRecent()
        {
            return LePlusRecent(Contexte.Vaccins);
        }
    }
}
