using FolderSynchronizator3000.Libs.ArgsParser;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FolderSynchronizator3000.Libs.DI;

internal class ServiceHelper
{
    private static ServiceProvider? _serviceProvider;

    public static App RegisterServices()
    {
        var services = new ServiceCollection();

        services.AddTransient<App>();

        services.AddSingleton<IParser, Parser>();

        _serviceProvider = services.BuildServiceProvider();

        return _serviceProvider.GetRequiredService<App>();
    }

    public TService ResolveService<TService>()
    {
        TService? service = _serviceProvider.GetRequiredService<TService>();
        
        ArgumentNullException.ThrowIfNull(service, nameof(service));

        return service;
    }
}
