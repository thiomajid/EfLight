namespace EfLight.Utils;

/// <summary>
/// Shape the query for a pagination request.
/// </summary>
public sealed partial class PageRequest
{
    /// <summary>
    /// The requested page index.
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// The number of entries to retrieve.
    /// </summary>
    public int PageSize { get; init; }

    public PageRequest(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
