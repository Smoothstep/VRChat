using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A1F RID: 2591
public class PauseMenu : MonoBehaviour
{
	// Token: 0x06004E4D RID: 20045 RVA: 0x001A3FE7 File Offset: 0x001A23E7
	private void Awake()
	{
		this.m_MenuToggle = base.GetComponent<Toggle>();
	}

	// Token: 0x06004E4E RID: 20046 RVA: 0x001A3FF5 File Offset: 0x001A23F5
	private void MenuOn()
	{
		this.m_TimeScaleRef = Time.timeScale;
		Time.timeScale = 0f;
		this.m_VolumeRef = AudioListener.volume;
		AudioListener.volume = 0f;
		this.m_Paused = true;
	}

	// Token: 0x06004E4F RID: 20047 RVA: 0x001A4028 File Offset: 0x001A2428
	public void MenuOff()
	{
		Time.timeScale = this.m_TimeScaleRef;
		AudioListener.volume = this.m_VolumeRef;
		this.m_Paused = false;
	}

	// Token: 0x06004E50 RID: 20048 RVA: 0x001A4048 File Offset: 0x001A2448
	public void OnMenuStatusChange()
	{
		if (this.m_MenuToggle.isOn && !this.m_Paused)
		{
			this.MenuOn();
		}
		else if (!this.m_MenuToggle.isOn && this.m_Paused)
		{
			this.MenuOff();
		}
	}

	// Token: 0x06004E51 RID: 20049 RVA: 0x001A409C File Offset: 0x001A249C
	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			this.m_MenuToggle.isOn = !this.m_MenuToggle.isOn;
			Cursor.visible = this.m_MenuToggle.isOn;
		}
	}

	// Token: 0x0400365F RID: 13919
	private Toggle m_MenuToggle;

	// Token: 0x04003660 RID: 13920
	private float m_TimeScaleRef = 1f;

	// Token: 0x04003661 RID: 13921
	private float m_VolumeRef = 1f;

	// Token: 0x04003662 RID: 13922
	private bool m_Paused;
}
