using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000630 RID: 1584
[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Event System (UICamera)")]
[RequireComponent(typeof(Camera))]
public class UICamera : MonoBehaviour
{
	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x0600350B RID: 13579 RVA: 0x0010C3F5 File Offset: 0x0010A7F5
	[Obsolete("Use new OnDragStart / OnDragOver / OnDragOut / OnDragEnd events instead")]
	public bool stickyPress
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x0600350C RID: 13580 RVA: 0x0010C3F8 File Offset: 0x0010A7F8
	public static Ray currentRay
	{
		get
		{
			return (!(UICamera.currentCamera != null) || UICamera.currentTouch == null) ? default(Ray) : UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
		}
	}

	// Token: 0x17000815 RID: 2069
	// (get) Token: 0x0600350D RID: 13581 RVA: 0x0010C446 File Offset: 0x0010A846
	// (set) Token: 0x0600350E RID: 13582 RVA: 0x0010C44D File Offset: 0x0010A84D
	[Obsolete("Use delegates instead such as UICamera.onClick, UICamera.onHover, etc.")]
	public static GameObject genericEventHandler
	{
		get
		{
			return UICamera.mGenericHandler;
		}
		set
		{
			UICamera.mGenericHandler = value;
		}
	}

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x0600350F RID: 13583 RVA: 0x0010C455 File Offset: 0x0010A855
	private bool handlesEvents
	{
		get
		{
			return UICamera.eventHandler == this;
		}
	}

	// Token: 0x17000817 RID: 2071
	// (get) Token: 0x06003510 RID: 13584 RVA: 0x0010C462 File Offset: 0x0010A862
	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = base.GetComponent<Camera>();
			}
			return this.mCam;
		}
	}

	// Token: 0x17000818 RID: 2072
	// (get) Token: 0x06003511 RID: 13585 RVA: 0x0010C488 File Offset: 0x0010A888
	public static bool isOverUI
	{
		get
		{
			if (UICamera.currentTouch != null)
			{
				return UICamera.currentTouch.isOverUI;
			}
			return !(UICamera.hoveredObject == null) && !(UICamera.hoveredObject == UICamera.fallThrough) && NGUITools.FindInParents<UIRoot>(UICamera.hoveredObject) != null;
		}
	}

	// Token: 0x17000819 RID: 2073
	// (get) Token: 0x06003512 RID: 13586 RVA: 0x0010C4E2 File Offset: 0x0010A8E2
	// (set) Token: 0x06003513 RID: 13587 RVA: 0x0010C4E9 File Offset: 0x0010A8E9
	public static GameObject selectedObject
	{
		get
		{
			return UICamera.mCurrentSelection;
		}
		set
		{
			UICamera.SetSelection(value, UICamera.currentScheme);
		}
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x0010C4F8 File Offset: 0x0010A8F8
	public static bool IsPressed(GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			if (UICamera.mMouse[i].pressed == go)
			{
				return true;
			}
		}
		foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
		{
			if (keyValuePair.Value.pressed == go)
			{
				return true;
			}
		}
		return UICamera.controller.pressed == go;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x0010C5B0 File Offset: 0x0010A9B0
	protected static void SetSelection(GameObject go, UICamera.ControlScheme scheme)
	{
		if (UICamera.mNextSelection != null)
		{
			UICamera.mNextSelection = go;
		}
		else if (UICamera.mCurrentSelection != go)
		{
			UICamera.mNextSelection = go;
			UICamera.mNextScheme = scheme;
			if (UICamera.list.size > 0)
			{
				UICamera uicamera = (!(UICamera.mNextSelection != null)) ? UICamera.list[0] : UICamera.FindCameraForLayer(UICamera.mNextSelection.layer);
				if (uicamera != null)
				{
					uicamera.StartCoroutine(uicamera.ChangeSelection());
				}
			}
		}
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x0010C650 File Offset: 0x0010AA50
	private IEnumerator ChangeSelection()
	{
		yield return new WaitForEndOfFrame();
		if (UICamera.onSelect != null)
		{
			UICamera.onSelect(UICamera.mCurrentSelection, false);
		}
		UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", false);
		UICamera.mCurrentSelection = UICamera.mNextSelection;
		UICamera.mNextSelection = null;
		if (UICamera.mCurrentSelection != null)
		{
			UICamera.current = this;
			UICamera.currentCamera = this.mCam;
			UICamera.currentScheme = UICamera.mNextScheme;
			UICamera.inputHasFocus = (UICamera.mCurrentSelection.GetComponent<UIInput>() != null);
			if (UICamera.onSelect != null)
			{
				UICamera.onSelect(UICamera.mCurrentSelection, true);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnSelect", true);
			UICamera.current = null;
		}
		else
		{
			UICamera.inputHasFocus = false;
		}
		yield break;
	}

	// Token: 0x1700081A RID: 2074
	// (get) Token: 0x06003517 RID: 13591 RVA: 0x0010C66C File Offset: 0x0010AA6C
	public static int touchCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value.pressed != null)
				{
					num++;
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].pressed != null)
				{
					num++;
				}
			}
			if (UICamera.controller.pressed != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x1700081B RID: 2075
	// (get) Token: 0x06003518 RID: 13592 RVA: 0x0010C72C File Offset: 0x0010AB2C
	public static int dragCount
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value.dragged != null)
				{
					num++;
				}
			}
			for (int i = 0; i < UICamera.mMouse.Length; i++)
			{
				if (UICamera.mMouse[i].dragged != null)
				{
					num++;
				}
			}
			if (UICamera.controller.dragged != null)
			{
				num++;
			}
			return num;
		}
	}

	// Token: 0x1700081C RID: 2076
	// (get) Token: 0x06003519 RID: 13593 RVA: 0x0010C7EC File Offset: 0x0010ABEC
	public static Camera mainCamera
	{
		get
		{
			UICamera eventHandler = UICamera.eventHandler;
			return (!(eventHandler != null)) ? null : eventHandler.cachedCamera;
		}
	}

	// Token: 0x1700081D RID: 2077
	// (get) Token: 0x0600351A RID: 13594 RVA: 0x0010C818 File Offset: 0x0010AC18
	public static UICamera eventHandler
	{
		get
		{
			for (int i = 0; i < UICamera.list.size; i++)
			{
				UICamera uicamera = UICamera.list.buffer[i];
				if (!(uicamera == null) && uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
				{
					return uicamera;
				}
			}
			return null;
		}
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x0010C87C File Offset: 0x0010AC7C
	private static int CompareFunc(UICamera a, UICamera b)
	{
		if (a.cachedCamera.depth < b.cachedCamera.depth)
		{
			return 1;
		}
		if (a.cachedCamera.depth > b.cachedCamera.depth)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600351C RID: 13596 RVA: 0x0010C8BC File Offset: 0x0010ACBC
	private static Rigidbody FindRootRigidbody(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody component = trans.GetComponent<Rigidbody>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	// Token: 0x0600351D RID: 13597 RVA: 0x0010C90C File Offset: 0x0010AD0C
	private static Rigidbody2D FindRootRigidbody2D(Transform trans)
	{
		while (trans != null)
		{
			if (trans.GetComponent<UIPanel>() != null)
			{
				return null;
			}
			Rigidbody2D component = trans.GetComponent<Rigidbody2D>();
			if (component != null)
			{
				return component;
			}
			trans = trans.parent;
		}
		return null;
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x0010C95C File Offset: 0x0010AD5C
	public static bool Raycast(Vector3 inPos)
	{
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			if (uicamera.enabled && NGUITools.GetActive(uicamera.gameObject))
			{
				UICamera.currentCamera = uicamera.cachedCamera;
				Vector3 vector = UICamera.currentCamera.ScreenToViewportPoint(inPos);
				if (!float.IsNaN(vector.x) && !float.IsNaN(vector.y))
				{
					if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f)
					{
						Ray ray = UICamera.currentCamera.ScreenPointToRay(inPos);
						int layerMask = UICamera.currentCamera.cullingMask & uicamera.eventReceiverMask;
						float num = (uicamera.rangeDistance <= 0f) ? (UICamera.currentCamera.farClipPlane - UICamera.currentCamera.nearClipPlane) : uicamera.rangeDistance;
						if (uicamera.eventType == UICamera.EventType.World_3D)
						{
							if (Physics.Raycast(ray, out UICamera.lastHit, num, layerMask))
							{
								UICamera.lastWorldPosition = UICamera.lastHit.point;
								UICamera.hoveredObject = UICamera.lastHit.collider.gameObject;
								if (!UICamera.list[0].eventsGoToColliders)
								{
									Rigidbody rigidbody = UICamera.FindRootRigidbody(UICamera.hoveredObject.transform);
									if (rigidbody != null)
									{
										UICamera.hoveredObject = rigidbody.gameObject;
									}
								}
								return true;
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI_3D)
						{
							RaycastHit[] array = Physics.RaycastAll(ray, num, layerMask);
							if (array.Length > 1)
							{
								int j = 0;
								while (j < array.Length)
								{
									GameObject gameObject = array[j].collider.gameObject;
									UIWidget component = gameObject.GetComponent<UIWidget>();
									if (component != null)
									{
										if (component.isVisible)
										{
											if (component.hitCheck == null || component.hitCheck(array[j].point))
											{
												goto IL_260;
											}
										}
									}
									else
									{
										UIRect uirect = NGUITools.FindInParents<UIRect>(gameObject);
										if (!(uirect != null) || uirect.finalAlpha >= 0.001f)
										{
											goto IL_260;
										}
									}
									IL_2E1:
									j++;
									continue;
									IL_260:
									UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject);
									if (UICamera.mHit.depth != 2147483647)
									{
										UICamera.mHit.hit = array[j];
										UICamera.mHit.point = array[j].point;
										UICamera.mHit.go = array[j].collider.gameObject;
										UICamera.mHits.Add(UICamera.mHit);
										goto IL_2E1;
									}
									goto IL_2E1;
								}
								UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
								for (int k = 0; k < UICamera.mHits.size; k++)
								{
									if (UICamera.IsVisible(ref UICamera.mHits.buffer[k]))
									{
										UICamera.lastHit = UICamera.mHits[k].hit;
										UICamera.hoveredObject = UICamera.mHits[k].go;
										UICamera.lastWorldPosition = UICamera.mHits[k].point;
										UICamera.mHits.Clear();
										return true;
									}
								}
								UICamera.mHits.Clear();
							}
							else if (array.Length == 1)
							{
								GameObject gameObject2 = array[0].collider.gameObject;
								UIWidget component2 = gameObject2.GetComponent<UIWidget>();
								if (component2 != null)
								{
									if (!component2.isVisible)
									{
										goto IL_7E2;
									}
									if (component2.hitCheck != null && !component2.hitCheck(array[0].point))
									{
										goto IL_7E2;
									}
								}
								else
								{
									UIRect uirect2 = NGUITools.FindInParents<UIRect>(gameObject2);
									if (uirect2 != null && uirect2.finalAlpha < 0.001f)
									{
										goto IL_7E2;
									}
								}
								if (UICamera.IsVisible(array[0].point, array[0].collider.gameObject))
								{
									UICamera.lastHit = array[0];
									UICamera.lastWorldPosition = array[0].point;
									UICamera.hoveredObject = UICamera.lastHit.collider.gameObject;
									return true;
								}
							}
						}
						else if (uicamera.eventType == UICamera.EventType.World_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, out num))
							{
								Vector3 point = ray.GetPoint(num);
								Collider2D collider2D = Physics2D.OverlapPoint(point, layerMask);
								if (collider2D)
								{
									UICamera.lastWorldPosition = point;
									UICamera.hoveredObject = collider2D.gameObject;
									if (!uicamera.eventsGoToColliders)
									{
										Rigidbody2D rigidbody2D = UICamera.FindRootRigidbody2D(UICamera.hoveredObject.transform);
										if (rigidbody2D != null)
										{
											UICamera.hoveredObject = rigidbody2D.gameObject;
										}
									}
									return true;
								}
							}
						}
						else if (uicamera.eventType == UICamera.EventType.UI_2D)
						{
							if (UICamera.m2DPlane.Raycast(ray, out num))
							{
								UICamera.lastWorldPosition = ray.GetPoint(num);
								Collider2D[] array2 = Physics2D.OverlapPointAll(UICamera.lastWorldPosition, layerMask);
								if (array2.Length > 1)
								{
									int l = 0;
									while (l < array2.Length)
									{
										GameObject gameObject3 = array2[l].gameObject;
										UIWidget component3 = gameObject3.GetComponent<UIWidget>();
										if (component3 != null)
										{
											if (component3.isVisible)
											{
												if (component3.hitCheck == null || component3.hitCheck(UICamera.lastWorldPosition))
												{
													goto IL_639;
												}
											}
										}
										else
										{
											UIRect uirect3 = NGUITools.FindInParents<UIRect>(gameObject3);
											if (!(uirect3 != null) || uirect3.finalAlpha >= 0.001f)
											{
												goto IL_639;
											}
										}
										IL_688:
										l++;
										continue;
										IL_639:
										UICamera.mHit.depth = NGUITools.CalculateRaycastDepth(gameObject3);
										if (UICamera.mHit.depth != 2147483647)
										{
											UICamera.mHit.go = gameObject3;
											UICamera.mHit.point = UICamera.lastWorldPosition;
											UICamera.mHits.Add(UICamera.mHit);
											goto IL_688;
										}
										goto IL_688;
									}
									UICamera.mHits.Sort((UICamera.DepthEntry r1, UICamera.DepthEntry r2) => r2.depth.CompareTo(r1.depth));
									for (int m = 0; m < UICamera.mHits.size; m++)
									{
										if (UICamera.IsVisible(ref UICamera.mHits.buffer[m]))
										{
											UICamera.hoveredObject = UICamera.mHits[m].go;
											UICamera.mHits.Clear();
											return true;
										}
									}
									UICamera.mHits.Clear();
								}
								else if (array2.Length == 1)
								{
									GameObject gameObject4 = array2[0].gameObject;
									UIWidget component4 = gameObject4.GetComponent<UIWidget>();
									if (component4 != null)
									{
										if (!component4.isVisible)
										{
											goto IL_7E2;
										}
										if (component4.hitCheck != null && !component4.hitCheck(UICamera.lastWorldPosition))
										{
											goto IL_7E2;
										}
									}
									else
									{
										UIRect uirect4 = NGUITools.FindInParents<UIRect>(gameObject4);
										if (uirect4 != null && uirect4.finalAlpha < 0.001f)
										{
											goto IL_7E2;
										}
									}
									if (UICamera.IsVisible(UICamera.lastWorldPosition, gameObject4))
									{
										UICamera.hoveredObject = gameObject4;
										return true;
									}
								}
							}
						}
					}
				}
			}
			IL_7E2:;
		}
		return false;
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x0010D160 File Offset: 0x0010B560
	private static bool IsVisible(Vector3 worldPoint, GameObject go)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(go);
		while (uipanel != null)
		{
			if (!uipanel.IsVisible(worldPoint))
			{
				return false;
			}
			uipanel = uipanel.parentPanel;
		}
		return true;
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x0010D19C File Offset: 0x0010B59C
	private static bool IsVisible(ref UICamera.DepthEntry de)
	{
		UIPanel uipanel = NGUITools.FindInParents<UIPanel>(de.go);
		while (uipanel != null)
		{
			if (!uipanel.IsVisible(de.point))
			{
				return false;
			}
			uipanel = uipanel.parentPanel;
		}
		return true;
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x0010D1E1 File Offset: 0x0010B5E1
	public static bool IsHighlighted(GameObject go)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
		{
			return UICamera.hoveredObject == go;
		}
		return UICamera.currentScheme == UICamera.ControlScheme.Controller && UICamera.selectedObject == go;
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x0010D214 File Offset: 0x0010B614
	public static UICamera FindCameraForLayer(int layer)
	{
		int num = 1 << layer;
		for (int i = 0; i < UICamera.list.size; i++)
		{
			UICamera uicamera = UICamera.list.buffer[i];
			Camera cachedCamera = uicamera.cachedCamera;
			if (cachedCamera != null && (cachedCamera.cullingMask & num) != 0)
			{
				return uicamera;
			}
		}
		return null;
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x0010D273 File Offset: 0x0010B673
	private static int GetDirection(KeyCode up, KeyCode down)
	{
		if (UICamera.GetKeyDown(up))
		{
			return 1;
		}
		if (UICamera.GetKeyDown(down))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x0010D29C File Offset: 0x0010B69C
	private static int GetDirection(KeyCode up0, KeyCode up1, KeyCode down0, KeyCode down1)
	{
		if (UICamera.GetKeyDown(up0) || UICamera.GetKeyDown(up1))
		{
			return 1;
		}
		if (UICamera.GetKeyDown(down0) || UICamera.GetKeyDown(down1))
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x0010D2F0 File Offset: 0x0010B6F0
	private static int GetDirection(string axis)
	{
		float time = RealTime.time;
		if (UICamera.mNextEvent < time && !string.IsNullOrEmpty(axis))
		{
			float num = UICamera.GetAxis(axis);
			if (num > 0.75f)
			{
				UICamera.mNextEvent = time + 0.25f;
				return 1;
			}
			if (num < -0.75f)
			{
				UICamera.mNextEvent = time + 0.25f;
				return -1;
			}
		}
		return 0;
	}

	// Token: 0x06003526 RID: 13606 RVA: 0x0010D358 File Offset: 0x0010B758
	public static void Notify(GameObject go, string funcName, object obj)
	{
		if (UICamera.mNotifying)
		{
			return;
		}
		UICamera.mNotifying = true;
		if (NGUITools.GetActive(go))
		{
			go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			if (UICamera.mGenericHandler != null && UICamera.mGenericHandler != go)
			{
				UICamera.mGenericHandler.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
			}
		}
		UICamera.mNotifying = false;
	}

	// Token: 0x06003527 RID: 13607 RVA: 0x0010D3BD File Offset: 0x0010B7BD
	public static UICamera.MouseOrTouch GetMouse(int button)
	{
		return UICamera.mMouse[button];
	}

	// Token: 0x06003528 RID: 13608 RVA: 0x0010D3C8 File Offset: 0x0010B7C8
	public static UICamera.MouseOrTouch GetTouch(int id)
	{
		UICamera.MouseOrTouch mouseOrTouch = null;
		if (id < 0)
		{
			return UICamera.GetMouse(-id - 1);
		}
		if (!UICamera.mTouches.TryGetValue(id, out mouseOrTouch))
		{
			mouseOrTouch = new UICamera.MouseOrTouch();
			mouseOrTouch.pressTime = RealTime.time;
			mouseOrTouch.touchBegan = true;
			UICamera.mTouches.Add(id, mouseOrTouch);
		}
		return mouseOrTouch;
	}

	// Token: 0x06003529 RID: 13609 RVA: 0x0010D41F File Offset: 0x0010B81F
	public static void RemoveTouch(int id)
	{
		UICamera.mTouches.Remove(id);
	}

	// Token: 0x0600352A RID: 13610 RVA: 0x0010D430 File Offset: 0x0010B830
	private void Awake()
	{
		UICamera.mWidth = Screen.width;
		UICamera.mHeight = Screen.height;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			this.useTouch = true;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				this.useMouse = false;
				this.useKeyboard = false;
				this.useController = false;
			}
		}
		UICamera.mMouse[0].pos = Input.mousePosition;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].lastPos = UICamera.mMouse[0].pos;
		}
		UICamera.lastTouchPosition = UICamera.mMouse[0].pos;
	}

	// Token: 0x0600352B RID: 13611 RVA: 0x0010D4F8 File Offset: 0x0010B8F8
	private void OnEnable()
	{
		UICamera.list.Add(this);
		BetterList<UICamera> betterList = UICamera.list;
		if (UICamera.f__mg0 == null)
		{
			UICamera.f__mg0 = new BetterList<UICamera>.CompareFunc(UICamera.CompareFunc);
		}
		betterList.Sort(UICamera.f__mg0);
	}

	// Token: 0x0600352C RID: 13612 RVA: 0x0010D52C File Offset: 0x0010B92C
	private void OnDisable()
	{
		UICamera.list.Remove(this);
	}

	// Token: 0x0600352D RID: 13613 RVA: 0x0010D53C File Offset: 0x0010B93C
	private void Start()
	{
		if (this.eventType != UICamera.EventType.World_3D && this.cachedCamera.transparencySortMode != TransparencySortMode.Orthographic)
		{
			this.cachedCamera.transparencySortMode = TransparencySortMode.Orthographic;
		}
		if (Application.isPlaying)
		{
			if (UICamera.fallThrough == null)
			{
				UIRoot uiroot = NGUITools.FindInParents<UIRoot>(base.gameObject);
				if (uiroot != null)
				{
					UICamera.fallThrough = uiroot.gameObject;
				}
				else
				{
					Transform transform = base.transform;
					UICamera.fallThrough = ((!(transform.parent != null)) ? base.gameObject : transform.parent.gameObject);
				}
			}
			this.cachedCamera.eventMask = 0;
		}
		if (this.handlesEvents)
		{
			NGUIDebug.debugRaycast = this.debug;
		}
	}

	// Token: 0x0600352E RID: 13614 RVA: 0x0010D608 File Offset: 0x0010BA08
	private void Update()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		UICamera.current = this;
		if (this.useTouch)
		{
			this.ProcessTouches();
		}
		else if (this.useMouse)
		{
			this.ProcessMouse();
		}
		if (UICamera.onCustomInput != null)
		{
			UICamera.onCustomInput();
		}
		if (this.useMouse && UICamera.mCurrentSelection != null)
		{
			if (this.cancelKey0 != KeyCode.None && UICamera.GetKeyDown(this.cancelKey0))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentKey = this.cancelKey0;
				UICamera.selectedObject = null;
			}
			else if (this.cancelKey1 != KeyCode.None && UICamera.GetKeyDown(this.cancelKey1))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentKey = this.cancelKey1;
				UICamera.selectedObject = null;
			}
		}
		if (UICamera.mCurrentSelection == null)
		{
			UICamera.inputHasFocus = false;
		}
		if (UICamera.mCurrentSelection != null)
		{
			this.ProcessOthers();
		}
		if (this.useMouse && UICamera.mHover != null)
		{
			float num = string.IsNullOrEmpty(this.scrollAxisName) ? 0f : UICamera.GetAxis(this.scrollAxisName);
			if (num != 0f)
			{
				if (UICamera.onScroll != null)
				{
					UICamera.onScroll(UICamera.mHover, num);
				}
				UICamera.Notify(UICamera.mHover, "OnScroll", num);
			}
			if (UICamera.showTooltips && this.mTooltipTime != 0f && (this.mTooltipTime < RealTime.time || UICamera.GetKey(KeyCode.LeftShift) || UICamera.GetKey(KeyCode.RightShift)))
			{
				this.mTooltip = UICamera.mHover;
				this.ShowTooltip(true);
			}
		}
		UICamera.current = null;
	}

	// Token: 0x0600352F RID: 13615 RVA: 0x0010D808 File Offset: 0x0010BC08
	private void LateUpdate()
	{
		if (!this.handlesEvents)
		{
			return;
		}
		int width = Screen.width;
		int height = Screen.height;
		if (width != UICamera.mWidth || height != UICamera.mHeight)
		{
			UICamera.mWidth = width;
			UICamera.mHeight = height;
			UIRoot.Broadcast("UpdateAnchors");
			if (UICamera.onScreenResize != null)
			{
				UICamera.onScreenResize();
			}
		}
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x0010D870 File Offset: 0x0010BC70
	public void ProcessMouse()
	{
		UICamera.lastTouchPosition = Input.mousePosition;
		UICamera.mMouse[0].delta = UICamera.lastTouchPosition - UICamera.mMouse[0].pos;
		UICamera.mMouse[0].pos = UICamera.lastTouchPosition;
		bool flag = UICamera.mMouse[0].delta.sqrMagnitude > 0.001f;
		for (int i = 1; i < 3; i++)
		{
			UICamera.mMouse[i].pos = UICamera.mMouse[0].pos;
			UICamera.mMouse[i].delta = UICamera.mMouse[0].delta;
		}
		bool flag2 = false;
		bool flag3 = false;
		for (int j = 0; j < 3; j++)
		{
			if (Input.GetMouseButtonDown(j))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
				flag3 = true;
				flag2 = true;
			}
			else if (Input.GetMouseButton(j))
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
				flag2 = true;
			}
		}
		if (flag2 || flag || this.mNextRaycast < RealTime.time)
		{
			this.mNextRaycast = RealTime.time + 0.02f;
			if (!UICamera.Raycast(Input.mousePosition))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			for (int k = 0; k < 3; k++)
			{
				UICamera.mMouse[k].current = UICamera.hoveredObject;
			}
		}
		bool flag4 = UICamera.mMouse[0].last != UICamera.mMouse[0].current;
		if (flag4)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
		}
		if (flag2)
		{
			this.mTooltipTime = 0f;
		}
		else if (flag && (!this.stickyTooltip || flag4))
		{
			if (this.mTooltipTime != 0f)
			{
				this.mTooltipTime = RealTime.time + this.tooltipDelay;
			}
			else if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
		}
		if (flag && UICamera.onMouseMove != null)
		{
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.onMouseMove(UICamera.currentTouch.delta);
			UICamera.currentTouch = null;
		}
		if ((flag3 || !flag2) && UICamera.mHover != null && flag4)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			UICamera.currentTouch = UICamera.mMouse[0];
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			if (UICamera.onHover != null)
			{
				UICamera.onHover(UICamera.mHover, false);
			}
			UICamera.Notify(UICamera.mHover, "OnHover", false);
			UICamera.mHover = null;
		}
		for (int l = 0; l < 3; l++)
		{
			bool mouseButtonDown = Input.GetMouseButtonDown(l);
			bool mouseButtonUp = Input.GetMouseButtonUp(l);
			if (mouseButtonDown || mouseButtonUp)
			{
				UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			}
			UICamera.currentTouch = UICamera.mMouse[l];
			UICamera.currentTouchID = -1 - l;
			UICamera.currentKey = KeyCode.Mouse0 + l;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			UICamera.currentKey = KeyCode.None;
		}
		if (!flag2 && flag4)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Mouse;
			this.mTooltipTime = RealTime.time + this.tooltipDelay;
			UICamera.mHover = UICamera.mMouse[0].current;
			UICamera.currentTouch = UICamera.mMouse[0];
			if (UICamera.onHover != null)
			{
				UICamera.onHover(UICamera.mHover, true);
			}
			UICamera.Notify(UICamera.mHover, "OnHover", true);
		}
		UICamera.currentTouch = null;
		UICamera.mMouse[0].last = UICamera.mMouse[0].current;
		for (int m = 1; m < 3; m++)
		{
			UICamera.mMouse[m].last = UICamera.mMouse[0].last;
		}
	}

	// Token: 0x06003531 RID: 13617 RVA: 0x0010DCA8 File Offset: 0x0010C0A8
	public void ProcessTouches()
	{
		UICamera.currentScheme = UICamera.ControlScheme.Touch;
		for (int i = 0; i < Input.touchCount; i++)
		{
			Touch touch = Input.GetTouch(i);
			UICamera.currentTouchID = ((!this.allowMultiTouch) ? 1 : touch.fingerId);
			UICamera.currentTouch = UICamera.GetTouch(UICamera.currentTouchID);
			bool flag = touch.phase == TouchPhase.Began || UICamera.currentTouch.touchBegan;
			bool flag2 = touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended;
			UICamera.currentTouch.touchBegan = false;
			UICamera.currentTouch.delta = ((!flag) ? (touch.position - UICamera.currentTouch.pos) : Vector2.zero);
			UICamera.currentTouch.pos = touch.position;
			if (!UICamera.Raycast(UICamera.currentTouch.pos))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (flag)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			if (touch.tapCount > 1)
			{
				UICamera.currentTouch.clickTime = RealTime.time;
			}
			this.ProcessTouch(flag, flag2);
			if (flag2)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
			if (!this.allowMultiTouch)
			{
				break;
			}
		}
		if (Input.touchCount == 0 && this.useMouse)
		{
			this.ProcessMouse();
		}
	}

	// Token: 0x06003532 RID: 13618 RVA: 0x0010DEA4 File Offset: 0x0010C2A4
	private void ProcessFakeTouches()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		bool mouseButton = Input.GetMouseButton(0);
		if (mouseButtonDown || mouseButtonUp || mouseButton)
		{
			UICamera.currentTouchID = 1;
			UICamera.currentTouch = UICamera.mMouse[0];
			UICamera.currentTouch.touchBegan = mouseButtonDown;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressTime = RealTime.time;
			}
			Vector2 vector = Input.mousePosition;
			UICamera.currentTouch.delta = ((!mouseButtonDown) ? (vector - UICamera.currentTouch.pos) : Vector2.zero);
			UICamera.currentTouch.pos = vector;
			if (!UICamera.Raycast(UICamera.currentTouch.pos))
			{
				UICamera.hoveredObject = UICamera.fallThrough;
			}
			if (UICamera.hoveredObject == null)
			{
				UICamera.hoveredObject = UICamera.mGenericHandler;
			}
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.hoveredObject;
			UICamera.lastTouchPosition = UICamera.currentTouch.pos;
			if (mouseButtonDown)
			{
				UICamera.currentTouch.pressedCam = UICamera.currentCamera;
			}
			else if (UICamera.currentTouch.pressed != null)
			{
				UICamera.currentCamera = UICamera.currentTouch.pressedCam;
			}
			this.ProcessTouch(mouseButtonDown, mouseButtonUp);
			if (mouseButtonUp)
			{
				UICamera.RemoveTouch(UICamera.currentTouchID);
			}
			UICamera.currentTouch.last = null;
			UICamera.currentTouch = null;
		}
	}

	// Token: 0x06003533 RID: 13619 RVA: 0x0010E020 File Offset: 0x0010C420
	public void ProcessOthers()
	{
		UICamera.currentTouchID = -100;
		UICamera.currentTouch = UICamera.controller;
		bool flag = false;
		bool flag2 = false;
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyDown(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag = true;
		}
		if (this.submitKey1 != KeyCode.None && UICamera.GetKeyDown(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag = true;
		}
		if (this.submitKey0 != KeyCode.None && UICamera.GetKeyUp(this.submitKey0))
		{
			UICamera.currentKey = this.submitKey0;
			flag2 = true;
		}
		if (this.submitKey1 != KeyCode.None && UICamera.GetKeyUp(this.submitKey1))
		{
			UICamera.currentKey = this.submitKey1;
			flag2 = true;
		}
		if (flag || flag2)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.mCurrentSelection;
			this.ProcessTouch(flag, flag2);
			UICamera.currentTouch.last = null;
		}
		int num = 0;
		int num2 = 0;
		if (this.useKeyboard)
		{
			if (UICamera.inputHasFocus)
			{
				num += UICamera.GetDirection(KeyCode.UpArrow, KeyCode.DownArrow);
				num2 += UICamera.GetDirection(KeyCode.RightArrow, KeyCode.LeftArrow);
			}
			else
			{
				num += UICamera.GetDirection(KeyCode.W, KeyCode.UpArrow, KeyCode.S, KeyCode.DownArrow);
				num2 += UICamera.GetDirection(KeyCode.D, KeyCode.RightArrow, KeyCode.A, KeyCode.LeftArrow);
			}
		}
		if (this.useController)
		{
			if (!string.IsNullOrEmpty(this.verticalAxisName))
			{
				num += UICamera.GetDirection(this.verticalAxisName);
			}
			if (!string.IsNullOrEmpty(this.horizontalAxisName))
			{
				num2 += UICamera.GetDirection(this.horizontalAxisName);
			}
		}
		if (num != 0)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			KeyCode keyCode = (num <= 0) ? KeyCode.DownArrow : KeyCode.UpArrow;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, keyCode);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", keyCode);
		}
		if (num2 != 0)
		{
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			KeyCode keyCode2 = (num2 <= 0) ? KeyCode.LeftArrow : KeyCode.RightArrow;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, keyCode2);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", keyCode2);
		}
		if (this.useKeyboard && UICamera.GetKeyDown(KeyCode.Tab))
		{
			UICamera.currentKey = KeyCode.Tab;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, KeyCode.Tab);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", KeyCode.Tab);
		}
		if (this.cancelKey0 != KeyCode.None && UICamera.GetKeyDown(this.cancelKey0))
		{
			UICamera.currentKey = this.cancelKey0;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, KeyCode.Escape);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", KeyCode.Escape);
		}
		if (this.cancelKey1 != KeyCode.None && UICamera.GetKeyDown(this.cancelKey1))
		{
			UICamera.currentKey = this.cancelKey1;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			if (UICamera.onKey != null)
			{
				UICamera.onKey(UICamera.mCurrentSelection, KeyCode.Escape);
			}
			UICamera.Notify(UICamera.mCurrentSelection, "OnKey", KeyCode.Escape);
		}
		UICamera.currentTouch = null;
		UICamera.currentKey = KeyCode.None;
	}

	// Token: 0x06003534 RID: 13620 RVA: 0x0010E3C8 File Offset: 0x0010C7C8
	public void ProcessTouch(bool pressed, bool unpressed)
	{
		bool flag = UICamera.currentScheme == UICamera.ControlScheme.Mouse;
		float num = (!flag) ? this.touchDragThreshold : this.mouseDragThreshold;
		float num2 = (!flag) ? this.touchClickThreshold : this.mouseClickThreshold;
		num *= num;
		num2 *= num2;
		if (pressed)
		{
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			UICamera.currentTouch.pressStarted = true;
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, false);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
			UICamera.currentTouch.pressed = UICamera.currentTouch.current;
			UICamera.currentTouch.dragged = UICamera.currentTouch.current;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			UICamera.currentTouch.totalDelta = Vector2.zero;
			UICamera.currentTouch.dragStarted = false;
			if (UICamera.onPress != null)
			{
				UICamera.onPress(UICamera.currentTouch.pressed, true);
			}
			UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", true);
			if (UICamera.currentTouch.pressed != UICamera.mCurrentSelection)
			{
				if (this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.currentScheme = UICamera.ControlScheme.Touch;
				UICamera.selectedObject = UICamera.currentTouch.pressed;
			}
		}
		else if (UICamera.currentTouch.pressed != null && (UICamera.currentTouch.delta.sqrMagnitude != 0f || UICamera.currentTouch.current != UICamera.currentTouch.last))
		{
			UICamera.currentTouch.totalDelta += UICamera.currentTouch.delta;
			float sqrMagnitude = UICamera.currentTouch.totalDelta.sqrMagnitude;
			bool flag2 = false;
			if (!UICamera.currentTouch.dragStarted && UICamera.currentTouch.last != UICamera.currentTouch.current)
			{
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
				UICamera.isDragging = true;
				if (UICamera.onDragStart != null)
				{
					UICamera.onDragStart(UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
				if (UICamera.onDragOver != null)
				{
					UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
				}
				UICamera.Notify(UICamera.currentTouch.last, "OnDragOver", UICamera.currentTouch.dragged);
				UICamera.isDragging = false;
			}
			else if (!UICamera.currentTouch.dragStarted && num < sqrMagnitude)
			{
				flag2 = true;
				UICamera.currentTouch.dragStarted = true;
				UICamera.currentTouch.delta = UICamera.currentTouch.totalDelta;
			}
			if (UICamera.currentTouch.dragStarted)
			{
				if (this.mTooltip != null)
				{
					this.ShowTooltip(false);
				}
				UICamera.isDragging = true;
				bool flag3 = UICamera.currentTouch.clickNotification == UICamera.ClickNotification.None;
				if (flag2)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragStart", null);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				else if (UICamera.currentTouch.last != UICamera.currentTouch.current)
				{
					if (UICamera.onDragStart != null)
					{
						UICamera.onDragStart(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragOver != null)
					{
						UICamera.onDragOver(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDragOver", UICamera.currentTouch.dragged);
				}
				if (UICamera.onDrag != null)
				{
					UICamera.onDrag(UICamera.currentTouch.dragged, UICamera.currentTouch.delta);
				}
				UICamera.Notify(UICamera.currentTouch.dragged, "OnDrag", UICamera.currentTouch.delta);
				UICamera.currentTouch.last = UICamera.currentTouch.current;
				UICamera.isDragging = false;
				if (flag3)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
				else if (UICamera.currentTouch.clickNotification == UICamera.ClickNotification.BasedOnDelta && num2 < sqrMagnitude)
				{
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
				}
			}
		}
		if (unpressed)
		{
			UICamera.currentTouch.pressStarted = false;
			if (this.mTooltip != null)
			{
				this.ShowTooltip(false);
			}
			if (UICamera.currentTouch.pressed != null)
			{
				if (UICamera.currentTouch.dragStarted)
				{
					if (UICamera.onDragOut != null)
					{
						UICamera.onDragOut(UICamera.currentTouch.last, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.last, "OnDragOut", UICamera.currentTouch.dragged);
					if (UICamera.onDragEnd != null)
					{
						UICamera.onDragEnd(UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.dragged, "OnDragEnd", null);
				}
				if (UICamera.onPress != null)
				{
					UICamera.onPress(UICamera.currentTouch.pressed, false);
				}
				UICamera.Notify(UICamera.currentTouch.pressed, "OnPress", false);
				if (flag)
				{
					if (UICamera.onHover != null)
					{
						UICamera.onHover(UICamera.currentTouch.current, true);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnHover", true);
				}
				UICamera.mHover = UICamera.currentTouch.current;
				if (UICamera.currentTouch.dragged == UICamera.currentTouch.current || (UICamera.currentScheme != UICamera.ControlScheme.Controller && UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.totalDelta.sqrMagnitude < num))
				{
					if (UICamera.currentTouch.pressed != UICamera.mCurrentSelection)
					{
						UICamera.mNextSelection = null;
						UICamera.mCurrentSelection = UICamera.currentTouch.pressed;
						if (UICamera.onSelect != null)
						{
							UICamera.onSelect(UICamera.currentTouch.pressed, true);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnSelect", true);
					}
					else
					{
						UICamera.mNextSelection = null;
						UICamera.mCurrentSelection = UICamera.currentTouch.pressed;
					}
					if (UICamera.currentTouch.clickNotification != UICamera.ClickNotification.None && UICamera.currentTouch.pressed == UICamera.currentTouch.current)
					{
						float time = RealTime.time;
						if (UICamera.onClick != null)
						{
							UICamera.onClick(UICamera.currentTouch.pressed);
						}
						UICamera.Notify(UICamera.currentTouch.pressed, "OnClick", null);
						if (UICamera.currentTouch.clickTime + 0.35f > time)
						{
							if (UICamera.onDoubleClick != null)
							{
								UICamera.onDoubleClick(UICamera.currentTouch.pressed);
							}
							UICamera.Notify(UICamera.currentTouch.pressed, "OnDoubleClick", null);
						}
						UICamera.currentTouch.clickTime = time;
					}
				}
				else if (UICamera.currentTouch.dragStarted)
				{
					if (UICamera.onDrop != null)
					{
						UICamera.onDrop(UICamera.currentTouch.current, UICamera.currentTouch.dragged);
					}
					UICamera.Notify(UICamera.currentTouch.current, "OnDrop", UICamera.currentTouch.dragged);
				}
			}
			UICamera.currentTouch.dragStarted = false;
			UICamera.currentTouch.pressed = null;
			UICamera.currentTouch.dragged = null;
		}
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x0010EC28 File Offset: 0x0010D028
	public void ShowTooltip(bool val)
	{
		this.mTooltipTime = 0f;
		if (UICamera.onTooltip != null)
		{
			UICamera.onTooltip(this.mTooltip, val);
		}
		UICamera.Notify(this.mTooltip, "OnTooltip", val);
		if (!val)
		{
			this.mTooltip = null;
		}
	}

	// Token: 0x06003536 RID: 13622 RVA: 0x0010EC80 File Offset: 0x0010D080
	private void OnApplicationPause()
	{
		UICamera.MouseOrTouch mouseOrTouch = UICamera.currentTouch;
		if (this.useTouch)
		{
			BetterList<int> betterList = new BetterList<int>();
			foreach (KeyValuePair<int, UICamera.MouseOrTouch> keyValuePair in UICamera.mTouches)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.pressed)
				{
					UICamera.currentTouch = keyValuePair.Value;
					UICamera.currentTouchID = keyValuePair.Key;
					UICamera.currentScheme = UICamera.ControlScheme.Touch;
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
					this.ProcessTouch(false, true);
					betterList.Add(UICamera.currentTouchID);
				}
			}
			for (int i = 0; i < betterList.size; i++)
			{
				UICamera.RemoveTouch(betterList[i]);
			}
		}
		if (this.useMouse)
		{
			for (int j = 0; j < 3; j++)
			{
				if (UICamera.mMouse[j].pressed)
				{
					UICamera.currentTouch = UICamera.mMouse[j];
					UICamera.currentTouchID = -1 - j;
					UICamera.currentKey = KeyCode.Mouse0 + j;
					UICamera.currentScheme = UICamera.ControlScheme.Mouse;
					UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
					this.ProcessTouch(false, true);
				}
			}
		}
		if (this.useController && UICamera.controller.pressed)
		{
			UICamera.currentTouch = UICamera.controller;
			UICamera.currentTouchID = -100;
			UICamera.currentScheme = UICamera.ControlScheme.Controller;
			UICamera.currentTouch.last = UICamera.currentTouch.current;
			UICamera.currentTouch.current = UICamera.mCurrentSelection;
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
			this.ProcessTouch(false, true);
			UICamera.currentTouch.last = null;
		}
		UICamera.currentTouch = mouseOrTouch;
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x0010EE64 File Offset: 0x0010D264
	static UICamera()
	{
		// Note: this type is marked as 'beforefieldinit'.
		if (UICamera.f__mg1 == null)
		{
			UICamera.f__mg1 = new UICamera.GetKeyStateFunc(Input.GetKeyDown);
		}
		UICamera.GetKeyDown = UICamera.f__mg1;
		if (UICamera.f__mg2 == null)
		{
			UICamera.f__mg2 = new UICamera.GetKeyStateFunc(Input.GetKeyUp);
		}
		UICamera.GetKeyUp = UICamera.f__mg2;
		if (UICamera.f__mg3 == null)
		{
			UICamera.f__mg3 = new UICamera.GetKeyStateFunc(Input.GetKey);
		}
		UICamera.GetKey = UICamera.f__mg3;
		if (UICamera.f__mg4 == null)
		{
			UICamera.f__mg4 = new UICamera.GetAxisFunc(Input.GetAxis);
		}
		UICamera.GetAxis = UICamera.f__mg4;
		UICamera.showTooltips = true;
		UICamera.lastTouchPosition = Vector2.zero;
		UICamera.lastWorldPosition = Vector3.zero;
		UICamera.current = null;
		UICamera.currentCamera = null;
		UICamera.currentScheme = UICamera.ControlScheme.Mouse;
		UICamera.currentTouchID = -1;
		UICamera.currentKey = KeyCode.None;
		UICamera.currentTouch = null;
		UICamera.inputHasFocus = false;
		UICamera.mCurrentSelection = null;
		UICamera.mNextSelection = null;
		UICamera.mNextScheme = UICamera.ControlScheme.Controller;
		UICamera.mMouse = new UICamera.MouseOrTouch[]
		{
			new UICamera.MouseOrTouch(),
			new UICamera.MouseOrTouch(),
			new UICamera.MouseOrTouch()
		};
		UICamera.controller = new UICamera.MouseOrTouch();
		UICamera.mNextEvent = 0f;
		UICamera.mTouches = new Dictionary<int, UICamera.MouseOrTouch>();
		UICamera.mWidth = 0;
		UICamera.mHeight = 0;
		UICamera.isDragging = false;
		UICamera.mHit = default(UICamera.DepthEntry);
		UICamera.mHits = new BetterList<UICamera.DepthEntry>();
		UICamera.m2DPlane = new Plane(Vector3.back, 0f);
		UICamera.mNotifying = false;
	}

	// Token: 0x04001E57 RID: 7767
	public static BetterList<UICamera> list = new BetterList<UICamera>();

	// Token: 0x04001E58 RID: 7768
	public static UICamera.GetKeyStateFunc GetKeyDown;

	// Token: 0x04001E59 RID: 7769
	public static UICamera.GetKeyStateFunc GetKeyUp;

	// Token: 0x04001E5A RID: 7770
	public static UICamera.GetKeyStateFunc GetKey;

	// Token: 0x04001E5B RID: 7771
	public static UICamera.GetAxisFunc GetAxis;

	// Token: 0x04001E5C RID: 7772
	public static UICamera.OnScreenResize onScreenResize;

	// Token: 0x04001E5D RID: 7773
	public UICamera.EventType eventType = UICamera.EventType.UI_3D;

	// Token: 0x04001E5E RID: 7774
	public bool eventsGoToColliders;

	// Token: 0x04001E5F RID: 7775
	public LayerMask eventReceiverMask = -1;

	// Token: 0x04001E60 RID: 7776
	public bool debug;

	// Token: 0x04001E61 RID: 7777
	public bool useMouse = true;

	// Token: 0x04001E62 RID: 7778
	public bool useTouch = true;

	// Token: 0x04001E63 RID: 7779
	public bool allowMultiTouch = true;

	// Token: 0x04001E64 RID: 7780
	public bool useKeyboard = true;

	// Token: 0x04001E65 RID: 7781
	public bool useController = true;

	// Token: 0x04001E66 RID: 7782
	public bool stickyTooltip = true;

	// Token: 0x04001E67 RID: 7783
	public float tooltipDelay = 1f;

	// Token: 0x04001E68 RID: 7784
	public float mouseDragThreshold = 4f;

	// Token: 0x04001E69 RID: 7785
	public float mouseClickThreshold = 10f;

	// Token: 0x04001E6A RID: 7786
	public float touchDragThreshold = 40f;

	// Token: 0x04001E6B RID: 7787
	public float touchClickThreshold = 40f;

	// Token: 0x04001E6C RID: 7788
	public float rangeDistance = -1f;

	// Token: 0x04001E6D RID: 7789
	public string scrollAxisName = "Mouse ScrollWheel";

	// Token: 0x04001E6E RID: 7790
	public string verticalAxisName = "Vertical";

	// Token: 0x04001E6F RID: 7791
	public string horizontalAxisName = "Horizontal";

	// Token: 0x04001E70 RID: 7792
	public KeyCode submitKey0 = KeyCode.Return;

	// Token: 0x04001E71 RID: 7793
	public KeyCode submitKey1 = KeyCode.JoystickButton0;

	// Token: 0x04001E72 RID: 7794
	public KeyCode cancelKey0 = KeyCode.Escape;

	// Token: 0x04001E73 RID: 7795
	public KeyCode cancelKey1 = KeyCode.JoystickButton1;

	// Token: 0x04001E74 RID: 7796
	public static UICamera.OnCustomInput onCustomInput;

	// Token: 0x04001E75 RID: 7797
	public static bool showTooltips;

	// Token: 0x04001E76 RID: 7798
	public static Vector2 lastTouchPosition;

	// Token: 0x04001E77 RID: 7799
	public static Vector3 lastWorldPosition;

	// Token: 0x04001E78 RID: 7800
	public static RaycastHit lastHit;

	// Token: 0x04001E79 RID: 7801
	public static UICamera current;

	// Token: 0x04001E7A RID: 7802
	public static Camera currentCamera;

	// Token: 0x04001E7B RID: 7803
	public static UICamera.ControlScheme currentScheme;

	// Token: 0x04001E7C RID: 7804
	public static int currentTouchID;

	// Token: 0x04001E7D RID: 7805
	public static KeyCode currentKey;

	// Token: 0x04001E7E RID: 7806
	public static UICamera.MouseOrTouch currentTouch;

	// Token: 0x04001E7F RID: 7807
	public static bool inputHasFocus;

	// Token: 0x04001E80 RID: 7808
	private static GameObject mGenericHandler;

	// Token: 0x04001E81 RID: 7809
	public static GameObject fallThrough;

	// Token: 0x04001E82 RID: 7810
	public static UICamera.VoidDelegate onClick;

	// Token: 0x04001E83 RID: 7811
	public static UICamera.VoidDelegate onDoubleClick;

	// Token: 0x04001E84 RID: 7812
	public static UICamera.BoolDelegate onHover;

	// Token: 0x04001E85 RID: 7813
	public static UICamera.BoolDelegate onPress;

	// Token: 0x04001E86 RID: 7814
	public static UICamera.BoolDelegate onSelect;

	// Token: 0x04001E87 RID: 7815
	public static UICamera.FloatDelegate onScroll;

	// Token: 0x04001E88 RID: 7816
	public static UICamera.VectorDelegate onDrag;

	// Token: 0x04001E89 RID: 7817
	public static UICamera.VoidDelegate onDragStart;

	// Token: 0x04001E8A RID: 7818
	public static UICamera.ObjectDelegate onDragOver;

	// Token: 0x04001E8B RID: 7819
	public static UICamera.ObjectDelegate onDragOut;

	// Token: 0x04001E8C RID: 7820
	public static UICamera.VoidDelegate onDragEnd;

	// Token: 0x04001E8D RID: 7821
	public static UICamera.ObjectDelegate onDrop;

	// Token: 0x04001E8E RID: 7822
	public static UICamera.KeyCodeDelegate onKey;

	// Token: 0x04001E8F RID: 7823
	public static UICamera.BoolDelegate onTooltip;

	// Token: 0x04001E90 RID: 7824
	public static UICamera.MoveDelegate onMouseMove;

	// Token: 0x04001E91 RID: 7825
	private static GameObject mCurrentSelection;

	// Token: 0x04001E92 RID: 7826
	private static GameObject mNextSelection;

	// Token: 0x04001E93 RID: 7827
	private static UICamera.ControlScheme mNextScheme;

	// Token: 0x04001E94 RID: 7828
	private static UICamera.MouseOrTouch[] mMouse;

	// Token: 0x04001E95 RID: 7829
	public static GameObject mHover;

	// Token: 0x04001E96 RID: 7830
	public static UICamera.MouseOrTouch controller;

	// Token: 0x04001E97 RID: 7831
	private static float mNextEvent;

	// Token: 0x04001E98 RID: 7832
	private static Dictionary<int, UICamera.MouseOrTouch> mTouches;

	// Token: 0x04001E99 RID: 7833
	private static int mWidth;

	// Token: 0x04001E9A RID: 7834
	private static int mHeight;

	// Token: 0x04001E9B RID: 7835
	private GameObject mTooltip;

	// Token: 0x04001E9C RID: 7836
	private Camera mCam;

	// Token: 0x04001E9D RID: 7837
	private float mTooltipTime;

	// Token: 0x04001E9E RID: 7838
	private float mNextRaycast;

	// Token: 0x04001E9F RID: 7839
	public static bool isDragging;

	// Token: 0x04001EA0 RID: 7840
	public static GameObject hoveredObject;

	// Token: 0x04001EA1 RID: 7841
	private static UICamera.DepthEntry mHit;

	// Token: 0x04001EA2 RID: 7842
	private static BetterList<UICamera.DepthEntry> mHits;

	// Token: 0x04001EA3 RID: 7843
	private static Plane m2DPlane;

	// Token: 0x04001EA4 RID: 7844
	private static bool mNotifying;

	// Token: 0x04001EA7 RID: 7847
	[CompilerGenerated]
	private static BetterList<UICamera>.CompareFunc f__mg0;

	// Token: 0x04001EA8 RID: 7848
	[CompilerGenerated]
	private static UICamera.GetKeyStateFunc f__mg1;

	// Token: 0x04001EA9 RID: 7849
	[CompilerGenerated]
	private static UICamera.GetKeyStateFunc f__mg2;

	// Token: 0x04001EAA RID: 7850
	[CompilerGenerated]
	private static UICamera.GetKeyStateFunc f__mg3;

	// Token: 0x04001EAB RID: 7851
	[CompilerGenerated]
	private static UICamera.GetAxisFunc f__mg4;

	// Token: 0x02000631 RID: 1585
	public enum ControlScheme
	{
		// Token: 0x04001EAD RID: 7853
		Mouse,
		// Token: 0x04001EAE RID: 7854
		Touch,
		// Token: 0x04001EAF RID: 7855
		Controller
	}

	// Token: 0x02000632 RID: 1586
	public enum ClickNotification
	{
		// Token: 0x04001EB1 RID: 7857
		None,
		// Token: 0x04001EB2 RID: 7858
		Always,
		// Token: 0x04001EB3 RID: 7859
		BasedOnDelta
	}

	// Token: 0x02000633 RID: 1587
	public class MouseOrTouch
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x0600353B RID: 13627 RVA: 0x0010F01E File Offset: 0x0010D41E
		public float deltaTime
		{
			get
			{
				return (!this.touchBegan) ? 0f : (RealTime.time - this.pressTime);
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x0600353C RID: 13628 RVA: 0x0010F041 File Offset: 0x0010D441
		public bool isOverUI
		{
			get
			{
				return this.current != null && this.current != UICamera.fallThrough && NGUITools.FindInParents<UIRoot>(this.current) != null;
			}
		}

		// Token: 0x04001EB4 RID: 7860
		public Vector2 pos;

		// Token: 0x04001EB5 RID: 7861
		public Vector2 lastPos;

		// Token: 0x04001EB6 RID: 7862
		public Vector2 delta;

		// Token: 0x04001EB7 RID: 7863
		public Vector2 totalDelta;

		// Token: 0x04001EB8 RID: 7864
		public Camera pressedCam;

		// Token: 0x04001EB9 RID: 7865
		public GameObject last;

		// Token: 0x04001EBA RID: 7866
		public GameObject current;

		// Token: 0x04001EBB RID: 7867
		public GameObject pressed;

		// Token: 0x04001EBC RID: 7868
		public GameObject dragged;

		// Token: 0x04001EBD RID: 7869
		public float pressTime;

		// Token: 0x04001EBE RID: 7870
		public float clickTime;

		// Token: 0x04001EBF RID: 7871
		public UICamera.ClickNotification clickNotification = UICamera.ClickNotification.Always;

		// Token: 0x04001EC0 RID: 7872
		public bool touchBegan = true;

		// Token: 0x04001EC1 RID: 7873
		public bool pressStarted;

		// Token: 0x04001EC2 RID: 7874
		public bool dragStarted;
	}

	// Token: 0x02000634 RID: 1588
	public enum EventType
	{
		// Token: 0x04001EC4 RID: 7876
		World_3D,
		// Token: 0x04001EC5 RID: 7877
		UI_3D,
		// Token: 0x04001EC6 RID: 7878
		World_2D,
		// Token: 0x04001EC7 RID: 7879
		UI_2D
	}

	// Token: 0x02000635 RID: 1589
	// (Invoke) Token: 0x0600353E RID: 13630
	public delegate bool GetKeyStateFunc(KeyCode key);

	// Token: 0x02000636 RID: 1590
	// (Invoke) Token: 0x06003542 RID: 13634
	public delegate float GetAxisFunc(string name);

	// Token: 0x02000637 RID: 1591
	// (Invoke) Token: 0x06003546 RID: 13638
	public delegate void OnScreenResize();

	// Token: 0x02000638 RID: 1592
	// (Invoke) Token: 0x0600354A RID: 13642
	public delegate void OnCustomInput();

	// Token: 0x02000639 RID: 1593
	// (Invoke) Token: 0x0600354E RID: 13646
	public delegate void MoveDelegate(Vector2 delta);

	// Token: 0x0200063A RID: 1594
	// (Invoke) Token: 0x06003552 RID: 13650
	public delegate void VoidDelegate(GameObject go);

	// Token: 0x0200063B RID: 1595
	// (Invoke) Token: 0x06003556 RID: 13654
	public delegate void BoolDelegate(GameObject go, bool state);

	// Token: 0x0200063C RID: 1596
	// (Invoke) Token: 0x0600355A RID: 13658
	public delegate void FloatDelegate(GameObject go, float delta);

	// Token: 0x0200063D RID: 1597
	// (Invoke) Token: 0x0600355E RID: 13662
	public delegate void VectorDelegate(GameObject go, Vector2 delta);

	// Token: 0x0200063E RID: 1598
	// (Invoke) Token: 0x06003562 RID: 13666
	public delegate void ObjectDelegate(GameObject go, GameObject obj);

	// Token: 0x0200063F RID: 1599
	// (Invoke) Token: 0x06003566 RID: 13670
	public delegate void KeyCodeDelegate(GameObject go, KeyCode key);

	// Token: 0x02000640 RID: 1600
	private struct DepthEntry
	{
		// Token: 0x04001EC8 RID: 7880
		public int depth;

		// Token: 0x04001EC9 RID: 7881
		public RaycastHit hit;

		// Token: 0x04001ECA RID: 7882
		public Vector3 point;

		// Token: 0x04001ECB RID: 7883
		public GameObject go;
	}
}
