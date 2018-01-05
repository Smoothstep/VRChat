using System;
using UnityEngine;
using VRC.Core;

// Token: 0x02000972 RID: 2418
public abstract class VRGUI : MonoBehaviour
{
	// Token: 0x06004952 RID: 18770 RVA: 0x00187B5C File Offset: 0x00185F5C
	private void Initialize()
	{
		if (this.guiRenderPlane != null)
		{
			UnityEngine.Object.Destroy(this.guiRenderPlane);
		}
		if (this.useCurvedSurface)
		{
			this.guiRenderPlane = (AssetManagement.Instantiate(Resources.Load("VRGUICurvedSurface")) as GameObject);
		}
		else
		{
			this.guiRenderPlane = (AssetManagement.Instantiate(Resources.Load("VRGUIFlatSurface")) as GameObject);
		}
		this.guiRenderPlane.transform.parent = base.transform;
		this.guiRenderPlane.transform.localPosition = this.guiPosition;
		this.guiRenderPlane.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		this.guiRenderPlane.transform.localScale = new Vector3(this.guiSize, this.guiSize, this.guiSize);
		this.guiRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
		this.guiRenderPlane.GetComponent<Renderer>().material.mainTexture = this.guiRenderTexture;
		if (this.customCursor != null)
		{
			this.cursor = this.customCursor;
		}
		else
		{
			this.cursor = (Resources.Load("SimpleCursor") as Texture);
		}
		this.isInitialized = true;
	}

	// Token: 0x06004953 RID: 18771 RVA: 0x00187CB0 File Offset: 0x001860B0
	private void Update()
	{
		if (this.follower != null && this.guiRenderPlane != null)
		{
			Vector3 position = this.guiRenderPlane.transform.position;
			position.y = this.follower.position.y;
			this.guiRenderPlane.transform.position = position;
		}
	}

	// Token: 0x06004954 RID: 18772 RVA: 0x00187D1C File Offset: 0x0018611C
	protected Rect MakeCenterRect()
	{
		float num = (float)((!VRCTrackingManager.IsInVRMode()) ? (Screen.width * 7 / 8) : (Screen.width / 2));
		float num2 = (float)((!VRCTrackingManager.IsInVRMode()) ? (Screen.height * 7 / 8) : (Screen.height / 2));
		return new Rect((float)(Screen.width / 2) - num / 2f, (float)(Screen.height / 2) - num2 / 2f, num, num2);
	}

	// Token: 0x06004955 RID: 18773 RVA: 0x00187D92 File Offset: 0x00186192
	protected virtual void OnEnable()
	{
		if (this.guiRenderPlane != null)
		{
			this.guiRenderPlane.SetActive(true);
		}
	}

	// Token: 0x06004956 RID: 18774 RVA: 0x00187DB1 File Offset: 0x001861B1
	protected virtual void OnDisable()
	{
		if (this.guiRenderPlane != null)
		{
			this.guiRenderPlane.SetActive(false);
		}
	}

	// Token: 0x06004957 RID: 18775 RVA: 0x00187DD0 File Offset: 0x001861D0
	public void OnGUI()
	{
		if (!VRCTrackingManager.IsInVRMode())
		{
			if (this.guiRenderPlane != null)
			{
				UnityEngine.Object.Destroy(this.guiRenderPlane);
				this.guiRenderPlane = null;
			}
			this.isInitialized = false;
			this.OnVRGUI();
		}
		else
		{
			if (!this.isInitialized || this.guiRenderPlane == null)
			{
				this.Initialize();
			}
			if (Event.current.isMouse && !this.acceptMouse)
			{
				return;
			}
			if (this.acceptMouse)
			{
				this.cursorPosition = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
			}
			if (Event.current.isKey && !this.acceptKeyboard)
			{
				return;
			}
			RenderTexture active = RenderTexture.active;
			if (Event.current.type == EventType.Repaint)
			{
				RenderTexture.active = this.guiRenderTexture;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
			}
			this.OnVRGUI();
			if (Event.current.type == EventType.Repaint)
			{
				if (this.acceptMouse)
				{
					GUI.DrawTexture(new Rect(this.cursorPosition.x, this.cursorPosition.y, (float)this.cursorSize, (float)this.cursorSize), this.cursor, ScaleMode.StretchToFill);
				}
				RenderTexture.active = active;
			}
		}
	}

	// Token: 0x06004958 RID: 18776
	public abstract void OnVRGUI();

	// Token: 0x040031A6 RID: 12710
	public Vector3 guiPosition = new Vector3(0f, 0f, 1f);

	// Token: 0x040031A7 RID: 12711
	public float guiSize = 1f;

	// Token: 0x040031A8 RID: 12712
	public bool useCurvedSurface = true;

	// Token: 0x040031A9 RID: 12713
	public bool acceptMouse = true;

	// Token: 0x040031AA RID: 12714
	public bool acceptKeyboard = true;

	// Token: 0x040031AB RID: 12715
	public int cursorSize = 32;

	// Token: 0x040031AC RID: 12716
	public Texture customCursor;

	// Token: 0x040031AD RID: 12717
	public Transform follower;

	// Token: 0x040031AE RID: 12718
	private GameObject guiRenderPlane;

	// Token: 0x040031AF RID: 12719
	private RenderTexture guiRenderTexture;

	// Token: 0x040031B0 RID: 12720
	private Vector2 cursorPosition = Vector2.zero;

	// Token: 0x040031B1 RID: 12721
	private Texture cursor;

	// Token: 0x040031B2 RID: 12722
	private bool isInitialized;
}
