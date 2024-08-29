using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoclogixTask.ValueObjects
{
    internal class Field
    {
        public string Property { get; set; }
        public string Op { get; set; }
        public string Value { get; set; }
    }
}
