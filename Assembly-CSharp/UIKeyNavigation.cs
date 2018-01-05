using System;
using UnityEngine;

// Token: 0x020005BC RID: 1468
[AddComponentMenu("NGUI/Interaction/Key Navigation")]
public class UIKeyNavigation : MonoBehaviour
{
	// Token: 0x060030CA RID: 12490 RVA: 0x000EBE60 File Offset: 0x000EA260
	protected virtual void OnEnable()
	{
		UIKeyNavigation.list.Add(this);
		if (this.startsSelected && (UICamera.selectedObject == null || !NGUITools.GetActive(UICamera.selectedObject)))
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.selectedObject = base.gameObject;
		}
	}

	// Token: 0x060030CB RID: 12491 RVA: 0x000EBEB3 File Offset: 0x000EA2B3
	protected virtual void OnDisable()
	{
		UIKeyNavigation.list.Remove(this);
	}

	// Token: 0x060030CC RID: 12492 RVA: 0x000EBEC1 File Offset: 0x000EA2C1
	protected GameObject GetLeft()
	{
		if (NGUITools.GetActive(this.onLeft))
		{
			return this.onLeft;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.left, true);
	}

	// Token: 0x060030CD RID: 12493 RVA: 0x000EBF00 File Offset: 0x000EA300
	private GameObject GetRight()
	{
		if (NGUITools.GetActive(this.onRight))
		{
			return this.onRight;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Vertical || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.right, true);
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x000EBF3F File Offset: 0x000EA33F
	protected GameObject GetUp()
	{
		if (NGUITools.GetActive(this.onUp))
		{
			return this.onUp;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.up, false);
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x000EBF7E File Offset: 0x000EA37E
	protected GameObject GetDown()
	{
		if (NGUITools.GetActive(this.onDown))
		{
			return this.onDown;
		}
		if (this.constraint == UIKeyNavigation.Constraint.Horizontal || this.constraint == UIKeyNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.down, false);
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x000EBFC0 File Offset: 0x000EA3C0
	protected GameObject Get(Vector3 myDir, bool horizontal)
	{
		Transform transform = base.transform;
		myDir = transform.TransformDirection(myDir);
		Vector3 center = UIKeyNavigation.GetCenter(base.gameObject);
		float num = float.MaxValue;
		GameObject result = null;
		for (int i = 0; i < UIKeyNavigation.list.size; i++)
		{
			UIKeyNavigation uikeyNavigation = UIKeyNavigation.list[i];
			if (!(uikeyNavigation == this))
			{
				UIButton component = uikeyNavigation.GetComponent<UIButton>();
				if (!(component != null) || component.isEnabled)
				{
					Vector3 direction = UIKeyNavigation.GetCenter(uikeyNavigation.gameObject) - center;
					float num2 = Vector3.Dot(myDir, direction.normalized);
					if (num2 >= 0.707f)
					{
						direction = transform.InverseTransformDirection(direction);
						if (horizontal)
						{
							direction.y *= 2f;
						}
						else
						{
							direction.x *= 2f;
						}
						float sqrMagnitude = direction.sqrMagnitude;
						if (sqrMagnitude <= num)
						{
							result = uikeyNavigation.gameObject;
							num = sqrMagnitude;
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x000EC0E8 File Offset: 0x000EA4E8
	protected static Vector3 GetCenter(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		if (component != null)
		{
			Vector3[] worldCorners = component.worldCorners;
			return (worldCorners[0] + worldCorners[2]) * 0.5f;
		}
		return go.transform.position;
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x000EC144 File Offset: 0x000EA544
	protected virtual void OnKey(KeyCode key)
	{
		if (!NGUITools.GetActive(this))
		{
			return;
		}
		GameObject gameObject = null;
		switch (key)
		{
		case KeyCode.UpArrow:
			gameObject = this.GetUp();
			break;
		case KeyCode.DownArrow:
			gameObject = this.GetDown();
			break;
		case KeyCode.RightArrow:
			gameObject = this.GetRight();
			break;
		case KeyCode.LeftArrow:
			gameObject = this.GetLeft();
			break;
		default:
			if (key == KeyCode.Tab)
			{
				if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
				{
					gameObject = this.GetLeft();
					if (gameObject == null)
					{
						gameObject = this.GetUp();
					}
					if (gameObject == null)
					{
						gameObject = this.GetDown();
					}
					if (gameObject == null)
					{
						gameObject = this.GetRight();
					}
				}
				else
				{
					gameObject = this.GetRight();
					if (gameObject == null)
					{
						gameObject = this.GetDown();
					}
					if (gameObject == null)
					{
						gameObject = this.GetUp();
					}
					if (gameObject == null)
					{
						gameObject = this.GetLeft();
					}
				}
			}
			break;
		}
		if (gameObject != null)
		{
			UICamera.selectedObject = gameObject;
		}
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x000EC272 File Offset: 0x000EA672
	protected virtual void OnClick()
	{
		if (NGUITools.GetActive(this) && NGUITools.GetActive(this.onClick))
		{
			UICamera.selectedObject = this.onClick;
		}
	}

	// Token: 0x04001B53 RID: 6995
	public static BetterList<UIKeyNavigation> list = new BetterList<UIKeyNavigation>();

	// Token: 0x04001B54 RID: 6996
	public UIKeyNavigation.Constraint constraint;

	// Token: 0x04001B55 RID: 6997
	public GameObject onUp;

	// Token: 0x04001B56 RID: 6998
	public GameObject onDown;

	// Token: 0x04001B57 RID: 6999
	public GameObject onLeft;

	// Token: 0x04001B58 RID: 7000
	public GameObject onRight;

	// Token: 0x04001B59 RID: 7001
	public GameObject onClick;

	// Token: 0x04001B5A RID: 7002
	public bool startsSelected;

	// Token: 0x020005BD RID: 1469
	public enum Constraint
	{
		// Token: 0x04001B5C RID: 7004
		None,
		// Token: 0x04001B5D RID: 7005
		Vertical,
		// Token: 0x04001B5E RID: 7006
		Horizontal,
		// Token: 0x04001B5F RID: 7007
		Explicit
	}
}
