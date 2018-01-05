using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005C2 RID: 1474
[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Popup List")]
public class UIPopupList : UIWidgetContainer
{
	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x06003102 RID: 12546 RVA: 0x000F05F3 File Offset: 0x000EE9F3
	// (set) Token: 0x06003103 RID: 12547 RVA: 0x000F062C File Offset: 0x000EEA2C
	public UnityEngine.Object ambigiousFont
	{
		get
		{
			if (this.trueTypeFont != null)
			{
				return this.trueTypeFont;
			}
			if (this.bitmapFont != null)
			{
				return this.bitmapFont;
			}
			return this.font;
		}
		set
		{
			if (value is Font)
			{
				this.trueTypeFont = (value as Font);
				this.bitmapFont = null;
				this.font = null;
			}
			else if (value is UIFont)
			{
				this.bitmapFont = (value as UIFont);
				this.trueTypeFont = null;
				this.font = null;
			}
		}
	}

	// Token: 0x17000753 RID: 1875
	// (get) Token: 0x06003104 RID: 12548 RVA: 0x000F0688 File Offset: 0x000EEA88
	// (set) Token: 0x06003105 RID: 12549 RVA: 0x000F0690 File Offset: 0x000EEA90
	[Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public UIPopupList.LegacyEvent onSelectionChange
	{
		get
		{
			return this.mLegacyEvent;
		}
		set
		{
			this.mLegacyEvent = value;
		}
	}

	// Token: 0x17000754 RID: 1876
	// (get) Token: 0x06003106 RID: 12550 RVA: 0x000F0699 File Offset: 0x000EEA99
	public bool isOpen
	{
		get
		{
			return this.mChild != null;
		}
	}

	// Token: 0x17000755 RID: 1877
	// (get) Token: 0x06003107 RID: 12551 RVA: 0x000F06A7 File Offset: 0x000EEAA7
	// (set) Token: 0x06003108 RID: 12552 RVA: 0x000F06AF File Offset: 0x000EEAAF
	public string value
	{
		get
		{
			return this.mSelectedItem;
		}
		set
		{
			this.mSelectedItem = value;
			if (this.mSelectedItem == null)
			{
				return;
			}
			if (this.mSelectedItem != null)
			{
				this.TriggerCallbacks();
			}
		}
	}

	// Token: 0x17000756 RID: 1878
	// (get) Token: 0x06003109 RID: 12553 RVA: 0x000F06D8 File Offset: 0x000EEAD8
	public object data
	{
		get
		{
			int num = this.items.IndexOf(this.mSelectedItem);
			return (num >= this.itemData.Count) ? null : this.itemData[num];
		}
	}

	// Token: 0x17000757 RID: 1879
	// (get) Token: 0x0600310A RID: 12554 RVA: 0x000F071A File Offset: 0x000EEB1A
	// (set) Token: 0x0600310B RID: 12555 RVA: 0x000F0722 File Offset: 0x000EEB22
	[Obsolete("Use 'value' instead")]
	public string selection
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	// Token: 0x17000758 RID: 1880
	// (get) Token: 0x0600310C RID: 12556 RVA: 0x000F072C File Offset: 0x000EEB2C
	// (set) Token: 0x0600310D RID: 12557 RVA: 0x000F0758 File Offset: 0x000EEB58
	private bool handleEvents
	{
		get
		{
			UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
			return component == null || !component.enabled;
		}
		set
		{
			UIKeyNavigation component = base.GetComponent<UIKeyNavigation>();
			if (component != null)
			{
				component.enabled = !value;
			}
		}
	}

	// Token: 0x17000759 RID: 1881
	// (get) Token: 0x0600310E RID: 12558 RVA: 0x000F0782 File Offset: 0x000EEB82
	private bool isValid
	{
		get
		{
			return this.bitmapFont != null || this.trueTypeFont != null;
		}
	}

	// Token: 0x1700075A RID: 1882
	// (get) Token: 0x0600310F RID: 12559 RVA: 0x000F07A4 File Offset: 0x000EEBA4
	private int activeFontSize
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? this.bitmapFont.defaultSize : this.fontSize;
		}
	}

	// Token: 0x1700075B RID: 1883
	// (get) Token: 0x06003110 RID: 12560 RVA: 0x000F07E0 File Offset: 0x000EEBE0
	private float activeFontScale
	{
		get
		{
			return (!(this.trueTypeFont != null) && !(this.bitmapFont == null)) ? ((float)this.fontSize / (float)this.bitmapFont.defaultSize) : 1f;
		}
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x000F082D File Offset: 0x000EEC2D
	public void Clear()
	{
		this.items.Clear();
		this.itemData.Clear();
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x000F0845 File Offset: 0x000EEC45
	public void AddItem(string text)
	{
		this.items.Add(text);
		this.itemData.Add(null);
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x000F085F File Offset: 0x000EEC5F
	public void AddItem(string text, object data)
	{
		this.items.Add(text);
		this.itemData.Add(data);
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x000F087C File Offset: 0x000EEC7C
	protected void TriggerCallbacks()
	{
		if (UIPopupList.current != this)
		{
			UIPopupList uipopupList = UIPopupList.current;
			UIPopupList.current = this;
			if (this.mLegacyEvent != null)
			{
				this.mLegacyEvent(this.mSelectedItem);
			}
			if (EventDelegate.IsValid(this.onChange))
			{
				EventDelegate.Execute(this.onChange);
			}
			else if (this.eventReceiver != null && !string.IsNullOrEmpty(this.functionName))
			{
				this.eventReceiver.SendMessage(this.functionName, this.mSelectedItem, SendMessageOptions.DontRequireReceiver);
			}
			UIPopupList.current = uipopupList;
		}
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x000F0920 File Offset: 0x000EED20
	private void OnEnable()
	{
		if (EventDelegate.IsValid(this.onChange))
		{
			this.eventReceiver = null;
			this.functionName = null;
		}
		if (this.font != null)
		{
			if (this.font.isDynamic)
			{
				this.trueTypeFont = this.font.dynamicFont;
				this.fontStyle = this.font.dynamicFontStyle;
				this.mUseDynamicFont = true;
			}
			else if (this.bitmapFont == null)
			{
				this.bitmapFont = this.font;
				this.mUseDynamicFont = false;
			}
			this.font = null;
		}
		if (this.textScale != 0f)
		{
			this.fontSize = ((!(this.bitmapFont != null)) ? 16 : Mathf.RoundToInt((float)this.bitmapFont.defaultSize * this.textScale));
			this.textScale = 0f;
		}
		if (this.trueTypeFont == null && this.bitmapFont != null && this.bitmapFont.isDynamic)
		{
			this.trueTypeFont = this.bitmapFont.dynamicFont;
			this.bitmapFont = null;
		}
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x000F0A60 File Offset: 0x000EEE60
	private void OnValidate()
	{
		Font x = this.trueTypeFont;
		UIFont uifont = this.bitmapFont;
		this.bitmapFont = null;
		this.trueTypeFont = null;
		if (x != null && (uifont == null || !this.mUseDynamicFont))
		{
			this.bitmapFont = null;
			this.trueTypeFont = x;
			this.mUseDynamicFont = true;
		}
		else if (uifont != null)
		{
			if (uifont.isDynamic)
			{
				this.trueTypeFont = uifont.dynamicFont;
				this.fontStyle = uifont.dynamicFontStyle;
				this.fontSize = uifont.defaultSize;
				this.mUseDynamicFont = true;
			}
			else
			{
				this.bitmapFont = uifont;
				this.mUseDynamicFont = false;
			}
		}
		else
		{
			this.trueTypeFont = x;
			this.mUseDynamicFont = true;
		}
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x000F0B30 File Offset: 0x000EEF30
	private void Start()
	{
		if (this.textLabel != null)
		{
			EventDelegate.Add(this.onChange, new EventDelegate.Callback(this.textLabel.SetCurrentSelection));
			this.textLabel = null;
		}
		if (Application.isPlaying)
		{
			if (string.IsNullOrEmpty(this.mSelectedItem))
			{
				if (this.items.Count > 0)
				{
					this.value = this.items[0];
				}
			}
			else
			{
				string value = this.mSelectedItem;
				this.mSelectedItem = null;
				this.value = value;
			}
		}
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x000F0BC9 File Offset: 0x000EEFC9
	private void OnLocalize()
	{
		if (this.isLocalized)
		{
			this.TriggerCallbacks();
		}
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x000F0BDC File Offset: 0x000EEFDC
	private void Highlight(UILabel lbl, bool instant)
	{
		if (this.mHighlight != null)
		{
			this.mHighlightedLabel = lbl;
			if (this.mHighlight.GetAtlasSprite() == null)
			{
				return;
			}
			Vector3 highlightPosition = this.GetHighlightPosition();
			if (instant || !this.isAnimated)
			{
				this.mHighlight.cachedTransform.localPosition = highlightPosition;
			}
			else
			{
				TweenPosition.Begin(this.mHighlight.gameObject, 0.1f, highlightPosition).method = UITweener.Method.EaseOut;
				if (!this.mTweening)
				{
					this.mTweening = true;
					base.StartCoroutine("UpdateTweenPosition");
				}
			}
		}
	}

	// Token: 0x0600311A RID: 12570 RVA: 0x000F0C7C File Offset: 0x000EF07C
	private Vector3 GetHighlightPosition()
	{
		if (this.mHighlightedLabel == null || this.mHighlight == null)
		{
			return Vector3.zero;
		}
		UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
		if (atlasSprite == null)
		{
			return Vector3.zero;
		}
		float pixelSize = this.atlas.pixelSize;
		float num = (float)atlasSprite.borderLeft * pixelSize;
		float y = (float)atlasSprite.borderTop * pixelSize;
		return this.mHighlightedLabel.cachedTransform.localPosition + new Vector3(-num, y, 1f);
	}

	// Token: 0x0600311B RID: 12571 RVA: 0x000F0D0C File Offset: 0x000EF10C
	private IEnumerator UpdateTweenPosition()
	{
		if (this.mHighlight != null && this.mHighlightedLabel != null)
		{
			TweenPosition tp = this.mHighlight.GetComponent<TweenPosition>();
			while (tp != null && tp.enabled)
			{
				tp.to = this.GetHighlightPosition();
				yield return null;
			}
		}
		this.mTweening = false;
		yield break;
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x000F0D28 File Offset: 0x000EF128
	private void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver)
		{
			UILabel component = go.GetComponent<UILabel>();
			this.Highlight(component, false);
		}
	}

	// Token: 0x0600311D RID: 12573 RVA: 0x000F0D4C File Offset: 0x000EF14C
	private void Select(UILabel lbl, bool instant)
	{
		this.Highlight(lbl, instant);
		UIEventListener component = lbl.gameObject.GetComponent<UIEventListener>();
		this.value = (component.parameter as string);
		UIPlaySound[] components = base.GetComponents<UIPlaySound>();
		int i = 0;
		int num = components.Length;
		while (i < num)
		{
			UIPlaySound uiplaySound = components[i];
			if (uiplaySound.trigger == UIPlaySound.Trigger.OnClick)
			{
				NGUITools.PlaySound(uiplaySound.audioClip, uiplaySound.volume, 1f);
			}
			i++;
		}
	}

	// Token: 0x0600311E RID: 12574 RVA: 0x000F0DC5 File Offset: 0x000EF1C5
	private void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed)
		{
			this.Select(go.GetComponent<UILabel>(), true);
		}
	}

	// Token: 0x0600311F RID: 12575 RVA: 0x000F0DDA File Offset: 0x000EF1DA
	private void OnItemClick(GameObject go)
	{
		this.Close();
	}

	// Token: 0x06003120 RID: 12576 RVA: 0x000F0DE4 File Offset: 0x000EF1E4
	private void OnKey(KeyCode key)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.handleEvents)
		{
			int num = this.mLabelList.IndexOf(this.mHighlightedLabel);
			if (num == -1)
			{
				num = 0;
			}
			if (key == KeyCode.UpArrow)
			{
				if (num > 0)
				{
					this.Select(this.mLabelList[num - 1], false);
				}
			}
			else if (key == KeyCode.DownArrow)
			{
				if (num + 1 < this.mLabelList.Count)
				{
					this.Select(this.mLabelList[num + 1], false);
				}
			}
			else if (key == KeyCode.Escape)
			{
				this.OnSelect(false);
			}
		}
	}

	// Token: 0x06003121 RID: 12577 RVA: 0x000F0EA9 File Offset: 0x000EF2A9
	private void OnDisable()
	{
		this.Close();
	}

	// Token: 0x06003122 RID: 12578 RVA: 0x000F0EB1 File Offset: 0x000EF2B1
	private void OnSelect(bool isSelected)
	{
		if (!isSelected)
		{
			this.Close();
		}
	}

	// Token: 0x06003123 RID: 12579 RVA: 0x000F0EC0 File Offset: 0x000EF2C0
	public void Close()
	{
		if (this.mChild != null)
		{
			this.mLabelList.Clear();
			this.handleEvents = false;
			if (this.isAnimated)
			{
				UIWidget[] componentsInChildren = this.mChild.GetComponentsInChildren<UIWidget>();
				int i = 0;
				int num = componentsInChildren.Length;
				while (i < num)
				{
					UIWidget uiwidget = componentsInChildren[i];
					Color color = uiwidget.color;
					color.a = 0f;
					TweenColor.Begin(uiwidget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
					i++;
				}
				Collider[] componentsInChildren2 = this.mChild.GetComponentsInChildren<Collider>();
				int j = 0;
				int num2 = componentsInChildren2.Length;
				while (j < num2)
				{
					componentsInChildren2[j].enabled = false;
					j++;
				}
				UnityEngine.Object.Destroy(this.mChild, 0.15f);
			}
			else
			{
				UnityEngine.Object.Destroy(this.mChild);
			}
			this.mBackground = null;
			this.mHighlight = null;
			this.mChild = null;
		}
	}

	// Token: 0x06003124 RID: 12580 RVA: 0x000F0FB8 File Offset: 0x000EF3B8
	private void AnimateColor(UIWidget widget)
	{
		Color color = widget.color;
		widget.color = new Color(color.r, color.g, color.b, 0f);
		TweenColor.Begin(widget.gameObject, 0.15f, color).method = UITweener.Method.EaseOut;
	}

	// Token: 0x06003125 RID: 12581 RVA: 0x000F1008 File Offset: 0x000EF408
	private void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 localPosition = widget.cachedTransform.localPosition;
		Vector3 localPosition2 = (!placeAbove) ? new Vector3(localPosition.x, 0f, localPosition.z) : new Vector3(localPosition.x, bottom, localPosition.z);
		widget.cachedTransform.localPosition = localPosition2;
		GameObject gameObject = widget.gameObject;
		TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
	}

	// Token: 0x06003126 RID: 12582 RVA: 0x000F1080 File Offset: 0x000EF480
	private void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject gameObject = widget.gameObject;
		Transform cachedTransform = widget.cachedTransform;
		float num = (float)this.activeFontSize * this.activeFontScale + this.mBgBorder * 2f;
		cachedTransform.localScale = new Vector3(1f, num / (float)widget.height, 1f);
		TweenScale.Begin(gameObject, 0.15f, Vector3.one).method = UITweener.Method.EaseOut;
		if (placeAbove)
		{
			Vector3 localPosition = cachedTransform.localPosition;
			cachedTransform.localPosition = new Vector3(localPosition.x, localPosition.y - (float)widget.height + num, localPosition.z);
			TweenPosition.Begin(gameObject, 0.15f, localPosition).method = UITweener.Method.EaseOut;
		}
	}

	// Token: 0x06003127 RID: 12583 RVA: 0x000F1134 File Offset: 0x000EF534
	private void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		this.AnimateColor(widget);
		this.AnimatePosition(widget, placeAbove, bottom);
	}

	// Token: 0x06003128 RID: 12584 RVA: 0x000F1146 File Offset: 0x000EF546
	private void OnClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick || this.openOn == UIPopupList.OpenOn.Manual)
		{
			return;
		}
		if (this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
		{
			return;
		}
		this.Show();
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x000F1180 File Offset: 0x000EF580
	private void OnDoubleClick()
	{
		if (this.openOn == UIPopupList.OpenOn.DoubleClick)
		{
			this.Show();
		}
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x000F1194 File Offset: 0x000EF594
	private IEnumerator CloseIfUnselected()
	{
		GameObject go = UICamera.selectedObject;
		do
		{
			yield return null;
		}
		while (!(UICamera.selectedObject != go));
		this.Close();
		yield break;
	}

	// Token: 0x0600312B RID: 12587 RVA: 0x000F11B0 File Offset: 0x000EF5B0
	public void Show()
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mChild == null && this.atlas != null && this.isValid && this.items.Count > 0)
		{
			this.mLabelList.Clear();
			if (this.mPanel == null)
			{
				this.mPanel = UIPanel.Find(base.transform);
				if (this.mPanel == null)
				{
					return;
				}
			}
			this.handleEvents = true;
			Transform transform = base.transform;
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform.parent, transform);
			this.mChild = new GameObject("Drop-down List");
			this.mChild.layer = base.gameObject.layer;
			Transform transform2 = this.mChild.transform;
			transform2.parent = transform.parent;
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			if (this.openOn == UIPopupList.OpenOn.Manual && UICamera.selectedObject != base.gameObject && bounds.size == Vector3.zero)
			{
				base.StopCoroutine("CloseIfUnselected");
				vector = transform2.parent.InverseTransformPoint(this.mPanel.anchorCamera.ScreenToWorldPoint(UICamera.lastTouchPosition));
				vector2 = vector;
				transform2.localPosition = vector;
				vector3 = transform2.position;
				base.StartCoroutine("CloseIfUnselected");
			}
			else
			{
				vector = bounds.min;
				vector2 = bounds.max;
				transform2.localPosition = vector;
				vector3 = transform.position;
			}
			transform2.localRotation = Quaternion.identity;
			transform2.localScale = Vector3.one;
			this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
			this.mBackground.pivot = UIWidget.Pivot.TopLeft;
			this.mBackground.depth = NGUITools.CalculateNextDepth(this.mPanel.gameObject);
			this.mBackground.color = this.backgroundColor;
			Vector4 border = this.mBackground.border;
			this.mBgBorder = border.y;
			this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
			this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
			this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
			this.mHighlight.color = this.highlightColor;
			UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
			if (atlasSprite == null)
			{
				return;
			}
			float num = (float)atlasSprite.borderTop;
			float num2 = (float)this.activeFontSize;
			float activeFontScale = this.activeFontScale;
			float num3 = num2 * activeFontScale;
			float num4 = 0f;
			float num5 = -this.padding.y;
			List<UILabel> list = new List<UILabel>();
			if (!this.items.Contains(this.mSelectedItem))
			{
				this.mSelectedItem = null;
			}
			int i = 0;
			int count = this.items.Count;
			while (i < count)
			{
				string text = this.items[i];
				UILabel uilabel = NGUITools.AddWidget<UILabel>(this.mChild);
				uilabel.name = i.ToString();
				uilabel.pivot = UIWidget.Pivot.TopLeft;
				uilabel.bitmapFont = this.bitmapFont;
				uilabel.trueTypeFont = this.trueTypeFont;
				uilabel.fontSize = this.fontSize;
				uilabel.fontStyle = this.fontStyle;
				uilabel.text = ((!this.isLocalized) ? text : Localization.Get(text));
				uilabel.color = this.textColor;
				uilabel.cachedTransform.localPosition = new Vector3(border.x + this.padding.x - uilabel.pivotOffset.x, num5, -1f);
				uilabel.overflowMethod = UILabel.Overflow.ResizeFreely;
				uilabel.alignment = this.alignment;
				list.Add(uilabel);
				num5 -= num3;
				num5 -= this.padding.y;
				num4 = Mathf.Max(num4, uilabel.printedSize.x);
				UIEventListener uieventListener = UIEventListener.Get(uilabel.gameObject);
				uieventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
				uieventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
				uieventListener.onClick = new UIEventListener.VoidDelegate(this.OnItemClick);
				uieventListener.parameter = text;
				if (this.mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(this.mSelectedItem)))
				{
					this.Highlight(uilabel, true);
				}
				this.mLabelList.Add(uilabel);
				i++;
			}
			num4 = Mathf.Max(num4, bounds.size.x * activeFontScale - (border.x + this.padding.x) * 2f);
			float num6 = num4;
			Vector3 vector4 = new Vector3(num6 * 0.5f, -num3 * 0.5f, 0f);
			Vector3 vector5 = new Vector3(num6, num3 + this.padding.y, 1f);
			int j = 0;
			int count2 = list.Count;
			while (j < count2)
			{
				UILabel uilabel2 = list[j];
				NGUITools.AddWidgetCollider(uilabel2.gameObject);
				uilabel2.autoResizeBoxCollider = false;
				BoxCollider component = uilabel2.GetComponent<BoxCollider>();
				if (component != null)
				{
					vector4.z = component.center.z;
					component.center = vector4;
					component.size = vector5;
				}
				else
				{
					BoxCollider2D component2 = uilabel2.GetComponent<BoxCollider2D>();
					component2.offset = vector4;
					component2.size = vector5;
				}
				j++;
			}
			int width = Mathf.RoundToInt(num4);
			num4 += (border.x + this.padding.x) * 2f;
			num5 -= border.y;
			this.mBackground.width = Mathf.RoundToInt(num4);
			this.mBackground.height = Mathf.RoundToInt(-num5 + border.y);
			int k = 0;
			int count3 = list.Count;
			while (k < count3)
			{
				UILabel uilabel3 = list[k];
				uilabel3.overflowMethod = UILabel.Overflow.ShrinkContent;
				uilabel3.width = width;
				k++;
			}
			float num7 = 2f * this.atlas.pixelSize;
			float f = num4 - (border.x + this.padding.x) * 2f + (float)atlasSprite.borderLeft * num7;
			float f2 = num3 + num * num7;
			this.mHighlight.width = Mathf.RoundToInt(f);
			this.mHighlight.height = Mathf.RoundToInt(f2);
			bool flag = this.position == UIPopupList.Position.Above;
			if (this.position == UIPopupList.Position.Auto)
			{
				UICamera uicamera = UICamera.FindCameraForLayer((UICamera.selectedObject ?? base.gameObject).layer);
				if (uicamera != null)
				{
					flag = (uicamera.cachedCamera.WorldToViewportPoint(vector3).y < 0.5f);
				}
			}
			if (this.isAnimated)
			{
				float bottom = num5 + num3;
				this.Animate(this.mHighlight, flag, bottom);
				int l = 0;
				int count4 = list.Count;
				while (l < count4)
				{
					this.Animate(list[l], flag, bottom);
					l++;
				}
				this.AnimateColor(this.mBackground);
				this.AnimateScale(this.mBackground, flag, bottom);
			}
			if (flag)
			{
				transform2.localPosition = new Vector3(vector.x, vector2.y - num5 - border.y, vector.z);
			}
			vector = transform2.localPosition;
			vector2.x = vector.x + (float)this.mBackground.width;
			vector2.y = vector.y + (float)this.mBackground.height;
			vector2.z = vector.z;
		}
		else
		{
			this.OnSelect(false);
		}
	}

	// Token: 0x04001B8F RID: 7055
	public static UIPopupList current;

	// Token: 0x04001B90 RID: 7056
	private const float animSpeed = 0.15f;

	// Token: 0x04001B91 RID: 7057
	public UIAtlas atlas;

	// Token: 0x04001B92 RID: 7058
	public UIFont bitmapFont;

	// Token: 0x04001B93 RID: 7059
	public Font trueTypeFont;

	// Token: 0x04001B94 RID: 7060
	public int fontSize = 16;

	// Token: 0x04001B95 RID: 7061
	public FontStyle fontStyle;

	// Token: 0x04001B96 RID: 7062
	public string backgroundSprite;

	// Token: 0x04001B97 RID: 7063
	public string highlightSprite;

	// Token: 0x04001B98 RID: 7064
	public UIPopupList.Position position;

	// Token: 0x04001B99 RID: 7065
	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	// Token: 0x04001B9A RID: 7066
	public List<string> items = new List<string>();

	// Token: 0x04001B9B RID: 7067
	public List<object> itemData = new List<object>();

	// Token: 0x04001B9C RID: 7068
	public Vector2 padding = new Vector3(4f, 4f);

	// Token: 0x04001B9D RID: 7069
	public Color textColor = Color.white;

	// Token: 0x04001B9E RID: 7070
	public Color backgroundColor = Color.white;

	// Token: 0x04001B9F RID: 7071
	public Color highlightColor = new Color(0.882352948f, 0.784313738f, 0.5882353f, 1f);

	// Token: 0x04001BA0 RID: 7072
	public bool isAnimated = true;

	// Token: 0x04001BA1 RID: 7073
	public bool isLocalized;

	// Token: 0x04001BA2 RID: 7074
	public UIPopupList.OpenOn openOn;

	// Token: 0x04001BA3 RID: 7075
	public List<EventDelegate> onChange = new List<EventDelegate>();

	// Token: 0x04001BA4 RID: 7076
	[HideInInspector]
	[SerializeField]
	private string mSelectedItem;

	// Token: 0x04001BA5 RID: 7077
	[HideInInspector]
	[SerializeField]
	private UIPanel mPanel;

	// Token: 0x04001BA6 RID: 7078
	[HideInInspector]
	[SerializeField]
	private GameObject mChild;

	// Token: 0x04001BA7 RID: 7079
	[HideInInspector]
	[SerializeField]
	private UISprite mBackground;

	// Token: 0x04001BA8 RID: 7080
	[HideInInspector]
	[SerializeField]
	private UISprite mHighlight;

	// Token: 0x04001BA9 RID: 7081
	[HideInInspector]
	[SerializeField]
	private UILabel mHighlightedLabel;

	// Token: 0x04001BAA RID: 7082
	[HideInInspector]
	[SerializeField]
	private List<UILabel> mLabelList = new List<UILabel>();

	// Token: 0x04001BAB RID: 7083
	[HideInInspector]
	[SerializeField]
	private float mBgBorder;

	// Token: 0x04001BAC RID: 7084
	[HideInInspector]
	[SerializeField]
	private GameObject eventReceiver;

	// Token: 0x04001BAD RID: 7085
	[HideInInspector]
	[SerializeField]
	private string functionName = "OnSelectionChange";

	// Token: 0x04001BAE RID: 7086
	[HideInInspector]
	[SerializeField]
	private float textScale;

	// Token: 0x04001BAF RID: 7087
	[HideInInspector]
	[SerializeField]
	private UIFont font;

	// Token: 0x04001BB0 RID: 7088
	[HideInInspector]
	[SerializeField]
	private UILabel textLabel;

	// Token: 0x04001BB1 RID: 7089
	private UIPopupList.LegacyEvent mLegacyEvent;

	// Token: 0x04001BB2 RID: 7090
	private bool mUseDynamicFont;

	// Token: 0x04001BB3 RID: 7091
	private bool mTweening;

	// Token: 0x020005C3 RID: 1475
	public enum Position
	{
		// Token: 0x04001BB5 RID: 7093
		Auto,
		// Token: 0x04001BB6 RID: 7094
		Above,
		// Token: 0x04001BB7 RID: 7095
		Below
	}

	// Token: 0x020005C4 RID: 1476
	public enum OpenOn
	{
		// Token: 0x04001BB9 RID: 7097
		ClickOrTap,
		// Token: 0x04001BBA RID: 7098
		RightClick,
		// Token: 0x04001BBB RID: 7099
		DoubleClick,
		// Token: 0x04001BBC RID: 7100
		Manual
	}

	// Token: 0x020005C5 RID: 1477
	// (Invoke) Token: 0x0600312D RID: 12589
	public delegate void LegacyEvent(string val);
}
