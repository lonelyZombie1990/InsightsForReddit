using Insights.Domain.Interfaces;
using Insights.Domain.Managers;
using Insights.Repository.Auth;
using Insights.Repository.Models;
using Insights.Repository.Reddit;
using Insights.Service;
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices(services =>
    {
        services.AddLogging(config =>
        {
            // Clear out default configuration
            config.ClearProviders();
            config.AddDebug();
            config.AddConsole();
            // Add other providers as needed
        });
        services.AddOptions<AuthInfo>()
            .Configure<IConfiguration>((authInfoOptions, configuration) =>
                {
                    configuration.GetSection("AuthInfo").Bind(authInfoOptions);
                });
        services.AddSingleton<IAuthRepository, AuthRepository>();
        services.AddSingleton<RedditRepository>();       
        services.AddSingleton<IAuthManager, AuthManager>();
        services.AddSingleton<IReportManager, ReportManager>();
        services.AddSingleton<IPostManager, PostManager>();
        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync();
