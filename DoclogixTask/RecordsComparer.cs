using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoclogixTask.Models;

namespace DoclogixTask
{
    public class RecordsComparer : IEqualityComparer<Record>
    {
        public bool Equals(Record? a, Record? b)
        {
            if (a == null && b == null)
                return true;

            if (a == null || b == null)
                return false;

            if (a.smac == b.smac && a.start == b.start && a.shost == b.shost &&
                a.rt == b.rt && a.dmac == b.dmac)
                return true;
            else
                return false;
        }

        public int GetHashCode(Record value)
        {
            if (object.ReferenceEquals(value, null)) 
                return 0;

            int hashsmac = value.smac == null ? 0 : value.smac.GetHashCode();
            int hashstart = value.start == null ? 0 : value.start.GetHashCode();
            int hashshost = value.shost == null ? 0 : value.shost.GetHashCode();
            int hashrt = value.rt == null ? 0 : value.rt.GetHashCode();
            int hashdmac = value.dmac == null ? 0 : value.dmac.GetHashCode();

            return hashsmac ^ hashstart ^ hashshost ^ hashrt ^ hashdmac;
        }
    }
}

