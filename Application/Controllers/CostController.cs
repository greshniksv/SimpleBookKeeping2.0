using Asp.Versioning;
using BLL.CommandAndQueries.Costs.Commands;
using BLL.CommandAndQueries.Costs.Queries;
using BLL.DtoModels;
using BLL.Models;
using BLL.Models.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class CostController : Controller
	{
		private readonly IMediator _mediator;

		public CostController(IMediator mediator)
		{
			_mediator = mediator;
		}

		///  <summary>
		///  Get list of Cost by plan
		///  </summary>
		///  <param name="planId"></param>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="IList{T}"/> of <see cref="CostModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Cost/byPlan/{planId}
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet("byPlan")]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<CostModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> ListByPlan(Guid planId)
		{
			IList<CostModel> costModels = await _mediator.Send(new GetCostsQuery { PlanId = planId });

			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<IList<CostModel>>(costModels));
		}

		///  <summary>
		///  Get list of Cost
		///  </summary>
		///  <param name="costId"></param>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="CostModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Cost
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<CostModel>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Item(Guid costId)
		{
			CostModel costModels = await _mediator.Send(new GetCostQuery { CostId = costId});

			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<CostModel>(costModels));
		}

		///  <summary>
		///  Remove Cost
		///  </summary>
		///  <param name="id"></param>
		///  <remarks>
		///  Sample request:
		/// 		DELETE /api/v1/Cost
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpDelete()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<CostModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Remove(Guid id)
		{
			await _mediator.Send(new RemoveCostCommand { CostId = id });
			return StatusCode(StatusCodes.Status200OK);
		}

		///  <summary>
		///  Generate Cost with details
		///  </summary>
		///  <param name="planId"></param>
		///  <remarks>
		///  Sample request:
		/// 		GET /api/v1/Cost/generate
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet("generate")]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<CostModel>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Generate(Guid planId)
		{
			CostModel cost = await _mediator.Send(new GenerateCostCommand(){ PlanId = planId });
			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<CostModel>(cost));
		}

		///  <summary>
		///  Save Cost with details
		///  </summary>
		///  <param name="model"></param>
		///  <remarks>
		///  Sample request:
		/// 		GET /api/v1/Cost/generate
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpPost("save")]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<CostModel>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Save([FromBody]CostModel model)
		{
			//if (model.PlanId == Guid.Empty)
			//{
			//	return RedirectToAction("Index", "Plan");
			//}

			foreach (var costDetailModel in model.CostDetails)
			{
				if (costDetailModel.Value == null)
				{
					costDetailModel.Value = 0;
				}
			}

			//if (ModelState.IsValid)
			//{
			//	//return RedirectToAction("Index", new { planId = model.PlanId });
			//}

			await _mediator.Send(new SaveCostCommand { Cost = model });

			return StatusCode(StatusCodes.Status200OK);
		}
	}
}
