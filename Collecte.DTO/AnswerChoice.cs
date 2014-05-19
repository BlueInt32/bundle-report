using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Collecte.DTO
{
	public class AnswerChoice
	{
		public int AnswerChoiceId { get; set; }

		public Guid? UserId { get; set; }
		public virtual User User { get; set; }

		public int QuestionNumber { get; set; }
		public int AnswerChosen { get; set; }
	}
}
