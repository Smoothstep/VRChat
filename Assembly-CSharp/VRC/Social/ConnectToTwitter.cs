using System;
using Twitter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VRC.Social
{
	// Token: 0x02000AF7 RID: 2807
	public class ConnectToTwitter : MonoBehaviour
	{
		// Token: 0x060054E1 RID: 21729 RVA: 0x001D44C2 File Offset: 0x001D28C2
		private void Start()
		{
			this.LoadTwitterUserInfo();
			this.SetupUI();
		}

		// Token: 0x060054E2 RID: 21730 RVA: 0x001D44D0 File Offset: 0x001D28D0
		private void GetRequestToken()
		{
			base.StartCoroutine(API.GetRequestToken(Tweeter.Instance.CONSUMER_KEY, Tweeter.Instance.CONSUMER_SECRET, new RequestTokenCallback(this.OnRequestTokenCallback)));
		}

		// Token: 0x060054E3 RID: 21731 RVA: 0x001D44FE File Offset: 0x001D28FE
		private void GetAccessToken(string pin)
		{
			base.StartCoroutine(API.GetAccessToken(Tweeter.Instance.CONSUMER_KEY, Tweeter.Instance.CONSUMER_SECRET, this.m_RequestTokenResponse.Token, pin, new AccessTokenCallback(this.OnAccessTokenCallback)));
		}

		// Token: 0x060054E4 RID: 21732 RVA: 0x001D4538 File Offset: 0x001D2938
		private void LoadTwitterUserInfo()
		{
			Tweeter.Instance.accessTokenResponse = new AccessTokenResponse();
			Tweeter.Instance.accessTokenResponse.UserId = PlayerPrefs.GetString("TwitterUserID");
			Tweeter.Instance.accessTokenResponse.ScreenName = PlayerPrefs.GetString("TwitterUserScreenName");
			Tweeter.Instance.accessTokenResponse.Token = PlayerPrefs.GetString("TwitterUserToken");
			Tweeter.Instance.accessTokenResponse.TokenSecret = PlayerPrefs.GetString("TwitterUserTokenSecret");
		}

		// Token: 0x060054E5 RID: 21733 RVA: 0x001D45B8 File Offset: 0x001D29B8
		private void SetupUI()
		{
			this.connectButton.onClick.AddListener(new UnityAction(this.GetRequestToken));
			this.disconnectButton.onClick.AddListener(new UnityAction(this.Disconnect));
			if (Tweeter.IsAccessTokenValid(Tweeter.Instance.accessTokenResponse))
			{
				this.connectButton.gameObject.SetActive(false);
				this.disconnectButton.gameObject.SetActive(true);
				this.inputField.gameObject.SetActive(false);
				this.statusText.text = Tweeter.Instance.accessTokenResponse.ScreenName;
			}
			else
			{
				this.connectButton.gameObject.SetActive(true);
				this.disconnectButton.gameObject.SetActive(false);
				this.inputField.gameObject.SetActive(false);
				this.statusText.text = "Not Connected";
			}
		}

		// Token: 0x060054E6 RID: 21734 RVA: 0x001D46A8 File Offset: 0x001D2AA8
		public void Disconnect()
		{
			Tweeter.Instance.accessTokenResponse = new AccessTokenResponse();
			PlayerPrefs.DeleteKey("TwitterUserID");
			PlayerPrefs.DeleteKey("TwitterUserScreenName");
			PlayerPrefs.DeleteKey("TwitterUserToken");
			PlayerPrefs.DeleteKey("TwitterUserTokenSecret");
			this.connectButton.gameObject.SetActive(true);
			this.disconnectButton.gameObject.SetActive(false);
			this.inputField.gameObject.SetActive(false);
			this.statusText.text = "Not Connected";
		}

		// Token: 0x060054E7 RID: 21735 RVA: 0x001D4730 File Offset: 0x001D2B30
		private void OnRequestTokenCallback(bool success, RequestTokenResponse response)
		{
			if (success)
			{
				this.m_RequestTokenResponse = response;
				API.OpenAuthorizationPage(response.Token);
				this.connectButton.gameObject.SetActive(false);
				this.disconnectButton.gameObject.SetActive(false);
				this.inputField.gameObject.SetActive(true);
				this.inputField.onEndEdit.AddListener(delegate(string text)
				{
					this.GetAccessToken(text);
					this.connectButton.gameObject.SetActive(false);
					this.disconnectButton.gameObject.SetActive(true);
					this.inputField.gameObject.SetActive(false);
					this.inputField.text = string.Empty;
					this.statusText.text = "Connecting...";
				});
			}
			else
			{
				MonoBehaviour.print("OnRequestTokenCallback - failed.");
				this.connectButton.gameObject.SetActive(true);
				this.disconnectButton.gameObject.SetActive(false);
				this.inputField.gameObject.SetActive(false);
				this.statusText.text = "Not Connected";
			}
		}

		// Token: 0x060054E8 RID: 21736 RVA: 0x001D47F8 File Offset: 0x001D2BF8
		private void OnAccessTokenCallback(bool success, AccessTokenResponse response)
		{
			if (success)
			{
				string text = "OnAccessTokenCallback - succeeded";
				text = text + "\n    UserId : " + response.UserId;
				text = text + "\n    ScreenName : " + response.ScreenName;
				text = text + "\n    Token : " + response.Token;
				text = text + "\n    TokenSecret : " + response.TokenSecret;
				MonoBehaviour.print(text);
				Tweeter.Instance.accessTokenResponse = response;
				PlayerPrefs.SetString("TwitterUserID", response.UserId);
				PlayerPrefs.SetString("TwitterUserScreenName", response.ScreenName);
				PlayerPrefs.SetString("TwitterUserToken", response.Token);
				PlayerPrefs.SetString("TwitterUserTokenSecret", response.TokenSecret);
				this.connectButton.gameObject.SetActive(false);
				this.disconnectButton.gameObject.SetActive(true);
				this.inputField.gameObject.SetActive(false);
				this.statusText.text = response.ScreenName;
			}
			else
			{
				MonoBehaviour.print("OnAccessTokenCallback - failed.");
				this.connectButton.gameObject.SetActive(true);
				this.disconnectButton.gameObject.SetActive(false);
				this.inputField.text = string.Empty;
				this.inputField.gameObject.SetActive(false);
				this.statusText.text = "Bad Pin";
			}
		}

		// Token: 0x04003BF1 RID: 15345
		public Button connectButton;

		// Token: 0x04003BF2 RID: 15346
		public Button disconnectButton;

		// Token: 0x04003BF3 RID: 15347
		public UiInputField inputField;

		// Token: 0x04003BF4 RID: 15348
		public Text statusText;

		// Token: 0x04003BF5 RID: 15349
		private const string PLAYER_PREFS_TWITTER_USER_ID = "TwitterUserID";

		// Token: 0x04003BF6 RID: 15350
		private const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME = "TwitterUserScreenName";

		// Token: 0x04003BF7 RID: 15351
		private const string PLAYER_PREFS_TWITTER_USER_TOKEN = "TwitterUserToken";

		// Token: 0x04003BF8 RID: 15352
		private const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

		// Token: 0x04003BF9 RID: 15353
		private RequestTokenResponse m_RequestTokenResponse;
	}
}
