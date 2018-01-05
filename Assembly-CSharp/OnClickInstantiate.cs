using System;
using UnityEngine;

// Token: 0x0200078F RID: 1935
public class OnClickInstantiate : MonoBehaviour
{
	// Token: 0x06003ED0 RID: 16080 RVA: 0x0013CDAC File Offset: 0x0013B1AC
	private void OnClick()
	{
		if (!PhotonNetwork.inRoom)
		{
			return;
		}
		int instantiateType = this.InstantiateType;
		if (instantiateType != 0)
		{
			if (instantiateType == 1)
			{
				PhotonNetwork.InstantiateSceneObject(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0, null);
			}
		}
		else
		{
			PhotonNetwork.Instantiate(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
		}
	}

	// Token: 0x06003ED1 RID: 16081 RVA: 0x0013CE54 File Offset: 0x0013B254
	private void OnGUI()
	{
		if (this.showGui)
		{
			GUILayout.BeginArea(new Rect((float)(Screen.width - 180), 0f, 180f, 50f));
			this.InstantiateType = GUILayout.Toolbar(this.InstantiateType, this.InstantiateTypeNames, new GUILayoutOption[0]);
			GUILayout.EndArea();
		}
	}

	// Token: 0x0400276E RID: 10094
	public GameObject Prefab;

	// Token: 0x0400276F RID: 10095
	public int InstantiateType;

	// Token: 0x04002770 RID: 10096
	private string[] InstantiateTypeNames = new string[]
	{
		"Mine",
		"Scene"
	};

	// Token: 0x04002771 RID: 10097
	public bool showGui;
}
