using System.Linq.Expressions;

namespace DynamicDataTable
{
    /// <summary>
    /// Custom property expression collection class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionCollection<T> : Dictionary<string, Expression<Func<T, object?>>>
        where T : class
    {
    }
}