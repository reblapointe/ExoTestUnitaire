
using DateUtil;
using SuiviVaccinCovid.Modele;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuiviVaccinCovid
{
    public class Program
    {
        public IFournisseurDeDate FournisseurDeDate { get; set; }
        public IDaoVaccin DaoVaccin { get; set; }

        static void Main(string[] args)
        {
            // Injection des dépendances
            Program p = new Program
            {
                FournisseurDeDate = new FournisseurDeMaintenant(),
                DaoVaccin = new DBVaccinContext()
            };
            p.Peupler();

            p.EnregistrerVaccin(p.CreerNouveauVaccin("SIOA95032911", "Pfizer"));

            Vaccin lePlusRecent = p.LePlusRecent();
            Console.WriteLine(lePlusRecent);
        }

        public void Peupler()
        {
            if (DaoVaccin.ObtenirVaccins().Count() == 0)
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

                DaoVaccin.AjouterVaccin(dose1Mylene);
                DaoVaccin.AjouterVaccin(dose1Gaston);
                DaoVaccin.Sauvegarder();
            }
        }

        public Vaccin CreerNouveauVaccin(string nam, string type)
        {
            return new Vaccin
            {
                NAMPatient = nam,
                Type = type,
                Date = FournisseurDeDate.Now
            };
        }

        public void EnregistrerVaccin(Vaccin vaccin)
        {
            var memePatient = DaoVaccin.ObtenirVaccins().Where(v => v.NAMPatient == vaccin.NAMPatient);
            if (memePatient.Count() > 1)
                throw new ArgumentException("Patient déjà vacciné deux fois");
            if (memePatient.Count() == 1 && memePatient.First().Type != vaccin.Type)
                throw new ArgumentException("Un patient ne peut pas recevoir deux " +
                    "types de vaccins");

            DaoVaccin.AjouterVaccin(vaccin);
            DaoVaccin.Sauvegarder();
        }

        public Vaccin LePlusRecent(IEnumerable<Vaccin> vaccins)
        {
            if (vaccins.Count() != 0)
                return vaccins.OrderBy(v => v.Date).Last();
            return null;
        }

        public Vaccin LePlusRecent()
        {
            return LePlusRecent(DaoVaccin.ObtenirVaccins());
        }
    }
}
