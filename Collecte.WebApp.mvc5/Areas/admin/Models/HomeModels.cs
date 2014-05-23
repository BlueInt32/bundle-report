using System.ComponentModel.DataAnnotations;
using Tools.Asp.net_MVC.Attributes;

namespace Collecte.WebApp.Areas.admin.Models
{
    public class HomeModel
    {
        [DataType(DataType.DateTime)]
        [Display(Name = "Date de début")]
        [DateValidation(isRequired: true, requiredErrorMessage: "Choisissez une date de début.", malformedErrorMessage: "Date de début non correcte.")]
        public string DateDebut { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date de fin")]
        [DateValidation(isRequired: true, requiredErrorMessage: "Choisissez une date de fin.", malformedErrorMessage: "Date de fin non correcte.")]
        public string DateFin { get; set; }

        public Stats Stats { get; set; }
    }

    public class Stats
    {
        public StatsTotal Total { get; set; }
		public StatsTradedoubler Tradedoubler { get; set; }
        public StatsAnalyse Analyse { get; set; }
        public StatsEpisodes Episodes { get; set; }
    }

    public class StatsTotal
    {
        public int NbParticipations { get; set; }
        public int NbParticipationsOptin { get; set; }
        public int NbParticipationsOptinAbonnés { get; set; }
        public int NbParticipationsOptinNonAbonnés { get; set; }
    }

	public class StatsTradedoubler
    {
        public int NbParticipants { get; set; }
        public int NbParticipantsOptin { get; set; }
        public int NbParticipantsOptinAbonnés { get; set; }
        public int NbParticipantsOptinNonAbonnés { get; set; }
    }

    public class StatsAnalyse
    {
        public int TauxOptinParticipants { get; set; }
        public int TauxOptinAbonnes { get; set; }
        public int TauxOptinNonAbonnes { get; set; }
		public int TauxParticipationsParticipantsTradedoubler { get; set; }
		public int TauxOptinAbonnesParticipantsTradedoubler { get; set; }
		public int TauxOptinNonAbonnesParticipantsTradedoubler { get; set; }
    }

    public class StatsEpisodes
    {
        public int NbJoueursEpisode1 { get; set; }
        public int NbJoueursEpisode2 { get; set; }
        public int NbJoueursEpisode3 { get; set; }
        public int NbJoueursEpisode4 { get; set; }
        public int NbJoueursEpisode5 { get; set; }
    }
}