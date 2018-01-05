using System;
using System.IO;
using Eyeshot360cam;
using UnityEngine;

// Token: 0x02000B40 RID: 2880
public class VRCCapturePanorama : Eyeshot360cam.Eyeshot360cam
{
	// Token: 0x0600585F RID: 22623 RVA: 0x001E9C07 File Offset: 0x001E8007
	public override void Start()
	{
		base.Start();
		this.saveImagePath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\VRChat";
		if (!Directory.Exists(this.saveImagePath))
		{
			Directory.CreateDirectory(this.saveImagePath);
		}
	}

	// Token: 0x06005860 RID: 22624 RVA: 0x001E9C44 File Offset: 0x001E8044
	public void TakeScreenShot(string name)
	{
		string filenameBase = string.Format("{0}_{1}_{2:yyyy-MM-dd_HH-mm-ss-fff}", this.panoramaName, name, DateTime.Now);
		base.CaptureScreenshotAsync(filenameBase, null);
	}

	// Token: 0x06005861 RID: 22625 RVA: 0x001E9C75 File Offset: 0x001E8075
	public override bool OnCaptureStart()
	{
		this.vrcPlayers = GameObject.FindGameObjectsWithTag("Player");
		if (this.vrcPlayers.Length == 0)
		{
			Debug.LogWarning("Cannot find Player object, no panorama captured");
			return false;
		}
		return true;
	}

	// Token: 0x06005862 RID: 22626 RVA: 0x001E9CA4 File Offset: 0x001E80A4
	public override Camera[] GetCaptureCameras()
	{
		this.vrcPlayerLocal = VRCPlayer.Instance.gameObject;
		return VRCTrackingManager.GetCameras();
	}

	// Token: 0x06005863 RID: 22627 RVA: 0x001E9CC8 File Offset: 0x001E80C8
	public override void BeforeRenderPanorama()
	{
		this.vrcPlayerLocal.SetActive(false);
		VRCUiManager.Instance.HideAll();
	}

	// Token: 0x06005864 RID: 22628 RVA: 0x001E9CE0 File Offset: 0x001E80E0
	public override void AfterRenderPanorama()
	{
		this.vrcPlayerLocal.SetActive(true);
		this.vrcPlayerLocal = null;
		this.vrcPlayers = null;
	}

	// Token: 0x06005865 RID: 22629 RVA: 0x001E9CFC File Offset: 0x001E80FC
	private GameObject GetAncestor(GameObject go, GameObject[] ancestorCandidates)
	{
		int num;
		for (;;)
		{
			num = Array.IndexOf<GameObject>(ancestorCandidates, go);
			if (num != -1)
			{
				break;
			}
			if (go.transform == null || go.transform.parent == null || go.transform.parent.gameObject == null)
			{
				goto IL_55;
			}
			go = go.transform.parent.gameObject;
		}
		return ancestorCandidates[num];
		IL_55:
		return null;
	}

	// Token: 0x06005866 RID: 22630 RVA: 0x001E9D78 File Offset: 0x001E8178
	public void CapturePanorama()
	{
		if (base.CaptureIsLocked)
		{
			return;
		}
		if (this.initializeFailed || this.panoramaWidth < 4 || (this.captureStereoscopic && this.numCirclePoints < 8))
		{
			if (this.panoramaWidth < 4)
			{
				base.UpdateStatus("Panorama Width must be at least 4. No panorama captured.", Color.red);
				Debug.LogError("Panorama Width must be at least 4. No panorama captured.");
			}
			if (this.captureStereoscopic && this.numCirclePoints < 8)
			{
				base.UpdateStatus("Num Circle Points must be at least 8. No panorama captured.", Color.red);
				Debug.LogError("Num Circle Points must be at least 8. No panorama captured.");
			}
			if (this.initializeFailed)
			{
				base.UpdateStatus("Initialization of Capture Panorama Control failed. Cannot capture content.", Color.red);
				Debug.LogError("Initialization of Capture Panorama Control failed. Cannot capture content.");
			}
			base.PlaySound(this.failSound);
			return;
		}
		if (base.IsConfigurationChanged(this.lastConfig))
		{
			base.Reinitialize();
		}
		if (!Eyeshot360cam.Eyeshot360cam.capturing)
		{
			base.UpdateStatus("Capturing...", Color.cyan);
			string text = string.Format("{0}_{1:yyyy-MM-dd_HH-mm-ss-fff}", this.panoramaName, DateTime.Now);
			base.Log("Panorama capture key pressed, capturing " + text);
			base.CaptureScreenshotAsync(text, null);
		}
		else
		{
			base.PlaySound(this.failSound);
		}
	}

	// Token: 0x04003F56 RID: 16214
	private GameObject[] vrcPlayers;

	// Token: 0x04003F57 RID: 16215
	private GameObject vrcPlayerLocal;
}
