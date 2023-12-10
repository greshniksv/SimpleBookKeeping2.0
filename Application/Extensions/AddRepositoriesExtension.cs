using System.Reflection;
using DAL.Interfaces;

namespace Application.Extensions
{
	public static class AddRepositoriesExtension
	{
		public static void AddRepositories(this IServiceCollection serviceCollection)
		{
			//Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			//List<Type> types = new List<Type>();
			//foreach (Assembly assembly in assemblies)
			//{
			//	try
			//	{
			//		types.AddRange(assembly.GetTypes().Where(x =>
			//			!x.IsAbstract && !x.IsInterface
			//			              && x.GetInterfaces().Any(i => i.IsGenericType
			//			                                            && i.GetGenericTypeDefinition().Name.Equals("IBaseRepository`2"))));
			//	}
			//	catch (Exception)
			//	{
			//		// Do nothing
			//	}
			//}

			//// Register
			//foreach (var type in types)
			//{
			//	List<Type> interfaceTypes = type.GetInterfaces().Where(x => !x.IsGenericType).ToList();
			//	foreach (var interfaceType in interfaceTypes)
			//	{
			//		if (interfaceType.GetInterfaces().Any(y => y?.GetGenericTypeDefinition().Name == "IBaseRepository`2"))
			//		{
			//			serviceCollection.AddScoped(interfaceType, type);
			//		}
			//	}
			//}


			// Get repositories
			Type mainType = typeof(IBaseRepository<>);
			List<Type> allTypesOfIRepository =
				(from x in AppDomain.CurrentDomain.GetAssemblies()
						.SelectMany(s => s.GetTypes())
				 where !x.IsAbstract && !x.IsInterface &&
					   x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == mainType)
				 select x).ToList();

			// Register
			foreach (var type in allTypesOfIRepository)
			{
				List<Type> interfaceTypes = type.GetInterfaces().Where(x => !x.IsGenericType).ToList();
				foreach (var interfaceType in interfaceTypes)
				{
					if (interfaceType.GetInterfaces().Any(y => y?.GetGenericTypeDefinition() == mainType))
					{
						serviceCollection.AddScoped(interfaceType, type);
					}
				}

				//Type interfaceType = type.GetInterfaces().First(x =>
				//	!x.IsGenericType && x.GetGenericTypeDefinition() == mainType);
				//serviceCollection.AddScoped(interfaceType, type);
			}
		}
	}
}
