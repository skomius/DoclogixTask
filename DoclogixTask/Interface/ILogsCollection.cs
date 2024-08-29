using DoclogixTask.Models;

namespace DoclogixTask.Interface
{
    public interface ILogsCollection
    {
        public List<Record> LogsRecords { get; set; }
    }
}
