using Searchle.GraphQL.Schema;
using Searchle.Dictionary.Common.Interfaces;
using Wordnet.Data;
using Searchle.DataAccess.Postgres;
using Searchle.GraphQL.Resolvers;
using Searchle.GraphQL.Filters;
using Searchle.GraphQL.Logging;
using Searchle.Common.Logging;

namespace Searchle.GraphQL.ApplicationStartup
{
  public class Startup
  {
    private IWebHostEnvironment _environment;

    public Startup(IWebHostEnvironment environment)
    {
      _environment = environment;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      var startupLoggerFactory = new SerilogLoggerFactory(new AppLoggingConfig
      {
        LogLevel = AppLogLevel.Debug
      });

      var logger = startupLoggerFactory.Create<Startup>();

      services.AddGraphQLServer(maxAllowedRequestSize: 10000)
        .AddGlobalObjectIdentification()
        .AddQueryType<GraphQLQuery>()
        .AddTypeExtension<DictionaryResolver>()
        .AddErrorFilter<SearchleErrorFilter>((f) =>
        {
          return new SearchleErrorFilter(f.GetService<IAppLoggerFactory>()!);
        });

      try
      {
        var appConfig = services.LoadConfiguration(startupLoggerFactory.Create<Startup>(), _environment);

        // Initialize a new logger factory using the real configuration.
        startupLoggerFactory = new SerilogLoggerFactory(appConfig.Logging!);
        logger = startupLoggerFactory.Create<Startup>();

        // Add loggers
        services.AddSingleton<IAppLoggerFactory, SerilogLoggerFactory>();
        services.AddDomainServices(startupLoggerFactory.Create<Startup>());

        // Data providers
        services.AddTransient<IDictionaryDataProvider, WordnetDataProvider>(f =>
          new WordnetDataProvider(
            new PostgresDataProvider(
              appConfig!.DictionaryConnectionConfig!,
              f.GetService<IAppLoggerFactory>()!
              )
            ));
      }
      catch (Exception ex)
      {
        logger.Critical("Error in startup: {Message}", ex, new { Message = ex.Message });
        throw;
      }
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      var appConfig = app.ApplicationServices.GetService<SearchleAppConfig>();

      app.UseRouting();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGet("/", async (context) =>
              {
                var metadata = appConfig!.Metadata;
                await context.Response.WriteAsync($"{metadata.ApplicationName} API Version {metadata.Version}, Environment {metadata.EnvironmentName}, Timestamp {DateTime.UtcNow}");
              });
        endpoints.MapGraphQL();
      });
    }
  }
}
