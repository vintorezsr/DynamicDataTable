namespace DynamicDataTable
{
    /// <summary>
    /// Extension class for queryable and enumerable entities.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Process given <see cref="IQueryable{TEntity}" /> of data
        /// with given <see cref="DataTableRequest"/> request parameter
        /// to generate <see cref="DataTableResponse"/> object.
        /// </summary>
        /// <typeparam name="TEntity">The data type.</typeparam>
        /// <param name="data">The queryable data.</param>
        /// <param name="request">The request parameter.</param>
        /// <returns>Instance of <see cref="DataTableResponse"/>.</returns>
        public static DataTableResponse ToDataTableResponse<TEntity>(
            this IQueryable<TEntity> data,
            DataTableRequest request)
            where TEntity : class
        {
            var recordsTotal = data.Count();
            foreach(var sortedColumn in request.SortableColumns)
            {
                var column = request.Columns[sortedColumn.ColumnIndex];
                if (!column.Sortable) continue;
                var orderByExpression = ExpressionHelper.GetPropertyExpression<TEntity>(column.Name!);
                data = sortedColumn.SortDirection == SortDirection.Ascending
                    ? data.OrderBy(orderByExpression)
                    : data.OrderByDescending(orderByExpression);
            }

            var expressionBuilder = new ExpressionBuilder<TEntity>();
            var useGlobalSearch = !string.IsNullOrEmpty(request.GlobalSearchValue);
            foreach (var column in request.Columns)
            {
                if (!column.Searchable) continue;
                var searchValue = useGlobalSearch ? request.GlobalSearchValue : column.SearchValue;
                if (searchValue == null) continue;
                expressionBuilder.Or(column.Name!, FilterOperator.Contains, searchValue);
            }
            var predicates = expressionBuilder.Build();
            var filteredData = predicates != null ? data.Where(predicates) : data;
            var recordsFiltered = filteredData.Count();
            var pagedData = filteredData.Skip(request.Start)
                .Take(request.Length);
            var dataTableResponse = new DataTableResponse
            {
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = pagedData.ToArray(),
                Draw = request.Draw,
                Error = null
            };
            return dataTableResponse;
        }

        /// <summary>
        /// Process given <see cref="IEnumerable{TEntity}" /> of data
        /// with given <see cref="DataTableRequest"/> request parameter
        /// to generate <see cref="DataTableResponse"/> object.
        /// </summary>
        /// <typeparam name="TEntity">The data type.</typeparam>
        /// <param name="data">The list of data.</param>
        /// <param name="request">The request parameter.</param>
        /// <returns>Instance of <see cref="DataTableResponse"/>.</returns>
        public static DataTableResponse ToDataTableResponse<TEntity>(
            this IEnumerable<TEntity> data,
            DataTableRequest request)
            where TEntity : class
        {
            return data.AsQueryable()
                .ToDataTableResponse(request);
        }
    }
}