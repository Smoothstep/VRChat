using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A21 RID: 2593
public class CameraSwitch : MonoBehaviour
{
	// Token: 0x06004E57 RID: 20055 RVA: 0x001A410C File Offset: 0x001A250C
	private void OnEnable()
	{
		this.text.text = this.objects[this.m_CurrentActiveObject].name;
	}

	// Token: 0x06004E58 RID: 20056 RVA: 0x001A412C File Offset: 0x001A252C
	public void NextCamera()
	{
		int num = (this.m_CurrentActiveObject + 1 < this.objects.Length) ? (this.m_CurrentActiveObject + 1) : 0;
		for (int i = 0; i < this.objects.Length; i++)
		{
			this.objects[i].SetActive(i == num);
		}
		this.m_CurrentActiveObject = num;
		this.text.text = this.objects[this.m_CurrentActiveObject].name;
	}

	// Token: 0x04003664 RID: 13924
	public GameObject[] objects;

	// Token: 0x04003665 RID: 13925
	public Text text;

	// Token: 0x04003666 RID: 13926
	private int m_CurrentActiveObject;
}
