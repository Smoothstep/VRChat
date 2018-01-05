using System;
using System.Collections;
using System.IO;
using RealisticEyeMovements;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRCSDK2;

namespace VRCCaptureUtils
{
	// Token: 0x020009F1 RID: 2545
	[RequireComponent(typeof(AudioSource))]
	public class PolaroidCamera : MonoBehaviour
	{
		// Token: 0x06004D62 RID: 19810 RVA: 0x0019F20C File Offset: 0x0019D60C
		private void Start()
		{
			this.SetupCameraAttributes();
			this.audioSource = base.GetComponent<AudioSource>();
			this.renderTexture = new RenderTexture(1920, 1080, 24);
			this.cam.targetTexture = this.renderTexture;
			this.cam.aspect = 1.77777779f;
			this.cam.fieldOfView = this.fov;
			this.backPreviewMesh.material.mainTexture = this.renderTexture;
			this.frontPreviewMesh.material.mainTexture = this.renderTexture;
			VRC.Player owner = VRC.Network.GetOwner(base.gameObject);
			if (owner != null)
			{
				this.creatorId = owner.playerApi.playerId;
				this.creator = owner.playerApi;
				this.engraving.text = this.creator.name;
			}
			base.StartCoroutine(this.screenShotCheck());
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x0019F2F8 File Offset: 0x0019D6F8
		public int GetCreator()
		{
			return this.creatorId;
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x0019F300 File Offset: 0x0019D700
		private void SetupCameraAttributes()
		{
			VRC_SceneDescriptor instance = VRC_SceneDescriptor.Instance;
			if (instance.ReferenceCamera != null)
			{
				Camera component = instance.ReferenceCamera.GetComponent<Camera>();
				if (component != null)
				{
					this.cam.nearClipPlane = Mathf.Clamp(component.nearClipPlane, 0.01f, 0.05f);
					this.cam.farClipPlane = component.farClipPlane;
					this.cam.clearFlags = component.clearFlags;
					this.cam.backgroundColor = component.backgroundColor;
					this.cam.allowHDR = component.allowHDR;
				}
				PostEffectManager.RemovePostEffects(this.cam.gameObject);
				PostEffectManager.InstallPostEffects(this.cam.gameObject, instance.ReferenceCamera);
			}
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x0019F3C6 File Offset: 0x0019D7C6
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.Local
		})]
		public void TakePhoto(VRC.Player instigator)
		{
			if (this.timer <= 0f)
			{
				VRC.Network.RPC(VRC_EventHandler.VrcTargetType.All, base.gameObject, "TakePhotoRPC", new object[0]);
				this.timer = this.timeBetweenPictures;
			}
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x0019F3FB File Offset: 0x0019D7FB
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.All
		})]
		private void TakePhotoRPC(VRC.Player instigator)
		{
			this._takePicture = true;
			this._takenBy = instigator;
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x0019F40C File Offset: 0x0019D80C
		private IEnumerator screenShotCheck()
		{
			for (;;)
			{
				yield return new WaitForEndOfFrame();
				if (this._takePicture)
				{
					RenderTexture renderTexture = new RenderTexture(1920, 1080, 24);
					renderTexture.antiAliasing = 8;
					this.cam.targetTexture = renderTexture;
					RenderTexture.active = renderTexture;
					this.cam.Render();
					this.screenShot = new Texture2D(1920, 1080);
					this.screenShot.ReadPixels(new Rect(0f, 0f, 1920f, 1080f), 0, 0);
					this.screenShot.Apply();
					RenderTexture.active = null;
					this.cam.targetTexture = this.renderTexture;
					this.audioSource.PlayOneShot(this.takePhotoSound);
					if (VRC.Network.IsMaster)
					{
						GameObject gameObject = VRC.Network.Instantiate(VRC_EventHandler.VrcBroadcastType.Always, "CapturePrefabs/PolaroidPhoto", this.photoSpawn.position, this.photoSpawn.rotation);
						Photo component = gameObject.GetComponent<Photo>();
						component.SetCameraAndUser(this, this._takenBy);
					}
					if (VRC.Network.LocalPlayer == this._takenBy && this.saveScreenshots)
					{
						this.SaveScreenshot();
					}
					this._takePicture = false;
				}
			}
			yield break;
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x0019F428 File Offset: 0x0019D828
		private void SetEyeLookTarget(VRC_PlayerApi p, bool forceCam)
		{
			EyeLookController componentInChildren = p.gameObject.GetComponentInChildren<EyeLookController>();
			if (componentInChildren == null)
			{
				return;
			}
			EyeAndHeadAnimator componentInChildren2 = p.gameObject.GetComponentInChildren<EyeAndHeadAnimator>();
			if (componentInChildren2 == null)
			{
				return;
			}
			if (forceCam)
			{
				componentInChildren.SetPhotoMode(base.transform);
			}
			else
			{
				componentInChildren.SetDefaultMode();
			}
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x0019F484 File Offset: 0x0019D884
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.All
		})]
		public void Pickup(VRC.Player instigator)
		{
			if (instigator == Networking.LocalPlayer)
			{
				foreach (VRC_PlayerApi p in VRC_PlayerApi.sPlayers)
				{
					this.SetEyeLookTarget(p, true);
				}
			}
			this.dropped = false;
			this.deathtimer = 0f;
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x0019F504 File Offset: 0x0019D904
		[VRCSDK2.RPC(new VRC_EventHandler.VrcTargetType[]
		{
			VRC_EventHandler.VrcTargetType.All
		})]
		public void Drop(VRC.Player instigator)
		{
			if (instigator == Networking.LocalPlayer)
			{
				foreach (VRC_PlayerApi p in VRC_PlayerApi.sPlayers)
				{
					this.SetEyeLookTarget(p, false);
				}
			}
			this.dropped = true;
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x0019F578 File Offset: 0x0019D978
		private void SaveScreenshot()
		{
			byte[] bytes = this.screenShot.EncodeToPNG();
			string path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "/VRChat/VRChat_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.fff") + ".png";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			File.WriteAllBytes(path, bytes);
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x0019F5D8 File Offset: 0x0019D9D8
		private void OnPlayerLeft(VRC_PlayerApi player)
		{
			if (player == this.creator)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x0019F5F8 File Offset: 0x0019D9F8
		private void Update()
		{
			if (this.timer > 0f)
			{
				this.timer -= Time.deltaTime;
			}
			if (this.dropped && Networking.LocalPlayer == this.creator)
			{
				this.deathtimer += Time.deltaTime;
				if (this.deathtimer >= this.lifetime)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}

		// Token: 0x04003566 RID: 13670
		[Tooltip("How long until the camera will be deleted when dropped.")]
		public float lifetime = 10f;

		// Token: 0x04003567 RID: 13671
		[Tooltip("The camera field of view. This overrides the setting on the camera object.")]
		public float fov = 60f;

		// Token: 0x04003568 RID: 13672
		[Tooltip("The camera that acts like the cameras camera.")]
		public Camera cam;

		// Token: 0x04003569 RID: 13673
		[Tooltip("The mesh to preview the live image")]
		public MeshRenderer backPreviewMesh;

		// Token: 0x0400356A RID: 13674
		public MeshRenderer frontPreviewMesh;

		// Token: 0x0400356B RID: 13675
		[Tooltip("The cooldown time before a photo can be taken again.")]
		public float timeBetweenPictures = 1.15f;

		// Token: 0x0400356C RID: 13676
		[Tooltip("The soundclip that is played when a photo is taken.")]
		public AudioClip takePhotoSound;

		// Token: 0x0400356D RID: 13677
		[Tooltip("The transform where the photo is spawned.")]
		public Transform photoSpawn;

		// Token: 0x0400356E RID: 13678
		[Tooltip("The UI text for engraving username.")]
		public Text engraving;

		// Token: 0x0400356F RID: 13679
		private RenderTexture renderTexture;

		// Token: 0x04003570 RID: 13680
		private AudioSource audioSource;

		// Token: 0x04003571 RID: 13681
		private float timer;

		// Token: 0x04003572 RID: 13682
		private float deathtimer;

		// Token: 0x04003573 RID: 13683
		private bool dropped = true;

		// Token: 0x04003574 RID: 13684
		private bool isReady;

		// Token: 0x04003575 RID: 13685
		public Texture2D screenShot;

		// Token: 0x04003576 RID: 13686
		public bool saveScreenshots;

		// Token: 0x04003577 RID: 13687
		private int creatorId;

		// Token: 0x04003578 RID: 13688
		private VRC_PlayerApi creator;

		// Token: 0x04003579 RID: 13689
		private bool _takePicture;

		// Token: 0x0400357A RID: 13690
		private VRC.Player _takenBy;
	}
}
