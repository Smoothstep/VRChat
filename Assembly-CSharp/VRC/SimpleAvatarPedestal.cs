using System;
using BestHTTP;
using UnityEngine;
using VRC.Core;
using VRCSDK2;

namespace VRC
{
	// Token: 0x02000AF1 RID: 2801
	public class SimpleAvatarPedestal : MonoBehaviour
	{
		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x060054C9 RID: 21705 RVA: 0x001D3E17 File Offset: 0x001D2217
		public string avatarUrl
		{
			get
			{
				return this.mAvatarUrl;
			}
		}

		// Token: 0x060054CA RID: 21706 RVA: 0x001D3E20 File Offset: 0x001D2220
		public void Refresh(ApiAvatar avatar)
		{
			VRC.Core.Logger.Log("Refreshing with : " + avatar, DebugLevel.All);
			this.mAvatarUrl = avatar.assetUrl;
			this.apiAvatar = avatar;
			this.InstantiateAvatar("local", this.tempAvatarPrefab);
			Downloader.DownloadAssetBundle(avatar, new OnDownloadProgressDelegate(this.OnDownloadProgress), new AssetBundleDownloadManager.OnDownloadCompleted(this.InstantiateAvatar), new AssetBundleDownloadManager.OnDownloadError(this.DownloadError), AssetBundleDownloadManager.UnpackType.Async);
		}

		// Token: 0x060054CB RID: 21707 RVA: 0x001D3E90 File Offset: 0x001D2290
		public void Refresh(string url)
		{
			this.mAvatarUrl = url;
			this.InstantiateAvatar("local", this.tempAvatarPrefab);
			Downloader.DownloadAssetBundle(this.apiAvatar, new OnDownloadProgressDelegate(this.OnDownloadProgress), new AssetBundleDownloadManager.OnDownloadCompleted(this.InstantiateAvatar), new AssetBundleDownloadManager.OnDownloadError(this.DownloadError), AssetBundleDownloadManager.UnpackType.Async);
		}

		// Token: 0x060054CC RID: 21708 RVA: 0x001D3EE5 File Offset: 0x001D22E5
		public void Clear()
		{
			UnityEngine.Object.Destroy(this.avatar);
		}

		// Token: 0x060054CD RID: 21709 RVA: 0x001D3EF4 File Offset: 0x001D22F4
		private void InstantiateAvatar(string url, AssetBundleDownload download)
		{
			UnityEngine.Object asset = download.asset;
			this.InstantiateAvatar(url, asset);
		}

		// Token: 0x060054CE RID: 21710 RVA: 0x001D3F10 File Offset: 0x001D2310
		private void InstantiateAvatar(string url, UnityEngine.Object asset)
		{
			if (this.avatar != null)
			{
				UnityEngine.Object.Destroy(this.avatar);
			}
			this.avatar = (AssetManagement.Instantiate(asset) as GameObject);
			Tools.SetLayerRecursively(this.avatar, base.gameObject.layer, -1);
			this.SetupAnimator(this.avatar);
			Camera[] componentsInChildren = this.avatar.GetComponentsInChildren<Camera>();
			foreach (Camera camera in componentsInChildren)
			{
				camera.enabled = false;
			}
			AudioListener[] componentsInChildren2 = this.avatar.GetComponentsInChildren<AudioListener>();
			foreach (AudioListener audioListener in componentsInChildren2)
			{
				audioListener.enabled = false;
			}
			Cloth[] componentsInChildren3 = this.avatar.GetComponentsInChildren<Cloth>();
			foreach (Cloth cloth in componentsInChildren3)
			{
				cloth.enabled = false;
			}
			if (this.onAvatarInstantiated != null)
			{
				this.onAvatarInstantiated(url, this.avatar);
			}
		}

		// Token: 0x060054CF RID: 21711 RVA: 0x001D402C File Offset: 0x001D242C
		private void SetupAnimator(GameObject avatar)
		{
			float num = VRCTrackingManager.GetTrackingScale();
			if (num <= 0f)
			{
				num = 1f;
			}
			Animator component = avatar.GetComponent<Animator>();
			VRC_AvatarDescriptor component2 = avatar.GetComponent<VRC_AvatarDescriptor>();
			float num2 = num;
			if (component != null && component.avatar != null && component.isHuman && component.avatar != null)
			{
				if (component2 != null)
				{
					num2 = num * VRCTracking.DefaultEyeHeight / component2.ViewPosition.y;
					if (component2.Animations == VRC_AvatarDescriptor.AnimationSet.Female)
					{
						component.runtimeAnimatorController = this.femaleAnims;
					}
					else
					{
						component.runtimeAnimatorController = this.maleAnims;
					}
				}
				else
				{
					component.runtimeAnimatorController = this.maleAnims;
				}
			}
			avatar.transform.position = base.transform.position;
			avatar.transform.rotation = base.transform.rotation;
			avatar.transform.localScale *= this.avatarScale * num2;
			avatar.transform.parent = base.transform;
		}

		// Token: 0x060054D0 RID: 21712 RVA: 0x001D414E File Offset: 0x001D254E
		private void DownloadError(string s1, string s2, LoadErrorReason reason)
		{
			this.InstantiateAvatar("local", this.errorAvatarPrefab);
		}

		// Token: 0x060054D1 RID: 21713 RVA: 0x001D4161 File Offset: 0x001D2561
		private void OnDownloadProgress(HTTPRequest request, int downloaded, int length)
		{
		}

		// Token: 0x04003BD5 RID: 15317
		public GameObject tempAvatarPrefab;

		// Token: 0x04003BD6 RID: 15318
		public GameObject errorAvatarPrefab;

		// Token: 0x04003BD7 RID: 15319
		public RuntimeAnimatorController maleAnims;

		// Token: 0x04003BD8 RID: 15320
		public RuntimeAnimatorController femaleAnims;

		// Token: 0x04003BD9 RID: 15321
		public Action<string, GameObject> onAvatarInstantiated;

		// Token: 0x04003BDA RID: 15322
		public float avatarScale = 1f;

		// Token: 0x04003BDB RID: 15323
		public ApiAvatar apiAvatar;

		// Token: 0x04003BDC RID: 15324
		private GameObject avatar;

		// Token: 0x04003BDD RID: 15325
		private string mAvatarUrl;
	}
}
