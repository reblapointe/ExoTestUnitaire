using System;
using System.Collections.Generic;
using System.Text;

namespace SuiviVaccinCovid.Modele
{
    public interface IDaoVaccin
    {
        public IEnumerable<Vaccin> ObtenirVaccins();
        public void AjouterVaccin(Vaccin v);
        public void Sauvegarder();
    }
}
