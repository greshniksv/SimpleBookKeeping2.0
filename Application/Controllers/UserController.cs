using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly Serilog.ILogger logger;
		private readonly IMediator mediator;

		public UserController(
			Serilog.ILogger logger,
			IMediator mediator)
		{
			this.logger = logger;
			this.mediator = mediator;
		}

		//[AllowAnonymous]
		//[HttpPost()]
		//public async Task<IActionResult> Create(CreateUserCommand model)
		//{
		//	int recordId = await mediator.Send(model);

		//	logger.Information($"Created user: {recordId.ToString()}");
		//	return StatusCode(StatusCodes.Status201Created, 
		//		new HttpBaseResponse<object>(new { UserId = recordId }));
		//}

		//[AllowAnonymous]
		//[HttpGet("{id:int}")]
		//public async Task<IActionResult> Get(int id)
		//{
		//	UserModel? userModel = await mediator.Send(new GetUserQuery(id));

		//	if (userModel == null)
		//	{
		//		return NotFound(new HttpBaseResponse<object>(new ErrorModel("User not found")));
		//	}

		//	return Ok(new HttpBaseResponse<UserModel>(userModel));
		//}

		//[AllowAnonymous]
		//[HttpGet("{mail}")]
		//public async Task<ActionResult> Notify(string mail)
		//{
		//	await mediator.Send(new SendMailNotify(mail));
		//	return Ok();
		//}
	}
}