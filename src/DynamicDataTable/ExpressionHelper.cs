using System.Linq.Expressions;
using System.Reflection;

namespace DynamicDataTable
{
    /// <summary>
    /// Expression helper to build custom predicate.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Method metadata for <see cref="string.Contains(string)"/>.
        /// </summary>
        private static readonly MethodInfo? ContainsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
        
        /// <summary>
        /// Method metadata for <see cref="string.Contains(string, StringComparison)"/>.
        /// </summary>
        private static readonly MethodInfo? ContainsMethodIgnoreCase = typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string), typeof(StringComparison) });
        
        /// <summary>
        /// Method metadata for <see cref="string.StartsWith(string)"/>.
        /// </summary>
        private static readonly MethodInfo? StartsWithMethod = typeof(string).GetMethod(nameof(string.StartsWith), new[] { typeof(string) });

        /// <summary>
        /// Method metadata for <see cref="string.EndsWith(string)"/>.
        /// </summary>
        private static readonly MethodInfo? EndsWithMethod = typeof(string).GetMethod(nameof(string.EndsWith), new[] { typeof(string) });

        /// <summary>
        /// Method metadata for <see cref="string.IsNullOrEmpty(string?)"/>.
        /// </summary>
        private static readonly MethodInfo? IsNullOrEmptyMethod = typeof(string).GetMethod(nameof(string.IsNullOrEmpty), new[] { typeof(string) });

        /// <summary>
        /// Method metadata for <see cref="object.ToString"/>.
        /// </summary>
        private static readonly MethodInfo? ToStringMethod = typeof(object).GetMethod(nameof(string.ToString));

        /// <summary>
        /// Get predicate expression of property name, filter operator, and the compared value from particular type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="value">The object value to be compared with the property value.</param>
        /// <returns>Predicate expression object.</returns>
        public static Expression<Func<TEntity, bool>> GetPredicate<TEntity>(string propertyName, FilterOperator filterOperator, object value)
            where TEntity : class
        {
            var expressionParameter = Expression.Parameter(typeof(TEntity));
            return Expression.Lambda<Func<TEntity, bool>>(CreateFilterExpression(expressionParameter, propertyName, filterOperator, value), expressionParameter);
        }

        /// <summary>
        /// Get property expression by name from particular type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Property expression object.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Expression<Func<TEntity, object>> GetPropertyExpression<TEntity>(string propertyName)
            where TEntity : class
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var parameterExpression = Expression.Parameter(typeof(TEntity));
            var propertyExpression = parameterExpression.GetNestedProperty(propertyName);
            var convertedProperty = Expression.Convert(propertyExpression, typeof(object));
            return Expression.Lambda<Func<TEntity, object>>(convertedProperty, parameterExpression);
        }

        /// <summary>
        /// Create a filter expression from parameter expression.
        /// </summary>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <param name="propertyName">The property name.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="value">Value to be filtered.</param>
        /// <returns>Filter expression object.</returns>
        public static Expression CreateFilterExpression(ParameterExpression parameterExpression, string propertyName, FilterOperator filterOperator, object? value)
        {
            var constantExpression = Expression.Constant(value);
            var propertyExpression = parameterExpression.GetNestedProperty(propertyName);
            return filterOperator switch
            {
                FilterOperator.Equals => GetEqualsMethodCallExpression(propertyExpression, constantExpression),
                FilterOperator.GreaterThan => Expression.GreaterThan(propertyExpression, constantExpression),
                FilterOperator.LessThan => Expression.LessThan(propertyExpression, constantExpression),
                FilterOperator.ContainsIgnoreCase => Expression.Call(propertyExpression, ContainsMethodIgnoreCase!, PrepareStringConstant(constantExpression), Expression.Constant(StringComparison.OrdinalIgnoreCase)),
                FilterOperator.Contains => GetContainsMethodCallExpression(propertyExpression, constantExpression),
                FilterOperator.NotContains => Expression.Not(GetContainsMethodCallExpression(propertyExpression, constantExpression)),
                FilterOperator.StartsWith => Expression.Call(propertyExpression, StartsWithMethod!, PrepareStringConstant(constantExpression)),
                FilterOperator.EndsWith => Expression.Call(propertyExpression, EndsWithMethod!, PrepareStringConstant(constantExpression)),
                FilterOperator.NotEqual => Expression.Not(GetEqualsMethodCallExpression(propertyExpression, constantExpression)),
                FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(propertyExpression, constantExpression),
                FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(propertyExpression, constantExpression),
                FilterOperator.IsEmpty => Expression.Call(IsNullOrEmptyMethod!, propertyExpression),
                FilterOperator.IsNotEmpty => Expression.Not(Expression.Call(IsNullOrEmptyMethod!, propertyExpression)),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Get <see cref="object.Equals(object?)"/> method call expression.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="constantExpression">The constant expression.</param>
        /// <returns>Method call <see cref="object.Equals(object?)"/> expression.</returns>
        private static Expression GetEqualsMethodCallExpression(MemberExpression propertyExpression, ConstantExpression constantExpression)
        {
            return propertyExpression.Type == typeof(bool) && bool.TryParse(constantExpression.Value?.ToString(), out var value)
                ? Expression.Equal(propertyExpression, Expression.Constant(value))
                : Expression.Equal(propertyExpression, constantExpression);
        }

        /// <summary>
        /// Get <see cref="string.Contains(string)"/> method call expression.
        /// </summary>
        /// <param name="propertyExpression">The property expression.</param>
        /// <param name="constantExpression">The constant expression.</param>
        /// <returns>Method call <see cref="string.Contains(string)"/> expression.</returns>
        /// <exception cref="NotImplementedException"></exception>
        private static Expression GetContainsMethodCallExpression(MemberExpression propertyExpression, ConstantExpression constantExpression)
        {
            if (propertyExpression.Type == typeof(string))
            {
                return Expression.Call(propertyExpression, ContainsMethod!, PrepareStringConstant(constantExpression));
            }
            throw new NotImplementedException($"{propertyExpression.Type} contains is not implemented.");
        }

        /// <summary>
        /// Convert given constant expression into string constant expression.
        /// Used for Contains, StartsWith, and EndsWith filter.
        /// </summary>
        /// <param name="constantExpression">The constant expression</param>
        /// <returns>Method call <see cref="string.ToString"/> expression.</returns>
        private static Expression PrepareStringConstant(ConstantExpression constantExpression)
        {
            if (constantExpression.Type == typeof(string)) return constantExpression;
            var convertedExpression = Expression.Convert(constantExpression, typeof(object));
            return Expression.Call(convertedExpression, ToStringMethod!);
        }
    }
}