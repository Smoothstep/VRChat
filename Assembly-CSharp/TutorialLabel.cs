using System;
using UnityEngine;

// Token: 0x02000B02 RID: 2818
public class TutorialLabel : MonoBehaviour
{
	// Token: 0x17000C4E RID: 3150
	// (get) Token: 0x06005528 RID: 21800 RVA: 0x001D58E8 File Offset: 0x001D3CE8
	public string TextString
	{
		get
		{
			TextMesh textMesh = (!(this.Text != null)) ? null : this.Text.GetComponent<TextMesh>();
			return (!(textMesh != null)) ? "<none>" : textMesh.text;
		}
	}

	// Token: 0x17000C4F RID: 3151
	// (get) Token: 0x06005529 RID: 21801 RVA: 0x001D5934 File Offset: 0x001D3D34
	public string TextSecondaryString
	{
		get
		{
			TextMesh textMesh = (!(this.TextSecondary != null)) ? null : this.TextSecondary.GetComponent<TextMesh>();
			return (!(textMesh != null)) ? "<none>" : textMesh.text;
		}
	}

	// Token: 0x17000C50 RID: 3152
	// (get) Token: 0x0600552A RID: 21802 RVA: 0x001D5980 File Offset: 0x001D3D80
	public bool IsActive
	{
		get
		{
			return this._isActive;
		}
	}

	// Token: 0x17000C51 RID: 3153
	// (get) Token: 0x0600552B RID: 21803 RVA: 0x001D5988 File Offset: 0x001D3D88
	public bool IsVisible
	{
		get
		{
			return this.IsActive || !Mathf.Approximately(this._currentAlpha, 0f);
		}
	}

	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x0600552C RID: 21804 RVA: 0x001D59AB File Offset: 0x001D3DAB
	// (set) Token: 0x0600552D RID: 21805 RVA: 0x001D59B3 File Offset: 0x001D3DB3
	public bool IsFloatingLabel { get; set; }

	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x0600552E RID: 21806 RVA: 0x001D59BC File Offset: 0x001D3DBC
	// (set) Token: 0x0600552F RID: 21807 RVA: 0x001D59C4 File Offset: 0x001D3DC4
	public bool InvertLabelAlignment { get; set; }

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x06005530 RID: 21808 RVA: 0x001D59CD File Offset: 0x001D3DCD
	// (set) Token: 0x06005531 RID: 21809 RVA: 0x001D59D5 File Offset: 0x001D3DD5
	public bool IsAttachedToView { get; set; }

	// Token: 0x06005532 RID: 21810 RVA: 0x001D59E0 File Offset: 0x001D3DE0
	public void Init()
	{
		this._textInitialOffsetX = this.Text.transform.localPosition.x;
		this._iconInitialOffsetX = this.Icon.transform.localPosition.x;
	}

	// Token: 0x06005533 RID: 21811 RVA: 0x001D5A2C File Offset: 0x001D3E2C
	public void SetTargetObject(Transform targetObject, AttachMode attachMode, Vector3 tetherPos, Vector3 labelPos, bool isFloatingLabel)
	{
		this._tetherLength = (labelPos - tetherPos).magnitude;
		base.transform.position = tetherPos;
		if (!isFloatingLabel)
		{
			base.transform.LookAt(labelPos, Vector3.up);
		}
		this.UpdateScale();
		if (!isFloatingLabel)
		{
			AttachToTransform component = base.GetComponent<AttachToTransform>();
			if (component != null)
			{
				component.Parent = targetObject;
				component.FollowPosition = true;
				component.FollowRotation = (attachMode == AttachMode.PositionAndRotation);
				component.ResetLocalTransform();
			}
		}
		this.SetAlpha(0f);
	}

	// Token: 0x06005534 RID: 21812 RVA: 0x001D5AC0 File Offset: 0x001D3EC0
	public void SetText(string text, string textSecondary)
	{
		this.Text.GetComponent<TextMesh>().text = text;
		this.Text.GetComponent<Renderer>().enabled = !string.IsNullOrEmpty(text);
		this.TextSecondary.GetComponent<TextMesh>().text = textSecondary;
		this.TextSecondary.GetComponent<Renderer>().enabled = !string.IsNullOrEmpty(textSecondary);
	}

	// Token: 0x06005535 RID: 21813 RVA: 0x001D5B24 File Offset: 0x001D3F24
	public void SetIcon(ControllerActionUI action, ControllerActionUI actionSecondary)
	{
		if (action != ControllerActionUI.None)
		{
			Renderer component = this.Icon.GetComponent<Renderer>();
			component.material = TutorialManager.Instance.GetIconMaterialForAction(action);
			component.enabled = true;
		}
		else
		{
			this.Icon.GetComponent<Renderer>().enabled = false;
		}
		if (actionSecondary != ControllerActionUI.None)
		{
			Renderer component2 = this.IconSecondary.GetComponent<Renderer>();
			component2.material = TutorialManager.Instance.GetIconMaterialForAction(actionSecondary);
			component2.enabled = true;
		}
		else
		{
			this.IconSecondary.GetComponent<Renderer>().enabled = false;
		}
	}

	// Token: 0x06005536 RID: 21814 RVA: 0x001D5BB4 File Offset: 0x001D3FB4
	public void RefreshComponentPositions()
	{
		this.Tether.SetActive(!this.IsFloatingLabel);
		this.Icon.transform.localPosition = new Vector3(this._iconInitialOffsetX, this.Icon.transform.localPosition.y, this.Icon.transform.localPosition.z);
		this.Text.transform.localPosition = new Vector3(this._textInitialOffsetX, this.Text.transform.localPosition.y, this.Text.transform.localPosition.z);
		this.IconSecondary.transform.localPosition = new Vector3(this._iconInitialOffsetX, this.IconSecondary.transform.localPosition.y, this.IconSecondary.transform.localPosition.z);
		this.TextSecondary.transform.localPosition = new Vector3(this._textInitialOffsetX, this.TextSecondary.transform.localPosition.y, this.TextSecondary.transform.localPosition.z);
		bool enabled = this.Icon.GetComponent<Renderer>().enabled;
		bool enabled2 = this.IconSecondary.GetComponent<Renderer>().enabled;
		if (!enabled)
		{
			this.Text.transform.localPosition = new Vector3(this._iconInitialOffsetX + this.Icon.transform.localScale.x / 2f, this.Text.transform.localPosition.y, this.Text.transform.localPosition.z);
		}
		if (!enabled2)
		{
			this.TextSecondary.transform.localPosition = new Vector3(this._iconInitialOffsetX + this.IconSecondary.transform.localScale.x / 2f, this.TextSecondary.transform.localPosition.y, this.TextSecondary.transform.localPosition.z);
		}
		float a = ((!enabled) ? 0f : this.Icon.transform.localScale.x) + this.GetTextMeshWidth(this.Text.GetComponent<TextMesh>());
		float b = ((!enabled2) ? 0f : this.IconSecondary.transform.localScale.x) + this.GetTextMeshWidth(this.TextSecondary.GetComponent<TextMesh>());
		float num = Mathf.Max(a, b);
		if (this.IsFloatingLabel)
		{
			Vector3 position = TutorialLabel.CalculateFloatingLabelPosition();
			base.transform.position = position;
			float num2 = num / 2f;
			this.Icon.transform.localPosition = new Vector3(this.Icon.transform.localPosition.x + num2, this.Icon.transform.localPosition.y, this.Icon.transform.localPosition.z);
			this.Text.transform.localPosition = new Vector3(this.Text.transform.localPosition.x + num2, this.Text.transform.localPosition.y, this.Text.transform.localPosition.z);
			this.IconSecondary.transform.localPosition = new Vector3(this.IconSecondary.transform.localPosition.x + num2, this.IconSecondary.transform.localPosition.y, this.IconSecondary.transform.localPosition.z);
			this.TextSecondary.transform.localPosition = new Vector3(this.TextSecondary.transform.localPosition.x + num2, this.TextSecondary.transform.localPosition.y, this.TextSecondary.transform.localPosition.z);
		}
		else if (this.InvertLabelAlignment)
		{
			this.Icon.transform.localPosition = new Vector3(this.Icon.transform.localPosition.x + num, this.Icon.transform.localPosition.y, this.Icon.transform.localPosition.z);
			this.Text.transform.localPosition = new Vector3(this.Text.transform.localPosition.x + num, this.Text.transform.localPosition.y, this.Text.transform.localPosition.z);
			this.IconSecondary.transform.localPosition = new Vector3(this.IconSecondary.transform.localPosition.x + num, this.IconSecondary.transform.localPosition.y, this.IconSecondary.transform.localPosition.z);
			this.TextSecondary.transform.localPosition = new Vector3(this.TextSecondary.transform.localPosition.x + num, this.TextSecondary.transform.localPosition.y, this.TextSecondary.transform.localPosition.z);
		}
		base.GetComponentInChildren<FaceCamera>().ForceUpdate();
	}

	// Token: 0x06005537 RID: 21815 RVA: 0x001D61F4 File Offset: 0x001D45F4
	private float GetTextMeshWidth(TextMesh mesh)
	{
		if (mesh == null)
		{
			return 0f;
		}
		float num = 0f;
		foreach (char ch in mesh.text)
		{
			CharacterInfo characterInfo;
			if (mesh.font.GetCharacterInfo(ch, out characterInfo, mesh.fontSize, mesh.fontStyle))
			{
				num += (float)characterInfo.advance;
			}
		}
		return num * mesh.characterSize * 0.1f * mesh.transform.localScale.x;
	}

	// Token: 0x06005538 RID: 21816 RVA: 0x001D628C File Offset: 0x001D468C
	public void SetTextScaleOverride(float s)
	{
		this._textScaleOverride = s;
		this.UpdateScale();
	}

	// Token: 0x06005539 RID: 21817 RVA: 0x001D629B File Offset: 0x001D469B
	public void Activate()
	{
		this._isActive = true;
	}

	// Token: 0x0600553A RID: 21818 RVA: 0x001D62A4 File Offset: 0x001D46A4
	public void Deactivate()
	{
		this._isActive = false;
	}

	// Token: 0x0600553B RID: 21819 RVA: 0x001D62B0 File Offset: 0x001D46B0
	private void Update()
	{
		float currentAlpha = this._currentAlpha;
		this._currentAlpha = Mathf.MoveTowards(this._currentAlpha, (!this._isActive) ? 0f : 1f, Time.deltaTime / 0.25f);
		if (!Mathf.Approximately(currentAlpha, this._currentAlpha))
		{
			this.SetAlpha(this._currentAlpha);
		}
		if (this.IsAttachedToView)
		{
			this.RefreshComponentPositions();
		}
		this.UpdateScale();
	}

	// Token: 0x0600553C RID: 21820 RVA: 0x001D6330 File Offset: 0x001D4730
	private void SetAlpha(float a)
	{
		Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer.material.HasProperty("_Color"))
			{
				Color color = renderer.material.color;
				color.a = a;
				renderer.material.color = color;
			}
		}
		TextMesh[] componentsInChildren2 = base.GetComponentsInChildren<TextMesh>();
		foreach (TextMesh textMesh in componentsInChildren2)
		{
			Color color2 = textMesh.color;
			color2.a = a;
			textMesh.color = color2;
		}
	}

	// Token: 0x0600553D RID: 21821 RVA: 0x001D63DC File Offset: 0x001D47DC
	private void UpdateScale()
	{
		Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
		float magnitude = (base.transform.position - trackedTransform.position).magnitude;
		float num = Mathf.Lerp(TutorialLabel.MinScale, TutorialLabel.MaxScale, (magnitude - TutorialLabel.NearViewDistance) / (TutorialLabel.FarViewDistance - TutorialLabel.NearViewDistance));
		this.Tether.transform.localScale = new Vector3(this._tetherLength * num, 1f, 1f);
		this.Label.transform.localPosition = new Vector3(0f, 0f, (this._tetherLength + 0.01f) * num);
		this.Label.transform.localScale = new Vector3(num * this._textScaleOverride, num * this._textScaleOverride, num * this._textScaleOverride);
	}

	// Token: 0x0600553E RID: 21822 RVA: 0x001D64B3 File Offset: 0x001D48B3
	public bool CanSeeLabel()
	{
		return VRCTrackingManager.IsPointWithinHMDView(this.Label.transform.position);
	}

	// Token: 0x0600553F RID: 21823 RVA: 0x001D64CC File Offset: 0x001D48CC
	public static void FindLabelAttachPoints(Component targetObject, out Vector3 tetherPos, out Vector3 labelPos)
	{
		Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
		Collider component = targetObject.GetComponent<Collider>();
		if (component != null)
		{
			tetherPos = component.bounds.center + new Vector3(0f, component.bounds.extents.y + 0.02f, 0f);
			labelPos = component.bounds.center;
			labelPos.y += component.bounds.extents.y + TutorialLabel.DefaultLabelOffset.y;
			labelPos += trackedTransform.right * TutorialLabel.DefaultLabelOffset.x;
			labelPos += trackedTransform.forward.GetPlanarDirection() * TutorialLabel.DefaultLabelOffset.z;
		}
		else
		{
			tetherPos = (labelPos = targetObject.transform.position);
			labelPos.y += TutorialLabel.DefaultLabelOffset.y;
			labelPos += trackedTransform.right * TutorialLabel.DefaultLabelOffset.x;
			labelPos += trackedTransform.forward.GetPlanarDirection() * TutorialLabel.DefaultLabelOffset.z;
		}
	}

	// Token: 0x06005540 RID: 21824 RVA: 0x001D6658 File Offset: 0x001D4A58
	public static Vector3 CalculateFloatingLabelPosition()
	{
		Transform trackedTransform = VRCTrackingManager.GetTrackedTransform(VRCTracking.ID.Hmd);
		Vector3 vector = trackedTransform.forward.GetPlanarDirection() * TutorialLabel.LabelFloatingDistanceZ;
		vector.y = ((!(QuickMenu.Instance != null) || !QuickMenu.Instance.IsActiveOnDesktop()) ? TutorialLabel.LabelFloatingDistanceY : TutorialLabel.LabelFloatingDistanceY_DesktopQuickMenu);
		vector += trackedTransform.position;
		vector.y = Mathf.Max(vector.y, VRCTrackingManager.GetTrackingWorldOrigin().y + 0.25f);
		return vector;
	}

	// Token: 0x04003C23 RID: 15395
	public GameObject Tether;

	// Token: 0x04003C24 RID: 15396
	public GameObject Label;

	// Token: 0x04003C25 RID: 15397
	public GameObject Text;

	// Token: 0x04003C26 RID: 15398
	public GameObject TextSecondary;

	// Token: 0x04003C27 RID: 15399
	public GameObject Icon;

	// Token: 0x04003C28 RID: 15400
	public GameObject IconSecondary;

	// Token: 0x04003C29 RID: 15401
	public bool ShowOffscreen;

	// Token: 0x04003C2A RID: 15402
	private static Vector3 DefaultLabelOffset = new Vector3(0.05f, 0.1f, 0f);

	// Token: 0x04003C2B RID: 15403
	private static float MinScale = 0.5f;

	// Token: 0x04003C2C RID: 15404
	private static float MaxScale = 2f;

	// Token: 0x04003C2D RID: 15405
	private static float NearViewDistance = 0.2f;

	// Token: 0x04003C2E RID: 15406
	private static float FarViewDistance = 2f;

	// Token: 0x04003C2F RID: 15407
	private static float LabelFloatingDistanceZ = 1f;

	// Token: 0x04003C30 RID: 15408
	private static float LabelFloatingDistanceY = -0.25f;

	// Token: 0x04003C31 RID: 15409
	private static float LabelFloatingDistanceY_DesktopQuickMenu = 0f;

	// Token: 0x04003C35 RID: 15413
	private bool _isActive;

	// Token: 0x04003C36 RID: 15414
	private float _currentAlpha;

	// Token: 0x04003C37 RID: 15415
	private float _tetherLength;

	// Token: 0x04003C38 RID: 15416
	private float _textScaleOverride = 1f;

	// Token: 0x04003C39 RID: 15417
	private float _textInitialOffsetX;

	// Token: 0x04003C3A RID: 15418
	private float _iconInitialOffsetX;
}
