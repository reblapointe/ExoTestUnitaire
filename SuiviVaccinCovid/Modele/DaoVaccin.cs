using System;
using System.Collections.Generic;
using System.Text;

namespace SuiviVaccinCovid.Modele
{
    public class DaoVaccin : IDaoVaccin
    {
        private VaccinContext contexte = new VaccinContext();

        public IEnumerable<Vaccin> ObtenirVaccins()
        {
            return contexte.Vaccins;
        }

        public void AjouterVaccin(Vaccin v)
        {
            contexte.Vaccins.Add(v);
        }

        public void Sauvegarder()
        {
            contexte.SaveChanges();
        }
    }
}
