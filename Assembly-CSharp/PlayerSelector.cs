using System;
using UnityEngine;

// Token: 0x02000C28 RID: 3112
public class PlayerSelector : MonoBehaviour
{
	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x06006082 RID: 24706 RVA: 0x0021FD21 File Offset: 0x0021E121
	public bool IsShowing
	{
		get
		{
			return this._isShowing && base.gameObject.activeInHierarchy;
		}
	}

	// Token: 0x06006083 RID: 24707 RVA: 0x0021FD3C File Offset: 0x0021E13C
	private void Start()
	{
		this.camMount = base.transform.parent.Find("CameraMount");
		this.render = base.GetComponent<Renderer>();
	}

	// Token: 0x06006084 RID: 24708 RVA: 0x0021FD68 File Offset: 0x0021E168
	private void Update()
	{
		if (this.camMount != null)
		{
			float num = 0.45f * this.camMount.localPosition.y / VRCTracking.DefaultEyeHeight;
			if (!Mathf.Approximately(num, this.lastScale))
			{
				base.transform.localPosition = new Vector3(0f, num * 2f, -num / 3f);
				base.transform.localScale = new Vector3(num * 0.889f, num, num * 0.889f);
				this.lastScale = num;
			}
		}
	}

	// Token: 0x06006085 RID: 24709 RVA: 0x0021FE04 File Offset: 0x0021E204
	private void Show(bool selected)
	{
		if (this.render != null)
		{
			this.render.enabled = true;
			Color color = this.render.material.color;
			if (selected)
			{
				color.a = this.alphaSelect;
			}
			else
			{
				color.a = this.alphaHover;
			}
			this.render.material.color = color;
		}
		this._isShowing = true;
	}

	// Token: 0x06006086 RID: 24710 RVA: 0x0021FE7C File Offset: 0x0021E27C
	private void Hide()
	{
		if (this.render != null)
		{
			this.render.enabled = false;
		}
		this._isShowing = false;
	}

	// Token: 0x06006087 RID: 24711 RVA: 0x0021FEA2 File Offset: 0x0021E2A2
	private void RefreshActiveState()
	{
		base.gameObject.SetActive(this._isAvatarVisible && !this._isPlayerBlocked);
	}

	// Token: 0x06006088 RID: 24712 RVA: 0x0021FEC6 File Offset: 0x0021E2C6
	public void Hover(bool flag)
	{
		if (!this.selected)
		{
			if (flag)
			{
				this.Show(false);
			}
			else
			{
				this.Hide();
			}
		}
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x0021FEEB File Offset: 0x0021E2EB
	public void Select(bool flag)
	{
		if (flag)
		{
			this.selected = true;
			this.Show(true);
		}
		else
		{
			this.Hide();
			this.selected = false;
		}
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x0021FF14 File Offset: 0x0021E314
	public void NotifyAvatarIsVisible(bool isVisible)
	{
		bool flag = this._isAvatarVisible != isVisible;
		this._isAvatarVisible = isVisible;
		if (flag)
		{
			this.RefreshActiveState();
		}
	}

	// Token: 0x0600608B RID: 24715 RVA: 0x0021FF44 File Offset: 0x0021E344
	public void NotifyPlayerIsBlocked(bool isBlocked)
	{
		bool flag = this._isPlayerBlocked != isBlocked;
		this._isPlayerBlocked = isBlocked;
		if (flag)
		{
			this.RefreshActiveState();
		}
	}

	// Token: 0x0400462E RID: 17966
	private float lastScale = 0.45f;

	// Token: 0x0400462F RID: 17967
	private float alphaHover = 0.5f;

	// Token: 0x04004630 RID: 17968
	private float alphaSelect = 1f;

	// Token: 0x04004631 RID: 17969
	private Transform camMount;

	// Token: 0x04004632 RID: 17970
	private Renderer render;

	// Token: 0x04004633 RID: 17971
	private bool selected;

	// Token: 0x04004634 RID: 17972
	private bool _isShowing;

	// Token: 0x04004635 RID: 17973
	private bool _isAvatarVisible = true;

	// Token: 0x04004636 RID: 17974
	private bool _isPlayerBlocked;
}
