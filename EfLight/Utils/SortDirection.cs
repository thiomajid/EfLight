﻿using EfLight.Abstractions;

namespace EfLight.Utils;

/// <summary>
/// Defines the sort direction when using methods from <see cref="IPagingAndSortingRepository{TEntity, TKey}"/>.
/// </summary>
public enum SortDirection
{
    Ascending,
    Descending
}
