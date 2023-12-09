using AutoMapper;
using DAL.DbModels;
using DAL.Repositories.Interfaces;
using MediatR;
using UserModel = BLL.DtoModels.UserModel;

namespace BLL.CommandAndQueries.Users.Queries.Handlers
{
	public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyList<UserModel>>
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
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
			List<User> users;

			if (message.UsersId != null)
			{
				users = await _userRepository.GetAsync(x => message.UsersId.Contains(x.Id)).ToListAsync(cancellationToken);
			}
			else
			{
				users = await _userRepository.GetAsync().ToListAsync(cancellationToken);
			}

			IList<UserModel> userModels = _mapper.Map<IList<UserModel>>(users);
			return userModels.AsReadOnly();
		}
	}
}