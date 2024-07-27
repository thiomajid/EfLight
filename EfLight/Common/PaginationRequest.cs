namespace EfLight.Common;

/// <summary>
///     Configures the number of table entries queried for a pagination request.
///     With this record (Index * Offset) + Offset table entries are retrieved for each request.
/// </summary>
/// <param name="Index">The Index of the requested page.</param>
/// <param name="Offset">The number of entries retrieved.</param>
public record PaginationRequest(int Index, int Offset)
{
    public int Skip => Index * Offset;
}