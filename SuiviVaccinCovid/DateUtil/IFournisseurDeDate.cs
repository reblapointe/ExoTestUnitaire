using System;
using System.Collections.Generic;
using System.Text;

namespace DateUtil
{
    public interface IFournisseurDeDate
    {
        public DateTime Now { get; }
    }
}
