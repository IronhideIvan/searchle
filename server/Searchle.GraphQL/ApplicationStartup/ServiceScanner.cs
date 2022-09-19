using System;
using System.Reflection;
using NetCore.AutoRegisterDi;
using Searchle.Dictionary.Business.Services;
using Wordnet.Data.Dao;

namespace Searchle.GraphQL.ApplicationStartup
{
  public static class ServiceScanner
  {
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
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

      return services;
    }
  }
}
