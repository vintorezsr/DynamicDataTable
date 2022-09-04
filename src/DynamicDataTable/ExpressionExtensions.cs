using System.Linq.Expressions;

namespace DynamicDataTable
{
    /// <summary>
    /// Extension class for <see cref="Expression"/>.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Get nested property expression from given property name.
        /// </summary>
        /// <param name="expression">The parameter expression.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Member expression object.</returns>
        public static MemberExpression GetNestedProperty(this Expression expression, string propertyName)
        {
            var propertyNames = propertyName.Split('.');
            var propertyExpression = Expression.Property(expression, propertyNames[0]);

            for (int i = 1; i < propertyNames.Length; i++)
            {
                propertyExpression = Expression.Property(propertyExpression, propertyNames[i]);
            }

            return propertyExpression;
        }
    }
}