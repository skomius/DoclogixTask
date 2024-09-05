using DoclogixTask.ValueObjects;

namespace DoclogixTask.Interface
{
    public interface IExpressionBuilder
    {
        public Func<T, bool> GetExpression<T>(SearchQuery filters);
    }
}
