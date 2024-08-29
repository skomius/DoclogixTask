using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DoclogixTask.ValueObjects
{
    public class ParseResult
    {
        public object Value { get; set; }
        public  Operator Operator { get; set; }
        public string Property { get; set; }
    }
}
