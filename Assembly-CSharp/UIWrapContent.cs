using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x020005DC RID: 1500
[AddComponentMenu("NGUI/Interaction/Wrap Content")]
public class UIWrapContent : MonoBehaviour
{
	// Token: 0x060031B3 RID: 12723 RVA: 0x000F5D5C File Offset: 0x000F415C
	protected virtual void Start()
	{
		this.SortBasedOnScrollMovement();
		this.WrapContent();
		if (this.mScroll != null)
		{
			this.mScroll.GetComponent<UIPanel>().onClipMove = new UIPanel.OnClippingMoved(this.OnMove);
		}
		this.mFirstTime = false;
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x000F5DAA File Offset: 0x000F41AA
	protected virtual void OnMove(UIPanel panel)
	{
		this.WrapContent();
	}

	// Token: 0x060031B5 RID: 12725 RVA: 0x000F5DB4 File Offset: 0x000F41B4
	[ContextMenu("Sort Based on Scroll Movement")]
	public void SortBasedOnScrollMovement()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		if (this.mHorizontal)
		{
			List<Transform> list = this.mChildren;
			if (UIWrapContent.f__mg0 == null)
			{
				UIWrapContent.f__mg0 = new Comparison<Transform>(UIGrid.SortHorizontal);
			}
			list.Sort(UIWrapContent.f__mg0);
		}
		else
		{
			List<Transform> list2 = this.mChildren;
			if (UIWrapContent.f__mg1 == null)
			{
				UIWrapContent.f__mg1 = new Comparison<Transform>(UIGrid.SortVertical);
			}
			list2.Sort(UIWrapContent.f__mg1);
		}
		this.ResetChildPositions();
	}

	// Token: 0x060031B6 RID: 12726 RVA: 0x000F5E74 File Offset: 0x000F4274
	[ContextMenu("Sort Alphabetically")]
	public void SortAlphabetically()
	{
		if (!this.CacheScrollView())
		{
			return;
		}
		this.mChildren.Clear();
		for (int i = 0; i < this.mTrans.childCount; i++)
		{
			this.mChildren.Add(this.mTrans.GetChild(i));
		}
		List<Transform> list = this.mChildren;
		if (UIWrapContent.f__mg2 == null)
		{
			UIWrapContent.f__mg2 = new Comparison<Transform>(UIGrid.SortByName);
		}
		list.Sort(UIWrapContent.f__mg2);
		this.ResetChildPositions();
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x000F5EFC File Offset: 0x000F42FC
	protected bool CacheScrollView()
	{
		this.mTrans = base.transform;
		this.mPanel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		this.mScroll = this.mPanel.GetComponent<UIScrollView>();
		if (this.mScroll == null)
		{
			return false;
		}
		if (this.mScroll.movement == UIScrollView.Movement.Horizontal)
		{
			this.mHorizontal = true;
		}
		else
		{
			if (this.mScroll.movement != UIScrollView.Movement.Vertical)
			{
				return false;
			}
			this.mHorizontal = false;
		}
		return true;
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x000F5F88 File Offset: 0x000F4388
	private void ResetChildPositions()
	{
		int i = 0;
		int count = this.mChildren.Count;
		while (i < count)
		{
			Transform transform = this.mChildren[i];
			transform.localPosition = ((!this.mHorizontal) ? new Vector3(0f, (float)(-(float)i * this.itemSize), 0f) : new Vector3((float)(i * this.itemSize), 0f, 0f));
			i++;
		}
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x000F6008 File Offset: 0x000F4408
	public void WrapContent()
	{
		float num = (float)(this.itemSize * this.mChildren.Count) * 0.5f;
		Vector3[] worldCorners = this.mPanel.worldCorners;
		for (int i = 0; i < 4; i++)
		{
			Vector3 vector = worldCorners[i];
			vector = this.mTrans.InverseTransformPoint(vector);
			worldCorners[i] = vector;
		}
		Vector3 vector2 = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
		bool flag = true;
		float num2 = num * 2f;
		if (this.mHorizontal)
		{
			float num3 = worldCorners[0].x - (float)this.itemSize;
			float num4 = worldCorners[2].x + (float)this.itemSize;
			int j = 0;
			int count = this.mChildren.Count;
			while (j < count)
			{
				Transform transform = this.mChildren[j];
				float num5 = transform.localPosition.x - vector2.x;
				if (num5 < -num)
				{
					Vector3 localPosition = transform.localPosition;
					localPosition.x += num2;
					num5 = localPosition.x - vector2.x;
					int num6 = Mathf.RoundToInt(localPosition.x / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num6 && num6 <= this.maxIndex))
					{
						transform.localPosition = localPosition;
						this.UpdateItem(transform, j);
						transform.name = num6.ToString();
					}
					else
					{
						flag = false;
					}
				}
				else if (num5 > num)
				{
					Vector3 localPosition2 = transform.localPosition;
					localPosition2.x -= num2;
					num5 = localPosition2.x - vector2.x;
					int num7 = Mathf.RoundToInt(localPosition2.x / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num7 && num7 <= this.maxIndex))
					{
						transform.localPosition = localPosition2;
						this.UpdateItem(transform, j);
						transform.name = num7.ToString();
					}
					else
					{
						flag = false;
					}
				}
				else if (this.mFirstTime)
				{
					this.UpdateItem(transform, j);
				}
				if (this.cullContent)
				{
					num5 += this.mPanel.clipOffset.x - this.mTrans.localPosition.x;
					if (!UICamera.IsPressed(transform.gameObject))
					{
						NGUITools.SetActive(transform.gameObject, num5 > num3 && num5 < num4, false);
					}
				}
				j++;
			}
		}
		else
		{
			float num8 = worldCorners[0].y - (float)this.itemSize;
			float num9 = worldCorners[2].y + (float)this.itemSize;
			int k = 0;
			int count2 = this.mChildren.Count;
			while (k < count2)
			{
				Transform transform2 = this.mChildren[k];
				float num10 = transform2.localPosition.y - vector2.y;
				if (num10 < -num)
				{
					Vector3 localPosition3 = transform2.localPosition;
					localPosition3.y += num2;
					num10 = localPosition3.y - vector2.y;
					int num11 = Mathf.RoundToInt(localPosition3.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num11 && num11 <= this.maxIndex))
					{
						transform2.localPosition = localPosition3;
						this.UpdateItem(transform2, k);
						transform2.name = num11.ToString();
					}
					else
					{
						flag = false;
					}
				}
				else if (num10 > num)
				{
					Vector3 localPosition4 = transform2.localPosition;
					localPosition4.y -= num2;
					num10 = localPosition4.y - vector2.y;
					int num12 = Mathf.RoundToInt(localPosition4.y / (float)this.itemSize);
					if (this.minIndex == this.maxIndex || (this.minIndex <= num12 && num12 <= this.maxIndex))
					{
						transform2.localPosition = localPosition4;
						this.UpdateItem(transform2, k);
						transform2.name = num12.ToString();
					}
					else
					{
						flag = false;
					}
				}
				else if (this.mFirstTime)
				{
					this.UpdateItem(transform2, k);
				}
				if (this.cullContent)
				{
					num10 += this.mPanel.clipOffset.y - this.mTrans.localPosition.y;
					if (!UICamera.IsPressed(transform2.gameObject))
					{
						NGUITools.SetActive(transform2.gameObject, num10 > num8 && num10 < num9, false);
					}
				}
				k++;
			}
		}
		this.mScroll.restrictWithinPanel = !flag;
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x000F6550 File Offset: 0x000F4950
	private void OnValidate()
	{
		if (this.maxIndex < this.minIndex)
		{
			this.maxIndex = this.minIndex;
		}
		if (this.minIndex > this.maxIndex)
		{
			this.maxIndex = this.minIndex;
		}
	}

	// Token: 0x060031BB RID: 12731 RVA: 0x000F658C File Offset: 0x000F498C
	protected virtual void UpdateItem(Transform item, int index)
	{
		if (this.onInitializeItem != null)
		{
			int realIndex = (this.mScroll.movement != UIScrollView.Movement.Vertical) ? Mathf.RoundToInt(item.localPosition.x / (float)this.itemSize) : Mathf.RoundToInt(item.localPosition.y / (float)this.itemSize);
			this.onInitializeItem(item.gameObject, index, realIndex);
		}
	}

	// Token: 0x04001C44 RID: 7236
	public int itemSize = 100;

	// Token: 0x04001C45 RID: 7237
	public bool cullContent = true;

	// Token: 0x04001C46 RID: 7238
	public int minIndex;

	// Token: 0x04001C47 RID: 7239
	public int maxIndex;

	// Token: 0x04001C48 RID: 7240
	public UIWrapContent.OnInitializeItem onInitializeItem;

	// Token: 0x04001C49 RID: 7241
	private Transform mTrans;

	// Token: 0x04001C4A RID: 7242
	private UIPanel mPanel;

	// Token: 0x04001C4B RID: 7243
	private UIScrollView mScroll;

	// Token: 0x04001C4C RID: 7244
	private bool mHorizontal;

	// Token: 0x04001C4D RID: 7245
	private bool mFirstTime = true;

	// Token: 0x04001C4E RID: 7246
	private List<Transform> mChildren = new List<Transform>();

	// Token: 0x04001C4F RID: 7247
	[CompilerGenerated]
	private static Comparison<Transform> f__mg0;

	// Token: 0x04001C50 RID: 7248
	[CompilerGenerated]
	private static Comparison<Transform> f__mg1;

	// Token: 0x04001C51 RID: 7249
	[CompilerGenerated]
	private static Comparison<Transform> f__mg2;

	// Token: 0x020005DD RID: 1501
	// (Invoke) Token: 0x060031BD RID: 12733
	public delegate void OnInitializeItem(GameObject go, int wrapIndex, int realIndex);
}
