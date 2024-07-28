using System.Reflection;

using EfLight.Attributes;
using EfLight.Core;
using Microsoft.Extensions.DependencyInjection;

namespace EfLight.Extensions;
public static class ServicesCollectionExtensions
{
    /// <summary>
    /// Registers all repositories in <typeparamref name="TAssembly"/> using <see cref="ServiceLifetime.Scoped"/> by default
    /// if they are not decorated with <see cref="RepositoryLifetimeAttribute"/> or <paramref name="options"/> is null.
    /// <br/>
    /// <strong>Note that using <see cref="ServiceLifetime.Singleton"/> will throw an  <see cref="Exception"/> when 
    /// WebApplicationBuilder will try to build the app.</strong>
    /// </summary>
    /// <typeparam name="TAssembly">
    ///     The assembly where the repositories are located.
    /// </typeparam>
    /// <param name="services"></param>
    /// <param name="options">
    ///     Setup options to indicate the default lifetime of the repositories if they are not decorated with
    ///     <see cref="RepositoryLifetimeAttribute"/>
    /// </param>
    /// <returns>
    ///     A reference to this instance after the operation has completed.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     If one of the repositories does not implement an interface extending from the generic repositories,
    ///     this exception will be thrown.
    /// </exception>
    public static IServiceCollection AddEfLight<TAssembly>(this IServiceCollection services, Action<EfLightOptions>? options = null)
    {
        // retrievng the options
        var efLightOptions = new EfLightOptions();
        options?.Invoke(efLightOptions);

        // registering repositories
        var targets = typeof(TAssembly).Assembly.ExportedTypes
            .Where(exportedType =>
                exportedType.IsLightRepository() &&
                exportedType.ImplementsLightRepository())
            .ToList();

        targets.ForEach(repoClass =>
        {
            var implInterface = repoClass.GetInterfaces()
             .SingleOrDefault(inter => inter.InheritsFromRepositoryInterface());

            var lifetimeAttribute = repoClass.GetCustomAttribute<RepositoryLifetimeAttribute>();
            
            if (implInterface is null)
            {
                throw new ArgumentException($"{repoClass.Name} must implement at least ICrudRepository");
            }

            var lifetime = lifetimeAttribute?.Lifetime switch
            {
                ServiceLifetime.Transient => ServiceLifetime.Transient,
                ServiceLifetime.Scoped => ServiceLifetime.Scoped,
                ServiceLifetime.Singleton => throw new ArgumentException($"Invalid lifetime for {repoClass.Name}. Singleton lifetime is not allowed, it prevents the app to build."),
                _ => options is null ? ServiceLifetime.Scoped : efLightOptions.DefaultLifetime
            };

            RegisterRepository(services, repository: repoClass, implementedInterface: implInterface, lifetime: lifetime);
        });

        return services;
    }

    private static void RegisterRepository(IServiceCollection services, Type repository, Type implementedInterface, ServiceLifetime? lifetime)
    {
        _ = lifetime switch
        {
            ServiceLifetime.Scoped => services.AddScoped(implementedInterface, repository),
            ServiceLifetime.Transient => services.AddTransient(implementedInterface, repository),
            _ => throw new ArgumentException($"Invalid lifetime for {repository.Name}")
        };
    }
}
