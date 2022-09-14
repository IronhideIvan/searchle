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

namespace Searchle.GraphQL
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddGraphQLServer(maxAllowedRequestSize: 10000)
        .AddGlobalObjectIdentification()
        .AddQueryType<GraphQLQuery>()
        .AddTypeExtension<DictionaryResolver>()
        .AddErrorFilter<SearchleErrorFilter>();

      var assembliesToScan = new[]{
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(LexicalWordService)),
        Assembly.GetAssembly(typeof(WordnetLexicalWordDao))
      };

      services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
        .Where(c =>
          // Object mappers
          c.Name.EndsWith("Transformer")
          // Data access objects
          || c.Name.EndsWith("Dao")
          // Services
          || c.Name.EndsWith("Service")
        )
        .IgnoreThisInterface<DataAccess.Common.Interfaces.IQuery>()
        .AsPublicImplementedInterfaces();

      // Data providers
      services.AddTransient<IDictionaryDataProvider, WordnetDataProvider>(f =>
        new WordnetDataProvider(new PostgresDataProvider(new PostgresConnectionConfig
        {
          ConnectionString = "Server=localhost;Port=5432;User Id=wordnet30_admin;Password=password!1;Database=wordnet30"
        })));
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
