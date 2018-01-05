using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace VRC
{
	// Token: 0x02000AAD RID: 2733
	public class YoutubeDL : IEnumerator
	{
		// Token: 0x06005269 RID: 21097 RVA: 0x001C4C27 File Offset: 0x001C3027
		public YoutubeDL(string url)
		{
			this.url = new Uri(url).ToString();
			this.Reset();
		}

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x0600526A RID: 21098 RVA: 0x001C4C5C File Offset: 0x001C305C
		public string Error
		{
			get
			{
				if (this.errors.Count == 0)
				{
					return null;
				}
				return string.Join("\n", this.errors.ToArray());
			}
		}

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x0600526B RID: 21099 RVA: 0x001C4C85 File Offset: 0x001C3085
		public object Current
		{
			get
			{
				return this.process;
			}
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x001C4C8D File Offset: 0x001C308D
		public bool MoveNext()
		{
			return !this.process.HasExited;
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x001C4CA0 File Offset: 0x001C30A0
		public void Reset()
		{
			this.process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					FileName = Path.Combine(Application.streamingAssetsPath, "youtube-dl.exe"),
					Arguments = " --prefer-free-formats -f \"bestvideo[ext=webm]+bestaudio[ext=ogg]/best\" --audio-format best --get-url \"" + this.url + "\""
				},
				EnableRaisingEvents = true
			};
			this.process.OutputDataReceived += delegate(object p, DataReceivedEventArgs args)
			{
				this.URIs.Add(args.Data);
			};
			this.process.ErrorDataReceived += delegate(object p, DataReceivedEventArgs args)
			{
				this.errors.Add(args.Data);
			};
			this.process.Start();
			this.process.BeginOutputReadLine();
			this.process.BeginErrorReadLine();
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x001C4D74 File Offset: 0x001C3174
		public static IEnumerator FindBestVideoURL(string url, bool fetchTest, Action<string> onFound, Action<string> onError)
		{
			YoutubeDL dl = new YoutubeDL(url);
			yield return dl;
			if (!string.IsNullOrEmpty(dl.Error))
			{
				onError(dl.Error);
				yield break;
			}
			List<string> uris = dl.URIs;
			if (uris.Count > 0)
			{
				if (!fetchTest)
				{
					onFound(uris[0]);
					yield break;
				}
				for (int idx = 0; idx < uris.Count; idx++)
				{
					using (WWW www = new WWW(uris[idx]))
					{
						while (!www.isDone && string.IsNullOrEmpty(www.error) && www.progress.AlmostEquals(0f, 0.0001f))
						{
							yield return null;
						}
						if (string.IsNullOrEmpty(www.error))
						{
							onFound(uris[idx]);
							yield return null;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x04003A56 RID: 14934
		public List<string> URIs = new List<string>();

		// Token: 0x04003A57 RID: 14935
		private List<string> errors = new List<string>();

		// Token: 0x04003A58 RID: 14936
		private Process process;

		// Token: 0x04003A59 RID: 14937
		private string url;
	}
}
