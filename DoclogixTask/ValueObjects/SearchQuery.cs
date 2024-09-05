using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static DoclogixTask.QParser;

namespace DoclogixTask.ValueObjects
{
    public class SearchQuery
    {
        public BoolOperator Operator { get; set; }

        public IEnumerable<Field> Fields { get; set; }
    }
}
