using System;
using System.Collections.Generic;
using System.Text;

namespace SuiviVaccinationCovid.Modele
{
    public class Vaccin
    {
        public int VaccinId { get; set; }
        public string Nom { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Vaccin vaccin &&
                   VaccinId == vaccin.VaccinId &&
                   Nom == vaccin.Nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(VaccinId, Nom);
        }

        public override string ToString()
        {
            return $"#{VaccinId} {Nom}";
        }
    }
}
