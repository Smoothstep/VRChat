using System;
using UnityEngine;

// Token: 0x02000588 RID: 1416
[AddComponentMenu("NGUI/Examples/Drag and Drop Item (Example)")]
public class ExampleDragDropItem : UIDragDropItem
{
	// Token: 0x06002FC6 RID: 12230 RVA: 0x000E9D2C File Offset: 0x000E812C
	protected override void OnDragDropRelease(GameObject surface)
	{
		if (surface != null)
		{
			ExampleDragDropSurface component = surface.GetComponent<ExampleDragDropSurface>();
			if (component != null)
			{
				GameObject gameObject = NGUITools.AddChild(component.gameObject, this.prefab);
				gameObject.transform.localScale = component.transform.localScale;
				Transform transform = gameObject.transform;
				transform.position = UICamera.lastWorldPosition;
				if (component.rotatePlacedObject)
				{
					transform.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
				}
				NGUITools.Destroy(base.gameObject);
				return;
			}
		}
		base.OnDragDropRelease(surface);
	}

	// Token: 0x04001A30 RID: 6704
	public GameObject prefab;
}
