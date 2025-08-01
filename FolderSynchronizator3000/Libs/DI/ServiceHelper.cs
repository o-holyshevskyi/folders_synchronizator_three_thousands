using FolderSynchronizator3000.Libs.ArgsParser;
using FolderSynchronizator3000.Libs.Helpers;
using FolderSynchronizator3000.Libs.Logging;
using FolderSynchronizator3000.Libs.Sync;
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
        services.AddSingleton<ISyncker, Syncker>();
        services.AddSingleton<ILog, Log>();
        services.AddSingleton<IFileHelper, FileHelper>();

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
