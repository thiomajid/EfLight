using EfLight.Abstractions;
using EfLight.Core;

namespace EfLight.Extensions;
internal static class RepositoryExtensions
{
    /// <summary>
    /// Checks if a given repository class inherits from <see cref="LightRepository"/>.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static bool IsLightRepository(this Type t) => t.IsSubclassOf(typeof(LightRepository));


    /// <summary>
    /// For a given repository class , checks if it implements  <see cref="ICrudRepository{TEntity, TKey}"/> or <see cref="IPagingAndSortingRepository{TEntity, TKey}"/>
    /// in its interface implementations.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static bool ImplementsLightRepository(this Type t) => 
        t.GetInterfaces()
            .Any(inter => inter.IsGenericType && 
            typeof(ILightRepository).IsAssignableFrom(inter.GetGenericTypeDefinition()));
}
