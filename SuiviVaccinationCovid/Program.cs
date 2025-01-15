
using Microsoft.EntityFrameworkCore;
using SuiviVaccinationCovid.Modele;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuiviVaccinationCovid
{
    public class Program
    {
        public VaccinationContext Contexte { get; set; }
        public Func<DateTime> FournisseurDeDate { get; set; }

        public static void Main(string[] _)
        {
            // Injection des dépendances
            Program p = new()
            {
                Contexte = new VaccinationContext(),
                FournisseurDeDate = () => DateTime.Now
            };
            p.Peupler();



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
                VaccinId = vaccin.VaccinId,
                Date = FournisseurDeDate()
            };
        }

        public void EnregistrerDose(Dose dose)
        {

            // Les doses doivent être espacées d’au moins 3 semaines (21 jours).
            // Les deux premières doses doivent être du même type de vaccin.

            var dosesPatient =
                (from d in Contexte.Doses
                 where d.NAMPatient == dose.NAMPatient
                 orderby d.Date
                 select d).Include(d => d.Vaccin);

            var nbDoses = dosesPatient.Count();
            if (nbDoses == 1 && dosesPatient.First().Vaccin != dose.Vaccin)
                throw new ArgumentException("Les deux premières doses doivent être du même vaccin.");

            if (nbDoses > 0 && dosesPatient.Last().Date > dose.Date.AddDays(-21))
                throw new ArgumentException("Les doses doivent être espacées de d'au moins 21 jours.");

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
