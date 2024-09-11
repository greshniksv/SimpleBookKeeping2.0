using Asp.Versioning;
using BLL.CommandAndQueries.Costs.Queries;
using BLL.CommandAndQueries.Credits.Queries;
using BLL.DtoModels;
using BLL.InternalServices.Interfaces;
using BLL.Models.Interfaces;
using BLL.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.CommandAndQueries.Spends.Commands;
using BLL.CommandAndQueries.Credits.Commands;

namespace Application.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class SpendCreditController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IHttpContextService _httpContextService;

		public SpendCreditController(IMediator mediator, IHttpContextService httpContextService)
		{
			_mediator = mediator;
			_httpContextService = httpContextService;
		}

		///  <summary>
		///  Get list of Plan by plan
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="IList{T}"/> of <see cref="PlanModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Plan/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<SpendCreditInfoModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Get(Guid costId)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			IReadOnlyList<SpendCreditInfoModel> creditSpends =
				await _mediator.Send(new GetCreditSpends() { UserId = userId, CostId = costId });

			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<IReadOnlyList<SpendCreditInfoModel>>(creditSpends));
		}

		///  <summary>
		///  Create Spend
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="IList{T}"/> of <see cref="PlanModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Plan/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpPost()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<PlanStatusModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Create(AddCreditSpendModel model)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			 await _mediator.Send(new SaveCreditSpendCommand {
				 SpendModels = new[] { model },
				 UserId = userId
			 });

			return StatusCode(StatusCodes.Status200OK);
		}

	}
}
