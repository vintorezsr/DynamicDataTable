using System.Linq.Expressions;

namespace DynamicDataTable
{
    /// <summary>
    /// Expression builder to build custom filter.
    /// </summary>
    public class ExpressionBuilder<TEntity>
    {
        /// <summary>
        /// Represent the base of <see cref="{TEntity}"/> parameter expression.
        /// </summary>
        private readonly ParameterExpression _parameterExpression;

        /// <summary>
        /// Gets or sets the current expression tree object.
        /// </summary>
        protected Expression? Expression { get; set; }

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ExpressionBuilder()
            : this(Expression.Parameter(typeof(TEntity)))
        {
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="parameterExpression">Initial parameter expression object.</param>
        public ExpressionBuilder(ParameterExpression parameterExpression) => _parameterExpression = parameterExpression;

        public ExpressionBuilder<TEntity> And(string propertyName, FilterOperator filterOperator, object? value)
        {
            var newExpression = ExpressionHelper.GetFilter(_parameterExpression, propertyName, filterOperator, value);
            Expression = Expression == null ? newExpression : Expression.AndAlso(Expression, newExpression);
            return this;
        }

        public ExpressionBuilder<TEntity> And(Action<ExpressionBuilder<TEntity>> action)
        {
            var builder = new ExpressionBuilder<TEntity>(_parameterExpression);
            action(builder);

            if (builder.Expression == null)
            {
                throw new Exception("Empty builder");
            }

            Expression = Expression == null ? builder.Expression : Expression.AndAlso(Expression, builder.Expression);
            return this;
        }

        public ExpressionBuilder<TEntity> Or(string propertyName, FilterOperator filterOperator, object? value)
        {
            var newExpression = ExpressionHelper.GetFilter(_parameterExpression, propertyName, filterOperator, value);
            Expression = Expression == null ? newExpression : Expression.OrElse(Expression, newExpression);
            return this;
        }

        public ExpressionBuilder<TEntity> Or(Action<ExpressionBuilder<TEntity>> action)
        {
            var builder = new ExpressionBuilder<TEntity>(_parameterExpression);
            action(builder);

            if (builder.Expression == null)
            {
                throw new Exception("Empty builder");
            }

            Expression = Expression == null ? builder.Expression : Expression.OrElse(Expression, builder.Expression);
            return this;
        }

        /// <summary>
        /// Build current expression tree object as lambda expression.
        /// </summary>
        /// <returns>Lamba expression of current expression tree object.</returns>
        public Expression<Func<TEntity, bool>>? Build()
        {
            if (Expression == null)
            {
                return null;
            }
            
            return Expression.Lambda<Func<TEntity, bool>>(Expression, _parameterExpression);
        }
    }
}