using Asp.Versioning;
using BLL.CommandAndQueries.Costs.Queries;
using BLL.CommandAndQueries.Credits.Queries;
using BLL.CommandAndQueries.Plans.Queries;
using BLL.DtoModels;
using BLL.InternalServices.Interfaces;
using BLL.Models.Interfaces;
using BLL.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.CommandAndQueries.Spends.Commands;

namespace Application.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class SpendController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IHttpContextService _httpContextService;

		public SpendController(IMediator mediator, IHttpContextService httpContextService)
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
			typeof(ICommonReturn<IList<PlanStatusModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Get(Guid costId)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			IList<CostSpendDetailModel> costSpend =
				await _mediator.Send(new GetActiveCostSpendDetailsQuery { UserId = userId, CostId = costId });

			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<IList<CostSpendDetailModel>>(costSpend));
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
		public async Task<IActionResult> Create(AddSpendModel model)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			await _mediator.Send(new SaveSpendCommand {
				SpendModels = new[] { model },
				UserId = userId
			});

			return StatusCode(StatusCodes.Status200OK);
		}

		///  <summary>
		///  Update Spend
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="IList{T}"/> of <see cref="PlanModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Plan/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpPut()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<PlanStatusModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Update(List<AddSpendModel> addSpendModels)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			await _mediator.Send(new SaveSpendCommand { SpendModels = addSpendModels, UserId = userId });

			return StatusCode(StatusCodes.Status200OK);
		}
	}
}
