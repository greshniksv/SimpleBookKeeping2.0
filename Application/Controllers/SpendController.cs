using Asp.Versioning;
using BLL.CommandAndQueries.Costs.Queries;
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
		///  Get list of Spend by plan
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="IList{T}"/> of <see cref="CostSpendDetailModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Spend/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet("{costId}")]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<CostSpendDetailModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Get(Guid costId)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			IList<CostSpendDetailModel> costSpend =
				await _mediator.Send(new GetActiveCostSpendDetailsQuery {
					UserId = userId, 
					CostId = costId
				});

			var costItems = 
				costSpend.Where(x => x.Date <= DateTime.Now)
					.OrderByDescending(x => x.Date).ToList();
			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<IList<CostSpendDetailModel>>(costItems));
		}

		///  <summary>
		///  Create Spend
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="AddSpendModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Spend/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpPost()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<AddSpendModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Create([FromBody] AddSpendModel model)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			await _mediator.Send(new InsertSpendCommand {
				SpendModel = model,
				UserId = userId
			});

			return StatusCode(StatusCodes.Status200OK, new HttpBaseResponse<bool>(true));
		}

		///  <summary>
		///  Update Spend
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="AddSpendModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Spend/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpPut()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<AddSpendModel>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Update([FromBody] AddSpendModel addSpendModels)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			await _mediator.Send(new UpdateSpendCommand {
				SpendModel = addSpendModels, 
				UserId = userId
			});
			return StatusCode(StatusCodes.Status200OK, new HttpBaseResponse<bool>(true));
		}

		///  <summary>
		///  Delete Spend
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="AddSpendModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Spend/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpDelete("{spendId}")]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<AddSpendModel>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Update(Guid spendId)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			await _mediator.Send(new DeleteSpendCommand { SpendId = spendId});
			return StatusCode(StatusCodes.Status200OK, new HttpBaseResponse<bool>(true));
		}
	}
}
