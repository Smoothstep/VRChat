using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Twitter
{
	// Token: 0x02000567 RID: 1383
	public class API
	{
		// Token: 0x06002F38 RID: 12088 RVA: 0x000E502C File Offset: 0x000E342C
		public static IEnumerator GetRequestToken(string consumerKey, string consumerSecret, RequestTokenCallback callback)
		{
			WWW web = API.WWWRequestToken(consumerKey, consumerSecret);
			yield return web;
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log(string.Format("GetRequestToken - failed. error : {0}", web.error));
				callback(false, null);
			}
			else
			{
				RequestTokenResponse requestTokenResponse = new RequestTokenResponse
				{
					Token = Regex.Match(web.text, "oauth_token=([^&]+)").Groups[1].Value,
					TokenSecret = Regex.Match(web.text, "oauth_token_secret=([^&]+)").Groups[1].Value
				};
				if (!string.IsNullOrEmpty(requestTokenResponse.Token) && !string.IsNullOrEmpty(requestTokenResponse.TokenSecret))
				{
					callback(true, requestTokenResponse);
				}
				else
				{
					Debug.Log(string.Format("GetRequestToken - failed. response : {0}", web.text));
					callback(false, null);
				}
			}
			yield break;
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x000E5055 File Offset: 0x000E3455
		public static void OpenAuthorizationPage(string requestToken)
		{
			Application.OpenURL(string.Format(API.AuthorizationURL, requestToken));
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x000E5068 File Offset: 0x000E3468
		public static IEnumerator GetAccessToken(string consumerKey, string consumerSecret, string requestToken, string pin, AccessTokenCallback callback)
		{
			WWW web = API.WWWAccessToken(consumerKey, consumerSecret, requestToken, pin);
			yield return web;
			if (!string.IsNullOrEmpty(web.error))
			{
				Debug.Log(string.Format("GetAccessToken - failed. error : {0}", web.error));
				callback(false, null);
			}
			else
			{
				AccessTokenResponse accessTokenResponse = new AccessTokenResponse
				{
					Token = Regex.Match(web.text, "oauth_token=([^&]+)").Groups[1].Value,
					TokenSecret = Regex.Match(web.text, "oauth_token_secret=([^&]+)").Groups[1].Value,
					UserId = Regex.Match(web.text, "user_id=([^&]+)").Groups[1].Value,
					ScreenName = Regex.Match(web.text, "screen_name=([^&]+)").Groups[1].Value
				};
				if (!string.IsNullOrEmpty(accessTokenResponse.Token) && !string.IsNullOrEmpty(accessTokenResponse.TokenSecret) && !string.IsNullOrEmpty(accessTokenResponse.UserId) && !string.IsNullOrEmpty(accessTokenResponse.ScreenName))
				{
					callback(true, accessTokenResponse);
				}
				else
				{
					Debug.Log(string.Format("GetAccessToken - failed. response : {0}", web.text));
					callback(false, null);
				}
			}
			yield break;
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x000E50A0 File Offset: 0x000E34A0
		private static WWW WWWRequestToken(string consumerKey, string consumerSecret)
		{
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("oauth_callback", "oob");
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			API.AddDefaultOAuthParams(dictionary, consumerKey, consumerSecret);
			dictionary.Add("oauth_callback", "oob");
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2["Authorization"] = API.GetFinalOAuthHeader("POST", API.RequestTokenURL, dictionary);
			return new WWW(API.RequestTokenURL, wwwform.data, dictionary2);
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000E5114 File Offset: 0x000E3514
		private static WWW WWWAccessToken(string consumerKey, string consumerSecret, string requestToken, string pin)
		{
			byte[] postData = new byte[]
			{
				0
			};
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			API.AddDefaultOAuthParams(dictionary2, consumerKey, consumerSecret);
			dictionary2.Add("oauth_token", requestToken);
			dictionary2.Add("oauth_verifier", pin);
			dictionary["Authorization"] = API.GetFinalOAuthHeader("POST", API.AccessTokenURL, dictionary2);
			return new WWW(API.AccessTokenURL, postData, dictionary);
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x000E517F File Offset: 0x000E357F
		private static string GetHeaderWithAccessToken(string httpRequestType, string apiURL, string consumerKey, string consumerSecret, AccessTokenResponse response, Dictionary<string, string> parameters)
		{
			API.AddDefaultOAuthParams(parameters, consumerKey, consumerSecret);
			parameters.Add("oauth_token", response.Token);
			parameters.Add("oauth_token_secret", response.TokenSecret);
			return API.GetFinalOAuthHeader(httpRequestType, apiURL, parameters);
		}

		// Token: 0x06002F3E RID: 12094 RVA: 0x000E51BC File Offset: 0x000E35BC
		public static IEnumerator PostTweet(string text, string consumerKey, string consumerSecret, AccessTokenResponse response, PostTweetCallback callback)
		{
			if (string.IsNullOrEmpty(text) || text.Length > 140)
			{
				Debug.Log(string.Format("PostTweet - text[{0}] is empty or too long.", text));
				callback(false);
			}
			else
			{
				Dictionary<string, string> parameters = new Dictionary<string, string>();
				parameters.Add("status", text);
				WWWForm form = new WWWForm();
				form.AddField("status", text);
				Dictionary<string, string> headers = new Dictionary<string, string>();
				headers["Authorization"] = API.GetHeaderWithAccessToken("POST", "https://api.twitter.com/1.1/statuses/update.json", consumerKey, consumerSecret, response, parameters);
				WWW web = new WWW("https://api.twitter.com/1.1/statuses/update.json", form.data, headers);
				yield return web;
				if (!string.IsNullOrEmpty(web.error))
				{
					Debug.Log(string.Format("PostTweet - failed. {0}\n{1}", web.error, web.text));
					callback(false);
				}
				else
				{
					string value = Regex.Match(web.text, "<error>([^&]+)</error>").Groups[1].Value;
					if (!string.IsNullOrEmpty(value))
					{
						Debug.Log(string.Format("PostTweet - failed. {0}", value));
						callback(false);
					}
					else
					{
						callback(true);
					}
				}
			}
			yield break;
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x000E51F4 File Offset: 0x000E35F4
		private static void AddDefaultOAuthParams(Dictionary<string, string> parameters, string consumerKey, string consumerSecret)
		{
			parameters.Add("oauth_version", "1.0");
			parameters.Add("oauth_nonce", API.GenerateNonce());
			parameters.Add("oauth_timestamp", API.GenerateTimeStamp());
			parameters.Add("oauth_signature_method", "HMAC-SHA1");
			parameters.Add("oauth_consumer_key", consumerKey);
			parameters.Add("oauth_consumer_secret", consumerSecret);
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x000E525C File Offset: 0x000E365C
		private static string GetFinalOAuthHeader(string HTTPRequestType, string URL, Dictionary<string, string> parameters)
		{
			string value = API.GenerateSignature(HTTPRequestType, URL, parameters);
			parameters.Add("oauth_signature", value);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("OAuth realm=\"{0}\"", "Twitter API");
			IOrderedEnumerable<KeyValuePair<string, string>> orderedEnumerable = from p in parameters
			where API.OAuthParametersToIncludeInHeader.Contains(p.Key)
			orderby p.Key, API.UrlEncode(p.Value)
			select p;
			foreach (KeyValuePair<string, string> keyValuePair in orderedEnumerable)
			{
				stringBuilder.AppendFormat(",{0}=\"{1}\"", API.UrlEncode(keyValuePair.Key), API.UrlEncode(keyValuePair.Value));
			}
			stringBuilder.AppendFormat(",oauth_signature=\"{0}\"", API.UrlEncode(parameters["oauth_signature"]));
			return stringBuilder.ToString();
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x000E5388 File Offset: 0x000E3788
		private static string GenerateSignature(string httpMethod, string url, Dictionary<string, string> parameters)
		{
			IEnumerable<KeyValuePair<string, string>> parameters2 = from p in parameters
			where !API.SecretParameters.Contains(p.Key)
			select p;
			string s = string.Format(CultureInfo.InvariantCulture, "{0}&{1}&{2}", new object[]
			{
				httpMethod,
				API.UrlEncode(API.NormalizeUrl(new Uri(url))),
				API.UrlEncode(parameters2)
			});
			string s2 = string.Format(CultureInfo.InvariantCulture, "{0}&{1}", new object[]
			{
				API.UrlEncode(parameters["oauth_consumer_secret"]),
				(!parameters.ContainsKey("oauth_token_secret")) ? string.Empty : API.UrlEncode(parameters["oauth_token_secret"])
			});
			HMACSHA1 hmacsha = new HMACSHA1(Encoding.ASCII.GetBytes(s2));
			byte[] inArray = hmacsha.ComputeHash(Encoding.ASCII.GetBytes(s));
			return Convert.ToBase64String(inArray);
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x000E5470 File Offset: 0x000E3870
		private static string GenerateTimeStamp()
		{
			return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds, CultureInfo.CurrentCulture).ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x000E54BC File Offset: 0x000E38BC
		private static string GenerateNonce()
		{
			return new System.Random().Next(123400, int.MaxValue).ToString("X", CultureInfo.InvariantCulture);
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x000E54F0 File Offset: 0x000E38F0
		private static string NormalizeUrl(Uri url)
		{
			string text = string.Format(CultureInfo.InvariantCulture, "{0}://{1}", new object[]
			{
				url.Scheme,
				url.Host
			});
			if ((!(url.Scheme == "http") || url.Port != 80) && (!(url.Scheme == "https") || url.Port != 443))
			{
				text = text + ":" + url.Port;
			}
			return text + url.AbsolutePath;
		}

		// Token: 0x06002F45 RID: 12101 RVA: 0x000E5594 File Offset: 0x000E3994
		private static string UrlEncode(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}
			value = Uri.EscapeDataString(value);
			value = Regex.Replace(value, "(%[0-9a-f][0-9a-f])", (Match c) => c.Value.ToUpper());
			value = value.Replace("(", "%28").Replace(")", "%29").Replace("$", "%24").Replace("!", "%21").Replace("*", "%2A").Replace("'", "%27");
			value = value.Replace("%7E", "~");
			return value;
		}

		// Token: 0x06002F46 RID: 12102 RVA: 0x000E5654 File Offset: 0x000E3A54
		private static string UrlEncode(IEnumerable<KeyValuePair<string, string>> parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			IOrderedEnumerable<KeyValuePair<string, string>> orderedEnumerable = from p in parameters
			orderby p.Key, p.Value
			select p;
			foreach (KeyValuePair<string, string> keyValuePair in orderedEnumerable)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("&");
				}
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0}={1}", new object[]
				{
					API.UrlEncode(keyValuePair.Key),
					API.UrlEncode(keyValuePair.Value)
				}));
			}
			return API.UrlEncode(stringBuilder.ToString());
		}

		// Token: 0x04001984 RID: 6532
		private static readonly string RequestTokenURL = "https://api.twitter.com/oauth/request_token";

		// Token: 0x04001985 RID: 6533
		private static readonly string AuthorizationURL = "https://api.twitter.com/oauth/authenticate?oauth_token={0}";

		// Token: 0x04001986 RID: 6534
		private static readonly string AccessTokenURL = "https://api.twitter.com/oauth/access_token";

		// Token: 0x04001987 RID: 6535
		private const string PostTweetURL = "https://api.twitter.com/1.1/statuses/update.json";

		// Token: 0x04001988 RID: 6536
		private static readonly string[] OAuthParametersToIncludeInHeader = new string[]
		{
			"oauth_version",
			"oauth_nonce",
			"oauth_timestamp",
			"oauth_signature_method",
			"oauth_consumer_key",
			"oauth_token",
			"oauth_verifier"
		};

		// Token: 0x04001989 RID: 6537
		private static readonly string[] SecretParameters = new string[]
		{
			"oauth_consumer_secret",
			"oauth_token_secret",
			"oauth_signature"
		};
	}
}
