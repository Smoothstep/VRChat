using System;
using UnityEngine;

// Token: 0x02000604 RID: 1540
[AddComponentMenu("NGUI/Internal/Event Listener")]
public class UIEventListener : MonoBehaviour
{
	// Token: 0x0600337A RID: 13178 RVA: 0x00107E9E File Offset: 0x0010629E
	private void OnSubmit()
	{
		if (this.onSubmit != null)
		{
			this.onSubmit(base.gameObject);
		}
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x00107EBC File Offset: 0x001062BC
	private void OnClick()
	{
		if (this.onClick != null)
		{
			this.onClick(base.gameObject);
		}
	}

	// Token: 0x0600337C RID: 13180 RVA: 0x00107EDA File Offset: 0x001062DA
	private void OnDoubleClick()
	{
		if (this.onDoubleClick != null)
		{
			this.onDoubleClick(base.gameObject);
		}
	}

	// Token: 0x0600337D RID: 13181 RVA: 0x00107EF8 File Offset: 0x001062F8
	private void OnHover(bool isOver)
	{
		if (this.onHover != null)
		{
			this.onHover(base.gameObject, isOver);
		}
	}

	// Token: 0x0600337E RID: 13182 RVA: 0x00107F17 File Offset: 0x00106317
	private void OnPress(bool isPressed)
	{
		if (this.onPress != null)
		{
			this.onPress(base.gameObject, isPressed);
		}
	}

	// Token: 0x0600337F RID: 13183 RVA: 0x00107F36 File Offset: 0x00106336
	private void OnSelect(bool selected)
	{
		if (this.onSelect != null)
		{
			this.onSelect(base.gameObject, selected);
		}
	}

	// Token: 0x06003380 RID: 13184 RVA: 0x00107F55 File Offset: 0x00106355
	private void OnScroll(float delta)
	{
		if (this.onScroll != null)
		{
			this.onScroll(base.gameObject, delta);
		}
	}

	// Token: 0x06003381 RID: 13185 RVA: 0x00107F74 File Offset: 0x00106374
	private void OnDragStart()
	{
		if (this.onDragStart != null)
		{
			this.onDragStart(base.gameObject);
		}
	}

	// Token: 0x06003382 RID: 13186 RVA: 0x00107F92 File Offset: 0x00106392
	private void OnDrag(Vector2 delta)
	{
		if (this.onDrag != null)
		{
			this.onDrag(base.gameObject, delta);
		}
	}

	// Token: 0x06003383 RID: 13187 RVA: 0x00107FB1 File Offset: 0x001063B1
	private void OnDragOver()
	{
		if (this.onDragOver != null)
		{
			this.onDragOver(base.gameObject);
		}
	}

	// Token: 0x06003384 RID: 13188 RVA: 0x00107FCF File Offset: 0x001063CF
	private void OnDragOut()
	{
		if (this.onDragOut != null)
		{
			this.onDragOut(base.gameObject);
		}
	}

	// Token: 0x06003385 RID: 13189 RVA: 0x00107FED File Offset: 0x001063ED
	private void OnDragEnd()
	{
		if (this.onDragEnd != null)
		{
			this.onDragEnd(base.gameObject);
		}
	}

	// Token: 0x06003386 RID: 13190 RVA: 0x0010800B File Offset: 0x0010640B
	private void OnDrop(GameObject go)
	{
		if (this.onDrop != null)
		{
			this.onDrop(base.gameObject, go);
		}
	}

	// Token: 0x06003387 RID: 13191 RVA: 0x0010802A File Offset: 0x0010642A
	private void OnKey(KeyCode key)
	{
		if (this.onKey != null)
		{
			this.onKey(base.gameObject, key);
		}
	}

	// Token: 0x06003388 RID: 13192 RVA: 0x00108049 File Offset: 0x00106449
	private void OnTooltip(bool show)
	{
		if (this.onTooltip != null)
		{
			this.onTooltip(base.gameObject, show);
		}
	}

	// Token: 0x06003389 RID: 13193 RVA: 0x00108068 File Offset: 0x00106468
	public static UIEventListener Get(GameObject go)
	{
		UIEventListener uieventListener = go.GetComponent<UIEventListener>();
		if (uieventListener == null)
		{
			uieventListener = go.AddComponent<UIEventListener>();
		}
		return uieventListener;
	}

	// Token: 0x04001D53 RID: 7507
	public object parameter;

	// Token: 0x04001D54 RID: 7508
	public UIEventListener.VoidDelegate onSubmit;

	// Token: 0x04001D55 RID: 7509
	public UIEventListener.VoidDelegate onClick;

	// Token: 0x04001D56 RID: 7510
	public UIEventListener.VoidDelegate onDoubleClick;

	// Token: 0x04001D57 RID: 7511
	public UIEventListener.BoolDelegate onHover;

	// Token: 0x04001D58 RID: 7512
	public UIEventListener.BoolDelegate onPress;

	// Token: 0x04001D59 RID: 7513
	public UIEventListener.BoolDelegate onSelect;

	// Token: 0x04001D5A RID: 7514
	public UIEventListener.FloatDelegate onScroll;

	// Token: 0x04001D5B RID: 7515
	public UIEventListener.VoidDelegate onDragStart;

	// Token: 0x04001D5C RID: 7516
	public UIEventListener.VectorDelegate onDrag;

	// Token: 0x04001D5D RID: 7517
	public UIEventListener.VoidDelegate onDragOver;

	// Token: 0x04001D5E RID: 7518
	public UIEventListener.VoidDelegate onDragOut;

	// Token: 0x04001D5F RID: 7519
	public UIEventListener.VoidDelegate onDragEnd;

	// Token: 0x04001D60 RID: 7520
	public UIEventListener.ObjectDelegate onDrop;

	// Token: 0x04001D61 RID: 7521
	public UIEventListener.KeyCodeDelegate onKey;

	// Token: 0x04001D62 RID: 7522
	public UIEventListener.BoolDelegate onTooltip;

	// Token: 0x02000605 RID: 1541
	// (Invoke) Token: 0x0600338B RID: 13195
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x02000606 RID: 1542
	// (Invoke) Token: 0x0600338F RID: 13199
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x02000607 RID: 1543
	// (Invoke) Token: 0x06003393 RID: 13203
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x02000608 RID: 1544
	// (Invoke) Token: 0x06003397 RID: 13207
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x02000609 RID: 1545
	// (Invoke) Token: 0x0600339B RID: 13211
	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	// Token: 0x0200060A RID: 1546
	// (Invoke) Token: 0x0600339F RID: 13215
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);
}
