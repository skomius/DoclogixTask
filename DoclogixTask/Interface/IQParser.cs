using DoclogixTask.ValueObjects;

namespace DoclogixTask.Interface
{
    public interface IQParser 
    {
        public SearchQuery QueryParser(string query);
    }
}
