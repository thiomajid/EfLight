namespace EfLight.Utils;

/// <summary>
/// Shape the query for a pagination request.
/// </summary>
public sealed partial class PageRequest
{
    /// <summary>
    /// The request page index.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// The number of items to fetch.
    /// </summary>
    public int PageSize { get; init; }

    public PageRequest(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
