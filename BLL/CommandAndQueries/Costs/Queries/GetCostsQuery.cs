using System.ComponentModel;
using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Costs.Queries
{
	public class GetCostsQuery : IQuery<IList<CostModel>>
	{
		public Guid PlanId { get; set; }

		[DefaultValue(false)]
		public bool ShowDeleted { get; set; }
	}
}