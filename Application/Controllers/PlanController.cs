using Asp.Versioning;
using BLL.CommandAndQueries.Costs.Queries;
using BLL.CommandAndQueries.Plans.Queries;
using BLL.DtoModels;
using BLL.Models.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.InternalServices.Interfaces;
using MediatR;
using BLL.CommandAndQueries.Users.Queries;
using BLL.CommandAndQueries.Plans.Commands;

namespace Application.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class PlanController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IHttpContextService _httpContextService;

		public PlanController(IMediator mediator, IHttpContextService httpContextService)
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
			typeof(ICommonReturn<IList<PlanModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Get()
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			IList<PlanModel> plans = await _mediator.Send(new GetPlansQuery { UserId = userId });

			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<IList<PlanModel>>(plans));
		}

		///  <summary>
		///  Get list of Plan by plan
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="PlanModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Plan/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<PlanModel>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> GetItem(Guid id)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			PlanModel model = await _mediator.Send(new GetPlanQuery { PlanId = id });
			//var users = await _mediator.Send(new GetUsersQuery());

			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<PlanModel>(model));
		}

		///  <summary>
		///  Delete Plan
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="PlanModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		DELETE /api/v1/Plan/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpDelete()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<PlanModel>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Delete(Guid id)
		{
			await _mediator.Send(new RemovePlanCommand { PlanId = id });

			return StatusCode(StatusCodes.Status200OK);
		}

		///  <summary>
		///  Create Plan
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="PlanModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		DELETE /api/v1/Plan/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpPost()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<PlanModel>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Create([FromBody]PlanModel model)
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			if (model.Start == DateTime.MinValue && model.End == DateTime.MinValue)
			{
				throw new Exception("");
			}

			PlanModel oldPlan = null;
			if (model.Id != Guid.Empty)
			{
				oldPlan = await _mediator.Send(new GetPlanQuery { PlanId = model.Id });
			}

			if (model.Start >= model.End)
			{
				throw new Exception("Дата начала должна быть меньше даты конца");
			}

			IList<CostModel> costs = null;
			if (model.Id != Guid.Empty)
			{
				costs = await _mediator.Send(new GetCostsQuery { PlanId = model.Id });
			}

			if (costs != null && costs.Any() && oldPlan != null &&
			    (oldPlan.Start.Date != model.Start.Date || oldPlan.End.Date != model.End.Date))
			{
				ModelState
					.AddModelError(nameof(model.Start),
						"Нельзя изменить дату начала, так как существуют расходы");
				ModelState
					.AddModelError(nameof(model.End),
						"Нельзя изменить дату завершения, так как существуют расходы");
			}

			await _mediator.Send(new SavePlanCommand {
				PlanModel = model,
				UserId = userId
			});

			//IReadOnlyList<UserModel> users = await _mediator.Send(new GetUsersQuery());

			return StatusCode(StatusCodes.Status200OK);
		}
	}
}
