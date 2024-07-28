using EfLight.Abstractions;
using EfLight.Core;

namespace EfLight.Extensions;

internal static class Reflection
{
    /// <summary>
    /// Checks if a given interface inherits from <see cref="ILightRepository"/> without being <see cref="ILightRepository"/>,
    /// <see cref="ICrudRepository{TEntity, TKey}"/>  or <see cref="IPagingAndSortingRepository{TEntity, TKey}"/> itself.
    /// </summary>
    /// <param name="interfaceType"></param>
    /// <returns></returns>
    public static bool IsSubInterfaceOfLightRepository(this Type interfaceType)
    {
        return !interfaceType.IsGenericType &&
               interfaceType != typeof(ILightRepository) &&
               typeof(ILightRepository).IsAssignableFrom(interfaceType);
    }

    /// <summary>
    /// Checks if a given repository class inherits from <see cref="Core.LightRepository"/>.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static bool IsSubclassOfLightRepository(this Type t)
    {
        if (t == null)
            return false;

        // Check if the type is a generic type
        if (!t.IsGenericType)
        {
            return t.BaseType != null && IsSubclassOfLightRepository(t.BaseType);
        }

        var genericTypeDefinition = t.GetGenericTypeDefinition();
        var isPagingRepository = genericTypeDefinition == typeof(PagingAndSortingRepository<,,>);
        var isCrudRepository = genericTypeDefinition == typeof(CrudRepository<,,>);
        var isLightRepo = genericTypeDefinition == typeof(LightRepository<>);

        return isPagingRepository || isCrudRepository || isLightRepo;

        // If not, check its base type
    }


    /// <summary>
    /// For a given repository class , checks if it implements  <see cref="ICrudRepository{TEntity, TKey}"/>
    /// or <see cref="IPagingAndSortingRepository{TEntity, TKey}"/> through its interface implementations.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns></returns>
    public static bool ExtendsLightRepositoryInterface(this Type repository)
    {
        return repository.GetInterfaces()
            .Any(@interface => @interface.IsSubInterfaceOfLightRepository());
    }
}