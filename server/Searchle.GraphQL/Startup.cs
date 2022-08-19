using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HotChocolate;
using Searchle.GraphQL.Schema;
using Searchle.Dictionary.Data.Services;
using Searchle.Dictionary.Common.Interfaces;
using Wordnet.Data;
using Wordnet.Data.Dao;
using Searchle.DataAccess.Postgres;
using Searchle.GraphQL.Resolvers;
using NetCore.AutoRegisterDi;
using System.Reflection;

namespace Searchle.GraphQL
{
  public class Startup
  {
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddGraphQLServer()
        .AddGlobalObjectIdentification()
        .AddQueryType<GraphQLQuery>()
        .AddTypeExtension<DictionaryWordResolver>();

      var assembliesToScan = new[]{
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(LexicalWordService)),
        Assembly.GetAssembly(typeof(WordnetLexicalWordDao))
      };

      services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
        .Where(c =>
          // Object mappers
          c.Name.EndsWith("Mapper")
          // Data access objects
          || c.Name.EndsWith("Dao")
          // Services
          || c.Name.EndsWith("Service")
        )
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
