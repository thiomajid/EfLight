namespace EfLight.Utils;

/// <summary>
/// Shape the query for a pagination request.
/// </summary>
public sealed partial class PageRequest
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }

    public PageRequest(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
