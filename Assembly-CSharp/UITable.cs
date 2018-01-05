using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020005D4 RID: 1492
[AddComponentMenu("NGUI/Interaction/Table")]
public class UITable : UIWidgetContainer
{
	// Token: 0x17000773 RID: 1907
	// (set) Token: 0x06003191 RID: 12689 RVA: 0x000F4E52 File Offset: 0x000F3252
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

	// Token: 0x06003192 RID: 12690 RVA: 0x000F4E68 File Offset: 0x000F3268
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
		if (this.sorting != UITable.Sorting.None)
		{
			if (this.sorting == UITable.Sorting.Alphabetic)
			{
				List<Transform> list2 = list;
				if (UITable.f__mg0 == null)
				{
					UITable.f__mg0 = new Comparison<Transform>(UIGrid.SortByName);
				}
				list2.Sort(UITable.f__mg0);
			}
			else if (this.sorting == UITable.Sorting.Horizontal)
			{
				List<Transform> list3 = list;
				if (UITable.f__mg1 == null)
				{
					UITable.f__mg1 = new Comparison<Transform>(UIGrid.SortHorizontal);
				}
				list3.Sort(UITable.f__mg1);
			}
			else if (this.sorting == UITable.Sorting.Vertical)
			{
				List<Transform> list4 = list;
				if (UITable.f__mg2 == null)
				{
					UITable.f__mg2 = new Comparison<Transform>(UIGrid.SortVertical);
				}
				list4.Sort(UITable.f__mg2);
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

	// Token: 0x06003193 RID: 12691 RVA: 0x000F4F99 File Offset: 0x000F3399
	protected virtual void Sort(List<Transform> list)
	{
		if (UITable.f__mg3 == null)
		{
			UITable.f__mg3 = new Comparison<Transform>(UIGrid.SortByName);
		}
		list.Sort(UITable.f__mg3);
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x000F4FBE File Offset: 0x000F33BE
	protected virtual void Start()
	{
		this.Init();
		this.Reposition();
		base.enabled = false;
	}

	// Token: 0x06003195 RID: 12693 RVA: 0x000F4FD3 File Offset: 0x000F33D3
	protected virtual void Init()
	{
		this.mInitDone = true;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
	}

	// Token: 0x06003196 RID: 12694 RVA: 0x000F4FED File Offset: 0x000F33ED
	protected virtual void LateUpdate()
	{
		if (this.mReposition)
		{
			this.Reposition();
		}
		base.enabled = false;
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x000F5007 File Offset: 0x000F3407
	private void OnValidate()
	{
		if (!Application.isPlaying && NGUITools.GetActive(this))
		{
			this.Reposition();
		}
	}

	// Token: 0x06003198 RID: 12696 RVA: 0x000F5024 File Offset: 0x000F3424
	protected void RepositionVariableSize(List<Transform> children)
	{
		float num = 0f;
		float num2 = 0f;
		int num3 = (this.columns <= 0) ? 1 : (children.Count / this.columns + 1);
		int num4 = (this.columns <= 0) ? children.Count : this.columns;
		Bounds[,] array = new Bounds[num3, num4];
		Bounds[] array2 = new Bounds[num4];
		Bounds[] array3 = new Bounds[num3];
		int num5 = 0;
		int num6 = 0;
		int i = 0;
		int count = children.Count;
		while (i < count)
		{
			Transform transform = children[i];
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform, !this.hideInactive);
			Vector3 localScale = transform.localScale;
			bounds.min = Vector3.Scale(bounds.min, localScale);
			bounds.max = Vector3.Scale(bounds.max, localScale);
			array[num6, num5] = bounds;
			array2[num5].Encapsulate(bounds);
			array3[num6].Encapsulate(bounds);
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
			}
			i++;
		}
		num5 = 0;
		num6 = 0;
		Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.cellAlignment);
		int j = 0;
		int count2 = children.Count;
		while (j < count2)
		{
			Transform transform2 = children[j];
			Bounds bounds2 = array[num6, num5];
			Bounds bounds3 = array2[num5];
			Bounds bounds4 = array3[num6];
			Vector3 localPosition = transform2.localPosition;
			localPosition.x = num + bounds2.extents.x - bounds2.center.x;
			localPosition.x -= Mathf.Lerp(0f, bounds2.max.x - bounds2.min.x - bounds3.max.x + bounds3.min.x, pivotOffset.x) - this.padding.x;
			if (this.direction == UITable.Direction.Down)
			{
				localPosition.y = -num2 - bounds2.extents.y - bounds2.center.y;
				localPosition.y += Mathf.Lerp(bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y, 0f, pivotOffset.y) - this.padding.y;
			}
			else
			{
				localPosition.y = num2 + bounds2.extents.y - bounds2.center.y;
				localPosition.y -= Mathf.Lerp(0f, bounds2.max.y - bounds2.min.y - bounds4.max.y + bounds4.min.y, pivotOffset.y) - this.padding.y;
			}
			num += bounds3.size.x + this.padding.x * 2f;
			transform2.localPosition = localPosition;
			if (++num5 >= this.columns && this.columns > 0)
			{
				num5 = 0;
				num6++;
				num = 0f;
				num2 += bounds4.size.y + this.padding.y * 2f;
			}
			j++;
		}
		if (this.pivot != UIWidget.Pivot.TopLeft)
		{
			pivotOffset = NGUIMath.GetPivotOffset(this.pivot);
			Bounds bounds5 = NGUIMath.CalculateRelativeWidgetBounds(base.transform);
			float num7 = Mathf.Lerp(0f, bounds5.size.x, pivotOffset.x);
			float num8 = Mathf.Lerp(-bounds5.size.y, 0f, pivotOffset.y);
			Transform transform3 = base.transform;
			for (int k = 0; k < transform3.childCount; k++)
			{
				Transform child = transform3.GetChild(k);
				SpringPosition component = child.GetComponent<SpringPosition>();
				if (component != null)
				{
					SpringPosition springPosition = component;
					springPosition.target.x = springPosition.target.x - num7;
					SpringPosition springPosition2 = component;
					springPosition2.target.y = springPosition2.target.y - num8;
				}
				else
				{
					Vector3 localPosition2 = child.localPosition;
					localPosition2.x -= num7;
					localPosition2.y -= num8;
					child.localPosition = localPosition2;
				}
			}
		}
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x000F5540 File Offset: 0x000F3940
	[ContextMenu("Execute")]
	public virtual void Reposition()
	{
		if (Application.isPlaying && !this.mInitDone && NGUITools.GetActive(this))
		{
			this.Init();
		}
		this.mReposition = false;
		Transform transform = base.transform;
		List<Transform> childList = this.GetChildList();
		if (childList.Count > 0)
		{
			this.RepositionVariableSize(childList);
		}
		if (this.keepWithinPanel && this.mPanel != null)
		{
			this.mPanel.ConstrainTargetToBounds(transform, true);
			UIScrollView component = this.mPanel.GetComponent<UIScrollView>();
			if (component != null)
			{
				component.UpdateScrollbars(true);
			}
		}
		if (this.onReposition != null)
		{
			this.onReposition();
		}
	}

	// Token: 0x04001C12 RID: 7186
	public int columns;

	// Token: 0x04001C13 RID: 7187
	public UITable.Direction direction;

	// Token: 0x04001C14 RID: 7188
	public UITable.Sorting sorting;

	// Token: 0x04001C15 RID: 7189
	public UIWidget.Pivot pivot;

	// Token: 0x04001C16 RID: 7190
	public UIWidget.Pivot cellAlignment;

	// Token: 0x04001C17 RID: 7191
	public bool hideInactive = true;

	// Token: 0x04001C18 RID: 7192
	public bool keepWithinPanel;

	// Token: 0x04001C19 RID: 7193
	public Vector2 padding = Vector2.zero;

	// Token: 0x04001C1A RID: 7194
	public UITable.OnReposition onReposition;

	// Token: 0x04001C1B RID: 7195
	public Comparison<Transform> onCustomSort;

	// Token: 0x04001C1C RID: 7196
	protected UIPanel mPanel;

	// Token: 0x04001C1D RID: 7197
	protected bool mInitDone;

	// Token: 0x04001C1E RID: 7198
	protected bool mReposition;

	// Token: 0x04001C1F RID: 7199
	[CompilerGenerated]
	private static Comparison<Transform> f__mg0;

	// Token: 0x04001C20 RID: 7200
	[CompilerGenerated]
	private static Comparison<Transform> f__mg1;

	// Token: 0x04001C21 RID: 7201
	[CompilerGenerated]
	private static Comparison<Transform> f__mg2;

	// Token: 0x04001C22 RID: 7202
	[CompilerGenerated]
	private static Comparison<Transform> f__mg3;

	// Token: 0x020005D5 RID: 1493
	// (Invoke) Token: 0x0600319B RID: 12699
	public delegate void OnReposition();

	// Token: 0x020005D6 RID: 1494
	public enum Direction
	{
		// Token: 0x04001C24 RID: 7204
		Down,
		// Token: 0x04001C25 RID: 7205
		Up
	}

	// Token: 0x020005D7 RID: 1495
	public enum Sorting
	{
		// Token: 0x04001C27 RID: 7207
		None,
		// Token: 0x04001C28 RID: 7208
		Alphabetic,
		// Token: 0x04001C29 RID: 7209
		Horizontal,
		// Token: 0x04001C2A RID: 7210
		Vertical,
		// Token: 0x04001C2B RID: 7211
		Custom
	}
}
