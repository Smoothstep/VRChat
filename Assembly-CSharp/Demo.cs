using System;
using Twitter;
using UnityEngine;

// Token: 0x02000561 RID: 1377
public class Demo : MonoBehaviour
{
	// Token: 0x06002F16 RID: 12054 RVA: 0x000E4A23 File Offset: 0x000E2E23
	private void Start()
	{
		this.LoadTwitterUserInfo();
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x000E4A2B File Offset: 0x000E2E2B
	private void Update()
	{
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x000E4A30 File Offset: 0x000E2E30
	private void OnGUI()
	{
		Rect position = new Rect((float)Screen.width * this.USER_LOG_IN_X, (float)Screen.height * this.USER_LOG_IN_Y, (float)Screen.width * this.USER_LOG_IN_WIDTH, (float)Screen.height * this.USER_LOG_IN_HEIGHT);
		if (string.IsNullOrEmpty(this.CONSUMER_KEY) || string.IsNullOrEmpty(this.CONSUMER_SECRET))
		{
			string text = "You need to register your game or application first.\n Click this button, register and fill CONSUMER_KEY and CONSUMER_SECRET of Demo game object.";
			if (GUI.Button(position, text))
			{
				Application.OpenURL("http://dev.twitter.com/apps/new");
			}
		}
		else
		{
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(this.m_AccessTokenResponse.ScreenName))
			{
				text2 = this.m_AccessTokenResponse.ScreenName + "\nClick to register with a different Twitter account";
			}
			else
			{
				text2 = "You need to register your game or application first.";
			}
			if (GUI.Button(position, text2))
			{
				base.StartCoroutine(API.GetRequestToken(this.CONSUMER_KEY, this.CONSUMER_SECRET, new RequestTokenCallback(this.OnRequestTokenCallback)));
			}
		}
		position.x = (float)Screen.width * this.PIN_INPUT_X;
		position.y = (float)Screen.height * this.PIN_INPUT_Y;
		position.width = (float)Screen.width * this.PIN_INPUT_WIDTH;
		position.height = (float)Screen.height * this.PIN_INPUT_HEIGHT;
		this.m_PIN = GUI.TextField(position, this.m_PIN);
		position.x = (float)Screen.width * this.PIN_ENTER_X;
		position.y = (float)Screen.height * this.PIN_ENTER_Y;
		position.width = (float)Screen.width * this.PIN_ENTER_WIDTH;
		position.height = (float)Screen.height * this.PIN_ENTER_HEIGHT;
		if (GUI.Button(position, "Enter PIN"))
		{
			base.StartCoroutine(API.GetAccessToken(this.CONSUMER_KEY, this.CONSUMER_SECRET, this.m_RequestTokenResponse.Token, this.m_PIN, new AccessTokenCallback(this.OnAccessTokenCallback)));
		}
		position.x = (float)Screen.width * this.TWEET_INPUT_X;
		position.y = (float)Screen.height * this.TWEET_INPUT_Y;
		position.width = (float)Screen.width * this.TWEET_INPUT_WIDTH;
		position.height = (float)Screen.height * this.TWEET_INPUT_HEIGHT;
		this.m_Tweet = GUI.TextField(position, this.m_Tweet);
		position.x = (float)Screen.width * this.POST_TWEET_X;
		position.y = (float)Screen.height * this.POST_TWEET_Y;
		position.width = (float)Screen.width * this.POST_TWEET_WIDTH;
		position.height = (float)Screen.height * this.POST_TWEET_HEIGHT;
		if (GUI.Button(position, "Post Tweet"))
		{
			Debug.Log(this.m_Tweet);
			Debug.Log(this.m_AccessTokenResponse.Token);
			Debug.Log(this.m_AccessTokenResponse.TokenSecret);
			base.StartCoroutine(API.PostTweet(this.m_Tweet, this.CONSUMER_KEY, this.CONSUMER_SECRET, this.m_AccessTokenResponse, new PostTweetCallback(this.OnPostTweet)));
		}
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x000E4D3C File Offset: 0x000E313C
	private void LoadTwitterUserInfo()
	{
		this.m_AccessTokenResponse = new AccessTokenResponse();
		this.m_AccessTokenResponse.UserId = PlayerPrefs.GetString("TwitterUserID");
		this.m_AccessTokenResponse.ScreenName = PlayerPrefs.GetString("TwitterUserScreenName");
		this.m_AccessTokenResponse.Token = PlayerPrefs.GetString("TwitterUserToken");
		this.m_AccessTokenResponse.TokenSecret = PlayerPrefs.GetString("TwitterUserTokenSecret");
		if (!string.IsNullOrEmpty(this.m_AccessTokenResponse.Token) && !string.IsNullOrEmpty(this.m_AccessTokenResponse.ScreenName) && !string.IsNullOrEmpty(this.m_AccessTokenResponse.Token) && !string.IsNullOrEmpty(this.m_AccessTokenResponse.TokenSecret))
		{
			string text = "LoadTwitterUserInfo - succeeded";
			text = text + "\n    UserId : " + this.m_AccessTokenResponse.UserId;
			text = text + "\n    ScreenName : " + this.m_AccessTokenResponse.ScreenName;
			text = text + "\n    Token : " + this.m_AccessTokenResponse.Token;
			text = text + "\n    TokenSecret : " + this.m_AccessTokenResponse.TokenSecret;
			MonoBehaviour.print(text);
		}
	}

	// Token: 0x06002F1A RID: 12058 RVA: 0x000E4E64 File Offset: 0x000E3264
	private void OnRequestTokenCallback(bool success, RequestTokenResponse response)
	{
		if (success)
		{
			string text = "OnRequestTokenCallback - succeeded";
			text = text + "\n    Token : " + response.Token;
			text = text + "\n    TokenSecret : " + response.TokenSecret;
			MonoBehaviour.print(text);
			this.m_RequestTokenResponse = response;
			API.OpenAuthorizationPage(response.Token);
		}
		else
		{
			MonoBehaviour.print("OnRequestTokenCallback - failed.");
		}
	}

	// Token: 0x06002F1B RID: 12059 RVA: 0x000E4EC8 File Offset: 0x000E32C8
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
			this.m_AccessTokenResponse = response;
			PlayerPrefs.SetString("TwitterUserID", response.UserId);
			PlayerPrefs.SetString("TwitterUserScreenName", response.ScreenName);
			PlayerPrefs.SetString("TwitterUserToken", response.Token);
			PlayerPrefs.SetString("TwitterUserTokenSecret", response.TokenSecret);
		}
		else
		{
			MonoBehaviour.print("OnAccessTokenCallback - failed.");
		}
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x000E4F85 File Offset: 0x000E3385
	private void OnPostTweet(bool success)
	{
		MonoBehaviour.print("OnPostTweet - " + ((!success) ? "failed." : "succedded."));
	}

	// Token: 0x04001960 RID: 6496
	public float USER_LOG_IN_X;

	// Token: 0x04001961 RID: 6497
	public float USER_LOG_IN_Y;

	// Token: 0x04001962 RID: 6498
	public float USER_LOG_IN_WIDTH;

	// Token: 0x04001963 RID: 6499
	public float USER_LOG_IN_HEIGHT;

	// Token: 0x04001964 RID: 6500
	public float PIN_INPUT_X;

	// Token: 0x04001965 RID: 6501
	public float PIN_INPUT_Y;

	// Token: 0x04001966 RID: 6502
	public float PIN_INPUT_WIDTH;

	// Token: 0x04001967 RID: 6503
	public float PIN_INPUT_HEIGHT;

	// Token: 0x04001968 RID: 6504
	public float PIN_ENTER_X;

	// Token: 0x04001969 RID: 6505
	public float PIN_ENTER_Y;

	// Token: 0x0400196A RID: 6506
	public float PIN_ENTER_WIDTH;

	// Token: 0x0400196B RID: 6507
	public float PIN_ENTER_HEIGHT;

	// Token: 0x0400196C RID: 6508
	public float TWEET_INPUT_X;

	// Token: 0x0400196D RID: 6509
	public float TWEET_INPUT_Y;

	// Token: 0x0400196E RID: 6510
	public float TWEET_INPUT_WIDTH;

	// Token: 0x0400196F RID: 6511
	public float TWEET_INPUT_HEIGHT;

	// Token: 0x04001970 RID: 6512
	public float POST_TWEET_X;

	// Token: 0x04001971 RID: 6513
	public float POST_TWEET_Y;

	// Token: 0x04001972 RID: 6514
	public float POST_TWEET_WIDTH;

	// Token: 0x04001973 RID: 6515
	public float POST_TWEET_HEIGHT;

	// Token: 0x04001974 RID: 6516
	public string CONSUMER_KEY;

	// Token: 0x04001975 RID: 6517
	public string CONSUMER_SECRET;

	// Token: 0x04001976 RID: 6518
	private const string PLAYER_PREFS_TWITTER_USER_ID = "TwitterUserID";

	// Token: 0x04001977 RID: 6519
	private const string PLAYER_PREFS_TWITTER_USER_SCREEN_NAME = "TwitterUserScreenName";

	// Token: 0x04001978 RID: 6520
	private const string PLAYER_PREFS_TWITTER_USER_TOKEN = "TwitterUserToken";

	// Token: 0x04001979 RID: 6521
	private const string PLAYER_PREFS_TWITTER_USER_TOKEN_SECRET = "TwitterUserTokenSecret";

	// Token: 0x0400197A RID: 6522
	private RequestTokenResponse m_RequestTokenResponse;

	// Token: 0x0400197B RID: 6523
	private AccessTokenResponse m_AccessTokenResponse;

	// Token: 0x0400197C RID: 6524
	private string m_PIN = "Please enter your PIN here.";

	// Token: 0x0400197D RID: 6525
	private string m_Tweet = "Please enter your tweet here.";
}
