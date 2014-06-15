using Collecte.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace Collecte.DAL
{
	/// <summary>
	/// 
	/// </summary>
	public class QualifDataService
	{
		public StdResult<AnswerChoice> SetAnswer(User u, int questionNumber, int answerChosen)
		{
			using (CollectContext context = new CollectContext())
			{
				if(!context.Users.Any(user => user.Id == u.Id))
					return StdResult<AnswerChoice>.BadResult("User introuvable en base.");

				var query = context.AnswerChoices.Where(answerChoice => answerChoice.User.Id == u.Id && answerChoice.QuestionNumber == questionNumber);
				bool alreadyAnswered = query.Any();
				AnswerChoice ac = null;
				if (query.Any())
				{
					ac = query.FirstOrDefault();
					ac.AnswerChosen = answerChosen;
				}
				else
				{
					ac = new AnswerChoice
					{
						UserId = u.Id,
						AnswerChosen = answerChosen,
						QuestionNumber = questionNumber
					};
					context.AnswerChoices.Add(ac);

					context.SaveChanges();
					u.AnswerChoices.Add(ac);
				}

				context.SaveChanges();
				return StdResult<AnswerChoice>.OkResultInstance(ac);
			}
		}

		public StdResult<List<AnswerChoice>> GetUsersAnswers(User u)
		{
			using (CollectContext context = new CollectContext())
			{
				if (!context.Users.Any(user => user.Id == u.Id))
					return StdResult<List<AnswerChoice>>.BadResult("User introuvable en base.");

				var query = context.AnswerChoices.Where(answerChoice => answerChoice.User.Id == u.Id);
				bool alreadyAnswered = query.Any();

				return StdResult<List<AnswerChoice>>.OkResultInstance(query.ToList());
			}
		}
	}
}
