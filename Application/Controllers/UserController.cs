using Asp.Versioning;
using BLL.CommandAndQueries.Users.Queries;
using BLL.DtoModels;
using BLL.InternalServices.Interfaces;
using BLL.Models;
using BLL.Models.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[Authorize]
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly IHttpContextService _httpContextService;

		public UserController(IMediator mediator, IHttpContextService httpContextService)
		{
			_mediator = mediator;
			_httpContextService = httpContextService;
		}

		///  <summary>
		///  Get list of Cost by plan
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="IList{T}"/> of <see cref="CostModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Cost/byPlan/{planId}
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<UserModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> List()
		{
			IReadOnlyList<UserModel> costModels = await _mediator.Send(new GetUsersQuery());

			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<IReadOnlyList<UserModel>>(costModels));
		}
	}
}