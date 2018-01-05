using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C1D RID: 3101
public class GestureMenu : MonoBehaviour
{
	// Token: 0x06005FFC RID: 24572 RVA: 0x0021C438 File Offset: 0x0021A838
	private void Awake()
	{
		this.root = base.transform.Find("Gestures").gameObject;
		this.piebase = this.root.transform.Find("Pie").GetComponent<Image>();
		this.icons = new Image[7];
		this.icons[0] = this.root.transform.Find("fist").GetComponent<Image>();
		this.icons[1] = this.root.transform.Find("point").GetComponent<Image>();
		this.icons[2] = this.root.transform.Find("peace").GetComponent<Image>();
		this.icons[3] = this.root.transform.Find("rocknroll").GetComponent<Image>();
		this.icons[4] = this.root.transform.Find("palm").GetComponent<Image>();
		this.icons[5] = this.root.transform.Find("gun").GetComponent<Image>();
		this.icons[6] = this.root.transform.Find("thumb").GetComponent<Image>();
		if (this.rightHand)
		{
			this._inputX = VRCInputManager.FindInput("PieMenuRightX");
			this._inputY = VRCInputManager.FindInput("PieMenuRightY");
			this._gestureSelect = VRCInputManager.FindInput("PieMenuRightShow");
			this._gestureActivate = VRCInputManager.FindInput("PieMenuRightActivate");
		}
		else
		{
			this._inputX = VRCInputManager.FindInput("PieMenuLeftX");
			this._inputY = VRCInputManager.FindInput("PieMenuLeftY");
			this._gestureSelect = VRCInputManager.FindInput("PieMenuLeftShow");
			this._gestureActivate = VRCInputManager.FindInput("PieMenuLeftActivate");
		}
		this.SelectItem(0);
		this.root.SetActive(false);
	}

	// Token: 0x06005FFD RID: 24573 RVA: 0x0021C61D File Offset: 0x0021AA1D
	public void Initialize(Transform w, Transform e)
	{
		this._wrist = w;
		this._eye = e;
		this._isAttached = true;
	}

	// Token: 0x06005FFE RID: 24574 RVA: 0x0021C634 File Offset: 0x0021AA34
	private void SelectItem(int n)
	{
		if (n == 0)
		{
			this.piebase.rectTransform.localEulerAngles = Vector3.zero;
			this.piebase.sprite = this.center;
		}
		else
		{
			float num = -60f * (float)(n - 1);
			if (num >= -180f)
			{
				num += 360f;
			}
			this.piebase.rectTransform.localEulerAngles = new Vector3(0f, 0f, num);
			this.piebase.sprite = this.slice;
		}
		this.icons[this._currentSelect].color = Color.white;
		this.icons[n].color = this.hiliteColor;
		this._currentSelect = n;
	}

	// Token: 0x06005FFF RID: 24575 RVA: 0x0021C6F2 File Offset: 0x0021AAF2
	private void SetVisible(bool flag)
	{
		this._isVisible = flag;
		this.root.SetActive(this._isVisible);
	}

	// Token: 0x06006000 RID: 24576 RVA: 0x0021C70C File Offset: 0x0021AB0C
	public bool Input(out int val)
	{
		val = -1;
		if (this._gestureSelect.button)
		{
			this.SetVisible(true);
			Vector2 axis;
			axis.x = this._inputX.axis;
			axis.y = this._inputY.axis;
			int num = this.DualAxisToPieMenu(axis, 6);
			this.SelectItem(num);
			val = num;
		}
		else
		{
			this.SetVisible(false);
		}
		return this._gestureActivate.button;
	}

	// Token: 0x06006001 RID: 24577 RVA: 0x0021C794 File Offset: 0x0021AB94
	private int DualAxisToPieMenu(Vector2 axis, int slices)
	{
		int result = 0;
		float num = 6.28318548f;
		if (axis.magnitude > 0.5f)
		{
			float num2 = (float)slices;
			float num3 = -1f;
			if (this.rightHand)
			{
				num3 = 1f;
			}
			float num4 = Mathf.Atan2(-axis.y, axis.x * num3);
			num4 += num / 3f;
			if (num4 < 0f)
			{
				num4 += num;
			}
			if (num4 >= num)
			{
				num4 -= num;
			}
			float num5 = 1f / num2 + Mathf.Floor(num2 * num4 / num) / num2;
			result = Mathf.FloorToInt(num5 * num2);
		}
		return result;
	}

	// Token: 0x06006002 RID: 24578 RVA: 0x0021C83C File Offset: 0x0021AC3C
	private void Update()
	{
		if (!this._isAttached)
		{
			return;
		}
		if (this._wrist != null)
		{
			Vector3 position = this._wrist.position + (this._eye.position - this._wrist.position) / 4.5f;
			Quaternion rotation = this._wrist.rotation;
			base.transform.position = position;
			base.transform.rotation = rotation;
			base.transform.localPosition += this.menuPosOffset;
			base.transform.localRotation *= Quaternion.Euler(this.menuRotOffset);
		}
	}

	// Token: 0x040045A5 RID: 17829
	public bool rightHand;

	// Token: 0x040045A6 RID: 17830
	public Sprite center;

	// Token: 0x040045A7 RID: 17831
	public Sprite slice;

	// Token: 0x040045A8 RID: 17832
	public Color hiliteColor;

	// Token: 0x040045A9 RID: 17833
	private GameObject root;

	// Token: 0x040045AA RID: 17834
	private Image piebase;

	// Token: 0x040045AB RID: 17835
	private Image[] icons;

	// Token: 0x040045AC RID: 17836
	private Transform _wrist;

	// Token: 0x040045AD RID: 17837
	private Transform _eye;

	// Token: 0x040045AE RID: 17838
	private bool _isVisible;

	// Token: 0x040045AF RID: 17839
	private bool _isAttached;

	// Token: 0x040045B0 RID: 17840
	private int _currentSelect;

	// Token: 0x040045B1 RID: 17841
	private VRCInput _inputX;

	// Token: 0x040045B2 RID: 17842
	private VRCInput _inputY;

	// Token: 0x040045B3 RID: 17843
	private VRCInput _gestureActivate;

	// Token: 0x040045B4 RID: 17844
	private VRCInput _gestureSelect;

	// Token: 0x040045B5 RID: 17845
	public Vector3 menuRotOffset = new Vector3(0f, 90f, 90f);

	// Token: 0x040045B6 RID: 17846
	public Vector3 menuPosOffset;
}
