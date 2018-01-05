using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000C68 RID: 3176
public abstract class VRCUiCursor : MonoBehaviour
{
	// Token: 0x060062A2 RID: 25250
	protected abstract void Initialize();

	// Token: 0x060062A3 RID: 25251
	public abstract void UpdateCursor(VRCUiCursor.CursorRaycast target, bool useForUi = true);

	// Token: 0x060062A4 RID: 25252 RVA: 0x0022F1A8 File Offset: 0x0022D5A8
	protected VRCUiCursor.CursorOver[] CheckCursorTouch(VRCUiCursor.CursorRaycast target)
	{
		if (VRCPlayer.Instance == null)
		{
			return new VRCUiCursor.CursorOver[0];
		}
		VRCHandGrasper handGrasper = VRCPlayer.Instance.GetHandGrasper((this.handedness != VRCUiCursor.CursorHandedness.Left) ? ControllerHand.Right : ControllerHand.Left);
		if (handGrasper != null && handGrasper.IsHoldingObject())
		{
			return new VRCUiCursor.CursorOver[0];
		}
		int num = -1;
		Vector3 point = Vector3.zero;
		Vector3 normal = Vector3.zero;
		float num2 = float.PositiveInfinity;
		VRC_Interactable[] array = null;
		VRC_Pickup vrc_Pickup = null;
		VRC_PlayerApi apiPlayer = VRCPlayer.Instance.apiPlayer;
		Collider[] array2 = Physics.OverlapSphere(target.touch.position, target.touch.radius, VRCUiCursorManager.GetCurrentInteractiveLayers());
		for (int i = 0; i < array2.Length; i++)
		{
			bool flag = false;
			Vector3 vector = PhysicsUtil.ClosestPointOnCollider(array2[i], target.touch.position, ref flag);
			Vector3 vector2 = vector - target.touch.position;
			float dist = (!flag) ? vector2.magnitude : 0f;
			if (dist <= target.touch.radius)
			{
				VRC_Interactable[] array3 = (from inter in array2[i].GetComponents<VRC_Interactable>()
				where inter.IsInteractiveForPlayer(apiPlayer)
				select inter).ToArray<VRC_Interactable>();
				VRC_Pickup vrc_Pickup2 = array2[i].GetComponent<VRC_Pickup>();
				if (array3.Length != 0 || !(vrc_Pickup2 == null))
				{
					if (array3 != null && (array3.Length == 0 || array3.All((VRC_Interactable x) => dist > x.proximity)))
					{
						array3 = null;
					}
					VRC_StationInternal component = array2[i].GetComponent<VRC_StationInternal>();
					if (array3 != null && component != null && component.Occupant == VRCPlayer.Instance.gameObject)
					{
						array3 = null;
					}
					if (vrc_Pickup2 != null && (!vrc_Pickup2.pickupable || !VRCPlayer.Instance.canPickupObjects || (vrc_Pickup2.IsHeld && vrc_Pickup2.DisallowTheft && vrc_Pickup2.currentPlayer != apiPlayer)))
					{
						vrc_Pickup2 = null;
					}
					if (vrc_Pickup2 != null && vrc_Pickup2.currentlyHeldBy != null && dist > VRCHandGrasper.LerpToHandMinDistance)
					{
						vrc_Pickup2 = null;
					}
					if ((vrc_Pickup2 != null && vrc_Pickup == null) || dist <= num2)
					{
						if (!(vrc_Pickup2 == null) || (array3 != null && array3.Length != 0))
						{
							num = i;
							num2 = dist;
							point = vector;
							normal = vector2.normalized;
							array = array3;
							vrc_Pickup = vrc_Pickup2;
						}
					}
				}
			}
		}
		if (num < 0)
		{
			return new VRCUiCursor.CursorOver[0];
		}
		target.hitInfo.distance = num2;
		target.hitInfo.normal = normal;
		target.hitInfo.point = point;
		target.pickup = vrc_Pickup;
		target.interactable = array;
		if (vrc_Pickup != null && array != null)
		{
			target.over = new VRCUiCursor.CursorOver[]
			{
				VRCUiCursor.CursorOver.Pickup,
				VRCUiCursor.CursorOver.Interactable
			};
		}
		else if (array != null)
		{
			target.over = new VRCUiCursor.CursorOver[]
			{
				VRCUiCursor.CursorOver.Interactable
			};
		}
		else if (vrc_Pickup != null)
		{
			target.over = new VRCUiCursor.CursorOver[]
			{
				VRCUiCursor.CursorOver.Pickup
			};
		}
		else
		{
			target.over = new VRCUiCursor.CursorOver[0];
		}
		return target.over;
	}

	// Token: 0x060062A5 RID: 25253 RVA: 0x0022F56C File Offset: 0x0022D96C
	protected VRCUiCursor.CursorOver[] CastCursorRay(VRCUiCursor.CursorRaycast target)
	{
		target.over = new VRCUiCursor.CursorOver[0];
		List<VRCUiCursor.CursorOver> list = new List<VRCUiCursor.CursorOver>();
		int layerMask = VRCUiCursorManager.GetCurrentInteractiveLayers();
		Ray ray = target.ray;
		float num = 0f;
		float num2 = 100f;
		while (num2 > 0f && list.Count == 0)
		{
			ray.origin += ray.direction * num;
			num2 -= num;
			RaycastHit hitInfo;
			if (!Physics.Raycast(ray, out hitInfo, num2, layerMask))
			{
				target.hitInfo.distance = 10f;
				target.hitInfo.point = target.ray.origin + target.ray.direction * 10f;
				break;
			}
			num = hitInfo.distance + 0.01f;
			GameObject gameObject = hitInfo.collider.gameObject;
			VRC_UiShape component = gameObject.GetComponent<VRC_UiShape>();
			if (component != null)
			{
				target.hitInfo = hitInfo;
				target.uiShape = component;
				list.Add(VRCUiCursor.CursorOver.Ui);
			}
			else if (gameObject.GetComponentInParent<VRCPlayer>() != null)
			{
				if (this.uiMenuLayer < 0)
				{
					this.uiMenuLayer = LayerMask.NameToLayer("UiMenu");
				}
				if (gameObject.layer == this.uiMenuLayer)
				{
					target.hitInfo = hitInfo;
					target.player = gameObject.GetComponentInParent<VRCPlayer>();
					list.Add(VRCUiCursor.CursorOver.Player);
				}
			}
			if (VRCPlayer.Instance != null)
			{
				VRCHandGrasper handGrasper = VRCPlayer.Instance.GetHandGrasper((this.handedness != VRCUiCursor.CursorHandedness.Left) ? ControllerHand.Right : ControllerHand.Left);
				if (!(handGrasper != null) || !handGrasper.IsHoldingObject())
				{
					float num3 = 0f;
					if (target.extraReach)
					{
						num3 = VRCTrackingManager.GetRayReach(target.ray);
					}
					VRC_PlayerApi apiPlayer = VRCPlayer.Instance.apiPlayer;
					float range = hitInfo.distance - num3;
					VRC_Interactable[] array = (from inter in gameObject.GetComponents<VRC_Interactable>()
					where range <= inter.proximity && inter.IsInteractiveForPlayer(apiPlayer)
					select inter).ToArray<VRC_Interactable>();
					VRC_Pickup vrc_Pickup = gameObject.GetComponent<VRC_Pickup>();
					if (array.Length == 0)
					{
						array = null;
					}
					VRC_StationInternal component2 = gameObject.GetComponent<VRC_StationInternal>();
					if (array != null && component2 != null && component2.Occupant == VRCPlayer.Instance.gameObject)
					{
						array = null;
					}
					if (vrc_Pickup != null && (!vrc_Pickup.pickupable || !VRCPlayer.Instance.canPickupObjects || (vrc_Pickup.IsHeld && vrc_Pickup.DisallowTheft && vrc_Pickup.currentPlayer != apiPlayer)))
					{
						vrc_Pickup = null;
					}
					if (array != null && vrc_Pickup != null)
					{
						target.hitInfo = hitInfo;
						target.interactable = array;
						target.pickup = vrc_Pickup;
						list.Add(VRCUiCursor.CursorOver.Pickup);
						list.Add(VRCUiCursor.CursorOver.Interactable);
					}
					else if (array != null)
					{
						target.hitInfo = hitInfo;
						target.interactable = array;
						list.Add(VRCUiCursor.CursorOver.Interactable);
						VRC_WebPanel component3 = gameObject.GetComponent<VRC_WebPanel>();
						if (component3 != null && component3.interactive)
						{
							target.hitInfo = hitInfo;
							target.webPanel = component3.GetComponent<WebPanelInternal>();
							list.Add(VRCUiCursor.CursorOver.Web);
						}
					}
					else if (vrc_Pickup != null && range <= 0f)
					{
						target.hitInfo = hitInfo;
						target.pickup = vrc_Pickup;
						list.Add(VRCUiCursor.CursorOver.Pickup);
					}
					if (list.Count == 0)
					{
						if (Vector3.Dot(Vector3.up, hitInfo.normal) > 0.7f)
						{
							target.hitInfo = hitInfo;
							list.Add(VRCUiCursor.CursorOver.Floor);
						}
						else
						{
							target.hitInfo = hitInfo;
							list.Add(VRCUiCursor.CursorOver.Other);
						}
					}
				}
			}
			else
			{
				VRC_WebPanel component4 = gameObject.GetComponent<VRC_WebPanel>();
				if (component4 != null && component4.interactive)
				{
					target.hitInfo = hitInfo;
					target.webPanel = component4.GetComponent<WebPanelInternal>();
					list.Add(VRCUiCursor.CursorOver.Web);
				}
			}
		}
		target.over = list.ToArray();
		return target.over;
	}

	// Token: 0x060062A6 RID: 25254 RVA: 0x0022F9C8 File Offset: 0x0022DDC8
	public void SetTargetInfo(VRCUiCursor.CursorRaycast target, bool useForUi)
	{
		if (this.inUse == null)
		{
			VRCUiCursor.CursorHandedness cursorHandedness = this.handedness;
			if (cursorHandedness != VRCUiCursor.CursorHandedness.Right)
			{
				if (cursorHandedness == VRCUiCursor.CursorHandedness.Left)
				{
					this.inUse = VRCInputManager.FindInput("UseLeft");
					this.inDrop = VRCInputManager.FindInput("DropLeft");
				}
			}
			else
			{
				this.inUse = VRCInputManager.FindInput("UseRight");
				this.inDrop = VRCInputManager.FindInput("DropRight");
			}
			if (this.inUse == null)
			{
				return;
			}
		}
		this.over = target.over;
		this.distance = target.hitInfo.distance;
		this.targetPosition = target.hitInfo.point;
		bool flag = false;
		if (useForUi)
		{
			if (target.over.Contains(VRCUiCursor.CursorOver.Ui))
			{
				Vector2 screenPos = new Vector2(0f, 0f);
				UiShapeGenerator uiShapeGenerator = target.uiShape as UiShapeGenerator;
				if (uiShapeGenerator != null)
				{
					screenPos = uiShapeGenerator.GetPointerPosition(target.hitInfo.textureCoord);
				}
				else
				{
					screenPos = VRCVrCamera.GetInstance().screenCamera.WorldToScreenPoint(target.hitInfo.point);
				}
				this.uiInput.SetInputState(screenPos, target.hitInfo.point, this.inUse.down, this.inUse.up, this.inDrop.down, this.inDrop.up);
				if (this.uiInput.IsOverSelection())
				{
					List<VRCUiCursor.CursorOver> list = new List<VRCUiCursor.CursorOver>
					{
						VRCUiCursor.CursorOver.UiSelectable
					};
					list.AddRange(target.over);
					target.over = list.ToArray();
				}
				flag = true;
			}
			if (target.over.Contains(VRCUiCursor.CursorOver.Player))
			{
				if (this.inUse.click)
				{
					this.SelectHoveredPlayer();
				}
				else
				{
					if (this.hoveredPlayer != null)
					{
						PlayerSelector playerSelector = this.hoveredPlayer.playerSelector;
						playerSelector.Hover(false);
					}
					this.hoveredPlayer = target.player;
					PlayerSelector playerSelector2 = this.hoveredPlayer.playerSelector;
					if (playerSelector2 != null)
					{
						playerSelector2.Hover(true);
					}
				}
				flag = true;
			}
			else if (this.hoveredPlayer != null)
			{
				PlayerSelector playerSelector3 = this.hoveredPlayer.playerSelector;
				if (playerSelector3 != null)
				{
					playerSelector3.Hover(false);
				}
				this.hoveredPlayer = null;
			}
			if (target.over.Contains(VRCUiCursor.CursorOver.Web))
			{
				if (this.activeWebView != target.webPanel && this.activeWebView != null)
				{
					this.activeWebView.HandleFocusLoss();
				}
				this.activeWebView = target.webPanel;
				if (this.activeWebView != null)
				{
					this.activeWebView.HandleRayHit(target.hitInfo);
					flag = true;
				}
			}
			else if (this.activeWebView != null)
			{
				this.activeWebView.HandleFocusLoss();
				this.activeWebView = null;
			}
			if (target.over.Length == 0)
			{
				this.uiInput.SetInputState(Vector2.zero, target.hitInfo.point, false, true, false, true);
			}
			VRCUiCursor.CursorHandedness hand = this.handedness;
			if (!flag)
			{
				hand = VRCUiCursor.CursorHandedness.None;
			}
			VRCUiCursorManager.ForceCursorOn(hand);
		}
		VRC_Pickup selectedPickup = null;
		VRC_Interactable[] selectedInteractable = null;
		if (!VRCInputManager.legacyGrasp)
		{
			Component component;
			if (target.interactable == null)
			{
				component = null;
			}
			else
			{
				component = target.interactable.FirstOrDefault((VRC_Interactable i) => i.GetComponent<VRC_UseEvents>() != null || (i.GetComponent<VRC_Trigger>() != null && (i.GetComponent<VRC_Trigger>().HasInteractiveTriggers || i.GetComponent<VRC_Trigger>().HasPickupTriggers)));
			}
			Component component2 = component;
			this.outline.Clone(null);
			if (this.over.Contains(VRCUiCursor.CursorOver.Interactable) && component2 != null)
			{
				Transform trackedTransform = VRCTrackingManager.GetTrackedTransform((this.handedness != VRCUiCursor.CursorHandedness.Left) ? VRCTracking.ID.HandTracker_RightPalm : VRCTracking.ID.HandTracker_LeftPalm);
				bool flag2 = trackedTransform == null || VRCTrackingManager.IsPointWithinHMDView(trackedTransform.position);
				if (flag2)
				{
					this.outline.Clone(component2);
					if (TutorialManager.Instance != null)
					{
						TutorialManager.Instance.InteractableSelected(target.interactable, component2, this.handedness == VRCUiCursor.CursorHandedness.Left);
					}
					selectedInteractable = target.interactable;
				}
			}
			if (this.over.Contains(VRCUiCursor.CursorOver.Pickup) && target.pickup != null)
			{
				if (target.pickup.currentlyHeldBy == null)
				{
					this.outline.Clone(target.pickup);
					if (TutorialManager.Instance != null)
					{
						TutorialManager.Instance.PickupSelected(target.pickup, this.handedness == VRCUiCursor.CursorHandedness.Left);
					}
					selectedPickup = target.pickup;
				}
				else
				{
					this.outline.Clone(null);
				}
			}
		}
		else
		{
			this.outline.Clone(null);
		}
		VRCHandGrasper vrchandGrasper = null;
		if (VRCPlayer.Instance != null)
		{
			vrchandGrasper = VRCPlayer.Instance.GetHandGrasper((this.handedness != VRCUiCursor.CursorHandedness.Left) ? ControllerHand.Right : ControllerHand.Left);
		}
		if (vrchandGrasper != null)
		{
			vrchandGrasper.SetSelectedObject(selectedPickup, selectedInteractable);
		}
	}

	// Token: 0x060062A7 RID: 25255 RVA: 0x0022FEF8 File Offset: 0x0022E2F8
	private void OnPlayerLeft(VRC_PlayerApi player)
	{
		VRCPlayer component = player.GetComponent<VRCPlayer>();
		if (component != null)
		{
			if (this.hoveredPlayer != null && this.hoveredPlayer == component)
			{
				this.hoveredPlayer = null;
			}
			if (this.selectedPlayer != null && this.selectedPlayer == component)
			{
				this.selectedPlayer = null;
			}
		}
	}

	// Token: 0x060062A8 RID: 25256 RVA: 0x0022FF6C File Offset: 0x0022E36C
	public VRCPlayer SelectHoveredPlayer()
	{
		VRCPlayer vrcplayer = this.hoveredPlayer;
		this.ClearPlayerSelection();
		this.selectedPlayer = vrcplayer;
		if (this.selectedPlayer != null)
		{
			PlayerSelector playerSelector = this.selectedPlayer.playerSelector;
			playerSelector.Select(true);
		}
		return this.selectedPlayer;
	}

	// Token: 0x060062A9 RID: 25257 RVA: 0x0022FFB8 File Offset: 0x0022E3B8
	public void ClearPlayerSelection()
	{
		if (this.selectedPlayer != null)
		{
			PlayerSelector playerSelector = this.selectedPlayer.playerSelector;
			playerSelector.Select(false);
			this.selectedPlayer = null;
		}
		if (this.hoveredPlayer != null)
		{
			PlayerSelector playerSelector2 = this.hoveredPlayer.playerSelector;
			playerSelector2.Hover(false);
			this.hoveredPlayer = null;
		}
	}

	// Token: 0x060062AA RID: 25258 RVA: 0x0023001B File Offset: 0x0022E41B
	public VRCPlayer GetSelectedPlayer()
	{
		return this.selectedPlayer;
	}

	// Token: 0x0400480F RID: 18447
	protected bool initialized;

	// Token: 0x04004810 RID: 18448
	public Color UiColor = Color.white;

	// Token: 0x04004811 RID: 18449
	public Color InteractiveColor = Color.green;

	// Token: 0x04004812 RID: 18450
	public Color FloorColor = Color.blue;

	// Token: 0x04004813 RID: 18451
	public VRCStandaloneInputModule uiInput;

	// Token: 0x04004814 RID: 18452
	public Texture2D icon;

	// Token: 0x04004815 RID: 18453
	public VRCUiCursor.CursorOver[] over;

	// Token: 0x04004816 RID: 18454
	public float distance = 1f;

	// Token: 0x04004817 RID: 18455
	public Vector3 targetPosition;

	// Token: 0x04004818 RID: 18456
	public VRCUiSelectedOutline outline;

	// Token: 0x04004819 RID: 18457
	private WebPanelInternal activeWebView;

	// Token: 0x0400481A RID: 18458
	private VRCPlayer hoveredPlayer;

	// Token: 0x0400481B RID: 18459
	private VRCPlayer selectedPlayer;

	// Token: 0x0400481C RID: 18460
	public VRCUiCursor.CursorHandedness handedness;

	// Token: 0x0400481D RID: 18461
	private VRCInput inUse;

	// Token: 0x0400481E RID: 18462
	private VRCInput inDrop;

	// Token: 0x0400481F RID: 18463
	private int uiMenuLayer = -1;

	// Token: 0x04004820 RID: 18464
	private const float MAX_RANGE = 10f;

	// Token: 0x02000C69 RID: 3177
	public enum CursorOver
	{
		// Token: 0x04004823 RID: 18467
		Ui,
		// Token: 0x04004824 RID: 18468
		UiSelectable,
		// Token: 0x04004825 RID: 18469
		Web,
		// Token: 0x04004826 RID: 18470
		Interactable,
		// Token: 0x04004827 RID: 18471
		Pickup,
		// Token: 0x04004828 RID: 18472
		Floor,
		// Token: 0x04004829 RID: 18473
		Player,
		// Token: 0x0400482A RID: 18474
		Other
	}

	// Token: 0x02000C6A RID: 3178
	public enum CursorHandedness
	{
		// Token: 0x0400482C RID: 18476
		None,
		// Token: 0x0400482D RID: 18477
		Right,
		// Token: 0x0400482E RID: 18478
		Left
	}

	// Token: 0x02000C6B RID: 3179
	public class CursorRaycast
	{
		// Token: 0x060062AC RID: 25260 RVA: 0x00230077 File Offset: 0x0022E477
		public CursorRaycast()
		{
			this.Reset();
		}

		// Token: 0x060062AD RID: 25261 RVA: 0x00230088 File Offset: 0x0022E488
		public void Reset()
		{
			this.ray = default(Ray);
			this.touch = default(BoundingSphere);
			this.over = new VRCUiCursor.CursorOver[0];
			this.hitInfo = default(RaycastHit);
			this.extraReach = false;
			this.uiShape = null;
			this.webPanel = null;
			this.interactable = null;
			this.pickup = null;
			this.player = null;
		}

		// Token: 0x0400482F RID: 18479
		public Ray ray;

		// Token: 0x04004830 RID: 18480
		public BoundingSphere touch;

		// Token: 0x04004831 RID: 18481
		public VRCUiCursor.CursorOver[] over;

		// Token: 0x04004832 RID: 18482
		public RaycastHit hitInfo;

		// Token: 0x04004833 RID: 18483
		public bool extraReach;

		// Token: 0x04004834 RID: 18484
		public VRCInput useButton;

		// Token: 0x04004835 RID: 18485
		public VRCInput gripButton;

		// Token: 0x04004836 RID: 18486
		public VRC_UiShape uiShape;

		// Token: 0x04004837 RID: 18487
		public WebPanelInternal webPanel;

		// Token: 0x04004838 RID: 18488
		public VRC_Interactable[] interactable;

		// Token: 0x04004839 RID: 18489
		public VRC_Pickup pickup;

		// Token: 0x0400483A RID: 18490
		public VRCPlayer player;
	}
}
