using AutoMapper;
using BLL.ViewModels;
using DAL.DbModels;

namespace BLL.AutoMapperProfiles
{
	public class DbToViewModelProfile : Profile
	{
		public DbToViewModelProfile()
		{
			CreateMap<User, UserModel>();
		}
	}
}
