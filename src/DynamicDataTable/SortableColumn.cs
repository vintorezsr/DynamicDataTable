namespace DynamicDataTable
{
    /// <summary>
    /// DataTables sortable column class.
    /// </summary>
    public class SortableColumn
    {
        /// <summary>
        /// Gets or sets the column index reference. This is an index reference to the columns array
        /// of information that is also submitted to the server.
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    }
}