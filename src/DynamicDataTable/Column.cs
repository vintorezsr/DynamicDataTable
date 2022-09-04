namespace DynamicDataTable
{
    /// <summary>
    /// DataTables column definition.
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Gets or sets the column data source name.
        /// </summary>
        public string? Data { get; set; }

        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the column search value.
        /// </summary>
        public string? SearchValue { get; set; }

        /// <summary>
        /// Determine wheter this column using regex or not.
        /// </summary>
        public bool UseRegex { get; set; }

        /// <summary>
        /// Determine if this column is searchable or not.
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        /// Determine if this column is sortable or not.
        /// </summary>
        public bool Sortable { get; set; }
    }
}