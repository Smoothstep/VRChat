using System;
using UnityEngine;

// Token: 0x02000A1D RID: 2589
[RequireComponent(typeof(GUIElement))]
public class stdButton : MonoBehaviour
{
	// Token: 0x06004E47 RID: 20039 RVA: 0x001A3F54 File Offset: 0x001A2354
	private void OnEnable()
	{
		this.m_Button = ButtonFactory.GetPlatformSpecificButtonImplementation();
		this.m_Button.Enable(this.buttonName, this.pairedWithInputManager, base.GetComponent<GUIElement>().GetScreenRect());
	}

	// Token: 0x06004E48 RID: 20040 RVA: 0x001A3F83 File Offset: 0x001A2383
	private void OnDisable()
	{
		this.m_Button.Disable();
	}

	// Token: 0x06004E49 RID: 20041 RVA: 0x001A3F90 File Offset: 0x001A2390
	private void Update()
	{
		this.m_Button.Update();
	}

	// Token: 0x0400365A RID: 13914
	public string buttonName = "Fire1";

	// Token: 0x0400365B RID: 13915
	public bool pairedWithInputManager;

	// Token: 0x0400365C RID: 13916
	private AbstractButton m_Button;
}
