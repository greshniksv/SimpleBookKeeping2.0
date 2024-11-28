using Asp.Versioning;
using BLL.DtoModels;
using BLL.Models.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BLL.CommandAndQueries.Plans.Queries;
using BLL.InternalServices.Interfaces;
using MediatR;

namespace Application.Controllers
{
	[Authorize(AuthenticationSchemes = "Bearer")]
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class PlanStatusController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IHttpContextService _httpContextService;

		public PlanStatusController(IMediator mediator, IHttpContextService httpContextService)
		{
			_mediator = mediator;
			_httpContextService = httpContextService;
		}

		///  <summary>
		///  Get list of Cost by plan
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="IList{T}"/> of <see cref="PlanStatusModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/Cost/byPlan/{planId}
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<PlanStatusModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> Get()
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			List<PlanStatusModel> planStatusModels = new ();
			var activePlans = await _mediator.Send(new GetPlansQuery { IsActive = true, UserId = userId });
			foreach (var activePlan in activePlans)
			{
				var planStatus = await _mediator.Send(new GetPlanStatusQuery { PlanId = activePlan.Id });
				planStatus.CurrentDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
				planStatusModels.Add(planStatus);
			}

			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<IList<PlanStatusModel>>(planStatusModels));
		}
	}
}
