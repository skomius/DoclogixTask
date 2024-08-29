using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoclogixTask.Dto;
using DoclogixTask.Interface;
using DoclogixTask.Models;
using DoclogixTask.ValueObjects;

namespace DoclogixTask
{
    public class Searcher: ISearcher
    {
        private readonly IQParser _qParser;
        private readonly IExpressionBuilder _expressionBuilder;
        private readonly ILogsCollection _logsCollection;

        public Searcher( IQParser qParser, IExpressionBuilder expressionBuilder, ILogsCollection logsCollection ) 
        {
            _qParser = qParser ?? throw new ArgumentNullException(nameof( qParser));
            _expressionBuilder = expressionBuilder ?? throw new ArgumentNullException( nameof( expressionBuilder));
            _logsCollection = logsCollection ?? throw new ArgumentNullException(nameof(logsCollection));
        }

        public SearchResult? Search(string query)
        {
            var parserResult = _qParser.QueryParser(query);

            if (parserResult == null)
            {
                throw new InvalidOperationException("Query parsing failed");
            }

            if (typeof(Record).GetProperty(parserResult.Property) == null)
            {
                Console.WriteLine("Property does not exist");
                return null;
            }

            Record[] results;

            var func = _expressionBuilder.GetExpression<Record>(new List<ParseResult>() { parserResult });
            results = _logsCollection.LogsRecords.Where(func).ToArray();

            return
                new SearchResult
                {
                    SearchQuery = query,
                    Count = results.Count(),
                    Results = results
                };
        }
    }
}
