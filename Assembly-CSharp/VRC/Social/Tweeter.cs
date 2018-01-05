using System;
using Twitter;
using UnityEngine;
using VRC.Core;

namespace VRC.Social
{
	// Token: 0x02000AF8 RID: 2808
	public class Tweeter : MonoBehaviour
	{
		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x060054EB RID: 21739 RVA: 0x001D49BF File Offset: 0x001D2DBF
		public static Tweeter Instance
		{
			get
			{
				return Tweeter.mInstance;
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x060054EC RID: 21740 RVA: 0x001D49C6 File Offset: 0x001D2DC6
		public static bool isConnected
		{
			get
			{
				return Tweeter.Instance != null && Tweeter.IsAccessTokenValid(Tweeter.Instance.accessTokenResponse);
			}
		}

		// Token: 0x060054ED RID: 21741 RVA: 0x001D49EA File Offset: 0x001D2DEA
		private void Awake()
		{
			if (Tweeter.mInstance == null)
			{
				Tweeter.mInstance = this;
			}
			else
			{
				VRC.Core.Logger.LogError("Too many Twitters!!", DebugLevel.Always);
				UnityEngine.Object.Destroy(this);
			}
		}

		// Token: 0x060054EE RID: 21742 RVA: 0x001D4A18 File Offset: 0x001D2E18
		public void PostTweet(string tweet)
		{
			base.StartCoroutine(API.PostTweet(tweet, this.CONSUMER_KEY, this.CONSUMER_SECRET, this.accessTokenResponse, new PostTweetCallback(this.OnPostTweet)));
		}

		// Token: 0x060054EF RID: 21743 RVA: 0x001D4A45 File Offset: 0x001D2E45
		private void OnPostTweet(bool success)
		{
			MonoBehaviour.print("OnPostTweet - " + ((!success) ? "failed." : "succedded."));
		}

		// Token: 0x060054F0 RID: 21744 RVA: 0x001D4A6C File Offset: 0x001D2E6C
		public static bool IsAccessTokenValid(AccessTokenResponse response)
		{
			return !string.IsNullOrEmpty(response.Token) && !string.IsNullOrEmpty(response.ScreenName) && !string.IsNullOrEmpty(response.Token) && !string.IsNullOrEmpty(response.TokenSecret);
		}

		// Token: 0x04003BFA RID: 15354
		private static Tweeter mInstance;

		// Token: 0x04003BFB RID: 15355
		public string CONSUMER_KEY;

		// Token: 0x04003BFC RID: 15356
		public string CONSUMER_SECRET;

		// Token: 0x04003BFD RID: 15357
		public AccessTokenResponse accessTokenResponse;
	}
}
