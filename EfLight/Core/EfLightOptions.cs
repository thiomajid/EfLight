using Microsoft.Extensions.DependencyInjection;

namespace EfLight.Core;

/// <summary>
/// Configuration object to provide when adding services implementing one of EfLight's interfaces.
/// </summary>
public class EfLightOptions
{
    public ServiceLifetime DefaultLifetime { get; set; }
}
