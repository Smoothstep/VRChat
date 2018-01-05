using System;
using System.Collections;
using System.IO;
using UnityEngine;

// Token: 0x02000AEA RID: 2794
public class Screenshot : MonoBehaviour
{
	// Token: 0x060054B3 RID: 21683 RVA: 0x001D3938 File Offset: 0x001D1D38
	public static string ScreenShotName(int width, int height)
	{
		return string.Format("{0}/VRChat/screen_{1}x{2}_{3}.png", new object[]
		{
			Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
			width,
			height,
			DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss.fff")
		});
	}

	// Token: 0x060054B4 RID: 21684 RVA: 0x001D3986 File Offset: 0x001D1D86
	private void Start()
	{
		base.StartCoroutine(this.screenShotCheck());
	}

	// Token: 0x060054B5 RID: 21685 RVA: 0x001D3995 File Offset: 0x001D1D95
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F12))
		{
			this.takeShotOnNextFrame = true;
		}
	}

	// Token: 0x060054B6 RID: 21686 RVA: 0x001D39B0 File Offset: 0x001D1DB0
	private IEnumerator screenShotCheck()
	{
		for (;;)
		{
			yield return new WaitForEndOfFrame();
			if (this.takeShotOnNextFrame)
			{
				this.takeShotOnNextFrame = false;
				Camera screenCamera = VRCVrCamera.GetInstance().screenCamera;
				RenderTexture targetTexture = screenCamera.targetTexture;
				float fieldOfView = screenCamera.fieldOfView;
				float aspect = screenCamera.aspect;
				RenderTexture renderTexture = new RenderTexture(this.resWidth, this.resHeight, 24);
				renderTexture.antiAliasing = 8;
				screenCamera.targetTexture = renderTexture;
				screenCamera.aspect = (float)this.resWidth / (float)this.resHeight;
				screenCamera.fieldOfView = 75f;
				Texture2D texture2D = new Texture2D(this.resWidth, this.resHeight, TextureFormat.RGB24, false);
				screenCamera.Render();
				RenderTexture.active = renderTexture;
				texture2D.ReadPixels(new Rect(0f, 0f, (float)this.resWidth, (float)this.resHeight), 0, 0);
				screenCamera.targetTexture = targetTexture;
				RenderTexture.active = null;
				screenCamera.fieldOfView = fieldOfView;
				screenCamera.aspect = aspect;
				UnityEngine.Object.Destroy(renderTexture);
				byte[] bytes = texture2D.EncodeToPNG();
				string text = Screenshot.ScreenShotName(this.resWidth, this.resHeight);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(text));
				}
				File.WriteAllBytes(text, bytes);
				Debug.Log(string.Format("Took screenshot to: {0}", text));
			}
		}
		yield break;
	}

	// Token: 0x04003BC4 RID: 15300
	public int resWidth = 1280;

	// Token: 0x04003BC5 RID: 15301
	public int resHeight = 720;

	// Token: 0x04003BC6 RID: 15302
	public bool takeShotOnNextFrame;
}
