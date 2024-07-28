using EfLight.Abstractions;

namespace EfLight.Extensions;
internal static class Reflection
{
    /// <summary>
    /// Checks if a given interface inherits from <see cref="ILightRepository"/> without being <see cref="ILightRepository"/>,
    /// <see cref="ICrudRepository{TEntity, TKey}"/>  or <see cref="IPagingAndSortingRepository{TEntity, TKey}"/> itself.
    /// </summary>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    public static bool InheritsFromRepositoryInterface(this Type interfaceType)
    {
        return !interfaceType.IsGenericType &&
                interfaceType != typeof(ILightRepository) &&
                typeof(ILightRepository).IsAssignableFrom(interfaceType);
    }
}
