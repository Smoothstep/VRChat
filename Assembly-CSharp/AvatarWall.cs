using System;
using UnityEngine;
using VRC.Core;
using VRCSDK2;

// Token: 0x02000A5A RID: 2650
public class AvatarWall : MonoBehaviour
{
	// Token: 0x06005039 RID: 20537 RVA: 0x001B76AD File Offset: 0x001B5AAD
	private void Start()
	{
		this.BuildWall();
	}

	// Token: 0x0600503A RID: 20538 RVA: 0x001B76B8 File Offset: 0x001B5AB8
	private void BuildWall()
	{
		for (int i = 0; i < this.avatars.Length; i++)
		{
			GameObject gameObject = AssetManagement.Instantiate(this.pedestalGO, base.transform.position, base.transform.rotation) as GameObject;
			gameObject.transform.parent = base.transform;
			Vector3 localPosition = gameObject.transform.localPosition;
			if (this.spawnRightToLeft)
			{
				localPosition.x = (float)(-(float)i % this.numColumns) * this.avatarMargin.x;
			}
			else
			{
				localPosition.x = (float)(i % this.numColumns) * this.avatarMargin.x;
			}
			localPosition.y = (float)(i / this.numColumns) * this.avatarMargin.y;
			gameObject.transform.localPosition = localPosition;
			VRC_AvatarPedestal component = gameObject.GetComponent<VRC_AvatarPedestal>();
			component.transform.Rotate(new Vector3(0f, 180f, 0f));
			component.scale = this.scale;
			GameObject gameObject2 = gameObject.transform.Find("UseVolume").gameObject;
			gameObject2.AddComponent<Highlightable>();
			CapsuleCollider component2 = gameObject2.GetComponent<CapsuleCollider>();
			component2.height *= this.scale * 1.3f;
			component2.radius *= this.scale * 1.3f;
			Vector3 center = new Vector3(component2.center.x, 0.7f, component2.center.z);
			component2.center = center;
		}
	}

	// Token: 0x0600503B RID: 20539 RVA: 0x001B7858 File Offset: 0x001B5C58
	private void OnEnable()
	{
		if (!this.hasTurnedOffAnimators)
		{
			Animator[] componentsInChildren = base.GetComponentsInChildren<Animator>(true);
			if (componentsInChildren.Length > 0)
			{
				foreach (Animator animator in componentsInChildren)
				{
					animator.enabled = false;
				}
				this.hasTurnedOffAnimators = true;
			}
		}
	}

	// Token: 0x04003900 RID: 14592
	public GameObject pedestalGO;

	// Token: 0x04003901 RID: 14593
	public GameObject[] avatars;

	// Token: 0x04003902 RID: 14594
	public Vector2 avatarMargin;

	// Token: 0x04003903 RID: 14595
	public int numColumns = 6;

	// Token: 0x04003904 RID: 14596
	public float scale = 1f;

	// Token: 0x04003905 RID: 14597
	public bool spawnRightToLeft;

	// Token: 0x04003906 RID: 14598
	private bool hasTurnedOffAnimators;
}
