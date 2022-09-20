using System;
using System.Reflection;
using NetCore.AutoRegisterDi;
using Searchle.Common.Logging;
using Searchle.Dictionary.Business.Services;
using Wordnet.Data.Dao;

namespace Searchle.GraphQL.ApplicationStartup
{
  public static class ServiceScanner
  {
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IAppLogger<Startup> logger)
    {
      var assembliesToScan = new[]{
        Assembly.GetExecutingAssembly(),
        Assembly.GetAssembly(typeof(LexicalWordService)),
        Assembly.GetAssembly(typeof(WordnetLexicalWordDao))
      };

      var result = services.RegisterAssemblyPublicNonGenericClasses(assembliesToScan)
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

      logger.Debug("Autoregistered the following interfaces {DIAutoRegistrations}",
        result.Select(r =>
        {
          return new
          {
            Interface = r.Interface != null ? r.Interface.Namespace + "." + r.Interface.Name : "N/A",
            Class = r.Class != null ? r.Class.Namespace + "." + r.Class.Name : "N/A",
            Lifetime = r.Lifetime.ToString()
          };
        }).OrderBy(r => r.Interface)
        );

      return services;
    }
  }
}
