using AutoMapper;
using BLL.Interfaces;
using DAL.DbModels;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.CommandAndQueries.Plans.Commands.Handlers
{
	public class SavePlanCommandHandler : ICommandHandler<SavePlanCommand, bool>
	{
		private readonly UserManager<ApplicationUser> _userRepository;
		private readonly IPlanRepository _planRepository;
		private readonly IPlanMemberRepository _planMemberRepository;
		private readonly IMapper _mapper;
		private readonly IMainContext _mainContext;

		public SavePlanCommandHandler(UserManager<ApplicationUser> userRepository, IPlanRepository planRepository,
			IPlanMemberRepository planMemberRepository, IMapper mapper, IMainContext mainContext)
		{
			_userRepository = userRepository;
			_planRepository = planRepository;
			_planMemberRepository = planMemberRepository;
			_mapper = mapper;
			_mainContext = mainContext;
		}

		/// <summary>Handles a request</summary>
		/// <param name="message">The request message</param>
		/// <param name="cancellationToken"></param>
		/// <returns>Response from the request</returns>
		public async Task<bool> Handle(SavePlanCommand message, CancellationToken cancellationToken)
		{
			bool exist = true;
			IList<PlanMember> existingPlanMembers = null;
			IList<ApplicationUser> users = await _userRepository.Users.ToListAsync(cancellationToken);

			Plan plan = await _planRepository.GetByIdAsync(message.PlanModel.Id);
			ApplicationUser? currentUser = await _userRepository.FindByIdAsync(message.UserId.ToString());

			if (plan == null)
			{
				exist = false;
				plan = new Plan {
					UserId = currentUser.Id
				};
			}
			else
			{
				// Get PlanMembers and remove it
				existingPlanMembers =
					await _planMemberRepository.GetAsync(x=>
						x.PlanId == message.PlanModel.Id).ToListAsync(cancellationToken);
			}

			_mapper.Map(message.PlanModel, plan);

			await using IDbContextTransaction
				transaction = await _mainContext.BeginTransactionAsync(cancellationToken);
			{
				try
				{
					// Add plan
					//session.SaveOrUpdate(plan);
					if (exist)
					{
						await _planRepository.InsertAsync(plan);
					}
					else
					{
						_planRepository.Update(plan);
					}

					await _planRepository.SaveChangesAsync(true, cancellationToken);

					// Remove old plan members
					if (existingPlanMembers != null && existingPlanMembers.Any())
					{
						foreach (var existingPlanMember in existingPlanMembers)
						{
							existingPlanMember.UserId = Guid.Empty;
							existingPlanMember.Plan = null;
							await _planMemberRepository.DeleteAsync(existingPlanMember, true);
						}
					}

					// Add plan members
					foreach (var userMember in message.PlanModel.UserMembers)
					{
						ApplicationUser user = users.First(x => x.Id == userMember);
						await _planMemberRepository.InsertAsync(new PlanMember { UserId = user.Id, Plan = plan });
						await _planMemberRepository.SaveChangesAsync(true, cancellationToken);
					}

					await transaction.CommitAsync(cancellationToken);
				}
				catch (Exception e)
				{
					await transaction.RollbackAsync(cancellationToken);
				}
			}

			return true;
		}
	}
}