using System;
using System.Collections.Generic;
using System.Text;

namespace SuiviVaccinationCovid.Modele
{
    public class Dose
    {
        public int DoseId { get; set; }
        public DateTime Date { get; set; }
        public string NAMPatient { get; set; }
        public int VaccinId { get; set; }
        public Vaccin Vaccin { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Dose dose &&
                   DoseId == dose.DoseId &&
                   Date == dose.Date &&
                   NAMPatient == dose.NAMPatient &&
                   VaccinId == dose.VaccinId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DoseId, Date, NAMPatient, Vaccin);
        }

        public override string ToString()
        {
            return $"Dose #{DoseId} ({Vaccin}), adiminstré le {Date} à {NAMPatient}";
        }

    }
}
