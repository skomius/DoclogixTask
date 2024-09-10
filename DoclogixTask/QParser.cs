using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DoclogixTask.Interface;
using DoclogixTask.ValueObjects;
using Newtonsoft.Json.Linq;
using static DoclogixTask.QParser;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DoclogixTask
{
    public class QParser : IQParser
    {
        public QParser() { }

        public SearchQuery QueryParser(string query)
        {
            IEnumerable<string> fields = Enumerable.Empty<string>();

            var andFields = query.Split(Constants.AndLogicalOperator).Select(s => s.Trim());
            var orFields = query.Split(Constants.OrLogicalOperator).Select(s => s.Trim());

            if(andFields.Count() > 1 && orFields.Count() > 1)
            {
                throw new LogSearchException("Error. Multiple logical operator not supported");
            }

            if (andFields.Count() > 1)
            {
                return new SearchQuery { Operator = LogicalOperator.AND, Fields = FieldParse(fields) };
            }

            fields = query.Split(Constants.OrLogicalOperator).Select(s => s.Trim());
            if (orFields.Count() > 1)
            {
                return new SearchQuery { Operator = LogicalOperator.OR, Fields = FieldParse(fields) };
            }

            return new SearchQuery { Operator = LogicalOperator.NONE, Fields = FieldParse(fields) };
        }

        IEnumerable<Field> FieldParse(IEnumerable<string> fields)
        {
            foreach (var field in fields)
            {
                var parsedField = field.Split('=');

                if (parsedField.Length != 2)
                {
                    throw new LogSearchException("Query parsing failed");
                }

                var property = parsedField[0].FirstCharToUpper();
                var value = parsedField[1];

                bool boolOut;

                if (bool.TryParse(value, out boolOut))
                {
                    yield return new Field { Value = boolOut, Operator = Operator.Equals, Property = property };
                }
                else if (value.StartsWith('\'') && value.EndsWith('\''))
                {
                    var valueNoQuotes = value.Trim('\'');
                    yield return new Field { Value = valueNoQuotes.Trim('*'), Operator = ValueParser(valueNoQuotes), Property = property };
                }
                else
                {
                    throw new InvalidOperationException("Query parsing failed");
                }
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

        public enum LogicalOperator
        {
            AND,
            OR,
            NONE
        }
    }
}
