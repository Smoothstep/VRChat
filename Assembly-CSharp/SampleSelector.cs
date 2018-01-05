using System;
using System.Collections.Generic;
using BestHTTP;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Examples;
using BestHTTP.Logger;
using BestHTTP.Statistics;
using UnityEngine;

// Token: 0x0200040D RID: 1037
public class SampleSelector : MonoBehaviour
{
	// Token: 0x060025A5 RID: 9637 RVA: 0x000B964C File Offset: 0x000B7A4C
	private void Awake()
	{
		HTTPManager.Logger.Level = Loglevels.All;
		HTTPManager.UseAlternateSSLDefaultValue = true;
		this.Samples.Add(new SampleDescriptor(null, "HTTP Samples", string.Empty, string.Empty)
		{
			IsLabel = true
		});
		this.Samples.Add(new SampleDescriptor(typeof(TextureDownloadSample), "Texture Download", "With HTTPManager.MaxConnectionPerServer you can control how many requests can be processed per server parallel.\n\nFeatures demoed in this example:\n-Parallel requests to the same server\n-Controlling the parallelization\n-Automatic Caching\n-Create a Texture2D from the downloaded data", CodeBlocks.TextureDownloadSample));
		this.Samples.Add(new SampleDescriptor(typeof(AssetBundleSample), "AssetBundle Download", "A small example that shows a possible way to download an AssetBundle and load a resource from it.\n\nFeatures demoed in this example:\n-Using HTTPRequest without a callback\n-Using HTTPRequest in a Coroutine\n-Loading an AssetBundle from the downloaded bytes\n-Automatic Caching", CodeBlocks.AssetBundleSample));
		this.Samples.Add(new SampleDescriptor(typeof(LargeFileDownloadSample), "Large File Download", "This example demonstrates how you can download a (large) file and continue the download after the connection is aborted.\n\nFeatures demoed in this example:\n-Setting up a streamed download\n-How to access the downloaded data while the download is in progress\n-Setting the HTTPRequest's StreamFragmentSize to controll the frequency and size of the fragments\n-How to use the SetRangeHeader to continue a previously disconnected download\n-How to disable the local, automatic caching", CodeBlocks.LargeFileDownloadSample));
		this.Samples.Add(new SampleDescriptor(null, "WebSocket Samples", string.Empty, string.Empty)
		{
			IsLabel = true
		});

		this.Samples.Add(new SampleDescriptor(typeof(CacheMaintenanceSample), "Cache Maintenance", "With this demo you can see how you can use the HTTPCacheService's BeginMaintainence function to delete too old cached entities and keep the cache size under a specified value.\n\nFeatures demoed in this example:\n-How to set up a HTTPCacheMaintananceParams\n-How to call the BeginMaintainence function", CodeBlocks.CacheMaintenanceSample));
		SampleSelector.SelectedSample = this.Samples[1];
	}

	// Token: 0x060025A6 RID: 9638 RVA: 0x000B9934 File Offset: 0x000B7D34
	private void Start()
	{
		GUIHelper.ClientArea = new Rect(0f, 165f, (float)Screen.width, (float)(Screen.height - 160 - 50));
	}

	// Token: 0x060025A7 RID: 9639 RVA: 0x000B9960 File Offset: 0x000B7D60
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (SampleSelector.SelectedSample != null && SampleSelector.SelectedSample.IsRunning)
			{
				SampleSelector.SelectedSample.DestroyUnityObject();
			}
			else
			{
				Application.Quit();
			}
		}
		if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && SampleSelector.SelectedSample != null && !SampleSelector.SelectedSample.IsRunning)
		{
			SampleSelector.SelectedSample.CreateUnityObject();
		}
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x000B99E4 File Offset: 0x000B7DE4
	private void OnGUI()
	{
		GeneralStatistics stats = HTTPManager.GetGeneralStatistics(StatisticsQueryFlags.All);
		GUIHelper.DrawArea(new Rect(0f, 0f, (float)(Screen.width / 3), 160f), false, delegate
		{
			GUIHelper.DrawCenteredText("Connections");
			GUILayout.Space(5f);
			GUIHelper.DrawRow("Sum:", stats.Connections.ToString());
			GUIHelper.DrawRow("Active:", stats.ActiveConnections.ToString());
			GUIHelper.DrawRow("Free:", stats.FreeConnections.ToString());
			GUIHelper.DrawRow("Recycled:", stats.RecycledConnections.ToString());
			GUIHelper.DrawRow("Requests in queue:", stats.RequestsInQueue.ToString());
		});
		GUIHelper.DrawArea(new Rect((float)(Screen.width / 3), 0f, (float)(Screen.width / 3), 160f), false, delegate
		{
			GUIHelper.DrawCenteredText("Cache");
			if (!HTTPCacheService.IsSupported)
			{
				GUI.color = Color.yellow;
				GUIHelper.DrawCenteredText("Disabled in WebPlayer, WebGL & Samsung Smart TV Builds!");
				GUI.color = Color.white;
			}
			else
			{
				GUILayout.Space(5f);
				GUIHelper.DrawRow("Cached entities:", stats.CacheEntityCount.ToString());
				GUIHelper.DrawRow("Sum Size (bytes): ", stats.CacheSize.ToString("N0"));
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Clear Cache", new GUILayoutOption[0]))
				{
					HTTPCacheService.BeginClear();
				}
				GUILayout.EndVertical();
			}
		});
		GUIHelper.DrawArea(new Rect((float)(Screen.width / 3 * 2), 0f, (float)(Screen.width / 3), 160f), false, delegate
		{
			GUIHelper.DrawCenteredText("Cookies");
			if (!CookieJar.IsSavingSupported)
			{
				GUI.color = Color.yellow;
				GUIHelper.DrawCenteredText("Saving and loading from disk is disabled in WebPlayer, WebGL & Samsung Smart TV Builds!");
				GUI.color = Color.white;
			}
			else
			{
				GUILayout.Space(5f);
				GUIHelper.DrawRow("Cookies:", stats.CookieCount.ToString());
				GUIHelper.DrawRow("Estimated size (bytes):", stats.CookieJarSize.ToString("N0"));
				GUILayout.BeginVertical(new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Clear Cookies", new GUILayoutOption[0]))
				{
					HTTPManager.OnQuit();
				}
				GUILayout.EndVertical();
			}
		});
		if (SampleSelector.SelectedSample == null || (SampleSelector.SelectedSample != null && !SampleSelector.SelectedSample.IsRunning))
		{
			GUIHelper.DrawArea(new Rect(0f, 165f, (float)((SampleSelector.SelectedSample != null) ? (Screen.width / 3) : Screen.width), (float)(Screen.height - 160 - 5)), false, delegate
			{
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, new GUILayoutOption[0]);
				for (int i = 0; i < this.Samples.Count; i++)
				{
					this.DrawSample(this.Samples[i]);
				}
				GUILayout.EndScrollView();
			});
			if (SampleSelector.SelectedSample != null)
			{
				this.DrawSampleDetails(SampleSelector.SelectedSample);
			}
		}
		else if (SampleSelector.SelectedSample != null && SampleSelector.SelectedSample.IsRunning)
		{
			GUILayout.BeginArea(new Rect(0f, (float)(Screen.height - 50), (float)Screen.width, 50f), string.Empty);
			GUILayout.FlexibleSpace();
			GUILayout.BeginHorizontal(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Back", new GUILayoutOption[]
			{
				GUILayout.MinWidth(100f)
			}))
			{
				SampleSelector.SelectedSample.DestroyUnityObject();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
		}
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x000B9BD0 File Offset: 0x000B7FD0
	private void DrawSample(SampleDescriptor sample)
	{
		if (sample.IsLabel)
		{
			GUILayout.Space(15f);
			GUIHelper.DrawCenteredText(sample.DisplayName);
			GUILayout.Space(5f);
		}
		else if (GUILayout.Button(sample.DisplayName, new GUILayoutOption[0]))
		{
			sample.IsSelected = true;
			if (SampleSelector.SelectedSample != null)
			{
				SampleSelector.SelectedSample.IsSelected = false;
			}
			SampleSelector.SelectedSample = sample;
		}
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000B9C44 File Offset: 0x000B8044
	private void DrawSampleDetails(SampleDescriptor sample)
	{
		Rect rect = new Rect((float)(Screen.width / 3), 165f, (float)(Screen.width / 3 * 2), (float)(Screen.height - 160 - 5));
		GUI.Box(rect, string.Empty);
		GUILayout.BeginArea(rect);
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUIHelper.DrawCenteredText(sample.DisplayName);
		GUILayout.Space(5f);
		GUILayout.Label(sample.Description, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Start Sample", new GUILayoutOption[0]))
		{
			sample.CreateUnityObject();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	// Token: 0x040012D5 RID: 4821
	public const int statisticsHeight = 160;

	// Token: 0x040012D6 RID: 4822
	private List<SampleDescriptor> Samples = new List<SampleDescriptor>();

	// Token: 0x040012D7 RID: 4823
	public static SampleDescriptor SelectedSample;

	// Token: 0x040012D8 RID: 4824
	private Vector2 scrollPos;
}
