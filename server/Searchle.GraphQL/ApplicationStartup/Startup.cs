using Searchle.GraphQL.Schema;
using Searchle.Dictionary.Business.Services;
using Searchle.Dictionary.Common.Interfaces;
using Wordnet.Data;
using Wordnet.Data.Dao;
using Searchle.DataAccess.Postgres;
using Searchle.GraphQL.Resolvers;
using NetCore.AutoRegisterDi;
using System.Reflection;
using Searchle.GraphQL.Filters;
using Searchle.GraphQL.Logging;
using Searchle.Common.Logging;

namespace Searchle.GraphQL.ApplicationStartup
{
  public class Startup
  {
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
        .AddErrorFilter<SearchleErrorFilter>();

      try
      {
        var appConfig = services.LoadConfiguration(startupLoggerFactory.Create<Startup>());

        // Add loggers
        services.AddSingleton<IAppLoggerFactory, SerilogLoggerFactory>();
        services.AddDomainServices();

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

      app.UseRouting();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGet("/", async context =>
              {
                await context.Response.WriteAsync("Hello World!");
              });
        endpoints.MapGraphQL();
      });
    }
  }
}
