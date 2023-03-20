using System.Threading;
using AutoFixture.Xunit2;
using AutoMapper;
using DAL.DbModels;
using DAL.Interfaces;
using Moq;
using TestProject.Tools.Attributes;
using Xunit;

namespace TestProject.UserCommands.Handlers
{
	public class CreateUserHandlerTests
	{
		//[Theory, AutoMoqData]
		//public async void Handle_WhenRepositoryExist_ThenExecuteRepository(
		//	[Frozen] Mock<IRepository<User, int>> repositoryFactory,
		//	[Frozen] Mock<IMapper> mapper,
		//	CreateUserHandler handler,
		//	CreateUserCommand command,
		//	User user)
		//{
		//	mapper.Setup(x => x.Map<CreateUserCommand, User>(command)).Returns(user);

		//	await handler.Handle(command, CancellationToken.None);

		//	repositoryFactory.Verify(x => x.ExecuteAsync(user), Times.Once);
		//}

		/* And other */
	}
}
