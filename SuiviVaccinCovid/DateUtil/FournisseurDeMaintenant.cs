using System;
using System.Collections.Generic;
using System.Text;

namespace DateUtil
{
    public class FournisseurDeMaintenant : IFournisseurDeDate
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}
