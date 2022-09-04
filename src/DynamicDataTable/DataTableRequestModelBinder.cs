using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DynamicDataTable
{
    /// <summary>
    /// Custom <see cref="DataTableRequest"/> model binder.
    /// </summary>
    public class DataTableRequestModelBinder : IModelBinder
    {
        // Maximum number of request column iterations.
        private const int MaxDepth = 200;

        /// <inheritdoc />
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProvider = bindingContext.ValueProvider;
            var dataTableRequest = new DataTableRequest
            {
                Draw = valueProvider.GetValueOrDefault("draw", 0),
                Start = valueProvider.GetValueOrDefault("start", 0),
                Length = valueProvider.GetValueOrDefault("length", 0),
                Columns = GetColumns(valueProvider),
                SortableColumns = GetSortableColumns(valueProvider),
                GlobalSearchValue = valueProvider.GetValueOrDefault<string>("search[value]"),
                GlobalSearchUseRegex = valueProvider.GetValueOrDefault("search[regex]", false),
            };

            bindingContext.Result = ModelBindingResult.Success(dataTableRequest);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Extract the <see cref="ColumnCollection"/> from given form request.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        /// <returns>Extracted <see cref="ColumnCollection"/> object.</returns>
        private static ColumnCollection GetColumns(IValueProvider valueProvider)
        {
            var counter = 0;
            var columnCollection = new ColumnCollection();
            while (counter < MaxDepth && valueProvider.TryGetValue($"columns[{counter}][data]", out var dataStringValue))
            {
                var columnName = valueProvider.GetValueOrDefault<string>($"columns[{counter}][name]");
                var searchValue = valueProvider.GetValueOrDefault<string>($"columns[{counter}][search][value]");
                var useRegex = valueProvider.GetValueOrDefault($"columns[{counter}][search][regex]", false);
                var searchable = valueProvider.GetValueOrDefault($"columns[{counter}][searchable]", false);
                var sortable = valueProvider.GetValueOrDefault($"columns[{counter}][orderable]", false);
                columnCollection.Add(new Column
                {
                    Data = dataStringValue,
                    Name = columnName,
                    SearchValue = searchValue,
                    UseRegex = useRegex,
                    Searchable = searchable,
                    Sortable = sortable,
                });
                counter++;
            }
            return columnCollection;
        }

        /// <summary>
        /// Extract the <see cref="SortableColumnCollection"/> from given form request.
        /// </summary>
        /// <param name="formCollections">The value provider.</param>
        /// <returns>Extracted <see cref="ColumnCollection"/> object.</returns>
        private static SortableColumnCollection GetSortableColumns(IValueProvider formCollections)
        {
            var counter = 0;
            var sortableColumnCollection = new SortableColumnCollection();
            while (counter < MaxDepth && formCollections.TryGetValue($"order[{counter}][dir]", out var dirStringValue))
            {
                var sortDirection = dirStringValue == "asc"
                    ? SortDirection.Ascending
                    : SortDirection.Descending;
                var sortColumnIndex = formCollections.GetValueOrDefault($"order[{counter}][column]", 0);
                sortableColumnCollection.Add(new SortableColumn
                {
                    ColumnIndex = sortColumnIndex,
                    SortDirection = sortDirection
                });
                counter++;
            }
            return sortableColumnCollection;
        }
    }
}