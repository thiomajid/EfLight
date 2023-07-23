using Microsoft.Extensions.DependencyInjection;

namespace EfLight.Attributes;

/// <summary>
/// Used during services registration to indicate the lifetime of the repository.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class RepositoryLifetimeAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; set; }

    public RepositoryLifetimeAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}
