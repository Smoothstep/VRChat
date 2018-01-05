using System;
using BestHTTP;
using UnityEngine;
using VRC.Core;

// Token: 0x02000A8A RID: 2698
public class Downloader
{
	// Token: 0x06005153 RID: 20819 RVA: 0x001BDFAC File Offset: 0x001BC3AC
	public static void DownloadAssetBundle(ApiWorld world, OnDownloadProgressDelegate onProgress = null, AssetBundleDownloadManager.OnDownloadCompleted onSuccess = null, AssetBundleDownloadManager.OnDownloadError onError = null, AssetBundleDownloadManager.UnpackType unpackType = AssetBundleDownloadManager.UnpackType.Async)
	{
		if (Downloader.downloadManager == null)
		{
			Downloader.InitializeDownloadManager();
		}
		AssetBundleDownloadManager.Instance.DownloadAssetBundle(world, onProgress, onSuccess, onError, unpackType);
	}

	// Token: 0x06005154 RID: 20820 RVA: 0x001BDFD3 File Offset: 0x001BC3D3
	public static void DownloadAssetBundle(ApiAvatar avatar, OnDownloadProgressDelegate onProgress = null, AssetBundleDownloadManager.OnDownloadCompleted onSuccess = null, AssetBundleDownloadManager.OnDownloadError onError = null, AssetBundleDownloadManager.UnpackType unpackType = AssetBundleDownloadManager.UnpackType.Async)
	{
		if (Downloader.downloadManager == null)
		{
			Downloader.InitializeDownloadManager();
		}
		AssetBundleDownloadManager.Instance.DownloadAssetBundle(avatar, onProgress, onSuccess, onError, unpackType);
	}

	// Token: 0x06005155 RID: 20821 RVA: 0x001BDFFA File Offset: 0x001BC3FA
	public static void CancelAssetBundleDownload(string url)
	{
		AssetBundleDownloadManager.Instance.CancelAssetBundleDownload(url);
	}

	// Token: 0x06005156 RID: 20822 RVA: 0x001BE008 File Offset: 0x001BC408
	public static void DownloadImage(string url, Action<string, Texture2D> onImageDownloaded = null, string fallbackUrl = "")
	{
		if (url != null)
		{
			if (url.StartsWith("file:"))
			{
				if (Downloader.downloadManager == null)
				{
					Downloader.InitializeDownloadManager();
				}
				LocalImageDownloader localImageDownloader = Downloader.downloadManager.AddComponent<LocalImageDownloader>();
				localImageDownloader.DownloadLocalImage(url, onImageDownloaded);
			}
			else
			{
				ImageDownloader.DownloadImage(url, delegate(Texture2D image)
				{
					if (onImageDownloaded != null)
					{
						onImageDownloaded(url, image);
					}
				}, fallbackUrl, false);
			}
		}
	}

	// Token: 0x06005157 RID: 20823 RVA: 0x001BE099 File Offset: 0x001BC499
	private static void InitializeDownloadManager()
	{
		Downloader.downloadManager = new GameObject();
		Downloader.downloadManager.name = "DownloadManager";
		Downloader.downloadManager.AddComponent<AssetBundleDownloadManager>();
	}

	// Token: 0x0400399E RID: 14750
	public static GameObject downloadManager;
}
