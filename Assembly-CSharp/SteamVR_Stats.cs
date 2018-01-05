using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Valve.VR;

// Token: 0x02000C0E RID: 3086
public class SteamVR_Stats : MonoBehaviour
{
	// Token: 0x06005F91 RID: 24465 RVA: 0x00219BA8 File Offset: 0x00217FA8
	private void Awake()
	{
		if (this.text == null)
		{
			this.text = base.GetComponent<GUIText>();
			this.text.enabled = false;
		}
		if (this.fadeDuration > 0f)
		{
			SteamVR_Fade.Start(this.fadeColor, 0f, false);
			SteamVR_Fade.Start(Color.clear, this.fadeDuration, false);
		}
	}

	// Token: 0x06005F92 RID: 24466 RVA: 0x00219C10 File Offset: 0x00218010
	private void Update()
	{
		if (this.text != null)
		{
			if (Input.GetKeyDown(KeyCode.I))
			{
				this.text.enabled = !this.text.enabled;
			}
			if (this.text.enabled)
			{
				CVRCompositor compositor = OpenVR.Compositor;
				if (compositor != null)
				{
					Compositor_FrameTiming compositor_FrameTiming = default(Compositor_FrameTiming);
					compositor_FrameTiming.m_nSize = (uint)Marshal.SizeOf(typeof(Compositor_FrameTiming));
					compositor.GetFrameTiming(ref compositor_FrameTiming, 0u);
					double flSystemTimeInSeconds = compositor_FrameTiming.m_flSystemTimeInSeconds;
					if (flSystemTimeInSeconds > this.lastUpdate)
					{
						double num = (this.lastUpdate <= 0.0) ? 0.0 : (1.0 / (flSystemTimeInSeconds - this.lastUpdate));
						this.lastUpdate = flSystemTimeInSeconds;
						this.text.text = string.Format("framerate: {0:N0}\ndropped frames: {1}", num, (int)compositor_FrameTiming.m_nNumDroppedFrames);
					}
					else
					{
						this.lastUpdate = flSystemTimeInSeconds;
					}
				}
			}
		}
	}

	// Token: 0x0400454C RID: 17740
	public GUIText text;

	// Token: 0x0400454D RID: 17741
	public Color fadeColor = Color.black;

	// Token: 0x0400454E RID: 17742
	public float fadeDuration = 1f;

	// Token: 0x0400454F RID: 17743
	private double lastUpdate;
}
