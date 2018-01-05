using System;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000C6C RID: 3180
public class VRCUiCursorManager : MonoBehaviour
{
	// Token: 0x060062AF RID: 25263 RVA: 0x00232BD4 File Offset: 0x00230FD4
	private void Start()
	{
		if (VRCUiCursorManager.instance != null)
		{
			Debug.LogError("Too Many Cursor Managers.");
		}
		VRCUiCursorManager.instance = this;
		this.uiInteractiveLayers = LayerMask.GetMask(new string[]
		{
			"UiMenu"
		});
		this.worldInteractiveLayers = (-1 ^ LayerMask.GetMask(new string[]
		{
			"UiMenu",
			"PlayerLocal",
			"UI"
		}));
		this.inDropLeft = VRCInputManager.FindInput("DropLeft");
		this.inDropRight = VRCInputManager.FindInput("DropRight");
		this.inUseLeft = VRCInputManager.FindInput("UseLeft");
		this.inUseRight = VRCInputManager.FindInput("UseRight");
		this.inNavigate = VRCInputManager.FindInput("Vertical");
	}

	// Token: 0x060062B0 RID: 25264 RVA: 0x00232CA0 File Offset: 0x002310A0
	private void UpdateActiveCursorSet()
	{
		if (this.activeCursor == VRCUiCursorManager.CursorType.None)
		{
			this.activeCursor = VRCUiCursorManager.CursorType.Gaze;
			if (VRCInputManager.IsEnabled(VRCInputManager.InputMethod.Vive) && VRCInputManager.IsPresent(VRCInputManager.InputMethod.Vive))
			{
				this.activeCursor = VRCUiCursorManager.CursorType.Hands;
			}
			if (VRCInputManager.IsEnabled(VRCInputManager.InputMethod.Hydra) && VRCInputManager.IsPresent(VRCInputManager.InputMethod.Hydra))
			{
				this.activeCursor = VRCUiCursorManager.CursorType.Hands;
			}
			if (VRCInputManager.IsEnabled(VRCInputManager.InputMethod.Oculus) && VRCInputManager.IsPresent(VRCInputManager.InputMethod.Oculus))
			{
				this.activeCursor = VRCUiCursorManager.CursorType.Hands;
			}
		}
		if (VRCInputManager.AnyKey(VRCInputManager.InputMethod.Hydra) || VRCInputManager.AnyKey(VRCInputManager.InputMethod.Vive) || VRCInputManager.AnyKey(VRCInputManager.InputMethod.Oculus))
		{
			this.activeCursor = VRCUiCursorManager.CursorType.Hands;
		}
		else if ((VRCInputManager.AnyKey(VRCInputManager.InputMethod.Controller) || VRCInputManager.AnyKey(VRCInputManager.InputMethod.Keyboard)) && HMDManager.IsHmdDetected())
		{
			this.activeCursor = VRCUiCursorManager.CursorType.Gaze;
		}
		else if (VRCInputManager.AnyKey(VRCInputManager.InputMethod.Mouse))
		{
			this.activeCursor = VRCUiCursorManager.CursorType.Mouse;
		}
	}

	// Token: 0x060062B1 RID: 25265 RVA: 0x00232D84 File Offset: 0x00231184
	private void UpdateWindowsCursorLock()
	{
		if (Input.GetKey(KeyCode.Tab) || this.uiActive)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = (Input.GetKey(KeyCode.Tab) || VRCApplicationSetup.IsEditor());
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	// Token: 0x060062B2 RID: 25266 RVA: 0x00232DD8 File Offset: 0x002311D8
	private void Update()
	{
		if (!VRCTrackingManager.IsInitialized())
		{
			return;
		}
		this.UpdateWindowsCursorLock();
		this.UpdateActiveCursorSet();
		bool flag = (this.inDropLeft.button && VRCInputManager.legacyGrasp) || this.forcedCursor == VRCUiCursor.CursorHandedness.Left;
		bool flag2 = (this.inDropRight.button && VRCInputManager.legacyGrasp) || this.forcedCursor == VRCUiCursor.CursorHandedness.Right;
		bool flag3 = this.inDropRight.button || this.forcedCursor == VRCUiCursor.CursorHandedness.Right;
		bool flag4 = this.inDropRight.button || this.forcedCursor == VRCUiCursor.CursorHandedness.Right;
		this.leftTarget.Reset();
		this.rightTarget.Reset();
		if (this.activeCursor == VRCUiCursorManager.CursorType.Hands)
		{
			if (this.inUseLeft.down || this.inDropLeft.down)
			{
				this.dominantHand = VRCUiCursor.CursorHandedness.Left;
			}
			else if (this.inUseRight.down || this.inDropRight.down)
			{
				this.dominantHand = VRCUiCursor.CursorHandedness.Right;
			}
			bool active = VRCTrackingManager.IsTracked(VRCTracking.ID.HandTracker_LeftPointer) && (this.uiActive || flag) && !this.blockLeftCursor;
			this.handLeftCursor.gameObject.SetActive(active);
			this.handLeftCursor.UpdateCursor(this.leftTarget, this.dominantHand == VRCUiCursor.CursorHandedness.Left);
			bool active2 = VRCTrackingManager.IsTracked(VRCTracking.ID.HandTracker_RightPointer) && (this.uiActive || flag2) && !this.blockRightCursor;
			this.handRightCursor.gameObject.SetActive(active2);
			this.handRightCursor.UpdateCursor(this.rightTarget, this.dominantHand == VRCUiCursor.CursorHandedness.Right);
			bool flag5 = VRCTrackingManager.IsTracked(VRCTracking.ID.HandTracker_LeftPointer) && LocomotionInputController.navigationCursorActive && !this.uiActive && VRCInputManager.locomotionMethod == VRCInputManager.LocomotionMethod.ThirdPerson;
			this.navigationCursor.gameObject.SetActive(flag5);
			if (flag5)
			{
				this.navigationCursor.UpdateCursor(null, true);
			}
			this.gazeCursor.gameObject.SetActive(false);
			this.mouseCursor.gameObject.SetActive(false);
		}
		else if (this.activeCursor == VRCUiCursorManager.CursorType.Gaze)
		{
			bool active3 = this.uiActive || flag4;
			this.gazeCursor.gameObject.SetActive(active3);
			this.gazeCursor.UpdateCursor(this.rightTarget, true);
			this.dominantHand = VRCUiCursor.CursorHandedness.Right;
			this.handLeftCursor.gameObject.SetActive(false);
			this.handRightCursor.gameObject.SetActive(false);
			this.navigationCursor.gameObject.SetActive(false);
			this.mouseCursor.gameObject.SetActive(false);
		}
		else if (this.activeCursor == VRCUiCursorManager.CursorType.Mouse)
		{
			bool active4 = this.uiActive || flag3;
			this.mouseCursor.gameObject.SetActive(active4);
			this.mouseCursor.UpdateCursor(this.rightTarget, true);
			this.dominantHand = VRCUiCursor.CursorHandedness.Right;
			this.handLeftCursor.gameObject.SetActive(false);
			this.handRightCursor.gameObject.SetActive(false);
			this.navigationCursor.gameObject.SetActive(false);
			this.gazeCursor.gameObject.SetActive(false);
		}
	}

	// Token: 0x060062B3 RID: 25267 RVA: 0x0023313C File Offset: 0x0023153C
	public static LayerMask GetCurrentInteractiveLayers()
	{
		if (VRCUiCursorManager.instance.uiActive)
		{
			return VRCUiCursorManager.instance.uiInteractiveLayers;
		}
		return VRCUiCursorManager.instance.worldInteractiveLayers;
	}

	// Token: 0x060062B4 RID: 25268 RVA: 0x00233162 File Offset: 0x00231562
	public static void SetUiActive(bool active)
	{
		VRCUiCursorManager.instance.uiActive = active;
	}

	// Token: 0x060062B5 RID: 25269 RVA: 0x0023316F File Offset: 0x0023156F
	public static void SetActiveCursor(VRCUiCursorManager.CursorType ct)
	{
		VRCUiCursorManager.instance.activeCursor = ct;
	}

	// Token: 0x060062B6 RID: 25270 RVA: 0x0023317C File Offset: 0x0023157C
	public static VRCUiCursorManager.CursorType GetActiveCursor()
	{
		return VRCUiCursorManager.instance.activeCursor;
	}

	// Token: 0x060062B7 RID: 25271 RVA: 0x00233188 File Offset: 0x00231588
	public static void BlockCursor(bool right, bool block)
	{
		if (right)
		{
			VRCUiCursorManager.instance.blockRightCursor = block;
		}
		else
		{
			VRCUiCursorManager.instance.blockLeftCursor = block;
		}
	}

	// Token: 0x060062B8 RID: 25272 RVA: 0x002331AB File Offset: 0x002315AB
	public static VRCUiCursorManager GetInstance()
	{
		return VRCUiCursorManager.instance;
	}

	// Token: 0x060062B9 RID: 25273 RVA: 0x002331B2 File Offset: 0x002315B2
	public static VRC_Interactable[] GetInteractable(VRCUiCursor.CursorHandedness handedness)
	{
		if (handedness == VRCUiCursor.CursorHandedness.Left)
		{
			return VRCUiCursorManager.instance.leftTarget.interactable;
		}
		if (handedness != VRCUiCursor.CursorHandedness.Right)
		{
			return null;
		}
		return VRCUiCursorManager.instance.rightTarget.interactable;
	}

	// Token: 0x060062BA RID: 25274 RVA: 0x002331E8 File Offset: 0x002315E8
	public static VRC_Pickup GetPickup(VRCUiCursor.CursorHandedness handedness)
	{
		if (handedness == VRCUiCursor.CursorHandedness.Left)
		{
			return VRCUiCursorManager.instance.leftTarget.pickup;
		}
		if (handedness != VRCUiCursor.CursorHandedness.Right)
		{
			return null;
		}
		return VRCUiCursorManager.instance.rightTarget.pickup;
	}

	// Token: 0x060062BB RID: 25275 RVA: 0x0023321E File Offset: 0x0023161E
	public static void ForceCursorOn(VRCUiCursor.CursorHandedness hand)
	{
		VRCUiCursorManager.instance.forcedCursor = hand;
	}

	// Token: 0x060062BC RID: 25276 RVA: 0x0023322B File Offset: 0x0023162B
	public static void SetDominantHand(VRCUiCursor.CursorHandedness hand)
	{
		VRCUiCursorManager.instance.dominantHand = hand;
	}

	// Token: 0x060062BD RID: 25277 RVA: 0x00233238 File Offset: 0x00231638
	public static VRCPlayer GetSelectedPlayer()
	{
		VRCUiCursorManager.CursorType cursorType = VRCUiCursorManager.instance.activeCursor;
		if (cursorType == VRCUiCursorManager.CursorType.Gaze)
		{
			return VRCUiCursorManager.instance.gazeCursor.GetSelectedPlayer();
		}
		if (cursorType != VRCUiCursorManager.CursorType.Mouse)
		{
			if (cursorType == VRCUiCursorManager.CursorType.Hands)
			{
				if (VRCUiCursorManager.instance.blockRightCursor)
				{
					return VRCUiCursorManager.instance.handLeftCursor.GetSelectedPlayer();
				}
				if (VRCUiCursorManager.instance.blockLeftCursor)
				{
					return VRCUiCursorManager.instance.handRightCursor.GetSelectedPlayer();
				}
			}
			return null;
		}
		return VRCUiCursorManager.instance.mouseCursor.GetSelectedPlayer();
	}

	// Token: 0x060062BE RID: 25278 RVA: 0x002332D0 File Offset: 0x002316D0
	public static void ClearSelectedPlayer()
	{
		if (VRCUiCursorManager.instance.gazeCursor != null)
		{
			VRCUiCursorManager.instance.gazeCursor.ClearPlayerSelection();
		}
		if (VRCUiCursorManager.instance.mouseCursor != null)
		{
			VRCUiCursorManager.instance.mouseCursor.ClearPlayerSelection();
		}
		if (VRCUiCursorManager.instance.handLeftCursor != null)
		{
			VRCUiCursorManager.instance.handLeftCursor.ClearPlayerSelection();
		}
		if (VRCUiCursorManager.instance.handRightCursor != null)
		{
			VRCUiCursorManager.instance.handRightCursor.ClearPlayerSelection();
		}
	}

	// Token: 0x0400483B RID: 18491
	public VRCUiCursor mouseCursor;

	// Token: 0x0400483C RID: 18492
	public VRCUiCursor gazeCursor;

	// Token: 0x0400483D RID: 18493
	public VRCUiCursor handLeftCursor;

	// Token: 0x0400483E RID: 18494
	public VRCUiCursor handRightCursor;

	// Token: 0x0400483F RID: 18495
	public VRCUiCursor navigationCursor;

	// Token: 0x04004840 RID: 18496
	[HideInInspector]
	public LayerMask uiInteractiveLayers;

	// Token: 0x04004841 RID: 18497
	[HideInInspector]
	public LayerMask worldInteractiveLayers;

	// Token: 0x04004842 RID: 18498
	private bool uiActive;

	// Token: 0x04004843 RID: 18499
	private VRCUiCursorManager.CursorType activeCursor;

	// Token: 0x04004844 RID: 18500
	private VRCInput inDropLeft;

	// Token: 0x04004845 RID: 18501
	private VRCInput inDropRight;

	// Token: 0x04004846 RID: 18502
	private VRCInput inUseLeft;

	// Token: 0x04004847 RID: 18503
	private VRCInput inUseRight;

	// Token: 0x04004848 RID: 18504
	private VRCInput inNavigate;

	// Token: 0x04004849 RID: 18505
	private bool blockLeftCursor;

	// Token: 0x0400484A RID: 18506
	private bool blockRightCursor;

	// Token: 0x0400484B RID: 18507
	private VRCUiCursor.CursorHandedness forcedCursor;

	// Token: 0x0400484C RID: 18508
	private VRCUiCursor.CursorHandedness dominantHand = VRCUiCursor.CursorHandedness.Right;

	// Token: 0x0400484D RID: 18509
	private VRCUiCursor.CursorRaycast leftTarget = new VRCUiCursor.CursorRaycast();

	// Token: 0x0400484E RID: 18510
	private VRCUiCursor.CursorRaycast rightTarget = new VRCUiCursor.CursorRaycast();

	// Token: 0x0400484F RID: 18511
	private static VRCUiCursorManager instance;

	// Token: 0x02000C6D RID: 3181
	public enum CursorType
	{
		// Token: 0x04004851 RID: 18513
		None,
		// Token: 0x04004852 RID: 18514
		Mouse,
		// Token: 0x04004853 RID: 18515
		Gaze,
		// Token: 0x04004854 RID: 18516
		Hands
	}
}
