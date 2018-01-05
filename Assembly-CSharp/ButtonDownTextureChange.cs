using System;
using UnityEngine;

// Token: 0x020009FE RID: 2558
[RequireComponent(typeof(stdButton))]
[RequireComponent(typeof(GUITexture))]
public class ButtonDownTextureChange : MonoBehaviour
{
	// Token: 0x06004DD1 RID: 19921 RVA: 0x001A1640 File Offset: 0x0019FA40
	private void OnEnable()
	{
		this.m_Button = base.GetComponent<stdButton>();
		this.guiTexture = base.GetComponent<GUITexture>();
	}

	// Token: 0x06004DD2 RID: 19922 RVA: 0x001A165C File Offset: 0x0019FA5C
	private void Update()
	{
		if (CrossPlatformInput.GetButtonDown(this.m_Button.buttonName) && !this.down)
		{
			this.guiTexture.texture = this.activeTexture;
			this.down = true;
		}
		if (CrossPlatformInput.GetButtonUp("NextCamera") && this.down)
		{
			this.guiTexture.texture = this.idleTexture;
			this.down = false;
		}
	}

	// Token: 0x040035C8 RID: 13768
	private stdButton m_Button;

	// Token: 0x040035C9 RID: 13769
	private GUITexture guiTexture;

	// Token: 0x040035CA RID: 13770
	public Texture idleTexture;

	// Token: 0x040035CB RID: 13771
	public Texture activeTexture;

	// Token: 0x040035CC RID: 13772
	private bool down;
}
