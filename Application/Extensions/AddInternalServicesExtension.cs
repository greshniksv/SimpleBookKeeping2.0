using System.Reflection;
using BLL.Interfaces;

namespace Application.Extensions
{
	public static class AddInternalServicesExtension
	{
		public static void AddInternalServices(this IServiceCollection serviceCollection)
		{
			// Get types
			Type mainType = typeof(IInternalService);
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> types = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					types.AddRange(assembly.GetTypes().Where(x =>
						!x.IsAbstract && !x.IsInterface && x.GetInterfaces().Any(i => i == mainType)));
				}
				catch (Exception)
				{
					// Do nothing
				}
			}

			// Register
			foreach (var type in types)
			{
				List<Type> interfaceTypes = type.GetInterfaces().Where(x => !x.IsGenericType).ToList();
				foreach (var interfaceType in interfaceTypes)
				{
					if (interfaceType.GetInterfaces().Any(y => y == mainType))
					{
						serviceCollection.AddScoped(interfaceType, type);
					}
				}
			}
		}
	}
}
