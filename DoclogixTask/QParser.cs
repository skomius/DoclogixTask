using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DoclogixTask.Interface;
using DoclogixTask.ValueObjects;
using Newtonsoft.Json.Linq;

namespace DoclogixTask
{
    public class QParser: IQParser
    {
        public QParser() {}

        public ParseResult QueryParser(string query)
        {
            var parsedField = query.Split('=');

            if (parsedField.Length != 2)
            {
                throw new InvalidOperationException("Query parsing failed");
            }

            var field = new Field
            {
                Property = parsedField[0],
                Op = "=",
                Value = parsedField[1]
            };

            bool boolOut;

            if (bool.TryParse(field.Value, out boolOut))
            {
                return new ParseResult { Value = boolOut, Operator = Operator.Equals, Property = field.Property };
            }
            else if (field.Value.StartsWith('\'') && field.Value.EndsWith('\''))
            {
                var valueNoQuotes = field.Value.Trim('\'');
                return new ParseResult { Value = valueNoQuotes.Trim('*'), Operator = ValueParser(valueNoQuotes), Property = field.Property };
            }
            else
            {
                throw new InvalidOperationException("Query parsing failed");
            }
        }

        Operator ValueParser(string value)
        {
            Operator opr = (value.StartsWith('*'), value.EndsWith('*')) switch
            {
                (false, false) => Operator.Equals,
                (true, false) => Operator.EndsWith,
                (false, true) => Operator.StartsWith,
                (true, true) => Operator.Contains
            };

            return opr;
        }
    }
}
