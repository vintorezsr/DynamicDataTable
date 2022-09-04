namespace DynamicDataTable
{
    /// <summary>
    /// DataTables response class.
    /// </summary>
    public class DataTableResponse
    {
        /// <summary>
        /// Gets or sets the draw counter value.
        /// The draw counter that this object is a response to - from the draw parameter
        /// sent as part of the <see cref="DataTableRequest"/>.
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Gets or sets the total of records before filtering.
        /// </summary>
        public int RecordsTotal { get; set; }

        /// <summary>
        /// Gets or sets the total of records after filtering.
        /// </summary>
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets the collection of data.
        /// </summary>
        public object[]? Data { get; set; }

        /// <summary>
        /// Gets or sets the error information if an error occurs
        /// during the running of the server-side processing.
        /// </summary>
        public string? Error { get; set; }
    }
}