namespace FitTrackApi.Application.Dto;

/// <summary>
/// Represents a paginated result set, including the returned items and pagination metadata
/// such as total count, current page number, page size, and total pages.
/// </summary>
/// <typeparam name="T"></typeparam>
public class PagedListResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
