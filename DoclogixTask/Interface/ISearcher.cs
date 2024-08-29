using DoclogixTask.Dto;
using DoclogixTask.ValueObjects;

namespace DoclogixTask.Interface
{
    public interface ISearcher
    {
        public SearchResult? Search(string query);
    }
}
