using DoclogixTask.ValueObjects;

namespace DoclogixTask.Interface
{
    public interface IQParser 
    {
        public ParseResult QueryParser(string query);
    }
}
