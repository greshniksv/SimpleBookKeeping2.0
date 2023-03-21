using BLL.DtoModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
	public class CostController : Controller
	{
		private readonly IMediator _mediator;

		public CostController(IMediator mediator)
		{
			_mediator = mediator;
		}

		// GET: Cost
		public ActionResult Index(Guid planId)
		{
			IList<CostModel> costModels =
				_mediator.Send(new GetCostsQuery { PlanId = planId });

			ViewBag.PlanId = planId;
			return View("Index", costModels);
		}

		public ActionResult Remove(Guid id)
		{
			_mediator.Send(new RemoveCostCommand { CostId = id });
			return new EmptyResult();
		}

		public ActionResult Create(Guid planId)
		{
			var model = _mediator.Send(new CreateCostCommand { PlanId = planId });
			return View("View", model);
		}

		public ActionResult View(Guid id)
		{
			CostModel item = _mediator.Send(new GetCostQuery { CostId = id });
			return View("View", item);
		}

		public ActionResult Save(CostModel model)
		{
			if (model.PlanId == Guid.Empty)
			{
				return RedirectToAction("Index", "Plan");
			}

			foreach (var costDetailModel in model.CostDetails)
			{
				if (costDetailModel.Value == null)
					costDetailModel.Value = 0;
			}

			if (ModelState.IsValid)
			{
				_mediator.Send(new SaveCostCommand { Cost = model });
				return RedirectToAction("Index", new { planId = model.PlanId });
			}

			return View("View", model);
		}
	}
}
