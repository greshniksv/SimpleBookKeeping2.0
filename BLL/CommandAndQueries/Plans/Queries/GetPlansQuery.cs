using System.ComponentModel;
using BLL.DtoModels;
using BLL.Interfaces;

namespace BLL.CommandAndQueries.Plans.Queries
{
	public class GetPlansQuery : IQuery<IList<PlanModel>>
	{
		public GetPlansQuery()
		{
			ShowDeleted = false;
			IsActive = null;
		}

		/// <summary>
		/// If value is TRUE, return only deleted plan; If FALSE, return only not deleted; If NULL return all;
		/// </summary>
		[DefaultValue(false)]
		public bool? ShowDeleted { get; set; }

		/// <summary>
		/// If value is TRUE, return only active plan; If FALSE, return only not active; If NULL return all;
		/// </summary>
		[DefaultValue(null)]
		public bool? IsActive { get; set; }

		public Guid UserId { get; set; }
	}
}