using Asp.Versioning;
using BLL.CommandAndQueries.Plans.Queries;
using BLL.DtoModels;
using BLL.InternalServices.Interfaces;
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
	public class PlanCostsController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IHttpContextService _httpContextService;

		public PlanCostsController(IMediator mediator, IHttpContextService httpContextService)
		{
			_mediator = mediator;
			_httpContextService = httpContextService;
		}

		///  <summary>
		///  Get list of Plan by plan
		///  </summary>
		///  <returns><see cref="ICommonReturn{T}"/> of <see cref="IList{T}"/> of <see cref="PlanCostsModel"/> </returns>
		///  <remarks>
		///  Sample request:
		/// 		POST /api/v1/PlanCosts/
		///  </remarks>
		///  <response code="500">Internal error</response>
		[HttpGet()]
		[ProducesResponseType(typeof(ICommonError), StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(typeof(IValidationError), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(
			typeof(ICommonReturn<IList<PlanCostsModel>>), StatusCodes.Status200OK)]
		[Produces("application/json")]
		public async Task<IActionResult> GetStatusOfPlan()
		{
			Guid userId = _httpContextService.GetCurrentUserId();
			var planCosts = await _mediator.Send(new GetActivePlanCostsQuery { UserId = userId });
			return StatusCode(StatusCodes.Status200OK,
				new HttpBaseResponse<IList<PlanCostsModel>>(
					planCosts.OrderBy(x => x.Name).ToList()));
		}
	}
}
