using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020005B4 RID: 1460
[AddComponentMenu("NGUI/Interaction/Grid")]
public class UIGrid : UIWidgetContainer
{
	// Token: 0x1700074E RID: 1870
	// (set) Token: 0x060030A5 RID: 12453 RVA: 0x000EEAA0 File Offset: 0x000ECEA0
	public bool repositionNow
	{
		set
		{
			if (value)
			{
				this.mReposition = true;
				base.enabled = true;
			}
		}
	}

	// Token: 0x060030A6 RID: 12454 RVA: 0x000EEAB8 File Offset: 0x000ECEB8
	public List<Transform> GetChildList()
	{
		Transform transform = base.transform;
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!this.hideInactive || (child && NGUITools.GetActive(child.gameObject)))
			{
				list.Add(child);
			}
		}
		if (this.sorting != UIGrid.Sorting.None && this.arrangement != UIGrid.Arrangement.CellSnap)
		{
			if (this.sorting == UIGrid.Sorting.Alphabetic)
			{
				List<Transform> list2 = list;
				if (UIGrid.f__mg0 == null)
				{
					UIGrid.f__mg0 = new Comparison<Transform>(UIGrid.SortByName);
				}
				list2.Sort(UIGrid.f__mg0);
			}
			else if (this.sorting == UIGrid.Sorting.Horizontal)
			{
				List<Transform> list3 = list;
				if (UIGrid.f__mg1 == null)
				{
					UIGrid.f__mg1 = new Comparison<Transform>(UIGrid.SortHorizontal);
				}
				list3.Sort(UIGrid.f__mg1);
			}
			else if (this.sorting == UIGrid.Sorting.Vertical)
			{
				List<Transform> list4 = list;
				if (UIGrid.f__mg2 == null)
				{
					UIGrid.f__mg2 = new Comparison<Transform>(UIGrid.SortVertical);
				}
				list4.Sort(UIGrid.f__mg2);
			}
			else if (this.onCustomSort != null)
			{
				list.Sort(this.onCustomSort);
			}
			else
			{
				this.Sort(list);
			}
		}
		return list;
	}

	// Token: 0x060030A7 RID: 12455 RVA: 0x000EEBF8 File Offset: 0x000ECFF8
	public Transform GetChild(int index)
	{
		List<Transform> childList = this.GetChildList();
		return (index >= childList.Count) ? null : childList[index];
	}

	// Token: 0x060030A8 RID: 12456 RVA: 0x000EEC25 File Offset: 0x000ED025
	public int GetIndex(Transform trans)
	{
		return this.GetChildList().IndexOf(trans);
	}

	// Token: 0x060030A9 RID: 12457 RVA: 0x000EEC33 File Offset: 0x000ED033
	public void AddChild(Transform trans)
	{
		this.AddChild(trans, true);
	}

	// Token: 0x060030AA RID: 12458 RVA: 0x000EEC3D File Offset: 0x000ED03D
	public void AddChild(Transform trans, bool sort)
	{
		if (trans != null)
		{
			trans.parent = base.transform;
			this.ResetPosition(this.GetChildList());
		}
	}

	// Token: 0x060030AB RID: 12459 RVA: 0x000EEC64 File Offset: 0x000ED064
	public bool RemoveChild(Transform t)
	{
		List<Transform> childList = this.GetChildList();
		if (childList.Remove(t))
		{
			this.ResetPosition(childList);
			return true;
		}
		return false;
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x000EEC8E File Offset: 0x000ED08E
	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x000EECA8 File Offset: 0x000ED0A8
	protected virtual void Start()
	{
		if (!this.mInitDone)
		{
			this.Init();
		}
		bool flag = this.animateSmoothly;
		this.animateSmoothly = false;
		this.Reposition();
		this.animateSmoothly = flag;
		base.enabled = false;
	}

	// Token: 0x060030AE RID: 12462 RVA: 0x000EECE8 File Offset: 0x000ED0E8
	protected virtual void Update()
	{
		this.Reposition();
		base.enabled = false;
	}

	// Token: 0x060030AF RID: 12463 RVA: 0x000EECF7 File Offset: 0x000ED0F7
	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	// Token: 0x060030B0 RID: 12464 RVA: 0x000EED14 File Offset: 0x000ED114
	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	// Token: 0x060030B1 RID: 12465 RVA: 0x000EED28 File Offset: 0x000ED128
	public static int SortHorizontal(Transform a, Transform b)
	{
		return a.localPosition.x.CompareTo(b.localPosition.x);
	}

	// Token: 0x060030B2 RID: 12466 RVA: 0x000EED58 File Offset: 0x000ED158
	public static int SortVertical(Transform a, Transform b)
	{
		return b.localPosition.y.CompareTo(a.localPosition.y);
	}

	// Token: 0x060030B3 RID: 12467 RVA: 0x000EED86 File Offset: 0x000ED186
	protected virtual void Sort(List<Transform> list)
	{
	}

	// Token: 0x060030B4 RID: 12468 RVA: 0x000EED88 File Offset: 0x000ED188
	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(base.gameObject))
		{
			this.Init();
		}
		if (this.sorted)
		{
			this.sorted = false;
			if (this.sorting == UIGrid.Sorting.None)
			{
				this.sorting = UIGrid.Sorting.Alphabetic;
			}
			NGUITools.SetDirty(this);
		}
		List<Transform> childList = this.GetChildList();
		this.ResetPosition(childList);
		if (this.keepWithinPanel)
		{
			this.ConstrainWithinPanel();
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x000EEE20 File Offset: 0x000ED220
	public void ConstrainWithinPanel()
	{
		if (this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(base.transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
	}

	// Token: 0x060030B6 RID: 12470 RVA: 0x000EEE70 File Offset: 0x000ED270
	protected void ResetPosition(List<Transform> list)
	{
		this.mReposition = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Transform transform = base.transform;
		int i = 0;
		int count = list.Count;
		while (i < count)
		{
			Transform transform2 = list[i];
			Vector3 vector = transform2.localPosition;
			float z = vector.z;
			if (this.arrangement == UIGrid.Arrangement.CellSnap)
			{
				if (this.cellWidth > 0f)
				{
					vector.x = Mathf.Round(vector.x / this.cellWidth) * this.cellWidth;
				}
				if (this.cellHeight > 0f)
				{
					vector.y = Mathf.Round(vector.y / this.cellHeight) * this.cellHeight;
				}
			}
			else
			{
				vector = ((this.arrangement != UIGrid.Arrangement.Horizontal) ? new Vector3(this.cellWidth * (float)num2, -this.cellHeight * (float)num, z) : new Vector3(this.cellWidth * (float)num, -this.cellHeight * (float)num2, z));
			}
			if (this.animateSmoothly && Application.isPlaying && Vector3.SqrMagnitude(transform2.localPosition - vector) >= 0.0001f)
			{
				SpringPosition springPosition = SpringPosition.Begin(transform2.gameObject, vector, 15f);
				springPosition.updateScrollView = true;
				springPosition.ignoreTimeScale = true;
			}
			else
			{
				transform2.localPosition = vector;
			}
			num3 = Mathf.Max(num3, num);
			num4 = Mathf.Max(num4, num2);
			if (++num >= this.maxPerLine && this.maxPerLine > 0)
			{
				num = 0;
				num2++;
			}
			i++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			float num5;
			float num6;
			if (this.arrangement == UIGrid.Arrangement.Horizontal)
			{
				num5 = Mathf.Lerp(0f, (float)num3 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num4) * this.cellHeight, 0f, pivotOffset.y);
			}
			else
			{
				num5 = Mathf.Lerp(0f, (float)num4 * this.cellWidth, pivotOffset.x);
				num6 = Mathf.Lerp((float)(-(float)num3) * this.cellHeight, 0f, pivotOffset.y);
			}
			for (int j = 0; j < transform.childCount; j++)
			{
				Transform child = transform.GetChild(j);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition springPosition2 = component;
					springPosition2.target.x = springPosition2.target.x - num5;
					SpringPosition springPosition3 = component;
					springPosition3.target.y = springPosition3.target.y - num6;
				}
				else
				{
					Vector3 localPosition = child.localPosition;
					localPosition.x -= num5;
					localPosition.y -= num6;
					child.localPosition = localPosition;
				}
			}
		}
	}

	// Token: 0x04001B22 RID: 6946
	public UIGrid.Arrangement arrangement;

	// Token: 0x04001B23 RID: 6947
	public UIGrid.Sorting sorting;

	// Token: 0x04001B24 RID: 6948
	public UIWidget.Pivot pivot;

	// Token: 0x04001B25 RID: 6949
	public int maxPerLine;

	// Token: 0x04001B26 RID: 6950
	public float cellWidth = 200f;

	// Token: 0x04001B27 RID: 6951
	public float cellHeight = 200f;

	// Token: 0x04001B28 RID: 6952
	public bool animateSmoothly;

	// Token: 0x04001B29 RID: 6953
	public bool hideInactive;

	// Token: 0x04001B2A RID: 6954
	public bool keepWithinPanel;

	// Token: 0x04001B2B RID: 6955
	public UIGrid.OnReposition onReposition;

	// Token: 0x04001B2C RID: 6956
	public Comparison<Transform> onCustomSort;

	// Token: 0x04001B2D RID: 6957
	[HideInInspector]
	[SerializeField]
	private bool sorted;

	// Token: 0x04001B2E RID: 6958
	protected bool mReposition;

	// Token: 0x04001B2F RID: 6959
	protected UIPanel mPanel;

	// Token: 0x04001B30 RID: 6960
	protected bool mInitDone;

	// Token: 0x04001B31 RID: 6961
	[CompilerGenerated]
	private static Comparison<Transform> f__mg0;

	// Token: 0x04001B32 RID: 6962
	[CompilerGenerated]
	private static Comparison<Transform> f__mg1;

	// Token: 0x04001B33 RID: 6963
	[CompilerGenerated]
	private static Comparison<Transform> f__mg2;

	// Token: 0x020005B5 RID: 1461
	// (Invoke) Token: 0x060030B8 RID: 12472
	public delegate void OnReposition();

	// Token: 0x020005B6 RID: 1462
	public enum Arrangement
	{
		// Token: 0x04001B35 RID: 6965
		Horizontal,
		// Token: 0x04001B36 RID: 6966
		Vertical,
		// Token: 0x04001B37 RID: 6967
		CellSnap
	}

	// Token: 0x020005B7 RID: 1463
	public enum Sorting
	{
		// Token: 0x04001B39 RID: 6969
		None,
		// Token: 0x04001B3A RID: 6970
		Alphabetic,
		// Token: 0x04001B3B RID: 6971
		Horizontal,
		// Token: 0x04001B3C RID: 6972
		Vertical,
		// Token: 0x04001B3D RID: 6973
		Custom
	}
}
