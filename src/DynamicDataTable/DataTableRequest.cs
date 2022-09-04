namespace DynamicDataTable
{
    /// <summary>
    /// DataTables request class.
    /// </summary>
    public class DataTableRequest
    {
        /// <summary>
        /// Gets or sets the draw counter value.
        /// This is used by DataTables to ensure that the Ajax returns
        /// from server-side processing requests are drawn in sequence by DataTables
        /// (Ajax requests are asynchronous and thus can return out of sequence).
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the paging first record indicator.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the number of records that the table can display.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Column"/>.
        /// </summary>
        public ColumnCollection Columns { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of <see cref="SortableColumn"/>.
        /// </summary>
        public SortableColumnCollection SortableColumns { get; set; } = new();

        /// <summary>
        /// Gets or sets the global search value.
        /// </summary>
        public string? GlobalSearchValue { get; set; }

        /// <summary>
        /// Determine whether the advanced search (using regex) is enabled or not.
        /// </summary>
        public bool GlobalSearchUseRegex { get; set; }
    }
}