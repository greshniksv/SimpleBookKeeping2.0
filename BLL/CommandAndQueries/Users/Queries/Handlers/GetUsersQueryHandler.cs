using AutoMapper;
using DAL.DbModels;
using DAL.Models;
using DAL.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserModel = BLL.DtoModels.UserModel;

namespace BLL.CommandAndQueries.Users.Queries.Handlers
{
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<UserModel>>
	{
		private readonly UserManager<ApplicationUser> _userRepository;
		private readonly IMapper _mapper;

		public GetUsersQueryHandler(UserManager<ApplicationUser> userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		/// <summary>Handles a request</summary>
		/// <param name="message">The request message</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Response from the request</returns>
		public async Task<IReadOnlyList<UserModel>> Handle(GetUsersQuery message, CancellationToken cancellationToken)
		{
			List<ApplicationUser> users;

			if (message.UsersId != null)
			{
				users = await _userRepository.Users.Where(x => message.UsersId.Contains(x.Id)).ToListAsync(cancellationToken);
			}
			else
			{
				users = await _userRepository.Users.ToListAsync(cancellationToken);
			}

			IList<UserModel> userModels = _mapper.Map<IList<UserModel>>(users);
			return userModels.AsReadOnly();
		}
	}
}