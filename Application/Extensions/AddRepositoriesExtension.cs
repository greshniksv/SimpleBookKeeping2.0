using DAL.Interfaces;

namespace Application.Extensions
{
	public static class AddRepositoriesExtension
	{
		public static void AddRepositories(this IServiceCollection serviceCollection)
		{
			// Get repositories
			Type mainType = typeof(IRepository<,>);
			List<Type> allTypesOfIRepository =
				(from x in AppDomain.CurrentDomain.GetAssemblies()
						.SelectMany(s => s.GetTypes())
				 where !x.IsAbstract && !x.IsInterface &&
					   x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == mainType)
				 select x).ToList();

			// Register
			foreach (var type in allTypesOfIRepository)
			{
				Type interfaceType = type.GetInterfaces().First(x=>
					x.IsGenericType && x.GetGenericTypeDefinition() == mainType);
				serviceCollection.AddScoped(interfaceType, type);
			}
		}
	}
}
