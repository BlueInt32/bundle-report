using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Safe_Web.Models
{
	public enum LikeType
	{
		Home,
		VoteVideo,
		MaVideo
	}
	public class FbLikeViewModel : BaseViewModel
	{
		public FbLikeViewModel(string pageUrl, LikeType likeType, bool sendButton = false)
		{
			PageUrl = pageUrl;
			LikeType = likeType;
			SendButton = sendButton;
		}

		public string PageUrl { get; set; }
		public LikeType LikeType { get; set; }
		public bool SendButton { get; set; }
		public string GetCssClassByLikeType(LikeType likeType)
		{
			switch (likeType)
			{
				case LikeType.Home:
					return "fb-like-home";
					break;
				case LikeType.VoteVideo:
					return "fb-like-votevideo";
					break;
				case LikeType.MaVideo:
					return "fb-like-mavideo";
					break;
				default:
					throw new ArgumentOutOfRangeException("likeType");
			}
		}
	}
}