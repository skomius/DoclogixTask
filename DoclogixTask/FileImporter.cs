using DoclogixTask.Dto;
using DoclogixTask.Interface;
using DoclogixTask.Models;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;

namespace DoclogixTask
{
    public class FileImporter: IFileImporter
    {
        private readonly ILogsCollection _logsCollection;

        public FileImporter( ILogsCollection logsCollection )  
        {
            _logsCollection = logsCollection ?? throw new ArgumentNullException(nameof(logsCollection));
        }

        public void ImportFile(IEnumerable<string> paths, int minSeverity = int.MaxValue)
        {
            foreach (var path in paths)
            {
                using (var reader = new StreamReader(path))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = csv.GetRecord<Record>();
                        _logsCollection.LogsRecords.Add(record);

                        if (record.severity <= minSeverity)
                        {
                            Console.WriteLine(JsonConvert.SerializeObject(record, Formatting.Indented));
                        }
                    }
                }
            }

            _logsCollection.LogsRecords = _logsCollection.LogsRecords.Distinct(new RecordsComparer()).ToList();
        }
    }
}
