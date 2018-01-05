using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Core;

// Token: 0x02000C50 RID: 3152
public abstract class UiVRCList : MonoBehaviour
{
	// Token: 0x060061AD RID: 25005 RVA: 0x002250D0 File Offset: 0x002234D0
	protected virtual void Awake()
	{
		this.paginatedLists = new Dictionary<int, List<ApiModel>>();
		this.scrollRect = base.GetComponent<ScrollRectEx>();
		this.scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollValueChanged));
		this.content = base.transform.Find("ViewPort/Content").GetComponent<RectTransform>();
		this.spacing = this.pickerPrefab.GetComponent<RectTransform>().rect.width;
		this.grid = base.GetComponentInChildren<GridLayoutGroup>();
		this.layoutElement = base.GetComponent<LayoutElement>();
		this.numElementsPerPage = this.collapsedCount;
		ScrollRect componentInParent = base.transform.parent.GetComponentInParent<ScrollRect>();
		if (componentInParent != null)
		{
			this.parentViewport = componentInParent.viewport;
		}
		this.UpdateToggleSprite();
		this.myListSiblingIndex = this.GetMyListSiblingOrdering();
	}

	// Token: 0x060061AE RID: 25006 RVA: 0x002251A8 File Offset: 0x002235A8
	protected virtual void OnEnable()
	{
		base.StartCoroutine(this.OnEnableCoroutine());
	}

	// Token: 0x060061AF RID: 25007 RVA: 0x002251B8 File Offset: 0x002235B8
	private IEnumerator OnEnableCoroutine()
	{
		yield return new WaitForSeconds((float)this.myListSiblingIndex / 2f);
		this.isOffScreen = this.IsVerticallyOffScreen(this.parentViewport, base.GetComponent<RectTransform>());
		this.defaultRect = this.content.transform.parent.GetComponent<RectTransform>().rect;
		this.FetchAndRenderElements(this.currentPage);
		this.HideOffScreenElements();
		this.isSetup = true;
		yield break;
	}

	// Token: 0x060061B0 RID: 25008 RVA: 0x002251D4 File Offset: 0x002235D4
	private int GetMyListSiblingOrdering()
	{
		int num = 0;
		for (int i = 0; i < base.transform.parent.childCount; i++)
		{
			UiVRCList component = base.transform.parent.GetChild(i).GetComponent<UiVRCList>();
			if (component != null)
			{
				if (component == this)
				{
					break;
				}
				num++;
			}
		}
		return num;
	}

	// Token: 0x060061B1 RID: 25009 RVA: 0x0022523D File Offset: 0x0022363D
	private void OnDisable()
	{
		this.Unexpand();
		this.isSetup = false;
	}

	// Token: 0x060061B2 RID: 25010 RVA: 0x0022524C File Offset: 0x0022364C
	public void OnScrollValueChanged(Vector2 v)
	{
		this.isOffScreen = this.IsVerticallyOffScreen(this.parentViewport, base.GetComponent<RectTransform>());
		if (this.wasOffScreenLastFrame && !this.isOffScreen)
		{
			this.FetchAndRenderElements(this.currentPage);
		}
		this.wasOffScreenLastFrame = this.isOffScreen;
		this.lastNumPages = this.numPages;
		this.numPages = this.pickers.Count / this.numElementsPerPage;
		if (this.lastNumPages != this.numPages)
		{
			return;
		}
		float num = this.scrollRect.horizontalNormalizedPosition;
		if (num > 1f)
		{
			num = 1f;
		}
		else if (num < 0f)
		{
			num = 0f;
		}
		this.lastPage = this.currentPage;
		this.currentPage = Mathf.RoundToInt(num * (float)this.numPages);
		if (this.usePagination && this.currentPage != this.lastPage && !this.isFetching)
		{
			this.FetchAndRenderElements(this.currentPage);
			this.isFetching = true;
		}
		if (this.lastPage == this.currentPage)
		{
			this.isFetching = false;
		}
		this.HideOffScreenElements();
	}

	// Token: 0x060061B3 RID: 25011 RVA: 0x00225380 File Offset: 0x00223780
	private bool IsVerticallyOffScreen(RectTransform viewPort, RectTransform list)
	{
		if (viewPort == null || list == null)
		{
			return false;
		}
		Vector3[] array = new Vector3[4];
		list.GetWorldCorners(array);
		Vector3[] array2 = new Vector3[4];
		viewPort.GetWorldCorners(array2);
		bool result = false;
		if (array[3].y > array2[2].y)
		{
			result = true;
		}
		else if (array[2].y < array2[3].y)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x060061B4 RID: 25012 RVA: 0x0022540C File Offset: 0x0022380C
	public void HideOffScreenElements()
	{
		for (int i = 0; i < this.pickers.Count; i++)
		{
			float num = this.content.GetComponent<GridLayoutGroup>().cellSize.x + this.content.GetComponent<GridLayoutGroup>().spacing.x;
			VRCUiContentButton vrcuiContentButton = this.pickers[i];
			if (vrcuiContentButton.transform.localPosition.x + num * 3f + this.content.transform.localPosition.x < 0f || vrcuiContentButton.transform.localPosition.x + this.content.transform.localPosition.x > num * 4f)
			{
				vrcuiContentButton.HideElement(true);
			}
			else
			{
				vrcuiContentButton.HideElement(false);
			}
		}
	}

	// Token: 0x060061B5 RID: 25013 RVA: 0x00225508 File Offset: 0x00223908
	private IEnumerator HideOffScreenElementsNextFrame()
	{
		yield return null;
		this.HideOffScreenElements();
		yield break;
	}

	// Token: 0x060061B6 RID: 25014 RVA: 0x00225523 File Offset: 0x00223923
	public void Refresh()
	{
		this.ClearList();
		this.FetchAndRenderElements(this.currentPage);
	}

	// Token: 0x060061B7 RID: 25015 RVA: 0x00225537 File Offset: 0x00223937
	public void FetchAndRenderElementsForCurrentPage()
	{
		this.FetchAndRenderElements(this.currentPage);
	}

	// Token: 0x060061B8 RID: 25016
	protected abstract void FetchAndRenderElements(int page);

	// Token: 0x060061B9 RID: 25017 RVA: 0x00225548 File Offset: 0x00223948
	protected void RenderElements<T>(List<T> list, int page)
	{
		if (list == null && this.hideWhenEmpty)
		{
			this.Hide(true);
			return;
		}
		if (list.Count == 0 && this.hideWhenEmpty)
		{
			this.Hide(true);
		}
		else
		{
			this.Hide(false);
		}
		if (this.isOffScreen)
		{
			return;
		}
		while (this.pickers.Count > list.Count)
		{
			VRCUiContentButton vrcuiContentButton = this.pickers[this.pickers.Count - 1];
			vrcuiContentButton.transform.SetParent(null);
			vrcuiContentButton.ClearImage();
			SimplePool.Despawn(vrcuiContentButton.gameObject);
			this.pickers.RemoveAt(this.pickers.Count - 1);
		}
		for (int i = 0; i < list.Count; i++)
		{
			object am = list[i];
			if (i < this.pickers.Count)
			{
				VRCUiContentButton picker = this.pickers[i];
				this.RefreshPickerFromApiModel(picker, am);
			}
			else
			{
				this.CreatePickerFromApiModel(am);
			}
		}
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.HideOffScreenElementsNextFrame());
		}
	}

	// Token: 0x060061BA RID: 25018 RVA: 0x00225680 File Offset: 0x00223A80
	private void RemovePickersInRange(int index, int count)
	{
		for (int i = index; i < index + count; i++)
		{
			VRCUiContentButton vrcuiContentButton = this.pickers[i];
			vrcuiContentButton.transform.parent = null;
			vrcuiContentButton.ClearImage();
			SimplePool.Despawn(vrcuiContentButton.gameObject);
		}
		this.pickers.RemoveRange(index, count);
	}

	// Token: 0x060061BB RID: 25019 RVA: 0x002256D8 File Offset: 0x00223AD8
	protected void CreatePickerFromApiModel(object am)
	{
		GameObject gameObject = SimplePool.Spawn(this.pickerPrefab, Vector3.zero, Quaternion.identity);
		RectTransform component = gameObject.GetComponent<RectTransform>();
		component.SetParent(this.content.transform);
		component.localScale = Vector3.one;
		component.localPosition = new Vector3(component.localPosition.x, 0f, 0f);
		component.localRotation = Quaternion.identity;
		VRCUiContentButton component2 = gameObject.GetComponent<VRCUiContentButton>();
		this.SetPickerContentFromApiModel(component2, am);
		this.pickers.Add(component2);
	}

	// Token: 0x060061BC RID: 25020 RVA: 0x00225767 File Offset: 0x00223B67
	protected void RefreshPickerFromApiModel(VRCUiContentButton picker, object am)
	{
		this.SetPickerContentFromApiModel(picker, am);
	}

	// Token: 0x060061BD RID: 25021
	protected abstract void SetPickerContentFromApiModel(VRCUiContentButton content, object am);

	// Token: 0x060061BE RID: 25022 RVA: 0x00225774 File Offset: 0x00223B74
	protected void ClearList()
	{
		foreach (VRCUiContentButton vrcuiContentButton in this.pickers)
		{
			vrcuiContentButton.transform.SetParent(null);
			vrcuiContentButton.ClearImage();
			SimplePool.Despawn(vrcuiContentButton.gameObject);
		}
		this.pickers.Clear();
	}

	// Token: 0x060061BF RID: 25023 RVA: 0x002257F4 File Offset: 0x00223BF4
	public void ClearAll()
	{
		this.ClearList();
		this.paginatedLists.Clear();
		this.currentPage = (this.lastPage = 0);
		this.isFetching = false;
		this.lastNumPages = (this.numPages = 0);
	}

	// Token: 0x060061C0 RID: 25024 RVA: 0x0022583C File Offset: 0x00223C3C
	protected void ClearListRange(int index, int count)
	{
		List<VRCUiContentButton> range = this.pickers.GetRange(index, count);
		foreach (VRCUiContentButton vrcuiContentButton in range)
		{
			vrcuiContentButton.transform.SetParent(null, false);
			vrcuiContentButton.ClearImage();
			SimplePool.Despawn(vrcuiContentButton.gameObject);
		}
		this.pickers.RemoveRange(index, count);
	}

	// Token: 0x060061C1 RID: 25025 RVA: 0x002258C8 File Offset: 0x00223CC8
	public void ToggleExtend()
	{
		if (this.extended)
		{
			this.Unexpand();
		}
		else
		{
			this.Expand(false);
		}
	}

	// Token: 0x060061C2 RID: 25026 RVA: 0x002258E8 File Offset: 0x00223CE8
	public void Expand(bool initial = false)
	{
		if (this.expandButton == null)
		{
			return;
		}
		if (!initial && this.pickers.Count < this.extendRows)
		{
			return;
		}
		if (!this.extended)
		{
			this.layoutElement.minHeight = this.expandedHeight;
			this.grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
			this.grid.constraintCount = this.extendRows;
			this.extended = true;
			this.numElementsPerPage = this.expandedCount;
			this.UpdateToggleSprite();
			this.Refresh();
		}
	}

	// Token: 0x060061C3 RID: 25027 RVA: 0x0022597C File Offset: 0x00223D7C
	public void Unexpand()
	{
		if (this.expandButton == null)
		{
			return;
		}
		if (this.extended)
		{
			this.layoutElement.minHeight = this.contractedHeight;
			this.grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
			this.grid.constraintCount = 1;
			this.extended = false;
			this.numElementsPerPage = this.collapsedCount;
			this.UpdateToggleSprite();
			if (this.pickers.Count > this.collapsedCount)
			{
				this.ClearListRange(this.collapsedCount, Mathf.Min(this.expandedCount, this.pickers.Count) - this.collapsedCount);
			}
		}
	}

	// Token: 0x060061C4 RID: 25028 RVA: 0x00225A28 File Offset: 0x00223E28
	private void UpdateToggleSprite()
	{
		if (this.expandButton == null)
		{
			return;
		}
		SpriteState spriteState = default(SpriteState);
		spriteState = this.expandButton.spriteState;
		if (this.extended)
		{
			this.expandButton.image.sprite = this.contractSprite;
			spriteState.highlightedSprite = this.expandSprite;
		}
		else
		{
			this.expandButton.image.sprite = this.expandSprite;
			spriteState.highlightedSprite = this.contractSprite;
		}
		this.expandButton.spriteState = spriteState;
	}

	// Token: 0x060061C5 RID: 25029 RVA: 0x00225AC0 File Offset: 0x00223EC0
	protected void Hide(bool shouldHide)
	{
		if (this.expandButton == null)
		{
			return;
		}
		if (shouldHide)
		{
			this.layoutElement.minHeight = 0f;
			this.expandButton.gameObject.SetActive(false);
		}
		else
		{
			this.layoutElement.minHeight = ((!this.extended) ? this.contractedHeight : this.expandedHeight);
			this.expandButton.gameObject.SetActive(true);
		}
	}

	// Token: 0x04004749 RID: 18249
	protected VRCUiContentButton focus;

	// Token: 0x0400474A RID: 18250
	protected Rect defaultRect;

	// Token: 0x0400474B RID: 18251
	public List<VRCUiContentButton> pickers = new List<VRCUiContentButton>();

	// Token: 0x0400474C RID: 18252
	public GameObject pickerPrefab;

	// Token: 0x0400474D RID: 18253
	public RectTransform content;

	// Token: 0x0400474E RID: 18254
	protected float spacing;

	// Token: 0x0400474F RID: 18255
	public ScrollRectEx scrollRect;

	// Token: 0x04004750 RID: 18256
	public float scrollPos;

	// Token: 0x04004751 RID: 18257
	[HideInInspector]
	public string searchQuery;

	// Token: 0x04004752 RID: 18258
	public int expandedCount = 60;

	// Token: 0x04004753 RID: 18259
	public int collapsedCount = 20;

	// Token: 0x04004754 RID: 18260
	public int extendRows = 3;

	// Token: 0x04004755 RID: 18261
	protected int numElementsPerPage;

	// Token: 0x04004756 RID: 18262
	protected GridLayoutGroup grid;

	// Token: 0x04004757 RID: 18263
	protected LayoutElement layoutElement;

	// Token: 0x04004758 RID: 18264
	private bool extended;

	// Token: 0x04004759 RID: 18265
	public Button expandButton;

	// Token: 0x0400475A RID: 18266
	public Sprite expandSprite;

	// Token: 0x0400475B RID: 18267
	public Sprite contractSprite;

	// Token: 0x0400475C RID: 18268
	public float expandedHeight = 630f;

	// Token: 0x0400475D RID: 18269
	public float contractedHeight = 250f;

	// Token: 0x0400475E RID: 18270
	private int lastNumPages;

	// Token: 0x0400475F RID: 18271
	private int numPages;

	// Token: 0x04004760 RID: 18272
	private int lastPage;

	// Token: 0x04004761 RID: 18273
	public int currentPage;

	// Token: 0x04004762 RID: 18274
	private bool isFetching;

	// Token: 0x04004763 RID: 18275
	public Dictionary<int, List<ApiModel>> paginatedLists;

	// Token: 0x04004764 RID: 18276
	public bool usePagination;

	// Token: 0x04004765 RID: 18277
	public bool hideWhenEmpty = true;

	// Token: 0x04004766 RID: 18278
	public RectTransform parentViewport;

	// Token: 0x04004767 RID: 18279
	public bool isOffScreen;

	// Token: 0x04004768 RID: 18280
	private bool wasOffScreenLastFrame;

	// Token: 0x04004769 RID: 18281
	public int myListSiblingIndex;

	// Token: 0x0400476A RID: 18282
	protected bool isSetup;
}
