namespace TheMovie.Application.Shared
{
    /// <summary>
    /// Represents a page of items with pagination metadata.
    /// </summary>
    /// <typeparam name="T">The type of the items.</typeparam>
    /// <remarks>
    /// <para>
    /// This type captures a single page of results alongside total counts and simple navigation flags.
    /// It assumes 1-based page numbering (i.e., the first page is 1).
    /// </para>
    /// <para>
    /// Example:
    /// <code><![CDATA[
    /// var page = new PagedResult<Movie>(items, pageNumber: 2, pageSize: 10, totalCount: 57);
    /// // page.TotalPages == 6, page.HasPreviousPage == true, page.HasNextPage == true
    /// ]]></code>
    /// </para>
    /// </remarks>
    public class PagedResult<T>(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        /// <summary>
        /// Items contained in the current page.
        /// </summary>
        public List<T> Items { get; } = items;

        /// <summary>
        /// Current 1-based page number.
        /// </summary>
        public int PageNumber { get; } = pageNumber;

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; } = pageSize;

        /// <summary>
        /// Total number of items across all pages.
        /// </summary>
        public int TotalCount { get; } = totalCount;

        /// <summary>
        /// Total number of pages, computed from <see cref="TotalCount"/> and <see cref="PageSize"/>.
        /// </summary>
        public int TotalPages { get; } = (int) Math.Ceiling(totalCount / (double) pageSize);

        /// <summary>
        /// True if a previous page exists (i.e., <see cref="PageNumber"/> is greater than 1).
        /// </summary>
        public bool HasPreviousPage => PageNumber > 1;

        /// <summary>
        /// True if a next page exists (i.e., <see cref="PageNumber"/> is less than <see cref="TotalPages"/>).
        /// </summary>
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
