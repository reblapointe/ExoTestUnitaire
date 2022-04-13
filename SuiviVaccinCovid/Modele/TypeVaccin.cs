using System;
using System.Collections.Generic;
using System.Text;

namespace SuiviVaccinCovid.Modele
{
    public class TypeVaccin
    {
        public int TypeVaccinId { get; set; }
        public string Nom { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TypeVaccin vaccin &&
                   TypeVaccinId == vaccin.TypeVaccinId &&
                   Nom == vaccin.Nom;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TypeVaccinId, Nom);
        }

        public override string ToString()
        {
            return $"#{TypeVaccinId} {Nom}";
        }
    }
}
