using System;
using UnityEngine;

// Token: 0x0200078A RID: 1930
public class InputToEvent : MonoBehaviour
{
	// Token: 0x17000A00 RID: 2560
	// (get) Token: 0x06003EB6 RID: 16054 RVA: 0x0013C1EE File Offset: 0x0013A5EE
	// (set) Token: 0x06003EB7 RID: 16055 RVA: 0x0013C1F5 File Offset: 0x0013A5F5
	public static GameObject goPointedAt { get; private set; }

	// Token: 0x17000A01 RID: 2561
	// (get) Token: 0x06003EB8 RID: 16056 RVA: 0x0013C1FD File Offset: 0x0013A5FD
	public Vector2 DragVector
	{
		get
		{
			return (!this.Dragging) ? Vector2.zero : (this.currentPos - this.pressedPosition);
		}
	}

	// Token: 0x06003EB9 RID: 16057 RVA: 0x0013C225 File Offset: 0x0013A625
	private void Start()
	{
		this.m_Camera = base.GetComponent<Camera>();
	}

	// Token: 0x06003EBA RID: 16058 RVA: 0x0013C234 File Offset: 0x0013A634
	private void Update()
	{
		if (this.DetectPointedAtGameObject)
		{
			InputToEvent.goPointedAt = this.RaycastObject(Input.mousePosition);
		}
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			this.currentPos = touch.position;
			if (touch.phase == TouchPhase.Began)
			{
				this.Press(touch.position);
			}
			else if (touch.phase == TouchPhase.Ended)
			{
				this.Release(touch.position);
			}
			return;
		}
		this.currentPos = Input.mousePosition;
		if (Input.GetMouseButtonDown(0))
		{
			this.Press(Input.mousePosition);
		}
		if (Input.GetMouseButtonUp(0))
		{
			this.Release(Input.mousePosition);
		}
		if (Input.GetMouseButtonDown(1))
		{
			this.pressedPosition = Input.mousePosition;
			this.lastGo = this.RaycastObject(this.pressedPosition);
			if (this.lastGo != null)
			{
				this.lastGo.SendMessage("OnPressRight", SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x06003EBB RID: 16059 RVA: 0x0013C34E File Offset: 0x0013A74E
	private void Press(Vector2 screenPos)
	{
		this.pressedPosition = screenPos;
		this.Dragging = true;
		this.lastGo = this.RaycastObject(screenPos);
		if (this.lastGo != null)
		{
			this.lastGo.SendMessage("OnPress", SendMessageOptions.DontRequireReceiver);
		}
	}

	// Token: 0x06003EBC RID: 16060 RVA: 0x0013C390 File Offset: 0x0013A790
	private void Release(Vector2 screenPos)
	{
		if (this.lastGo != null)
		{
			GameObject x = this.RaycastObject(screenPos);
			if (x == this.lastGo)
			{
				this.lastGo.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
			}
			this.lastGo.SendMessage("OnRelease", SendMessageOptions.DontRequireReceiver);
			this.lastGo = null;
		}
		this.pressedPosition = Vector2.zero;
		this.Dragging = false;
	}

	// Token: 0x06003EBD RID: 16061 RVA: 0x0013C404 File Offset: 0x0013A804
	private GameObject RaycastObject(Vector2 screenPos)
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(this.m_Camera.ScreenPointToRay(screenPos), out raycastHit, 200f))
		{
			InputToEvent.inputHitPos = raycastHit.point;
			return raycastHit.collider.gameObject;
		}
		return null;
	}

	// Token: 0x04002756 RID: 10070
	private GameObject lastGo;

	// Token: 0x04002757 RID: 10071
	public static Vector3 inputHitPos;

	// Token: 0x04002758 RID: 10072
	public bool DetectPointedAtGameObject;

	// Token: 0x0400275A RID: 10074
	private Vector2 pressedPosition = Vector2.zero;

	// Token: 0x0400275B RID: 10075
	private Vector2 currentPos = Vector2.zero;

	// Token: 0x0400275C RID: 10076
	public bool Dragging;

	// Token: 0x0400275D RID: 10077
	private Camera m_Camera;
}
