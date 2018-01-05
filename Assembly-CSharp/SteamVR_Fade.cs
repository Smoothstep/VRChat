using System;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

// Token: 0x02000BFD RID: 3069
public class SteamVR_Fade : MonoBehaviour
{
	// Token: 0x06005F12 RID: 24338 RVA: 0x00214374 File Offset: 0x00212774
	public static void Start(Color newColor, float duration, bool fadeOverlay = false)
	{
		SteamVR_Events.Fade.Send(newColor, duration, fadeOverlay);
	}

	// Token: 0x06005F13 RID: 24339 RVA: 0x00214384 File Offset: 0x00212784
	public static void View(Color newColor, float duration)
	{
		CVRCompositor compositor = OpenVR.Compositor;
		if (compositor != null)
		{
			compositor.FadeToColor(duration, newColor.r, newColor.g, newColor.b, newColor.a, false);
		}
	}

	// Token: 0x06005F14 RID: 24340 RVA: 0x002143C1 File Offset: 0x002127C1
	public void OnStartFade(Color newColor, float duration, bool fadeOverlay)
	{
		if (duration > 0f)
		{
			this.targetColor = newColor;
			this.deltaColor = (this.targetColor - this.currentColor) / duration;
		}
		else
		{
			this.currentColor = newColor;
		}
	}

	// Token: 0x06005F15 RID: 24341 RVA: 0x00214400 File Offset: 0x00212800
	private void OnEnable()
	{
		if (SteamVR_Fade.fadeMaterial == null)
		{
			SteamVR_Fade.fadeMaterial = new Material(Shader.Find("Custom/SteamVR_Fade"));
			SteamVR_Fade.fadeMaterialColorID = Shader.PropertyToID("fadeColor");
		}
		SteamVR_Events.Fade.Listen(new UnityAction<Color, float, bool>(this.OnStartFade));
		SteamVR_Events.FadeReady.Send();
	}

	// Token: 0x06005F16 RID: 24342 RVA: 0x00214460 File Offset: 0x00212860
	private void OnDisable()
	{
		SteamVR_Events.Fade.Remove(new UnityAction<Color, float, bool>(this.OnStartFade));
	}

	// Token: 0x06005F17 RID: 24343 RVA: 0x00214478 File Offset: 0x00212878
	private void OnPostRender()
	{
		if (this.currentColor != this.targetColor)
		{
			if (Mathf.Abs(this.currentColor.a - this.targetColor.a) < Mathf.Abs(this.deltaColor.a) * Time.deltaTime)
			{
				this.currentColor = this.targetColor;
				this.deltaColor = new Color(0f, 0f, 0f, 0f);
			}
			else
			{
				this.currentColor += this.deltaColor * Time.deltaTime;
			}
			if (this.fadeOverlay)
			{
				SteamVR_Overlay instance = SteamVR_Overlay.instance;
				if (instance != null)
				{
					instance.alpha = 1f - this.currentColor.a;
				}
			}
		}
		if (this.currentColor.a > 0f && SteamVR_Fade.fadeMaterial)
		{
			SteamVR_Fade.fadeMaterial.SetColor(SteamVR_Fade.fadeMaterialColorID, this.currentColor);
			SteamVR_Fade.fadeMaterial.SetPass(0);
			GL.Begin(7);
			GL.Vertex3(-1f, -1f, 0f);
			GL.Vertex3(1f, -1f, 0f);
			GL.Vertex3(1f, 1f, 0f);
			GL.Vertex3(-1f, 1f, 0f);
			GL.End();
		}
	}

	// Token: 0x040044B1 RID: 17585
	private Color currentColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x040044B2 RID: 17586
	private Color targetColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x040044B3 RID: 17587
	private Color deltaColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x040044B4 RID: 17588
	private bool fadeOverlay;

	// Token: 0x040044B5 RID: 17589
	private static Material fadeMaterial;

	// Token: 0x040044B6 RID: 17590
	private static int fadeMaterialColorID = -1;
}
