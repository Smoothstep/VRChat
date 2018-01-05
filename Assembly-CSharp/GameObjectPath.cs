using System;
using UnityEngine;

// Token: 0x02000A8C RID: 2700
public class GameObjectPath : MonoBehaviour
{
	// Token: 0x0600515C RID: 20828 RVA: 0x001BE204 File Offset: 0x001BC604
	public static string GetGameObjectPath(GameObject obj)
	{
		string text = "/" + obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			text = "/" + obj.name + text;
		}
		return text;
	}
}
